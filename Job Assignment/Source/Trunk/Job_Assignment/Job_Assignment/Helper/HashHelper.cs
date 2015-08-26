using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Job_Assignment
{
    public class HashHelper
    {
        public static String HashAlgorithm = "SHA1";
        public static int KeyLength = 16;

        public static String generateSalt()
        {
            byte[] x_salt = new byte[8];
            RandomNumberGenerator x_rand = RandomNumberGenerator.Create();
            x_rand.GetBytes(x_salt);

            return Convert.ToBase64String(x_salt);
        }
        public static String computeHash(String pass) //
        {
            // create the random salt value
            byte[] x_salt = Convert.FromBase64String("");

            // create the derivation protocol class
            PasswordDeriveBytes x_pwd = new PasswordDeriveBytes(pass, x_salt);

            // specify the number of iterations
            x_pwd.IterationCount = 100;
            // specify the hashing algorithm
            x_pwd.HashName = HashAlgorithm;

            byte[] x_key = x_pwd.GetBytes(KeyLength);

            return Convert.ToBase64String(x_key);
        }
    }
}
