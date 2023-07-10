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
    public class PanchayatRepository : IPanchayatRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public PanchayatRepository(AppDbContext appDbContext, ILogger<PanchayatRepository> logger)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
        }

        public async Task<List<Panchayat>> GetAllPanchayat()
        {
            logger.LogInformation($"PanchayatRepository : GetAllVidhanSabha : Started");
            List<Panchayat> panchayat = new List<Panchayat>();
            try
            {
                panchayat = await appDbContext.Panchayat.AsNoTracking().ToListAsync();
                logger.LogInformation($"PanchayatRepository : GetAllVidhanSabha : End");
                return panchayat.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"PanchayatRepository : GetAllDistrict ", ex);
            }

            return panchayat;
        }

        public async Task<Panchayat> SavePanchayat(Panchayat panchayat)
        {
            logger.LogInformation($"PanchayatRepository : SavePanchayat : Started");

            try
            {
                if (panchayat.Id > 0)
                {
                    appDbContext.Entry(panchayat).State = EntityState.Modified;
                }
                else
                {
                    panchayat.PanchayatGuidId = Guid.NewGuid();
                    appDbContext.Panchayat.Add(panchayat);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"PanchayatRepository : SavePanchayat : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"PanchayatRepository : SavePanchayat ", ex);
            }
            return panchayat;
        }

        public async Task<Panchayat> GetPanchayatByDistrictAndVidhanSabhaId(int districtId,int vidhanSabhaId)
        {
            logger.LogInformation($"VidhanSabhaRepository : GetPanchayatByDistrictAndVidhanSabhaId : Started");

            var panchayat = await appDbContext.Panchayat.AsNoTracking().FirstOrDefaultAsync(x => x.DistrictId == districtId && x.VidhanSabhaId== vidhanSabhaId);

            logger.LogInformation($"VidhanSabhaRepository : GetPanchayatByDistrictAndVidhanSabhaId : End");

            return panchayat;
        }

    }
}
