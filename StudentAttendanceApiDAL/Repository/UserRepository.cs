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

        public async Task<Users?> GetUserById(int userId,int type)
        {
            logger.LogInformation($"UserRepository : GetUserById : Started");

            Users user = new Users();
            try
            {
                user = await appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId && x.Type==type);
                if (user != null)
                {
                    return user;
                }

                logger.LogInformation($"UserRepository : GetUserById : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetUserById", ex);
                throw ex;
            }
            return user;
        }

        public async Task<Users?> LoginUser(string userName, string password)
        {
            logger.LogInformation($"UserRepository : LoginUser : Started");

            Users user = new Users();
            try
            {
                user = await appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.PhoneNumber == userName && x.Password == password);
                if (user != null)
                {
                    ///Generate token for user
                    #region JWT
                    user.Token = CommonUtility.GenerateToken(configuration, user.Email, user.Name);
                    #endregion
                }

                logger.LogInformation($"UserRepository : LoginUser : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : LoginUser ", ex);
                throw ex;
            }
            return user;
        }


        public async Task<Users> SaveLogin(Users user)
        {
            logger.LogInformation($"UserRepository : SaveLogin : Started");

            try
            {
                if (user.Id > 0)
                {
                    appDbContext.Entry(user).State = EntityState.Modified;
                }
                else
                {//
                    user.CreatedOn = DateTime.UtcNow;

                    appDbContext.Users.Add(user);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveLogin : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveLogin ", ex);
                throw ex;
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
                {//
                    user.CreatedOn = DateTime.UtcNow;

                    appDbContext.Users.Add(user);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveSuperAdmin : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveSuperAdmin ", ex);
                throw ex;
            }
            return user;
        }

        public async Task<string> CheckUserMobileNumber(string mobileNumber)
        {
            logger.LogInformation($"UserRepository : CheckUserMobileNumber : Started");

            string? mobileNo = string.Empty;
            try
            {
                mobileNo = appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.PhoneNumber == mobileNumber).Result.PhoneNumber;

                logger.LogInformation($"UserRepository : GetUserById : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetUserById", ex);
                throw ex;
            }
            return mobileNo;
        }

        public async Task<List<Users>> GetRegisteredTeachers()
        {
            logger.LogInformation($"UserRepository : GetRegisteredTeachers : Started");

            List<Users> users = new List<Users>();
            try
            {
                users = await appDbContext.Users.AsNoTracking().Where(x => x.AssignedTeacherStatus.Value && x.Type == (int)Constant.Type.Teacher).ToListAsync();

                logger.LogInformation($"UserRepository : GetRegisteredTeachers : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetRegisteredTeachers", ex);
            }
            return users;
        }

        public async Task<List<Users>> GetAllRegionalAdmins()
        {
            logger.LogInformation($"UserRepository : GetAllRegionalAdmins : Started");

            List<Users> users = new List<Users>();
            try
            {
                users = await appDbContext.Users.AsNoTracking().Where(x => x.Type == (int)Constant.Type.RegionalAdmin).ToListAsync();

                logger.LogInformation($"UserRepository : GetAllRegionalAdmins : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllRegionalAdmins", ex);
                throw ex;
            }
            return users;
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
