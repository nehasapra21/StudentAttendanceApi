using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentAttendanceApiDAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public UserRepository(AppDbContext appDbContext, ILogger<UserRepository> logger, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<Users?> LoginSuperAdmin(string userName, string password)
        {
            logger.LogInformation($"UserRepository : LoginSuperAdmin : Started");

            Users user = new Users();
            try
            {
                user = await appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == userName && x.Password == password);
                if (user != null)
                {
                    user.Password = password;
                    ///Generate token for user
                    #region JWT
                    user.Token = GenerateToken(user);
                    #endregion
                }
                
                logger.LogInformation($"UserRepository : LoginUser : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : LoginUser ", ex);
            }
            return user;
        }


        public async Task<Users> SaveSuperAdmin(Users user)
        {
            logger.LogInformation($"UserRepository : SaveSuperAdmin : Started");

            try
            {
                if (user.Id > 0)
                {
                    appDbContext.Entry(user).State = EntityState.Modified;
                }
                else
                {
                    user.Type = 1;//
                    user.CreatedOn = DateTime.UtcNow;
                    user.UserGuidId = Guid.NewGuid();
                    appDbContext.Users.Add(user);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveSuperAdmin : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveSuperAdmin ", ex);
            }
            return user;
        }
        public string GenerateToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            Claim[] claims;
            if (string.IsNullOrEmpty(user.Name))
            {
                claims = new[]
               {
                    new Claim(ClaimTypes.Email, user.Email)
                };
            }
            else
            {
                claims = new[]
                 {
                    new Claim(ClaimTypes.NameIdentifier, user.Name.ToString())
                    //new Claim(ClaimTypes.Email, masterAdmin.Email.ToString())
                  };
            }

            var token = new JwtSecurityToken(
           configuration["Jwt:Issuer"],
           configuration["Jwt:Audience"],
           claims,
           expires: DateTime.Now.AddMinutes(1440),
           signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
