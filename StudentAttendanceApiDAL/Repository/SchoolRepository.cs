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
    public class SchoolRepository : ISchoolRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public SchoolRepository(AppDbContext appDbContext, ILogger<TeacherRepository> logger, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

  
        public async Task<School> SaveSchool(School school)
        {
            logger.LogInformation($"UserRepository : SaveSuperAdmin : Started");

            try
            {
                if (school.Id > 0)
                {
                    appDbContext.Entry(school).State = EntityState.Modified;
                }
                else
                {
                    school.CreatedOn = DateTime.Now;
                    appDbContext.School.Add(school);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveSchool : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveSuperAdmin ", ex);
            }
            return school;
        }


        public async Task<List<School>> GetAllSchools()
        {
            logger.LogInformation($"UserRepository : SaveSuperAdmin : Started");
            List<School> list = new List<School>();
            try
            {
                list = await appDbContext.School.AsNoTracking().ToListAsync();
                
                logger.LogInformation($"UserRepository : SaveSchool : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveSuperAdmin ", ex);
            }
            return list;
        }

    }
}
