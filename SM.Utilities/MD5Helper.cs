using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SM.Utilities
{
    public class MD5Helper
    {
        public static string GetMD5Hash(string rawString)
        {
            System.Text.UnicodeEncoding encode = new System.Text.UnicodeEncoding();
            Byte[] passwordBytes = encode.GetBytes(rawString);
            Byte[] hash;
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            hash = md5.ComputeHash(passwordBytes);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (byte outputByte in hash)
            {
                sb.Append(outputByte.ToString("x2").ToUpper());
            }

            return sb.ToString();
        }
    }
}
