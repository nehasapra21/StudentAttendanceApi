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
    public class StudentRepository : IStudentRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public StudentRepository(AppDbContext appDbContext, ILogger<StudentRepository> logger, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<Student> SaveStudent(Student student)
        {
            logger.LogInformation($"UserRepository : SaveStudent : Started");

            try
            {
                if (student.Id > 0)
                {
                    appDbContext.Entry(student).State = EntityState.Modified;
                }
                else
                {
                    student.CreatedOn = DateTime.UtcNow;
                  
                    appDbContext.Student.Add(student);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveStudent : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudent ", ex);
                throw ex;
            }
            return student;
        }
     
    }
}
