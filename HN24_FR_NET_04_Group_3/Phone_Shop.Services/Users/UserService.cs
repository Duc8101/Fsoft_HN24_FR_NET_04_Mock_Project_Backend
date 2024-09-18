using AutoMapper;
using Common.DTOs.UserDTO;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Common.DTOs.UserDTO;
using Phone_Shop.Common.Entity;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Responses;
using Phone_Shop.DataAccess.Extensions;
using Phone_Shop.DataAccess.Helper;
using Phone_Shop.DataAccess.UnitOfWorks;
using Phone_Shop.Services.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;

namespace Phone_Shop.Services.Users
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public ResponseBase ChangePassword(ChangePasswordDTO DTO, int userId)
        {
            try
            {
                User? user = _unitOfWork.UserRepository.FindById(userId);
                if (user == null)
                {
                    return new ResponseBase($"Not found user with id = {userId}", (int)HttpStatusCode.NotFound);
                }

                if (StringHelper.isStringNullOrEmpty(DTO.CurrentPassword) || StringHelper.isStringNullOrEmpty(DTO.ConfirmPassword) || StringHelper.isStringNullOrEmpty(DTO.NewPassword))
                {
                    return new ResponseBase("Password not empty", (int)HttpStatusCode.Conflict);
                }

                if (DTO.NewPassword.Length < (int)UserLength.Min_Password || DTO.ConfirmPassword.Length < (int)UserLength.Min_Password || DTO.CurrentPassword.Length < (int)UserLength.Min_Password)
                {
                    return new ResponseBase($"Password at least {(int)UserLength.Min_Password} characters", (int)HttpStatusCode.Conflict);
                }

                if (!user.Password.Equals(UserHelper.HashPassword(DTO.CurrentPassword)))
                {
                    return new ResponseBase("Current password not correct", (int)HttpStatusCode.Conflict);
                }

                if (!DTO.ConfirmPassword.Equals(DTO.NewPassword))
                {
                    return new ResponseBase("Confirm password not correct", (int)HttpStatusCode.Conflict);
                }

                user.Password = UserHelper.HashPassword(DTO.NewPassword);
                user.UpdateAt = DateTime.Now;

                _unitOfWork.BeginTransaction();
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Password was changed successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Detail(int userId)
        {
            try
            {
                User? user = _unitOfWork.UserRepository.FindById(userId);
                if (user == null)
                {
                    return new ResponseBase($"Not found user with id = {userId}", (int)HttpStatusCode.NotFound);
                }

                UserDetailDTO data = _mapper.Map<UserDetailDTO>(user);
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }

        }

        public async Task<ResponseBase> ForgotPassword(ForgotPasswordDTO DTO)
        {
            try
            {
                User? user = _unitOfWork.UserRepository.GetFirst(null, u => u.Email == DTO.Email.Trim());
                if (user == null)
                {
                    return new ResponseBase($"Not found email '{DTO.Email.Trim()}'", (int)HttpStatusCode.NotFound);
                }

                string newPass = UserHelper.RandomPassword();
                string body = UserHelper.BodyEmailForForgotPassword(newPass);
                await UserHelper.sendEmail("Welcome to our phone shop", body, DTO.Email.Trim());
                string hashPass = UserHelper.HashPassword(newPass);
                user.Password = hashPass;
                user.UpdateAt = DateTime.Now;

                _unitOfWork.BeginTransaction();
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Password was reset successfully. Please check your email");
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase GetUserByToken(string token)
        {
            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken security = handler.ReadJwtToken(token);
                string? UserId = security.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                if (UserId == null)
                {
                    return new ResponseBase("Not found user id", (int)HttpStatusCode.NotFound);
                }

                if (!int.TryParse(UserId, out int userId))
                {
                    return new ResponseBase("User id invalid", (int)HttpStatusCode.NotFound);
                }

                User? user = _unitOfWork.UserRepository.GetSingle(item => item.Include(u => u.Role), u => u.UserId == userId);
                if (user == null)
                {
                    return new ResponseBase($"Not found user with id = {userId}", (int)HttpStatusCode.NotFound);
                }

                string hardwareInfo = HardwareHelper.Generate();
                Client? client = _unitOfWork.ClientRepository.GetFirst(null, c => c.HardwareInfo == hardwareInfo);
                if (client == null)
                {
                    return new ResponseBase("Not found client", (int)HttpStatusCode.NotFound);
                }

                Expression<Func<UserClient, bool>> predicate = uc => uc.UserId == userId && uc.ClientId == client.ClientId;
                UserClient? userClient = _unitOfWork.UserClientRepository.GetSingle(null, predicate);
                if (userClient == null)
                {
                    return new ResponseBase("User not register on this client", (int)HttpStatusCode.Conflict);
                }

                if (userClient.Token != token)
                {
                    return new ResponseBase("Invalid token", (int)HttpStatusCode.Conflict);
                }

                if (userClient.ExpireDate < DateTime.Now)
                {
                    return new ResponseBase("Token expired", (int)HttpStatusCode.Conflict);
                }

                UserLoginInfoDTO data = _mapper.Map<UserLoginInfoDTO>(user);
                data.Token = token;
                data.ExpireDate = userClient.ExpireDate;
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Login(LoginDTO DTO)
        {
            try
            {
                User? user = _unitOfWork.UserRepository.GetFirst(item => item.Include(u => u.Role).Include(u => u.Carts), u => u.Username == DTO.Username);
                if (user == null || !user.Password.Equals(UserHelper.HashPassword(DTO.Password)))
                {
                    return new ResponseBase("Username or password incorrect", (int)HttpStatusCode.Conflict);
                }

                string hardwareInfo = HardwareHelper.Generate();
                int clientId;
                Client? client = _unitOfWork.ClientRepository.GetFirst(null, c => c.HardwareInfo == hardwareInfo);

                _unitOfWork.BeginTransaction();
                // nếu chưa đăng ký thiết bị
                if (client == null)
                {
                    client = new Client()
                    {
                        HardwareInfo = hardwareInfo,
                        CreatedAt = DateTime.Now,
                        UpdateAt = DateTime.Now,
                    };
                    _unitOfWork.ClientRepository.Add(client);
                    clientId = client.ClientId;
                }
                else
                {
                    clientId = client.ClientId;
                }

                string accessToken;
                UserClient? userClient = _unitOfWork.UserClientRepository.GetSingle(null, uc => uc.UserId == user.UserId
                && uc.ClientId == clientId);

                DateTime dateNow = DateTime.Now;
                DateTime expireDate = dateNow.AddDays(1);

                // nếu lần đầu đăng nhập trên thiết bị
                if (userClient == null)
                {
                    accessToken = UserHelper.getAccessToken(user, expireDate);
                    userClient = new UserClient()
                    {
                        UserId = user.UserId,
                        ClientId = clientId,
                        Token = accessToken,
                        ExpireDate = expireDate,
                        CreatedAt = dateNow,
                        UpdateAt = dateNow,
                    };
                    _unitOfWork.UserClientRepository.Add(userClient);
                }
                else
                {
                    // nếu token hết hạn
                    if (userClient.ExpireDate < dateNow)
                    {
                        accessToken = UserHelper.getAccessToken(user, expireDate);
                        userClient.Token = accessToken;
                        userClient.ExpireDate = expireDate;
                        userClient.UpdateAt = dateNow;
                    }
                    else
                    {
                        accessToken = UserHelper.getAccessToken(user, userClient.ExpireDate);
                    }
                }

                // delete cart
                List<Cart> carts = user.Carts.ToList();
                _unitOfWork.CartRepository.DeleteMultiple(carts);

                UserLoginInfoDTO data = _mapper.Map<UserLoginInfoDTO>(user);
                data.Token = accessToken;
                data.ExpireDate = userClient.ExpireDate;
                _unitOfWork.Commit();
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Logout(int userId)
        {
            try
            {
                string hardwareInfo = HardwareHelper.Generate();
                Client? client = _unitOfWork.ClientRepository.GetFirst(null, c => c.HardwareInfo == hardwareInfo);

                if (client == null)
                {
                    return new ResponseBase("Not found client", (int)HttpStatusCode.NotFound);
                }

                Expression<Func<UserClient, bool>> predicate = uc => uc.UserId == userId && uc.ClientId == client.ClientId;
                UserClient? userClient = _unitOfWork.UserClientRepository.GetSingle(null, predicate);
                if (userClient == null)
                {
                    return new ResponseBase("User not register on this client", (int)HttpStatusCode.Conflict);
                }

                List<Cart> carts = _unitOfWork.CartRepository.GetAll(null, null, c => c.CustomerId == userId).ToList();

                _unitOfWork.BeginTransaction();

                // remove cart
                _unitOfWork.CartRepository.DeleteMultiple(carts);

                // set token expire
                userClient.ExpireDate = DateTime.Now;
                userClient.UpdateAt = DateTime.Now;
                _unitOfWork.Commit();
                return new ResponseBase(true, "Logout successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Register(RegisterDTO DTO)
        {
            try
            {
                if (StringHelper.isStringNullOrEmpty(DTO.FullName))
                {
                    return new ResponseBase("You have to input full name", (int)HttpStatusCode.Conflict);
                }

                Regex regexPhone = new Regex(Pattern.Phone.getDescription());
                if (DTO.Phone != null && !regexPhone.IsMatch(DTO.Phone))
                {
                    return new ResponseBase("Phone only number and must be 10 numbers", (int)HttpStatusCode.Conflict);
                }

                Regex regexEmail = new Regex(Pattern.Email.getDescription());
                if (!regexEmail.IsMatch(DTO.Email.Trim()))
                {
                    return new ResponseBase("Email invalid", (int)HttpStatusCode.Conflict);
                }

                Regex regexUsername = new Regex(Pattern.Username.getDescription());
                if (!regexUsername.IsMatch(DTO.Username))
                {
                    return new ResponseBase($"The username starts with alphabet , contains only alphabet and numbers, at least {(int)UserLength.Min_Username} characters, max {(int)UserLength.Max_Username} characters", (int)HttpStatusCode.Conflict);
                }

                if (StringHelper.isStringNullOrEmpty(DTO.Password) || StringHelper.isStringNullOrEmpty(DTO.ConfirmPassword))
                {
                    return new ResponseBase("Password not empty", (int)HttpStatusCode.Conflict);
                }

                if (DTO.Password.Length < (int)UserLength.Min_Password || DTO.ConfirmPassword.Length < (int)UserLength.Min_Password)
                {
                    return new ResponseBase($"Password at least {(int)UserLength.Min_Password} characters", (int)HttpStatusCode.Conflict);
                }

                if (!DTO.ConfirmPassword.Equals(DTO.Password))
                {
                    return new ResponseBase("Confirm password not correct", (int)HttpStatusCode.Conflict);
                }

                if (_unitOfWork.UserRepository.Any(u => u.Username == DTO.Username || u.Email == DTO.Email.Trim()))
                {
                    return new ResponseBase("Username or email already exists", (int)HttpStatusCode.Conflict);
                }

                User user = _mapper.Map<User>(DTO);
                user.RoleId = (int)Roles.Customer;
                user.CreatedAt = DateTime.Now;
                user.UpdateAt = DateTime.Now;
                _unitOfWork.BeginTransaction();
                _unitOfWork.UserRepository.Add(user);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Register successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase UpdateProfile(ProfileDTO DTO, int userId)
        {
            try
            {
                User? user = _unitOfWork.UserRepository.FindById(userId);
                if (user == null)
                {
                    return new ResponseBase($"Not found user with id = {userId}", (int)HttpStatusCode.NotFound);
                }

                if (StringHelper.isStringNullOrEmpty(DTO.FullName))
                {
                    return new ResponseBase("You have to input full name", (int)HttpStatusCode.Conflict);
                }

                Regex regexPhone = new Regex(Pattern.Phone.getDescription());
                if (DTO.Phone != null && !regexPhone.IsMatch(DTO.Phone))
                {
                    return new ResponseBase("Phone only number and must be 10 numbers", (int)HttpStatusCode.Conflict);
                }

                Regex regexEmail = new Regex(Pattern.Email.getDescription());
                if (!regexEmail.IsMatch(DTO.Email.Trim()))
                {
                    return new ResponseBase("Email invalid", (int)HttpStatusCode.Conflict);
                }

                if (_unitOfWork.UserRepository.Any(u => u.Email == DTO.Email.Trim() && u.UserId != userId))
                {
                    return new ResponseBase("Email already exists", (int)HttpStatusCode.Conflict);
                }

                user.FullName = DTO.FullName.Trim();
                user.Email = DTO.Email.Trim();
                user.Phone = DTO.Phone;
                user.Address = StringHelper.getStringValue(DTO.Address);
                user.UpdateAt = DateTime.Now;

                _unitOfWork.BeginTransaction();
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Update profile successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
