using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Cms;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System.Data;
using System.Text.Json.Nodes;
using static StudentAttendanceApiDAL.Constant;

namespace StudentAttendanceApiDAL.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public DashboardRepository(AppDbContext appDbContext, ILogger<DashboardRepository> logger)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
        }
        public async Task<string> GetClassCountByMonth(int centerId, int month)
        {
            logger.LogInformation($"DashboardRepository : GetTotalGenderRatioByCenterId : Started");
            dynamic rootJsonObject = new JObject();
            rootJsonObject.Data = new JArray() as dynamic;
            dynamic student = new JObject();
            try
            {
                List<Class> classCount = await appDbContext.Class.Where(x => x.CenterId == centerId).ToListAsync();

                List<Holidays> HolidayCount = await appDbContext.Holidays.Where(x => x.CenterId == centerId && (x.StartDate.Value.Date <= DateTime.Now.Date && x.EndDate.Value.Date >= DateTime.Now.Date)).ToListAsync();

                List<ClassCancelTeacher> classCancelTeacherCount = await appDbContext.ClassCancelTeacher.AsNoTracking().Where(x => x.CenterId == centerId && (x.StartingDate.Value.Date <= DateTime.Now.Date && x.EndingDate.Value.Date >= DateTime.Now.Date)).ToListAsync();

                int holidayCount = HolidayCount.Count;
                int classCountValue = classCount.Count;
                int classCancelTeacherCountVal = classCancelTeacherCount.Count;

                rootJsonObject.Status = true;

                student = new JObject();
                student.HolidayCount = holidayCount;
                student.ClassCount = classCountValue;
                student.ClassCancelTeacherCount = classCancelTeacherCountVal;

                rootJsonObject.Data.Add(student);

                logger.LogInformation($"DashboardRepository : GetTotalGenderRatioByCenterId : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DashboardRepository : GetTotalGenderRatioByCenterId ", ex);
            }

            return JsonConvert.SerializeObject(rootJsonObject);
        }

        public async Task<string> GetTotalGenderRatioByCenterId(int centerId)
        {
            logger.LogInformation($"DashboardRepository : GetClassCountByMonth : Started");
            dynamic rootJsonObject = new JObject();
            rootJsonObject.Data = new JArray() as dynamic;
            dynamic student = new JObject();
            try
            {

                List<Student> TotalStudents = appDbContext.Student.Where(x => x.CenterId == centerId).ToList();

                int TotalStudentCount = TotalStudents.Count();
                int FeMaleCount = TotalStudents.Where(x => x.Gender == "FeMale").ToList().Count();
                int MaleCount = TotalStudents.Where(x => x.Gender == "Male").ToList().Count();


                rootJsonObject.Status = true;

                student = new JObject();
                student.FeMaleCount = FeMaleCount;
                student.MaleCount = MaleCount;
                student.TotalStudentCount = TotalStudentCount;
                rootJsonObject.Data.Add(student);

                logger.LogInformation($"DashboardRepository : GetClassCountByMonth : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DashboardRepository : GetClassCountByMonth ", ex);
            }

            return JsonConvert.SerializeObject(rootJsonObject);
        }

        public async Task<string> GetTotalStudentOfClass(int centerId)
        {
            logger.LogInformation($"DashboardRepository : GetTotalStudentOfClass : Started");
            dynamic rootJsonObject = new JObject();
            dynamic student = new JObject();
            rootJsonObject.Data = new JArray() as dynamic;
            try
            {
                var students = await (from s in appDbContext.Student //Left Data Source
                                      where s.CenterId == centerId
                                      group s by new { s.Grade } into g
                                      orderby g.Key.Grade
                                      select new
                                      {
                                          Grade = g.Key.Grade,
                                          Total = g.Count(),
                                          FeMaleCount = g.Where(x => x.Gender == "FeMale").Count(),
                                          MaleCount = g.Where(x => x.Gender == "Male").Count(),
                                      }).ToListAsync();

                rootJsonObject.Status = true;
                rootJsonObject.TotalStudents = appDbContext.Student.Where(x => x.CenterId == centerId).Count();

                foreach (var item in students)
                {
                    student = new JObject();
                    student.Grade = item.Grade;
                    student.FeMaleCount = item.FeMaleCount;
                    student.MaleCount = item.MaleCount;
                    student.TotalStudentCount = item.Total;
                    rootJsonObject.Data.Add(student);
                }

                logger.LogInformation($"DashboardRepository : GetTotalStudentOfClass : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DashboardRepository : GetTotalStudentOfClass ", ex);
            }

            return JsonConvert.SerializeObject(rootJsonObject);
        }

        public async Task<string> GetCenterDetailByMonth(int centerId, int month, int year)
        {
            logger.LogInformation($"UserRepository : SaveStudentAttendance : Started");

            List<Student> students = new List<Student>();
            DataSet ds = new DataSet();
            dynamic rootJsonObject = new JObject();
            rootJsonObject.Data = new JArray() as dynamic;

            dynamic data = new JObject();
            try
            {
                var targetYear = year;
                var targetMonth = month; // August

                var startDate = new DateTime(targetYear, targetMonth, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);


                for (var currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
                {
                    data = new JObject();
                    data.Date = currentDate.Date;

                    var classVal = appDbContext.Class.Where(x => x.CenterId == centerId && x.StartedDate.Value.Date == currentDate.Date).FirstOrDefault();

                    var holidayDetail = appDbContext.Holidays.Where(x => x.CenterId == centerId && x.StartDate.Value.Date <= currentDate.Date && x.EndDate.Value.Date >= currentDate.Date).FirstOrDefault();

                    var classCancelByTeacher = appDbContext.ClassCancelTeacher.Where(x => x.CenterId == centerId && x.StartingDate.Value.Date <= currentDate.Date && x.EndingDate.Value.Date >= currentDate.Date).FirstOrDefault();

                    if (classVal != null)
                    {
                        data.Class = classVal.Name;
                        data.ClassTotalStudents = classVal.TotalStudents;
                        data.SubStatus = classVal.SubStatus;
                        data.Id = classVal.Id;
                        data.StartedDate = classVal.StartedDate;
                        data.EndDate = classVal.EndDate;
                        data.Type = 1;
                    }
                    else if (holidayDetail != null)
                    {
                        data.holidayName = holidayDetail.Name;
                        data.StartDate = holidayDetail.StartDate;
                        data.EndDate = holidayDetail.EndDate;
                        data.Type = 2;
                    }
                    else if (classCancelByTeacher != null)
                    {
                        data.classCancelByTeacher = classCancelByTeacher.Reason;
                        data.StartingDate = classCancelByTeacher.StartingDate;
                        data.EndingDate = classCancelByTeacher.EndingDate;
                        data.Type = 3;
                    }
                    else
                    {
                        data.CenterStatus = "Deactivate";
                        data.Type = 4;
                    }

                    rootJsonObject.Data.Add(data);
                }


                return JsonConvert.SerializeObject(rootJsonObject);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudentAttendance ", ex);
                throw ex;
            }
        }

        public async Task<string> GetTotalBpl(int centerId, bool BplValue)
        {
            logger.LogInformation($"DashboardRepository : GetTotalBpl : Started");
            dynamic rootJsonObject = new JObject();
            dynamic student = new JObject();
            rootJsonObject.Data = new JArray() as dynamic;
            try
            {

                var students = await appDbContext.Student.Where(x => x.CenterId == centerId && x.Bpl == BplValue).ToListAsync();

                var studentFeMale = await appDbContext.Student.Where(x => x.CenterId == centerId && x.Bpl == BplValue && x.Gender == "FeMale").ToListAsync();

                var studentMale = await appDbContext.Student.Where(x => x.CenterId == centerId && x.Bpl == BplValue && x.Gender == "Male").ToListAsync();

                int maleCount = studentMale.ToList().Count();
                int feMaleCount = studentFeMale.ToList().Count();

                rootJsonObject.Status = true;
                rootJsonObject.TotalStudents = appDbContext.Student.Where(x => x.CenterId == centerId).Count();

                student = new JObject();
                student.FeMaleCount = feMaleCount;
                student.MaleCount = maleCount;
                student.TotalBplStudents = students.ToList().Count();
                rootJsonObject.Data.Add(student);

                logger.LogInformation($"DashboardRepository : GetTotalStudentOfClass : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DashboardRepository : GetTotalStudentOfClass ", ex);
            }

            return JsonConvert.SerializeObject(rootJsonObject);
        }

        public async Task<string> GetTotalStudentCategoryOfClass(int centerId)
        {
            logger.LogInformation($"DashboardRepository : GetTotalStudentOfClass : Started");
            dynamic rootJsonObject = new JObject();
            dynamic jobject = new JObject();
            rootJsonObject.Data = new JArray() as dynamic;
            try
            {
                List<Student> students = await (from s in appDbContext.Student //Left Data Source
                                                where s.CenterId == centerId
                                                group s by new { s.Category } into g
                                                orderby g.Key.Category
                                                select new Student
                                                {
                                                    Category = g.Key.Category,
                                                    TotalStudentCount = g.Count()
                                                }).ToListAsync();

                rootJsonObject.Status = true;
                rootJsonObject.TotalStudents = appDbContext.Student.Where(x => x.CenterId == centerId).Count();

                var myList = new List<string> { "General", "OBC", "SC", "EWS", "Others" };
                Student studentData = new Student();

                foreach (var handler in myList)
                {
                    switch (handler)
                    {
                        case "General":
                            // do apple stuff
                            jobject = AddStudentForCategory(students, "General");
                            rootJsonObject.Data.Add(jobject);
                            break;
                        case "OBC":
                            jobject = AddStudentForCategory(students, "OBC");
                            rootJsonObject.Data.Add(jobject);
                            break;
                        case "SC":
                            jobject = AddStudentForCategory(students, "SC");
                            rootJsonObject.Data.Add(jobject);
                            break;
                        case "ST":
                            jobject = AddStudentForCategory(students, "ST");
                            rootJsonObject.Data.Add(jobject);
                            break;
                        case "EWS":
                            jobject = AddStudentForCategory(students, "EWS");
                            rootJsonObject.Data.Add(jobject);
                            break;
                        case "Others":
                            jobject = AddStudentForCategory(students, "Others");
                            rootJsonObject.Data.Add(jobject);
                            break;
                        default:
                            break;
                    }
                }
                //var attendancestatus = students
                //           .Any(sa => sa.id == centerId)
                //           ? "data exists"
                //           : "no record";

                //foreach (var item in students)
                //{
                //    student = new JObject();
                //    student.Category = item.Category;
                //    student.TotalStudentCount = item.Total;
                //    rootJsonObject.Data.Add(student);
                //}


                logger.LogInformation($"DashboardRepository : GetTotalStudentOfClass : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DashboardRepository : GetTotalStudentOfClass ", ex);
            }

            return JsonConvert.SerializeObject(rootJsonObject);
        }

        private JObject AddStudentForCategory(List<Student> students, string category)
        {
            Student item = students.Where(x => x.Category == category).FirstOrDefault();
            dynamic student = new JObject();
            if (item != null)
            {
                student.Category = item.Category;
                student.TotalStudentCount = item.TotalStudentCount;
            }
            else
            {
                student.Category = category;
                student.TotalStudentCount = 0;
            }
            return student;
        }

    }
}
