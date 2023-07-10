using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace StudentAttendanceApiDAL.Repository
{
    public class MasterAdminRepository : IMasterAdminRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public MasterAdminRepository(AppDbContext appDbContext, ILogger<MasterAdminRepository> logger,IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<List<MasterAdmin>> GetAllMasterAdmin()
        {
            logger.LogInformation($"MasterAdminRepository : GetAllMasterAdmin : Started");
            List<MasterAdmin> masterAdmins = new List<MasterAdmin>();
            try
            {
                masterAdmins = await appDbContext.MasterAdmin.AsNoTracking().ToListAsync();
                logger.LogInformation($"MasterAdminRepository : GetAllMasterAdmin : End");
                return masterAdmins.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"MasterAdminRepository : GetAllMasterAdmin ", ex);
            }

            return masterAdmins;
        }


        public async Task<MasterAdmin> SaveMasterAdmin(MasterAdmin masterAdmin)
        {
            logger.LogInformation($"MasterAdminRepository : SaveMasterAdmin : Started");

            try
            {
                if (masterAdmin.Id > 0)
                {
                    appDbContext.Entry(masterAdmin).State = EntityState.Modified;
                }
                else
                {
                    masterAdmin.MasterAdminGuidId = Guid.NewGuid();
                    appDbContext.MasterAdmin.Add(masterAdmin);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"MasterAdminRepository : SaveMasterAdmin : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"MasterAdminRepository : SaveMasterAdmin ", ex);
            }
            return masterAdmin;
        }

        public async Task<MasterAdmin> LoginSuperAdmin(string userName, string password)
        {
            logger.LogInformation($"MasterAdminRepository : LoginSuperAdmin : Started");

            MasterAdmin masterAdmin = new MasterAdmin();
            try
            {
                masterAdmin = await appDbContext.MasterAdmin.AsNoTracking().FirstOrDefaultAsync(x => x.FullName == userName && x.Password == password);
                if (masterAdmin != null)
                {
                    ///Generate token for user
                    #region JWT
                   masterAdmin.Token = GenerateToken(masterAdmin);
                    #endregion
                }
                else
                {
                    return null;
                }
                logger.LogInformation($"UserRepository : LoginUser : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : LoginUser ", ex);
            }
            return masterAdmin;
        }


        public string GenerateToken(MasterAdmin masterAdmin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]));
            var credential = new SigningCredentials(securityKey,  SecurityAlgorithms.HmacSha256Signature);

            Claim[] claims;
            if (string.IsNullOrEmpty(masterAdmin.FullName))
            {
                claims = new[]
               {
                    new Claim(ClaimTypes.Email, masterAdmin.Email.ToString())
                };
            }
            else
            {
                claims = new[]
                 {
                    new Claim(ClaimTypes.NameIdentifier, masterAdmin.FullName.ToString())
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
