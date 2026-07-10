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
    public class VidhanSabhaRepository : IVidhanSabhaRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public VidhanSabhaRepository(AppDbContext appDbContext, ILogger<VidhanSabhaRepository> logger)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
        }

        public async Task<List<VidhanSabha>> GetAllVidhanSabha(int offset, int limit)
        {
            logger.LogInformation($"VidhanSabhaRepository : GetAllVidhanSabha : Started");
            List<VidhanSabha> vidanSabha = new List<VidhanSabha>();
            try
            {
                if (offset == 0 && limit == 0)
                {
                    vidanSabha = await (from v in appDbContext.VidhanSabha
                                        join d in appDbContext.District
                                     on v.DistrictId equals d.Id
                                        select new VidhanSabha
                                        {
                                            Id = v.Id,
                                            VidhanSabhaGuidId = v.VidhanSabhaGuidId,
                                            Name = v.Name,
                                            DistrictId = v.DistrictId,
                                            DistrictName = d.Name,
                                            CreatedOn = v.CreatedOn,
                                            CreatedBy = v.CreatedBy,
                                            Status = v.Status
                                        }).AsNoTracking().ToListAsync();
                }
                else
                {
                    vidanSabha = await (from v in appDbContext.VidhanSabha
                                        join d in appDbContext.District
                                     on v.DistrictId equals d.Id
                                        select new VidhanSabha
                                        {
                                            Id = v.Id,
                                            VidhanSabhaGuidId = v.VidhanSabhaGuidId,
                                            Name = v.Name,
                                            DistrictId = v.DistrictId,
                                            DistrictName = d.Name,
                                            CreatedOn = v.CreatedOn,
                                            CreatedBy = v.CreatedBy,
                                            Status = v.Status
                                        })
                                        .AsNoTracking()
                                        .Skip(offset)
                                        .Take(limit)
                                        .ToListAsync();
                }
               
                logger.LogInformation($"VidhanSabhaRepository : GetAllVidhanSabha : End");
                return vidanSabha.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VidhanSabhaRepository : GetAllVidhanSabha ", ex);
                throw ex;
            }

            return vidanSabha;
        }

        public async Task<VidhanSabha> SaveVidhanSabha(VidhanSabha vidhanSabha)
        {
            logger.LogInformation($"VidhanSabhaRepository : SaveVidhanSabha : Started");

            try
            {
                if (vidhanSabha.Id > 0)
                {
                    appDbContext.Entry(vidhanSabha).State = EntityState.Modified;
                }
                else
                {   
                    vidhanSabha.CreatedOn = DateTime.Now;
                    vidhanSabha.VidhanSabhaGuidId = Guid.NewGuid();
                    appDbContext.VidhanSabha.Add(vidhanSabha);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"VidhanSabhaRepository : SaveVidhanSabha : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VidhanSabhaRepository : SaveVidhanSabha ", ex);
                throw ex;
            }
            return vidhanSabha;
        }

        public async Task<VidhanSabha> GetVidhanSabhaByDistrictId(int districtId)
        {
            logger.LogInformation($"VidhanSabhaRepository : GetVidhanSabhaByDistrictId : Started");

            var vidhanSabha = await appDbContext.VidhanSabha.AsNoTracking().FirstOrDefaultAsync(x => x.DistrictId == districtId);

            logger.LogInformation($"VidhanSabhaRepository : GetVidhanSabhaByDistrictId : End");

            return vidhanSabha;
        }

        public async Task<string> CheckVidhanSabhaName(string name)
        {
            logger.LogInformation($"UserRepository : CheckVidhanSabhaName : Started");

            VidhanSabha vidhanSabha = new VidhanSabha();
            try
            {
                vidhanSabha = appDbContext.VidhanSabha.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name).Result;

                logger.LogInformation($"UserRepository : CheckVidhanSabhaName : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : CheckVidhanSabhaName", ex);
                throw ex;
            }
            return vidhanSabha == null ? null : vidhanSabha.Name;
        }
    }
}
