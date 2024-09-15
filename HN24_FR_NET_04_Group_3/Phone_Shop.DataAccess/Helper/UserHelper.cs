using MailKit.Net.Smtp;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Phone_Shop.Common.Configuration;
using Phone_Shop.Common.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Phone_Shop.DataAccess.Helper
{
    public class UserHelper
    {

        public static string HashPassword(string password)
        {
            // using SHA256 for hash password
            byte[] hashPw = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashPw.Length; i++)
            {
                // convert into hexadecimal
                builder.Append(hashPw[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static string getAccessToken(User user, DateTime expireDate)
        {
            byte[] key = Encoding.UTF8.GetBytes(ConfigData.JwtKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            //  create list claim  to store user's information
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.UserId.ToString()),
                new Claim("username", user.Username),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
            };

            JwtSecurityToken security = new JwtSecurityToken(ConfigData.JwtIssuer,
                ConfigData.JwtAudience, claims, expires: expireDate,
                signingCredentials: credentials);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            // get access token
            return handler.WriteToken(security);
        }

        public static async Task sendEmail(string subject, string body, string to)
        {
            // create message to send
            MimeMessage mime = new MimeMessage();
            MailboxAddress mailFrom = MailboxAddress.Parse(ConfigData.MailUser);
            MailboxAddress mailTo = MailboxAddress.Parse(to);
            mime.From.Add(mailFrom);
            mime.To.Add(mailTo);
            mime.Subject = subject;
            mime.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
            // send message
            SmtpClient smtp = new SmtpClient();
            smtp.Connect(ConfigData.MailHost);
            smtp.Authenticate(ConfigData.MailUser, ConfigData.MailPassword);
            await smtp.SendAsync(mime);
            smtp.Disconnect(true);
        }

        public static string RandomPassword()
        {
            Random random = new Random();
            // password contain both alphabets and numbers
            string format = "abcdefghijklmnopqrstuvwxyz0123456789QWERTYUIOPASDFGHJKLZXCVBNM";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                // get random index character
                int index = random.Next(format.Length);
                builder.Append(format[index]);
            }
            return builder.ToString();
        }

        public static string BodyEmailForForgotPassword(string password)
        {
            string body = "<h1>Mật khẩu mới</h1>\n" +
                            "<p>Mật khẩu mới là: " + password + "</p>\n" +
                            "<p>Không nên chia sẻ mật khẩu của bạn với người khác.</p>";
            return body;
        }

        public static string BodyEmailForAdminReceiveOrder()
        {
            string body = "<h1>New Order</h1>\n"
                + "<p>Please check information order</p>\n";
            return body;
        }

    }
}
