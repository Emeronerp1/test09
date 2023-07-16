using System;

    public class EncryptDecryptHelper
    {
        public EncryptDecryptHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string Encrypt(string key)
        {
            string encryptedstring;
            byte[] data;

            data = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            //ASCII.GetBytes(key);

            encryptedstring = Convert.ToBase64String(data);

            //		 System.Text.ASCIIEncoding.ASCII.GetBytes(args(0))
            //	   Dim str As String = Convert.ToBase64String(data)
            //				   Console.WriteLine(str
            return encryptedstring;
        }

        public static string Decrypt(string key)
        {
            byte[] data;
            string decryptedstring;

            data = Convert.FromBase64String(key);


            decryptedstring = System.Text.ASCIIEncoding.ASCII.GetString(data);
            return decryptedstring;
        }
    }