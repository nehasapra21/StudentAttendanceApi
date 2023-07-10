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

        public async Task<List<VidhanSabha>> GetAllVidhanSabha()
        {
            logger.LogInformation($"VidhanSabhaRepository : GetAllVidhanSabha : Started");
            List<VidhanSabha> vidanSabha = new List<VidhanSabha>();
            try
            {
                vidanSabha = await appDbContext.VidhanSabha.AsNoTracking().ToListAsync();
                logger.LogInformation($"VidhanSabhaRepository : GetAllVidhanSabha : End");
                return vidanSabha.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VidhanSabhaRepository : GetAllVidhanSabha ", ex);
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
                    vidhanSabha.VidhanSabhaGuidId = Guid.NewGuid();
                    appDbContext.VidhanSabha.Add(vidhanSabha);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"VidhanSabhaRepository : SaveVidhanSabha : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VidhanSabhaRepository : SaveVidhanSabha ", ex);
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
    }
}
