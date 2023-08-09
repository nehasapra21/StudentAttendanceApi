using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System.Data;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
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

        public async Task<int> SaveManualStudentAttendance(StudentAttendance studentAttendance)
        {
            logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");

            int attendance = -1;//Student attendance already exists
                                //attedance =1  student attendance already exists
                                //attedance =0  Manual attendance already exists with 6 times
                                //attedance =-2  Manual attendance already exists with 6 times

            try
            {
                if (studentAttendance.ListOfStudentIds != null && studentAttendance.ListOfStudentIds.Count > 1)
                {
                    foreach (var item in studentAttendance.ListOfStudentIds)
                    {
                        studentAttendance.StudentId = item;
                        var studentAttendanceExists = appDbContext.StudentAttendance.FirstOrDefaultAsync(x => x.StudentId == item && x.ClassId == studentAttendance.ClassId
                           && x.ScanDate.Value.Date == DateTime.Now.Date).Result;
                        if (studentAttendanceExists != null)
                        {
                            continue;
                        }
                        else
                        {
                            Student student = appDbContext.Student.FirstOrDefaultAsync(x => x.Id == item).Result;

                            //mannual attendance

                            //if mannaul attendance >6
                            if (student.ManualAttendance != null && student.ManualAttendance.Value < 6)
                            {
                                UpdateScanDateAndCounter(studentAttendance, student, false);
                                if (student.ManualAttendance == null || student.ManualAttendance == 0)
                                {
                                    student.ManualAttendance = 1;
                                }
                                else
                                {
                                    student.ManualAttendance = student.ManualAttendance + 1;
                                }

                                attendance = 1;

                                await appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                attendance = 0;
                            }

                        }

                    }
                }
                else
                {
                    studentAttendance.StudentId = studentAttendance.ListOfStudentIds[0];
                    //student attendance exists in same date
                    var studentAttendanceExists = appDbContext.StudentAttendance.FirstOrDefaultAsync(x => x.StudentId == studentAttendance.StudentId && x.ClassId == studentAttendance.ClassId
                       && x.ScanDate.Value.Date == DateTime.Now.Date).Result;
                    if (studentAttendanceExists != null)
                    {
                        return attendance;//set -1
                    }
                    else
                    {
                        Student student = appDbContext.Student.FirstOrDefaultAsync(x => x.Id == studentAttendance.StudentId).Result;

                        //mannual attendance

                        //if mannaul attendance >6
                        if (student.ManualAttendance != null && student.ManualAttendance.Value < 6)
                        {
                            UpdateScanDateAndCounter(studentAttendance, student, false);
                            if (student.ManualAttendance == null || student.ManualAttendance == 0)
                            {
                                student.ManualAttendance = 1;
                            }
                            else
                            {
                                student.ManualAttendance = student.ManualAttendance + 1;
                            }

                            attendance = 1;
                            await appDbContext.SaveChangesAsync();
                        }
                        else
                        {
                            attendance = 0;
                        }

                    }
                }

                logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudentAttendance ", ex);
                throw ex;
            }
            return attendance;
        }


        public async Task<int> SaveAtuomaticStudentAttendance(StudentAttendance studentAttendance)
        {
            logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");

            int attendance = -1;//Student attendance already exists
                                //attedance =-1  student attendance already exists
                                //attedance =0  student inactive
                                //attedance =1  Student attendance applied
                                //attedance =-2  student not exists in center

            try
            {
                Student student = null;
                studentAttendance.StudentId = studentAttendance.ListOfStudentIds[0];
                //Check student active
                student = await appDbContext.Student.FirstOrDefaultAsync(x => x.Id == studentAttendance.StudentId && x.Status.Value);
                if (student == null)
                {
                    attendance = 0;
                    return attendance;
                }

                //Check student exist in center
                student = await appDbContext.Student.FirstOrDefaultAsync(x => x.Id == studentAttendance.StudentId && x.CenterId == studentAttendance.CenterId);
                if (student == null)
                {
                    attendance = -2;
                    return attendance;
                }

                //student attendance exists in same date
                var studentAttendanceExists = appDbContext.StudentAttendance.FirstOrDefaultAsync(x => x.StudentId == studentAttendance.StudentId && x.ClassId == studentAttendance.ClassId
                   && x.ScanDate.Value.Date == DateTime.Now.Date).Result;
                if (studentAttendanceExists != null)
                {
                    return attendance;//set -1
                }
                else
                {
                    student = appDbContext.Student.FirstOrDefaultAsync(x => x.Id == studentAttendance.StudentId).Result;

                    UpdateScanDateAndCounter(studentAttendance, student, true);
                    attendance = 1;

                    await appDbContext.SaveChangesAsync();
                    // }
                }


                logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudentAttendance ", ex);
                throw ex;
            }
            return attendance;
        }

        public async Task<int> SaveStudentAttendance(StudentAttendance studentAttendance)
        {
            logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");

            int attendance = -1;//Student attendance already exists
                                //attedance =-1  student attendance already exists
                                //attedance =0  student inactive
                                //attedance =1  Student attendance applied
                                //attedance =-2  student not exists in center

            try
            {
                Student student = null;
                studentAttendance.StudentId = studentAttendance.ListOfStudentIds[0];
                //Check student active
                student = await appDbContext.Student.FirstOrDefaultAsync(x => x.Id == studentAttendance.StudentId && x.Status.Value);
                if (student == null)
                {
                    attendance = 0;
                    return attendance;
                }

                //Check student exist in center
                student = await appDbContext.Student.FirstOrDefaultAsync(x => x.Id == studentAttendance.StudentId && x.CenterId == studentAttendance.CenterId);
                if (student == null)
                {
                    attendance = -2;
                    return attendance;
                }

                //student attendance exists in same date
                var studentAttendanceExists = appDbContext.StudentAttendance.FirstOrDefaultAsync(x => x.StudentId == studentAttendance.StudentId && x.ClassId == studentAttendance.ClassId
                   && x.ScanDate.Value.Date == studentAttendance.ScanDate.Value.Date).Result;
                if (studentAttendanceExists != null)
                {
                    return attendance;//set -1
                }
                else
                {
                    student = appDbContext.Student.FirstOrDefaultAsync(x => x.Id == studentAttendance.StudentId).Result;

                    UpdateScanDateAndCounter1(studentAttendance, student, true);
                    attendance = 1;

                    await appDbContext.SaveChangesAsync();
                    // }
                }


                logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudentAttendance ", ex);
                throw ex;
            }
            return attendance;
        }

        private void UpdateScanDateAndCounter1(StudentAttendance studentAttendance, Student student, bool isAutomatic)
        {
            studentAttendance.ScanDate = studentAttendance.ScanDate;
            studentAttendance.CenterId = student.CenterId;
            studentAttendance.Id = 0;
            if (isAutomatic)
            {
                studentAttendance.Type = false;
            }
            else
            {
                studentAttendance.Type = true;
            }
            appDbContext.StudentAttendance.Add(studentAttendance);

            #region  Set student ActiveClassStatus active

            if (student != null)
            {
                //int counter = 0;
                //if (student.Counter == null)
                //{
                //    counter = 0;
                //}
                //else
                //{
                //    counter = student.Counter.Value + 1;
                //}
                student.ActiveClassStatus = true;
                //student.Counter = counter;
                appDbContext.Student.Update(student);
            }

            //update avilable student in class
            var classVal = appDbContext.Class.FirstOrDefaultAsync(x => x.CenterId == student.CenterId).Result;
            if (classVal != null)
            {
                int avilCounter = 0;
                if (classVal.AvilableStudents == 0 || classVal.AvilableStudents == null)
                {
                    avilCounter = 1;
                }
                else
                {
                    avilCounter = classVal.AvilableStudents.Value + 1;
                }
                classVal.AvilableStudents = avilCounter;
            }
            appDbContext.Class.Update(classVal);
            #endregion
        }
        private void UpdateScanDateAndCounter(StudentAttendance studentAttendance, Student student, bool isAutomatic)
        {
            studentAttendance.ScanDate = DateTime.Now;
            studentAttendance.CenterId = student.CenterId;
            studentAttendance.Id = 0;
            if (isAutomatic)
            {
                studentAttendance.Type = false;
            }
            else
            {
                studentAttendance.Type = true;
            }
            appDbContext.StudentAttendance.Add(studentAttendance);

            #region  Set student ActiveClassStatus active

            if (student != null)
            {
                student.ActiveClassStatus = true;
                appDbContext.Student.Update(student);
            }

            //update avilable student in class
            var classVal = appDbContext.Class.FirstOrDefaultAsync(x => x.CenterId == student.CenterId).Result;
            if (classVal != null)
            {
                int avilCounter = 0;
                if (classVal.AvilableStudents == 0 || classVal.AvilableStudents == null)
                {
                    avilCounter = 1;
                }
                else
                {
                    avilCounter = classVal.AvilableStudents.Value + 1;
                }
                classVal.AvilableStudents = avilCounter;

                appDbContext.Class.Update(classVal);
            }
            #endregion
        }


        public async Task<List<Student>> GetAllStudentWihAvgAttendance(int centerId)
        {
            logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");

            List<Student> students = new List<Student>();
            try
            {
                //students = await (from s in appDbContext.Student
                //                  join sa in appDbContext.StudentAttendance
                //                  on s.Id equals sa.StudentId
                //                  where s.CenterId == centerId
                //                  group new { s, sa } by new { s.Id, s.EnrollmentId, s.FullName, s.CreatedOn } into g
                //                  select new Student
                //                  {
                //                      Id = g.Key.Id,
                //                      EnrollmentId = g.Key.EnrollmentId,
                //                      FullName = g.Key.FullName,
                //                      CreatedOn = g.Key.CreatedOn,
                //                      AvgAttendance = Convert.ToDecimal(appDbContext.StudentAttendance.Where(x => x.StudentId == g.Key.Id).Count() * 100 / appDbContext.Class.Where(x => x.CenterId == centerId && (x.Status == 1 || x.Status == 2)).Count())
                //                  }).ToListAsync();


                students = await (from s in appDbContext.Student //Left Data Source
                                  join sa in appDbContext.StudentAttendance //Right Data Source
                                  on s.Id equals sa.StudentId//Inner Join Condition
                                  into EmployeeAddressGroup //Performing LINQ Group Join
                                  from saa in EmployeeAddressGroup.DefaultIfEmpty()
                                  where s.CenterId == centerId
                                  group new { s, saa } by new { s.Id, s.EnrollmentId, s.FullName, s.CreatedOn } into g
                                  select new Student
                                  {
                                      Id = g.Key.Id,
                                      FullName = g.Key.FullName,
                                      EnrollmentId = g.Key.EnrollmentId,
                                      CreatedOn = g.Key.CreatedOn,
                                      AvgAttendance = Convert.ToDecimal(appDbContext.StudentAttendance.Where(x => x.StudentId == g.Key.Id).Count() * 100 / appDbContext.Class.Where(x => x.CenterId == centerId && (x.Status == 1 || x.Status == 2)).Count())
                                  }).Distinct().ToListAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudentAttendance ", ex);
                throw ex;
            }
            return students;
        }

        public async Task<List<Student>> GetAllAbsentAttendance(int centerId)
        {
            logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");

            List<Student> students = new List<Student>();
            try
            {
                //List<StudentAttendance> studentAttendance = appDbContext.StudentAttendance.Where(x => x.CenterId == centerId).ToList();
                // List<int> studentIds = studentAttendance.Select(x => x.StudentId.Value).ToList();

                students = appDbContext.Student.AsNoTracking().Where(x => x.CenterId == centerId).ToList();
                List<int> studentIds = students.Select(x => x.Id).ToList();//all student ids

                List<StudentAttendance> studentAttendance = await appDbContext.StudentAttendance.Where(x => x.CenterId == centerId).ToListAsync(); //student id 
                if (studentAttendance != null)
                {
                    List<int> studentofHavingAttendIds = studentAttendance.Select(x => x.StudentId.Value).ToList(); students = students.Where(x => !studentofHavingAttendIds.Contains(x.Id)).ToList();
                }
                else
                {
                    return students;
                }


            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudentAttendance ", ex);
                throw ex;
            }
            return students;
        }


        public async Task<List<Student>> GetAllStudentAttendancStatus(int centerId, string scanDate)
        {
            logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");

            List<Student> students = new List<Student>();
            try
            {


                students = await (from s in appDbContext.Student //Left Data Source
                                  join sa in appDbContext.StudentAttendance //Right Data Source
                                  on s.Id equals sa.StudentId//Inner Join Condition
                                  into EmployeeAddressGroup //Performing LINQ Group Join
                                  from saa in EmployeeAddressGroup.DefaultIfEmpty()
                                  where s.CenterId == centerId
                                  group new { s, saa } by new { s.Id, s.FullName, s.EnrollmentId } into g
                                  select new Student
                                  {
                                      Id = g.Key.Id,
                                      FullName = g.Key.FullName,
                                      EnrollmentId = g.Key.EnrollmentId,
                                      StudentStaus = appDbContext.StudentAttendance.Where(x => x.ScanDate.Value.Date == Convert.ToDateTime(scanDate).Date && x.StudentId == g.Key.Id).ToList().Count > 0 ? "Present" : "Absent"
                                  }).Distinct().ToListAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudentAttendance ", ex);
                throw ex;
            }
            return students;
        }

        public async Task<List<Student>> GetAllStudentAttendancByMonth(int centerId, int studentId, int month, int year)
        {
            logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");

            List<Student> students = new List<Student>();
            DataSet ds = new DataSet();

            try
            {
                var targetYear = year;
                var targetMonth =month; // August

                var startDate = new DateTime(targetYear, targetMonth, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var attendanceRecords = appDbContext.StudentAttendance
                    .Where(sa => sa.ScanDate >= startDate && sa.ScanDate <= endDate)
                    .ToList();

                var studentRecord = appDbContext.Student.Where(x => x.Id == studentId && x.Status.Value && x.CenterId == centerId);

                List<Student> attendanceReport = new List<Student>();

                foreach (var student in studentRecord)
                {
                    for (var currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
                    {
                        var attendanceStatus = attendanceRecords
                            .Any(sa => sa.StudentId == student.Id && sa.ScanDate.Value.Date == currentDate.Date)
                            ? "Present"
                            : "Absent";

                        attendanceReport.Add(new Student
                        {
                            Id = studentId,
                            FullName = student.FullName,
                            CreatedOn = currentDate,
                            StudentStaus = attendanceStatus
                        });
                    }
                }

                return attendanceReport;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudentAttendance ", ex);
                throw ex;
            }
        }

        public SqlCommand CreateSqlCommand(string spName, SqlConnection sqlConnection)
        {
            sqlConnection.Open();

            var sqlCommand = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.StoredProcedure,
                CommandText = spName

            };

            return sqlCommand;
        }
    }
}
