using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CorePractice.Helpers
{
    public static class SecurityHelper
    {
        /// <summary>
        /// Computes a SHA256 hash and return it as a byte array
        /// </summary>
        /// <param name="plainPassword">A plain text</param>
        /// <returns>A byte array containing a computed SHA256 hash</returns>
        public static byte[] ComputeHash(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            HashAlgorithm hash = new SHA256Managed();
            return hash.ComputeHash(plainTextBytes);
        }

        /// <summary>
        /// Generates a random salt
        /// </summary>
        /// <returns>A byte array containing a random salt</returns>
        public static byte[] GetRandomSalt()
        {
            int minSaltSize = 16;
            int maxSaltSize = 32;

            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);
            byte[] saltBytes = new byte[saltSize];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(saltBytes);
            return saltBytes;
        }

        /// <summary>
        /// Encrypts a plain password and auto generates a random salt
        /// </summary>
        /// <param name="plainPassword">The plain password</param>
        /// <param name="generatedRandomSalt">When this method returns, contains the auto generated random salt</param>
        /// <returns>The encrypted password</returns>
        public static string EncryptPassword(string plainPassword, out string generatedRandomSalt)
        {
            byte[] passwordHash = ComputeHash(plainPassword);
            byte[] salt = GetRandomSalt();
            generatedRandomSalt = Convert.ToBase64String(salt);
            byte[] passwordHashAndSalt = new byte[passwordHash.Length + salt.Length];
            for (int i = 0; i < passwordHash.Length; i++)
                passwordHashAndSalt[i] = passwordHash[i];
            for (int i = 0; i < salt.Length; i++)
                passwordHashAndSalt[passwordHash.Length + i] = salt[i];

            return Convert.ToBase64String(passwordHashAndSalt);
        }

        /// <summary>
        /// Encrypts a plain password with an existing salt
        /// </summary>
        /// <param name="plainPassword">The plain password</param>
        /// <param name="existingSalt">The existing salt</param>
        /// <returns>The encrypted password</returns>
        public static string EncryptPassword(string plainPassword, string existingSalt)
        {
            if (existingSalt == null)
                existingSalt = string.Empty;
            byte[] passwordHash = ComputeHash(plainPassword);
            byte[] salt = Convert.FromBase64String(existingSalt);
            byte[] passwordHashAndSalt = new byte[passwordHash.Length + salt.Length];
            for (int i = 0; i < passwordHash.Length; i++)
                passwordHashAndSalt[i] = passwordHash[i];
            for (int i = 0; i < salt.Length; i++)
                passwordHashAndSalt[passwordHash.Length + i] = salt[i];

            return Convert.ToBase64String(passwordHashAndSalt);
        }

        #region simple character shift encrypt and decrypt

        /// <summary>
        /// simple character shift encryp 
        /// </summary>
        /// <param name="str">plaintext string</param>
        /// <returns></returns>
        public static string Encrypto(string str)
        {
            string result = "";

            int i = 0;
            foreach (byte b in Encoding.GetEncoding(1252).GetBytes(str))
            {
                int c = b + i;

                if (string.IsNullOrEmpty(result))
                {
                    result += c.ToString();
                }
                else
                {
                    result += "-" + c.ToString();
                }
                i++;
            }

            return result;
        }



        /// <summary>
        /// simple character shift decrypt 
        /// </summary>
        /// <param name="str">encrypted string</param>
        /// <returns></returns>
        public static string Decrypto(string str)
        {
            string result = "";
            int i = 0;
            foreach (string s in str.Split('-'))
            {
                int r = 0;
                if (!int.TryParse(s, out r))
                {
                    continue;
                }

                char c = (char)(r - i);
                result += c.ToString();
                i++;
            }

            return result;
        }

        #endregion
    }

}
