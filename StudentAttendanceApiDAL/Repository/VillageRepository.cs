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
    public class VillageRepository : IVillageRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public VillageRepository(AppDbContext appDbContext, ILogger<VillageRepository> logger)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
        }

        public async Task<List<Village>> GetAllVillage()
        {
            logger.LogInformation($"VillageRepository : GetAllVillage : Started");
            List<Village> village = new List<Village>();
            try
            {
                village = await appDbContext.Village.AsNoTracking().ToListAsync();
                logger.LogInformation($"VillageRepository : GetAllVillage : End");
                return village.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageRepository : GetAllVillage ", ex);
            }

            return village;
        }


        public async Task<Village> SaveVillage(Village village)
        {
            logger.LogInformation($"VillageRepository : SaveVillage : Started");

            try
            {
                if (village.Id > 0)
                {
                    appDbContext.Entry(village).State = EntityState.Modified;
                }
                else
                {
                    village.VillageGuidId= Guid.NewGuid();
                    appDbContext.Village.Add(village);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"VillageRepository : SaveVillage : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageRepository : SaveVillage ", ex);
            }
            return village;
        }

        public async Task<Village> GetVillageByDistrictVidhanSabhaAndPanchId(int districtId, int vidhanSabhaId, int panchayatId)
        {
            logger.LogInformation($"VillageRepository : GetVillageByDistrictVidhanSabhaAndPanchId : Started");

            var village = await appDbContext.Village.AsNoTracking().FirstOrDefaultAsync(x => x.DistrictId == districtId && x.VidhanSabhaId == vidhanSabhaId && x.PanchayatId==panchayatId);

            logger.LogInformation($"VillageRepository : GetVillageByDistrictVidhanSabhaAndPanchId : End");

            return village;
        }

    }
}
