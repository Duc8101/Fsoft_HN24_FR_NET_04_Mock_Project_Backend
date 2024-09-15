using System.Management;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;

namespace Phone_Shop.DataAccess.Helper
{
    public class HardwareHelper
    {

        [SupportedOSPlatform("windows")]
        public static string Generate()
        {
            // ------------------- dùng lớp VMI Win32_BaseBoard để lấy thông tin motherboard
            ManagementClass managementClass = new ManagementClass("Win32_BaseBoard");
            ManagementObjectCollection baseBoards = managementClass.GetInstances();
            string?[] baseBoardInfo = new string?[4];
            foreach (ManagementObject baseBoard in baseBoards)
            {
                baseBoardInfo[0] = baseBoard["Manufacturer"].ToString();
                baseBoardInfo[1] = baseBoard["Product"].ToString();
                baseBoardInfo[2] = baseBoard["SerialNumber"].ToString();
                baseBoardInfo[3] = baseBoard["Version"].ToString();
            }

            string input = string.Join("\n", baseBoardInfo);
            // ------------------ dùng sha1 mã hóa -----------------------
            byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                // convert into hexadecimal
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
