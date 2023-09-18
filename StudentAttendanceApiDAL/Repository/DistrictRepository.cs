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
    public class DistrictRepository : IDistrictRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public DistrictRepository(AppDbContext appDbContext, ILogger<DistrictRepository> logger)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
        }

        public async Task<List<District>> GetAllDistrict(int offset, int limit)
        {
            logger.LogInformation($"DistrictRepository : GetAllDistrict : Started");
            List<District> district = new List<District>();
            try
            {
                if (offset == 0 && limit == 0)
                {
                    district = await appDbContext.District.AsNoTracking().ToListAsync();
                }
                else
                {
                    district = await appDbContext.District.AsNoTracking()
                                                                       .Skip(offset)
                                                                       .Take(limit).ToListAsync();
                }
                logger.LogInformation($"DistrictRepository : GetAllDistrict : End");
                return district.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictRepository : GetAllDistrict ", ex);
            }

            return district;
        }

        public async Task<District> SaveDistrict(District district)
        {
            logger.LogInformation($"DistrictRepository : SaveDistrict : Started");

            try
            {
                if (district.Id > 0)
                {
                    appDbContext.Entry(district).State = EntityState.Modified;
                }
                else
                {
                    district.CreatedOn = DateTime.Now;
                    district.DistrictGuidId = Guid.NewGuid();
                    appDbContext.District.Add(district);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"DistrictRepository : SaveDistrict : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictRepository : SaveDistrict ", ex);
            }
            return district;
        }

        public async Task<string> CheckDistrictName(string name)
        {
            logger.LogInformation($"UserRepository : CheckDistrictName : Started");

            District district = new District();
            try
            {
                district = appDbContext.District.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name).Result;

                logger.LogInformation($"UserRepository : CheckDistrictName : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : CheckDistrictName", ex);
            }
            return district == null ? null : district.Name;
        }

    }
}
