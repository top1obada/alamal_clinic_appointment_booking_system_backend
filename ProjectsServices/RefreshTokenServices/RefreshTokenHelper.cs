using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsServices.RefreshTokenServices
{
    public static class clsRefreshTokenHelper
    {
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64]; 
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber); 
        }

        public static string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.Append(b.ToString("x2")); 
            return sb.ToString(); 
        }
    }
}
