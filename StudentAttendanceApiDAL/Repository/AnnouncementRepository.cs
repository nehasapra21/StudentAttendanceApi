using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentAttendanceApiDAL.Repository
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public AnnouncementRepository(AppDbContext appDbContext, ILogger<AnnouncementRepository> logger, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<Announcement> SaveAnnouncement(Announcement announcement)
        {
            logger.LogInformation($"UserRepository : SaveAnnouncement : Started");

            try
            {
                if (announcement.Id > 0)
                {
                    appDbContext.Entry(announcement).State = EntityState.Modified;
                }
                else
                {
                    appDbContext.Announcement.Add(announcement);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveAnnouncement : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveHolidays ", ex);
                throw ex;
            }
            return announcement;
        }

        public async Task<List<Announcement>> GetAnnouncement()
        {
            logger.LogInformation($"DistrictRepository : v : Started");
            List<Announcement> announcements = new List<Announcement>();
            try
            {

                announcements = await appDbContext.Announcement.ToListAsync();
                logger.LogInformation($"DistrictRepository : GetAnnouncement : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictRepository : GetAnnouncement ", ex);
            }

            return announcements;
        }

    }
}
