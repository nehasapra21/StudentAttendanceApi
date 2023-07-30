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
                    student.Status = true;
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

        public async Task<Student> GetStudentById(int id)
        {
            logger.LogInformation($"UserRepository : GetStudentById : Started");

            Student student = new Student();
            try
            {
                student = await appDbContext.Student.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                logger.LogInformation($"UserRepository : GetStudentById : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetStudentById", ex);
                throw ex;
            }
            return student;
        }

        public async Task<Student> GetStudentByCenterId(int centerId)
        {
            logger.LogInformation($"UserRepository : GetStudentCenterById : Started");

            Student student = new Student();
            try
            {
                student = await appDbContext.Student.AsNoTracking().FirstOrDefaultAsync(x => x.CenterId == centerId);

                logger.LogInformation($"UserRepository : GetStudentCenterById : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetStudentCenterById", ex);
                throw ex;
            }
            return student;
        }

        public async Task<Student> UpdateStudentActiveOrInactive(int id, int status)
        {
            logger.LogInformation($"UserRepository : UpdateStudentActive : Started");

            Student studentVal = new Student();
            try
            {
                if (id > 0)
                {
                    studentVal = appDbContext.Student.FirstOrDefaultAsync(x => x.Id == id).Result;
                    if (studentVal != null)
                    {
                        studentVal.Status = status == 1 ? true : false;
                        appDbContext.Student.Update(studentVal);
                    }
                }

                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : UpdateStudentActive : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : UpdateStudentActive ", ex);
                throw ex;
            }
            return studentVal;
        }

        public async Task<Dictionary<int, int>> GetTotalStudentPresent()
        {
            logger.LogInformation($"UserRepository : GetTotalStudentPresent : Started");

            Dictionary<int, int> totalPresentStudentData = new Dictionary<int, int>();
            try
            {
                List<Student> students = await appDbContext.Student.AsNoTracking().ToListAsync();
                List<Student> totalStudents = students.Where(x => x.Status.Value).ToList();

                List<Student> presentStudents = students.Where(x => x.ActiveClassStatus.Value).ToList();

                totalPresentStudentData.Add(presentStudents.Count(), totalStudents.Count());
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : UpdateStudentActive : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : UpdateStudentActive ", ex);
                throw ex;
            }
            return totalPresentStudentData;
        }
    }
}
