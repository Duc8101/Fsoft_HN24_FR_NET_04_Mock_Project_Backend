using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;

namespace Phone_Shop.DataAccess.Helper
{
    public class HardwareHelper
    {

        public static string Generate()
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Wmi();
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Dmidecode();
            }

            return GetIoregOutput();
        }

        private static string Hash(string value)
        {
            // ------------------ dùng sha1 mã hóa -----------------------
            byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(value));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                // convert into hexadecimal
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }

        [SupportedOSPlatform("windows")]
        private static string Wmi()
        {
            // ------------------- dùng lớp WMI Win32_BaseBoard để lấy thông tin motherboard
            ManagementClass managementClass = new ManagementClass("Win32_BaseBoard");
            ManagementObjectCollection baseBoards = managementClass.GetInstances();
            string?[] baseBoardInfo = new string?[4];
            foreach (ManagementObject baseBoard in baseBoards)
            {
                baseBoardInfo[0] = baseBoard["Manufacturer"].ToString();
                baseBoardInfo[1] = baseBoard["Product"].ToString();
                baseBoardInfo[2] = baseBoard["SerialNumber"].ToString();
                baseBoardInfo[3] = baseBoard["Version"].ToString();
                break;
            }

            string input = string.Join("\n", baseBoardInfo);
            return Hash(input);
        }

        private static string Dmidecode()
        {
            string baseboardManufacturer = File.ReadAllText("/sys/class/dmi/id/board_vendor").Trim();
            string baseboardName = File.ReadAllText("/sys/class/dmi/id/board_name").Trim();
            string baseboardVersion = File.ReadAllText("/sys/class/dmi/id/board_version").Trim();
            string[] baseBoardInfo =
            {
                    baseboardManufacturer, baseboardName, baseboardVersion
            };

            string input = string.Join("\n", baseBoardInfo);
            return Hash(input);
        }

        private static string GetIoregOutput()
        {
            // Tạo Process để chạy lệnh shell
            Process process = new Process();
            process.StartInfo.FileName = "/bin/bash";
            process.StartInfo.Arguments = "-c \"ioreg -l | grep IOPlatformSerialNumber\"";  // Lệnh lấy số serial bo mạch chủ
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // Chạy lệnh
            process.Start();

            // Đọc đầu ra
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return Hash(output);
        }
    }
}
