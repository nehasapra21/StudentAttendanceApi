using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System.Text.Json.Nodes;

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
                                                    FeMaleCount = g.Where(x=>x.Gender=="FeMale").Count(),
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
    }
}
