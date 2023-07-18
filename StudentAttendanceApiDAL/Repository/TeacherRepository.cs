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
    public class TeacherRepository : ITeacherRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public TeacherRepository(AppDbContext appDbContext, ILogger<TeacherRepository> logger, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<Teacher> LoginTeacher(string userName, string password)
        {
            logger.LogInformation($"UserRepository : LoginSuperAdmin : Started");

            Teacher teacher= new Teacher();
            try
            {
                teacher = await appDbContext.Teacher.AsNoTracking().FirstOrDefaultAsync(x => x.FullName == userName && x.Password == password);
                if (teacher != null)
                {
                    teacher.Password = password;
                    ///Generate token for user
                    #region JWT
                    teacher.Token = CommonUtility.GenerateToken(configuration,teacher.Email,teacher.FullName);
                    #endregion
                }
                
                logger.LogInformation($"UserRepository : LoginUser : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : LoginUser ", ex);
            }
            return teacher;
        }


        public async Task<Teacher> SaveTeacher(Teacher teacher)
        {
            logger.LogInformation($"UserRepository : SaveSuperAdmin : Started");

            try
            {
                if (teacher.Id > 0)
                {
                    appDbContext.Entry(teacher).State = EntityState.Modified;
                }
                else
                {
                    teacher.CreatedOn = DateTime.UtcNow;
                    teacher.TeacherGuidId = Guid.NewGuid();
                    appDbContext.Teacher.Add(teacher);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveSuperAdmin : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveSuperAdmin ", ex);
            }
            return teacher;
        }
     
    }
}
