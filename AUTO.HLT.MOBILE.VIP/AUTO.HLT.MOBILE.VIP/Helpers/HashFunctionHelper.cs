using System.Security.Cryptography;
using System.Text;

namespace AUTO.HLT.MOBILE.VIP.Helpers
{
    public class HashFunctionHelper
    {
        private static HashAlgorithm[] _hash = { new MD5CryptoServiceProvider(), new SHA1CryptoServiceProvider(), new SHA256CryptoServiceProvider(), new SHA512CryptoServiceProvider() };

        /// <summary>
        /// hàm chuyển đổi một chuỗi sang mã băm
        /// </summary>
        /// <param name="input">dữ liệu đầu vào</param>
        /// <param name="hash">
        /// 1. md5
        /// 2. sha1
        /// 3.sha256
        /// 4. sha512
        /// </param>
        /// <returns></returns>
        public static string GetHashCode(string input, int hash)
        {
            var str = Encoding.UTF8.GetBytes(input);
            var data = _hash[hash].ComputeHash(str);
            var output = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                output += data[i].ToString("X2");
            }
            return output;
        }
    }
}