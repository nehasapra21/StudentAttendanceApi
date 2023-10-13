using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using static StudentAttendanceApiDAL.Constant;

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
                        student.Bpl = false;
                        student.Status = studentVal.Status;
                        student.LastClass = studentVal.LastClass;
                        student.ActiveClassStatus = studentVal.ActiveClassStatus;
                        student.Counter = studentVal.Counter;
                        student.CreatedOn = DateTime.Now;
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
                if (student != null)
                {
                    //student.FatherOccupation = Enum.GetName(typeof(FatherOccupation), Convert.ToInt32(student.FatherOccupation));
                    student.FatherOccupation = student.FatherOccupation;
                    School school = appDbContext.School.Where(x => x.Id == student.SchoolId).FirstOrDefault();
                    if (school != null)
                    {
                        student.SchoolName = school.SchoolName;
                    }

                    Center center = appDbContext.Center.Where(x => x.Id == student.CenterId).FirstOrDefault();
                    if (center != null)
                    {
                        student.CenterName = center.CenterName;
                        Users user = appDbContext.Users.Where(x => x.Id == center.AssignedTeachers).FirstOrDefault();
                        if (user != null)
                        {
                            student.TeacherName = user.Name;
                        }
                    }
                }
               
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

        public async Task<Dictionary<int, int>> GetTotalStudentPresent(int userId, int type)
        {
            logger.LogInformation($"UserRepository : GetTotalStudentPresent : Started");
            Dictionary<int, int> ClassData = new Dictionary<int, int>();
            Dictionary<int, int> totalPresentStudentData = new Dictionary<int, int>();
            try
            {
                List<Student> students = null;
                List<Student> totalStudents = null;
                List<Student> presentStudents = null;
                if (userId == 0 && type == 0)
                {
                    students = await appDbContext.Student.AsNoTracking().ToListAsync();

                    totalStudents = students.Where(x => x.Status.Value).ToList();

                    presentStudents = students.Where(x => x.ActiveClassStatus.Value).ToList();

                }
                else
                {
                    List<int> centerIds = appDbContext.Center.Where(x => x.AssignedRegionalAdmin == userId).Select(x => x.Id).ToList();

                    students = await appDbContext.Student.Where(x => centerIds.Contains(x.CenterId)).AsNoTracking().ToListAsync();

                    totalStudents = students.Where(x => x.Status.Value).ToList();

                    presentStudents = students.Where(x => x.ActiveClassStatus.Value).ToList();

                }

                totalPresentStudentData.Add(presentStudents.Count(), totalStudents.Count());

                logger.LogInformation($"UserRepository : UpdateStudentActive : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : UpdateStudentActive ", ex);
                throw ex;
            }
            return totalPresentStudentData;
        }

        public async Task<Dictionary<int, int>> GetActiveClass(int userId, int type)
        {
            logger.LogInformation($"UserRepository : GetActiveClass : Started");

            Dictionary<int, int> ClassData = new Dictionary<int, int>();
            try
            {
                int classes = 0;
                int activeClasses = 0;

                if (userId == 0 && type == 0)
                {
                    classes = appDbContext.Center.AsNoTracking().ToList().Count();
                    activeClasses = appDbContext.Class.AsNoTracking().Where(x => x.Status.Value == 1 && x.StartedDate.Value.Date == DateTime.Now.Date).ToList().Count();


                }
                else
                {
                    classes = appDbContext.Center.Where(x => x.AssignedRegionalAdmin == userId).AsNoTracking().ToList().Count();
                    activeClasses = appDbContext.Class.AsNoTracking().Where(x => x.Status.Value == 1 && x.StartedDate.Value.Date == DateTime.Now.Date).ToList().Count();


                }
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

        public async Task<Dictionary<int, int>> GetTotalUpComingAndCompletedClass(int userId, int type)
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
                    List<int> allCenterIds = new List<int>();
                    if (userId == 0)
                    {
                        allCenterIds = appDbContext.Center.Select(x => x.Id).ToList();
                    }
                    else
                    {
                        allCenterIds = appDbContext.Center.Where(x => x.AssignedRegionalAdmin == userId).Select(x => x.Id).ToList();
                    }

                    completedClassesCount = classes.Where(x => x.Status.Value == (int)Constant.ClassStatus.Completed).ToList().Count();


                    List<int> centerIds = classes.Select(x => x.CenterId).ToList();//41

                    if (userId == 0 && type == 0)
                    {
                        upcomingCount = appDbContext.Center.AsNoTracking().Where(x => !centerIds.Contains
                                                (x.Id)).ToList().Count();//42
                    }
                    else
                    {
                        upcomingCount = appDbContext.Center.AsNoTracking().Where(x => !centerIds.Contains
                                              (x.Id) && x.AssignedRegionalAdmin == userId).ToList().Count();//42
                    }
                }
                else
                {
                    if (userId == 0 && type == 0)
                    {
                        upcomingCount = appDbContext.Center.AsNoTracking().ToList().Count();
                    }
                    else
                    {
                        upcomingCount = appDbContext.Center.Where(x => x.AssignedRegionalAdmin == userId).AsNoTracking().ToList().Count();
                    }

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

        public async Task<int> GetCancelClassCount(int userId, int type)
        {
            logger.LogInformation($"UserRepository : GetActiveClass : Started");
            int cancelCount = 0;
            try
            {
                if (userId == 0 && type == 0)
                {
                    cancelCount = appDbContext.ClassCancelTeacher.AsNoTracking().Where(
                        x => x.StartingDate.Value.Date <= DateTime.Now.
                        Date && x.EndingDate.Value.Date >= DateTime.Now.Date).ToList().Count();
                }
                else
                {
                    List<ClassCancelTeacher> classCancel = appDbContext.ClassCancelTeacher.AsNoTracking().Where(
                     x => x.StartingDate.Value.Date <= DateTime.Now.
                     Date && x.EndingDate.Value.Date >= DateTime.Now.Date).ToList();

                    List<int> centerIds = appDbContext.Center.Where(x => x.AssignedRegionalAdmin == userId).Select(x => x.Id).ToList();

                    cancelCount = classCancel.Where(x => centerIds.Contains(x.Id)).ToList().Count();
                }

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
