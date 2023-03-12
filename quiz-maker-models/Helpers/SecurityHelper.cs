using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.Helpers
{
    public static class SecurityHelper
    {
        private static MD5 md5Hash => MD5.Create();
        private const string key = "QUIZ@MAKER";

        public static string GetMd5HashPassword(string username, string password)
        {
            return GetMd5Hash($"{username}{key}{password}");
        }

        public static string GetMd5Hash(string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input)); // Convert the input string to a byte array and compute the hash.
            var sBuilder = new StringBuilder(); // Create a new Stringbuilder to collect the bytes and create a string.

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString(); // Return the hexadecimal string.
        }

        // Verify a hash against a string.
        public static bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input); // Hash the input.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase; // Create a StringComparer an compare the hashes.
            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }
}
