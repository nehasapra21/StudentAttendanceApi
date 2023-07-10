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

        public async Task<List<District>> GetAllDistrict()
        {
            logger.LogInformation($"DistrictRepository : GetAllDistrict : Started");
            List<District> district = new List<District>();
            try
            {
                district = await appDbContext.District.AsNoTracking().ToListAsync();
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
    }
}
