using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentAttendanceApiDAL.Repository
{
    public class StudentAttendanceRepository : IStudentAttendanceRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public StudentAttendanceRepository(AppDbContext appDbContext, ILogger<StudentAttendanceRepository> logger, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<int> SaveStudentAttendance(StudentAttendance studentAttendance)
        {
            logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");

            int attendance = -1;
            try
            {

                //checked student active 

                //check student centerid

                var studentAttendanceExists = appDbContext.StudentAttendance.FirstOrDefaultAsync(x => x.StudentId == studentAttendance.Id && x.ClassId == studentAttendance.ClassId
                           && x.ScanDate == DateTime.UtcNow.Date).Result;
                if (studentAttendanceExists != null)
                {
                    return attendance;
                }
                else
                {
                    Student student = appDbContext.Student.FirstOrDefaultAsync(x => x.Id == studentAttendance.StudentId).Result;

                    //mannual attendance
                    if (studentAttendance.ManualAttendance != null && studentAttendance.ManualAttendance.Value)
                    {
                        //if mannaul attendance >6
                        if (student.ManualAttendance.Value < 6)
                        {
                            UpdateScanDateAndCounter(studentAttendance, student);
                            attendance = 1;
                        }
                        else
                        {
                            attendance = 0;
                        }
                        student.ManualAttendance = student.ManualAttendance.Value + 1;
                    }
                    else
                    {
                        //Automatic attendance
                        UpdateScanDateAndCounter(studentAttendance, student);
                        attendance = 1;
                    }


                    await appDbContext.SaveChangesAsync();



                }

                //Set status of student

                logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudentAttendance ", ex);
                throw ex;
            }
            return attendance;
        }

        private void UpdateScanDateAndCounter(StudentAttendance studentAttendance, Student student)
        {
            studentAttendance.ScanDate = DateTime.UtcNow;
            studentAttendance.CenterId = student.CenterId;
            appDbContext.StudentAttendance.Add(studentAttendance);

            #region  Set student status active

            if (student != null)
            {
                int counter = 0;
                if (student.Counter == null)
                {
                    counter = 0;
                }
                else
                {
                    counter = student.Counter.Value + 1;
                }
                student.Status = true;
                student.ActiveClassStatus = true;
                student.Counter = counter;
                appDbContext.Student.Update(student);
            }

            //update avilable student in class
            var classVal = appDbContext.Class.FirstOrDefaultAsync(x => x.CenterId == student.CenterId).Result;
            if (classVal != null)
            {
                int avilCounter = 0;
                if (classVal.AvilableStudents == null)
                {
                    avilCounter = 0;
                }
                else
                {
                    avilCounter = classVal.AvilableStudents.Value + 1;
                }
                classVal.AvilableStudents = avilCounter + 1;
            }
            appDbContext.Class.Update(classVal);
            #endregion
        }


        public async Task<List<Student>> GetAllStudentWihAvgAttendance(int centerId)
        {
            logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");

            List<Student> students = new List<Student>();
            try
            {
                students = await (from s in appDbContext.Student
                                       join sa in appDbContext.StudentAttendance
                                       on s.Id equals sa.StudentId
                                       where s.CenterId == centerId
                                       group new { s, sa } by new { s.Id ,s.EnrollmentId,s.FullName,s.CreatedOn} into g
                                       select new Student
                                       {
                                           Id=g.Key.Id,
                                           EnrollmentId = g.Key.EnrollmentId,
                                           FullName = g.Key.FullName,
                                           CreatedOn = g.Key.CreatedOn,
                                           value= appDbContext.StudentAttendance.Where(x => x.StudentId ==g.Key.Id).Count(),
                                           classcount= appDbContext.Class.Where(x => x.CenterId == centerId && (x.Status == 1 || x.Status == 2)).Count(),
                                           AvgAttendance = (Convert.ToDecimal(g.Sum(x => x.sa.StudentId).Value) / appDbContext.Class.Where(x => x.CenterId == centerId && (x.Status==1|| x.Status==2)).Count()) *100
                                       }).ToListAsync();


            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudentAttendance ", ex);
                throw ex;
            }
            return students;
        }

        public async Task<List<Student>> GetAllStudentAttendancStatus(int centerId, string centerDate)
        {
            logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");

            List<Student> students = new List<Student>();
            try
            {
                students= await (from s in appDbContext.Student
                                       join sa in appDbContext.StudentAttendance
                                       on s.Id equals sa.StudentId
                                       where s.CenterId == centerId && sa.ScanDate.Value.Date == Convert.ToDateTime(centerDate).Date
                group new { s, sa } by new { s.Id, s.FullName,s.EnrollmentId } into g
                                  select new Student
                                  {
                                      Id = g.Key.Id,
                                      EnrollmentId = g.Key.EnrollmentId,
                                      FullName = g.Key.FullName,
                                      CreatedOn=g.Select(x=>x.sa.ScanDate.Value).First(),
                                      StudentStaus = g.Sum(x => x.sa.StudentId).Value == 0 ? "Absent" : "Present",
                                  }).ToListAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudentAttendance ", ex);
                throw ex;
            }
            return students;
        }


    }
}
