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
    public class ClassRepository : IClassRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public ClassRepository(AppDbContext appDbContext, ILogger<ClassRepository> logger, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<Class> SaveClass(Class cls)
        {
            logger.LogInformation($"UserRepository : SaveClass : Started");

            try
            {
                if (cls.Id > 0)
                {
                    appDbContext.Entry(cls).State = EntityState.Modified;
                }
                else
                {
                    cls.StartedDate = DateTime.UtcNow;
                    cls.Status = true;

                    appDbContext.Class.Add(cls);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveStudent : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudent ", ex);
                throw ex;
            }
            return cls;
        }
     
    }
}
