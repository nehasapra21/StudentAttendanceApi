using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.Repository
{
    public class MasterAdminRepository : IMasterAdminRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public MasterAdminRepository(AppDbContext appDbContext, ILogger<MasterAdminRepository> logger)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
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

    }
}
