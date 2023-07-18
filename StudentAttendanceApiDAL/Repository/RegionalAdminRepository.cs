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
    public class RegionalAdminRepository : IRegionalAdminRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public RegionalAdminRepository(AppDbContext appDbContext, ILogger<RegionalAdminRepository> logger,IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<List<RegionalAdmin>> GetAllRegionalAdmin()
        {
            logger.LogInformation($"MasterAdminRepository : GetAllMasterAdmin : Started");
            List<RegionalAdmin> masterAdmins = new List<RegionalAdmin>();
            try
            {
                masterAdmins = await appDbContext.RegionalAdmin.AsNoTracking().ToListAsync();
                logger.LogInformation($"MasterAdminRepository : GetAllMasterAdmin : End");
                return masterAdmins.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"MasterAdminRepository : GetAllMasterAdmin ", ex);
            }

            return masterAdmins;
        }

        public async Task<RegionalAdmin> LoginRegionalAdmin(string userName, string password)
        {
            logger.LogInformation($"UserRepository : LoginRegionalAdmin : Started");

            RegionalAdmin regionalAdmin = new RegionalAdmin();
            try
            {
                regionalAdmin = await appDbContext.RegionalAdmin.AsNoTracking().FirstOrDefaultAsync(x => x.FullName == userName && x.Password == password);
                if (regionalAdmin != null)
                {
                    ///Generate token for user
                    #region JWT
                    regionalAdmin.Token = CommonUtility.GenerateToken(configuration, regionalAdmin.Email, regionalAdmin.FullName);
                    #endregion
                }

                logger.LogInformation($"UserRepository : LoginRegionalAdmin : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : LoginRegionalAdmin ", ex);
            }
            return regionalAdmin;
        }


        public async Task<RegionalAdmin> SaveRegionalAdmin(RegionalAdmin regionalAdmin)
        {
            logger.LogInformation($"MasterAdminRepository : SaveRegionalAdmin : Started");

            try
            {
                if (regionalAdmin.Id > 0)
                {
                    appDbContext.Entry(regionalAdmin).State = EntityState.Modified;
                }
                else
                {
                    regionalAdmin.Type = 2;
                    regionalAdmin.CreatedOn = DateTime.UtcNow;
                    regionalAdmin.RegionalAdminGuidId = Guid.NewGuid();
                    appDbContext.RegionalAdmin.Add(regionalAdmin);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"MasterAdminRepository : SaveRegionalAdmin : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"MasterAdminRepository : SaveRegionalAdmin ", ex);
                throw ex;
            }
            return regionalAdmin;
        }

   
  
    }

}
