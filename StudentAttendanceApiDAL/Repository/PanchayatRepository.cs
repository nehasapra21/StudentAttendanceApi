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

        public async Task<List<Panchayat>> GetAllPanchayat(int offset, int limit)
        {
            logger.LogInformation($"PanchayatRepository : GetAllVidhanSabha : Started");
            List<Panchayat> panchayat = new List<Panchayat>();
            try
            {
                if (offset == 0 && limit == 0)
                {
                    panchayat = await (from p in appDbContext.Panchayat
                                       join v in appDbContext.VidhanSabha
                                    on p.VidhanSabhaId equals v.Id
                                       join d in appDbContext.District
                                       on p.DistrictId equals d.Id
                                       select new Panchayat
                                       {
                                           Id = p.Id,
                                           PanchayatGuidId = p.PanchayatGuidId,
                                           Name = p.Name,
                                           DistrictId = p.DistrictId,
                                           DistrictName = d.Name,
                                           VidhanSabhaId = p.VidhanSabhaId,
                                           VidhanSabhaName = v.Name,
                                           CreatedOn = p.CreatedOn,
                                           CreatedBy = p.CreatedBy,
                                           Status = p.Status
                                       }).AsNoTracking().ToListAsync();
                }
                else
                {
                    panchayat = await (from p in appDbContext.Panchayat
                                       join v in appDbContext.VidhanSabha
                                    on p.VidhanSabhaId equals v.Id
                                       join d in appDbContext.District
                                       on p.DistrictId equals d.Id
                                       select new Panchayat
                                       {
                                           Id = p.Id,
                                           PanchayatGuidId = p.PanchayatGuidId,
                                           Name = p.Name,
                                           DistrictId = p.DistrictId,
                                           DistrictName = d.Name,
                                           VidhanSabhaId = p.VidhanSabhaId,
                                           VidhanSabhaName = v.Name,
                                           CreatedOn = p.CreatedOn,
                                           CreatedBy = p.CreatedBy,
                                           Status = p.Status
                                       })
                                       .AsNoTracking()
                                       .Skip(offset)
                                       .Take(limit)
                                       .ToListAsync();

                }
                logger.LogInformation($"PanchayatRepository : GetAllVidhanSabha : End");
                return panchayat.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"PanchayatRepository : GetAllDistrict ", ex);
                throw ex;
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
                    panchayat.CreatedOn = DateTime.Now;
                    panchayat.PanchayatGuidId = Guid.NewGuid();
                    appDbContext.Panchayat.Add(panchayat);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"PanchayatRepository : SavePanchayat : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"PanchayatRepository : SavePanchayat ", ex);
                throw ex;
            }
            return panchayat;
        }

        public async Task<Panchayat> GetPanchayatByDistrictAndVidhanSabhaId(int districtId, int vidhanSabhaId)
        {
            logger.LogInformation($"VidhanSabhaRepository : GetPanchayatByDistrictAndVidhanSabhaId : Started");

            var panchayat = await appDbContext.Panchayat.AsNoTracking().FirstOrDefaultAsync(x => x.DistrictId == districtId && x.VidhanSabhaId == vidhanSabhaId);

            logger.LogInformation($"VidhanSabhaRepository : GetPanchayatByDistrictAndVidhanSabhaId : End");

            return panchayat;
        }

        public async Task<string> CheckPanchayatName(string name)
        {
            logger.LogInformation($"UserRepository : CheckPanchayatName : Started");

            Panchayat panchayat = new Panchayat();
            try
            {
                panchayat = appDbContext.Panchayat.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name).Result;

                logger.LogInformation($"UserRepository : CheckPanchayatName : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : CheckPanchayatName", ex);
                throw ex;
            }
            return panchayat == null ? null : panchayat.Name;
        }

    }
}
