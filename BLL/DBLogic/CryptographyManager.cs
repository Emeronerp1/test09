using System;
using System.Text;
using System.Security.Cryptography;


    public static class CryptographyManager
    {
        /// <summary>
        /// Class for manage Encrypt and Decrypt
        /// <list type="bullet">
        /// <item>
        /// <term>Author :</term>
        /// <description>Sanoop K (sanoopkuttur@gmail.com)</description>
        /// <term>Date</term>
        /// <description>11-04-2014</description>
        /// </item>
        /// </list>
        /// </summary>


        public static string EncryptString(string message, string passPhrase)
        {
            byte[] results;
            System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passPhrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider tdesAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            tdesAlgorithm.Key = tdesKey;
            tdesAlgorithm.Mode = CipherMode.ECB;
            tdesAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] dataToEncrypt = utf8.GetBytes(message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(results);
        }

        public static string DecryptString(string message, string passphrase)
        {
            byte[] results;
            System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider tdesAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            tdesAlgorithm.Key = tdesKey;
            tdesAlgorithm.Mode = CipherMode.ECB;
            tdesAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] dataToDecrypt = Convert.FromBase64String(message);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform decryptor = tdesAlgorithm.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return utf8.GetString(results);
        }

        public static string GetHashedPassword(string password)
        {
            try
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] hashedBytes;
                UTF8Encoding encoder = new UTF8Encoding();
                hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes("salt" + password + "pepper"));
                StringBuilder s = new StringBuilder();
                foreach (byte by in hashedBytes)
                {
                    s.Append(by.ToString("x2").ToLower());
                }
                return s.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string DecryptHashedPassword(string password)
        {
            try
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] hashedBytes;
                UTF8Encoding encoder = new UTF8Encoding();
                hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes("salt" + password + "pepper"));
                StringBuilder s = new StringBuilder();
                foreach (byte by in hashedBytes)
                {
                    s.Append(by.ToString("x2").ToLower());
                }
                return s.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
