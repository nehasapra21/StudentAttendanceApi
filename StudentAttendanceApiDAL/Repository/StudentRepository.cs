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
                    var studentVal = appDbContext.Student.AsNoTracking().FirstOrDefaultAsync(x => x.Id == student.Id).Result;
                    if (student != null)
                    {
                        student.Status = studentVal.Status;
                        student.LastClass = studentVal.LastClass;
                        student.ActiveClassStatus = studentVal.ActiveClassStatus;
                        student.Counter = studentVal.Counter;
                        appDbContext.Entry(student).State = EntityState.Modified;
                    }
                }
                else
                {
                    student.Status = true;
                    student.CreatedOn = DateTime.Now;
                    student.ActiveClassStatus = false;
                    student.ManualAttendance = 0;
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
            Dictionary<int, int> ClassData = new Dictionary<int, int>();
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

        public async Task<Dictionary<int, int>> GetActiveClass()
        {
            logger.LogInformation($"UserRepository : GetActiveClass : Started");

            Dictionary<int, int> ClassData = new Dictionary<int, int>();
            try
            {
                int classes = appDbContext.Center.AsNoTracking().ToList().Count();
                int activeClasses = appDbContext.Class.AsNoTracking().Where(x=>x.Status.Value==1 && x.StartedDate.Value.Date==DateTime.Now.Date).ToList().Count();

                ClassData.Add(activeClasses, classes);

                logger.LogInformation($"UserRepository : GetAllClasses : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllClasses", ex);
                throw ex;
            }
            return ClassData;
        }

        public async Task<Dictionary<int, int>> GetTotalUpComingAndCompletedClass()
        {
            logger.LogInformation($"UserRepository : GetActiveClass : Started");

            Dictionary<int, int> ClassData = new Dictionary<int, int>();
            try
            {
                List<Class> classes = appDbContext.Class.AsNoTracking().Where(x => x.StartedDate.Value.Date == DateTime.Now.Date).ToList();
                int completedClassesCount = 0;
                int upcomingCount = 0;
                if (classes != null && classes.Count > 0)
                {
                    completedClassesCount = classes.Where(x => x.Status.Value == (int)Constant.ClassStatus.Completed).ToList().Count();
                    List<int> centerIds = classes.Select(x => x.CenterId).ToList();//41

                    upcomingCount = appDbContext.Center.AsNoTracking().Where(x => !centerIds.Contains
                                            (x.Id)).ToList().Count();//42
                }
                else
                {
                    upcomingCount = appDbContext.Center.AsNoTracking().ToList().Count();

                }



                ClassData.Add(completedClassesCount, upcomingCount);

                logger.LogInformation($"UserRepository : GetAllClasses : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllClasses", ex);
                throw ex;
            }
            return ClassData;
        }

        public async Task<int> GetCancelClassCount()
        {
            logger.LogInformation($"UserRepository : GetActiveClass : Started");
            int cancelCount = 0;
            try
            {
                cancelCount = appDbContext.ClassCancelTeacher.AsNoTracking().Where(
                    x => x.StartingDate.Value.Date <= DateTime.Now.Date && x.EndingDate.Value.Date >= DateTime.Now.Date).ToList().Count();

                logger.LogInformation($"UserRepository : GetAllClasses : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllClasses", ex);
                throw ex;
            }
            return cancelCount;
        }
    }
}
