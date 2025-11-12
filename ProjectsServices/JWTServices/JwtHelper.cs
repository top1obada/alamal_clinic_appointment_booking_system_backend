using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ProjectsServices.JWTServices
{

    

    public static class clsJWTHelper
    {

        public static string GenerateJwtToken(
          IEnumerable<Claim> Calims, clsJWTTemplate JWTTemplate)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JWTTemplate.Key)
            );

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var NativeClaims = new List<Claim>()
            {
                
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               
            };

            NativeClaims.AddRange(Calims);

            


            var token = new JwtSecurityToken(
                issuer: JWTTemplate.Issuer,
                audience: JWTTemplate.Audience,
                claims: NativeClaims,
                expires: DateTime.UtcNow.AddMinutes(JWTTemplate.DurationInMinutes ?? 60),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }


        public static clsJWTTemplate GetToken(IConfiguration configuration)
        {
            return new clsJWTTemplate
            {
                Issuer = configuration["JwtSettings:Issuer"],
                Audience = configuration["JwtSettings:Audience"],
                DurationInMinutes = configuration.GetValue<int>("JwtSettings:DurationInMinutes"),
                Key = configuration["JwtSettings:Key"]
            };
        }

    }
}
