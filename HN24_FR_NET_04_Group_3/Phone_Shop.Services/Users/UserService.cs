using AutoMapper;
using Common.DTOs.UserDTO;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Common.DTOs.UserDTO;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Extensions;
using Phone_Shop.Common.Responses;
using Phone_Shop.DataAccess.Entity;
using Phone_Shop.DataAccess.Helper;
using Phone_Shop.DataAccess.UnitOfWorks;
using Phone_Shop.Services.Base;
using System.IdentityModel.Tokens.Jwt;
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

                if (DTO.CurrentPassword.Trim().Length == 0 || DTO.ConfirmPassword.Trim().Length == 0 || DTO.NewPassword.Trim().Length == 0)
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
                _unitOfWork.Rollback();
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
                User? user = _unitOfWork.UserRepository.GetFirst(null, null, u => u.Email == DTO.Email.Trim());
                if (user == null)
                {
                    return new ResponseBase($"Not found email '{DTO.Email.Trim()}'", (int)HttpStatusCode.NotFound);
                }

                string newPass = UserHelper.RandomPassword();
                string body = UserHelper.BodyEmailForForgotPassword(newPass);
                await UserHelper.SendEmail("Welcome to our phone shop", body, DTO.Email.Trim());
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
                _unitOfWork.Rollback();
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

                UserToken? userToken = _unitOfWork.UserTokenRepository.FindById(userId);
                if (userToken == null)
                {
                    return new ResponseBase("User not register on this client", (int)HttpStatusCode.Conflict);
                }

                if (userToken.Token != token)
                {
                    return new ResponseBase("Invalid token", (int)HttpStatusCode.Conflict);
                }

                if (userToken.ExpireDate < DateTime.Now)
                {
                    return new ResponseBase("Token expired", (int)HttpStatusCode.Conflict);
                }

                UserLoginInfoDTO data = _mapper.Map<UserLoginInfoDTO>(user);
                data.Token = token;
                data.ExpireDate = userToken.ExpireDate;
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
                User? user = _unitOfWork.UserRepository.GetFirst(item => item.Include(u => u.Role).Include(u => u.Carts), null, u => u.Username == DTO.Username);
                if (user == null || !user.Password.Equals(UserHelper.HashPassword(DTO.Password)))
                {
                    return new ResponseBase("Username or password incorrect", (int)HttpStatusCode.Conflict);
                }

                UserToken? userToken = _unitOfWork.UserTokenRepository.FindById(user.UserId);
                string accessToken;
                DateTime dateNow = DateTime.Now;
                DateTime expireDate = dateNow.AddDays(1);

                _unitOfWork.BeginTransaction();
                if (userToken == null)
                {
                    accessToken = UserHelper.GetAccessToken(user, expireDate);
                    userToken = new UserToken()
                    {
                        UserId = user.UserId,
                        Token = accessToken,
                        CreatedAt = dateNow,
                        ExpireDate = expireDate,
                        UpdateAt = dateNow,
                    };
                    _unitOfWork.UserTokenRepository.Add(userToken);
                }
                else
                {
                    // nếu token hết hạn
                    if (userToken.ExpireDate < dateNow)
                    {
                        accessToken = UserHelper.GetAccessToken(user, expireDate);
                        userToken.Token = accessToken;
                        userToken.ExpireDate = expireDate;
                        userToken.UpdateAt = dateNow;
                        _unitOfWork.UserTokenRepository.Update(userToken);
                    }
                    else
                    {
                        accessToken = UserHelper.GetAccessToken(user, userToken.ExpireDate);
                    }
                }

                // delete cart
                List<Cart> carts = user.Carts.ToList();
                _unitOfWork.CartRepository.DeleteMultiple(carts);

                UserLoginInfoDTO data = _mapper.Map<UserLoginInfoDTO>(user);
                data.Token = accessToken;
                data.ExpireDate = userToken.ExpireDate;
                _unitOfWork.Commit();
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Logout(int userId)
        {
            try
            {
                UserToken? userToken = _unitOfWork.UserTokenRepository.FindById(userId);
                if (userToken == null)
                {
                    return new ResponseBase("User not register on this client", (int)HttpStatusCode.Conflict);
                }

                List<Cart> carts = _unitOfWork.CartRepository.GetAll(null, null, c => c.CustomerId == userId).ToList();

                _unitOfWork.BeginTransaction();

                // remove cart
                _unitOfWork.CartRepository.DeleteMultiple(carts);

                // set token expire
                userToken.ExpireDate = DateTime.Now;
                userToken.UpdateAt = DateTime.Now;

                _unitOfWork.UserTokenRepository.Update(userToken);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Logout successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Register(RegisterDTO DTO)
        {
            try
            {
                if (DTO.FullName.Trim().Length == 0)
                {
                    return new ResponseBase("You have to input full name", (int)HttpStatusCode.Conflict);
                }

                Regex regexPhone = new Regex(Pattern.Phone.GetDescription());
                if (DTO.Phone != null && !regexPhone.IsMatch(DTO.Phone))
                {
                    return new ResponseBase("Phone only number and must be 10 numbers", (int)HttpStatusCode.Conflict);
                }

                Regex regexEmail = new Regex(Pattern.Email.GetDescription());
                if (!regexEmail.IsMatch(DTO.Email.Trim()))
                {
                    return new ResponseBase("Email invalid", (int)HttpStatusCode.Conflict);
                }

                Regex regexUsername = new Regex(Pattern.Username.GetDescription());
                if (!regexUsername.IsMatch(DTO.Username))
                {
                    return new ResponseBase($"The username starts with alphabet , contains only alphabet and numbers, at least {(int)UserLength.Min_Username} characters, max {(int)UserLength.Max_Username} characters", (int)HttpStatusCode.Conflict);
                }

                if (DTO.Password.Trim().Length == 0 || DTO.ConfirmPassword.Trim().Length == 0)
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
                _unitOfWork.Rollback();
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

                if (DTO.FullName.Trim().Length == 0)
                {
                    return new ResponseBase("You have to input full name", (int)HttpStatusCode.Conflict);
                }

                Regex regexPhone = new Regex(Pattern.Phone.GetDescription());
                if (DTO.Phone != null && !regexPhone.IsMatch(DTO.Phone))
                {
                    return new ResponseBase("Phone only number and must be 10 numbers", (int)HttpStatusCode.Conflict);
                }

                Regex regexEmail = new Regex(Pattern.Email.GetDescription());
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
                user.Address = DTO.Address == null || DTO.Address.Trim().Length == 0 ? null : DTO.Address.Trim();
                user.UpdateAt = DateTime.Now;

                _unitOfWork.BeginTransaction();
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Update profile successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
