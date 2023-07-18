using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentAttendanceApiDAL.Tables;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentAttendanceApiDAL
{
    public static class CommonUtility
    {
        public static string GenerateToken(IConfiguration configuration,string email,string name)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            Claim[] claims;
            if (string.IsNullOrEmpty(name))
            {
                claims = new[]
               {
                    new Claim(ClaimTypes.Email, email)
                };
            }
            else
            {
                claims = new[]
                 {
                    new Claim(ClaimTypes.NameIdentifier, name)
                    //new Claim(ClaimTypes.Email, masterAdmin.Email.ToString())
                  };
            }

            var token = new JwtSecurityToken(
           configuration["Jwt:Issuer"],
           configuration["Jwt:Audience"],
           claims,
           expires: DateTime.Now.AddMinutes(180),
           signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }




    }
}