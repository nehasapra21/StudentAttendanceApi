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
        public async Task<dynamic> GetClassCountByMonth(int centerId, int month)
        {
            logger.LogInformation($"DashboardRepository : GetTotalGenderRatioByCenterId : Started");
            dynamic rootJsonObject = new JObject();
            try
            {
                List<Class> classCount = await appDbContext.Class.Where(x => x.CenterId == centerId).ToListAsync();

                List<Holidays> HolidayCount = await appDbContext.Holidays.Where(x => x.CenterId == centerId && (x.StartDate.Value.Date <= DateTime.Now.Date && x.EndDate.Value.Date >= DateTime.Now.Date)).ToListAsync();

                List<ClassCancelTeacher> classCancelTeacherCount = await appDbContext.ClassCancelTeacher.AsNoTracking().Where(x => x.CenterId == centerId && (x.StartingDate.Value.Date <= DateTime.Now.Date && x.EndingDate.Value.Date >= DateTime.Now.Date)).ToListAsync();

                int holidayCount = HolidayCount.Count;
                int classCountValue = classCount.Count;
                int classCancelTeacherCountVal = classCancelTeacherCount.Count;

                rootJsonObject.Status = true;
                rootJsonObject.HolidayCount = holidayCount;
                rootJsonObject.ClassCount = classCountValue;
                rootJsonObject.ClassCancelTeacherCount = classCancelTeacherCountVal;

                logger.LogInformation($"DashboardRepository : GetTotalGenderRatioByCenterId : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DashboardRepository : GetTotalGenderRatioByCenterId ", ex);
            }

            return JsonConvert.SerializeObject(rootJsonObject);         }

        public async Task<dynamic> GetTotalGenderRatioByCenterId(int centerId)
        {
            logger.LogInformation($"DashboardRepository : GetClassCountByMonth : Started");
            dynamic rootJsonObject = new JObject();
            try
            {

                List<Student> TotalStudents = appDbContext.Student.Where(x => x.CenterId == centerId).ToList();

                int TotalStudentCount = TotalStudents.Count();
                int FeMaleCount = TotalStudents.Where(x => x.Gender == "FeMale").ToList().Count();
                int MaleCount = TotalStudents.Where(x => x.Gender == "Male").ToList().Count();


                rootJsonObject.Status = true;
                rootJsonObject.FeMaleCount = FeMaleCount;
                rootJsonObject.MaleCount = MaleCount;
                rootJsonObject.TotalStudentCount = TotalStudentCount;

                logger.LogInformation($"DashboardRepository : GetClassCountByMonth : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DashboardRepository : GetClassCountByMonth ", ex);
            }

            return JsonConvert.SerializeObject(rootJsonObject);
        }

        public async Task<dynamic> GetTotalStudentOfClass(int centerId)
        {
            logger.LogInformation($"DashboardRepository : GetTotalStudentOfClass : Started");
            dynamic rootJsonObject = new JObject();
            rootJsonObject.Data = new JArray() as dynamic;
            dynamic student = new JObject();
            string cleanJsonString = string.Empty;
            JObject jsonObject = new JObject();
            try
            {

                List<Student> students = await (from s in appDbContext.Student //Left Data Source
                                                where s.CenterId == centerId
                                                group new
                                                {
                                                    s
                                                } by new
                                                {
                                                    s.Id,
                                                    s.Grade,
                                                    s.Gender
                                                } into g
                                                select new Student
                                                {
                                                    Id = g.Key.Id,
                                                    Grade = g.Key.Grade,
                                                    Gender = g.Key.Gender,
                                                    TotalStudentCount =g.Count()
                                                }).OrderBy(x => x.Grade).ToListAsync();

                foreach (var item in students)
                {
                    student = new JObject();
                    student.Grade = item.Grade;
                    student.Gender = item.Gender;
                    student.TotalStudentCount = item.TotalStudentCount;
                    rootJsonObject.Data.Add(student);
                }
                rootJsonObject.Status = true;
                // Remove backslashes
                string JsonString = JsonConvert.SerializeObject(rootJsonObject);
                // Remove backslashes
                cleanJsonString = JsonString.Replace("\\", "");

                 jsonObject = JsonConvert.DeserializeObject<JObject>(cleanJsonString);

                // Access data as needed, for example:
                JArray data = jsonObject["Data"] as JArray;
                logger.LogInformation($"DashboardRepository : GetTotalStudentOfClass : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DashboardRepository : GetTotalStudentOfClass ", ex);
            }

            return jsonObject;
        }
    }
}
