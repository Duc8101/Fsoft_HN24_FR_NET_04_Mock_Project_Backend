using Microsoft.Extensions.Configuration;

namespace Phone_Shop.Common.Configuration
{
    public class ConfigData
    {
        public static string SqlConnection => getConfigValue("ConnectionStrings:DefaultConnection");
        public static string JwtKey => getConfigValue("Jwt:Key");
        public static string JwtIssuer => getConfigValue("Jwt:Issuer");
        public static string JwtAudience => getConfigValue("Jwt:Audience");
        public static string MailHost => getConfigValue("MailAddress:Host");
        public static string MailUser => getConfigValue("MailAddress:Username");
        public static string MailPassword => getConfigValue("MailAddress:Password");

        private static string getConfigValue(string key)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationRoot config = builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            return config.GetSection(key).Value ?? string.Empty;
        }

    }
}
