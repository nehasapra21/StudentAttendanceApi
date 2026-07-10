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

        public async Task<List<Village>> GetAllVillage(int offset, int limit)
        {
            logger.LogInformation($"VillageRepository : GetAllVillage : Started");
            List<Village> village = new List<Village>();
            try
            {
                if (offset == 0 && limit == 0)
                {
                    village = await (from v in appDbContext.Village
                                     join p in appDbContext.Panchayat
                                     on v.PanchayatId equals p.Id
                                     join d in appDbContext.District
                                    on v.DistrictId equals d.Id
                                     join vid in appDbContext.VidhanSabha
                                    on v.VidhanSabhaId equals vid.Id
                                     select new Village
                                     {
                                         Id = v.Id,
                                         VillageGuidId = v.VillageGuidId,
                                         Name = v.Name,
                                         DistrictId = v.DistrictId,
                                         DistrictName = d.Name,
                                         VidhanSabhaId = v.VidhanSabhaId,
                                         VidhanSabhaName = vid.Name,
                                         PanchayatId = v.PanchayatId,
                                         PanchayatName = p.Name,
                                         CreatedOn = v.CreatedOn,
                                         CreatedBy = v.CreatedBy,
                                         Status = v.Status
                                     }).AsNoTracking().ToListAsync();
                }
                else
                {
                    village = await (from v in appDbContext.Village
                                     join p in appDbContext.Panchayat
                                     on v.PanchayatId equals p.Id
                                     join d in appDbContext.District
                                    on v.DistrictId equals d.Id
                                     join vid in appDbContext.VidhanSabha
                                    on v.VidhanSabhaId equals vid.Id
                                     select new Village
                                     {
                                         Id = v.Id,
                                         VillageGuidId = v.VillageGuidId,
                                         Name = v.Name,
                                         DistrictId = v.DistrictId,
                                         DistrictName = d.Name,
                                         VidhanSabhaId = v.VidhanSabhaId,
                                         VidhanSabhaName = vid.Name,
                                         PanchayatId = v.PanchayatId,
                                         PanchayatName = p.Name,
                                         CreatedOn = v.CreatedOn,
                                         CreatedBy = v.CreatedBy,
                                         Status = v.Status
                                     })
                                    .AsNoTracking()
                                    .Skip(offset)
                                    .Take(limit)
                                    .ToListAsync();
                }
               
                logger.LogInformation($"VillageRepository : GetAllVillage : End");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageRepository : GetAllVillage ", ex);
                throw ex;
            }

            return village.ToList();
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
                    village.CreatedOn = DateTime.Now;
                    village.VillageGuidId = Guid.NewGuid();
                    appDbContext.Village.Add(village);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"VillageRepository : SaveVillage : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageRepository : SaveVillage ", ex);
                throw ex;
            }
            return village;
        }

        public async Task<Village> GetVillageByDistrictVidhanSabhaAndPanchId(int districtId, int vidhanSabhaId, int panchayatId)
        {
            logger.LogInformation($"VillageRepository : GetVillageByDistrictVidhanSabhaAndPanchId : Started");

            var village = await appDbContext.Village.AsNoTracking().FirstOrDefaultAsync(x => x.DistrictId == districtId && x.VidhanSabhaId == vidhanSabhaId && x.PanchayatId == panchayatId);

            logger.LogInformation($"VillageRepository : GetVillageByDistrictVidhanSabhaAndPanchId : End");

            return village;
        }

        public async Task<string> CheckVillageName(string name)
        {
            logger.LogInformation($"UserRepository : CheckVillageName : Started");

            Village village = new Village();
            try
            {
                village = appDbContext.Village.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name).Result;

                logger.LogInformation($"UserRepository : CheckVillageName : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : CheckVillageName", ex);
                throw ex;
            }
            return village == null ? null : village.Name;
        }

    }
}
