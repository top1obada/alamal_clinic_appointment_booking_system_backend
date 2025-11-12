using System;
using System.Linq;
using System.Security.Cryptography;

namespace ProjectsServices.PasswordServices
{
    public static class clsPasswordEncrypt
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        // توليد Hash وSalt
        public static (byte[] Hash, byte[] Salt) HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] saltBytes = new byte[SaltSize];
            rng.GetBytes(saltBytes);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(KeySize);

            return (key, saltBytes);
        }

        // التحقق من كلمة المرور
        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, Iterations, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(KeySize);
            return key.SequenceEqual(storedHash);
        }
    }
}