using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace StudentAttendanceApiDAL.Repository
{

    public class ClassRepository : IClassRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public ClassRepository(AppDbContext appDbContext, ILogger<ClassRepository> logger, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<Class> SaveClass(Class cls)
        {
            logger.LogInformation($"UserRepository : SaveClass : Started");

            try
            {
                var classExists = appDbContext.Class.AsNoTracking().FirstOrDefaultAsync(x => x.ClassEnrolmentId == cls.ClassEnrolmentId && x.StartedDate.Value.Date == DateTime.Now.Date && x.CenterId == cls.CenterId).Result;
                if (classExists != null)
                {
                    return null;
                }
                else
                {
                    if (cls.Id == 0)
                    {
                        cls.StartedDate = DateTime.Now;
                        cls.Status = (int)Constant.ClassStatus.Active;
                        cls.SubStatus = 0;
                        appDbContext.Class.Add(cls);
                    }
                    else
                    {
                        var classVal = appDbContext.Class.AsNoTracking().FirstOrDefaultAsync(x => x.Id == cls.Id).Result;
                        if (classVal != null)
                        {
                            cls.Status = classVal.Status;
                        }

                    }
                }
                await appDbContext.SaveChangesAsync();

                //update class status in center
                Center center = appDbContext.Center.FirstOrDefaultAsync(x => x.Id == cls.CenterId).Result;
                if (center != null)
                {
                    center.ClassStatus = true;
                    appDbContext.Update(center);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveStudent : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudent ", ex);
                throw ex;
            }
            return cls;
        }

        public async Task<Class> CancelClass(Class cls)
        {
            logger.LogInformation($"UserRepository : CancelClass : Started");

            try
            {
                Class clasVal = await appDbContext.Class.AsNoTracking().FirstOrDefaultAsync(x => x.Id == cls.Id);
                if (clasVal != null)
                {
                    clasVal.Reason = cls.Reason;
                    clasVal.CancelBy = cls.CancelBy;
                    clasVal.CancelDate = DateTime.Now;
                    clasVal.Status = (int)Constant.ClassStatus.Cancel;
                    appDbContext.Update(clasVal);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveStudent : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudent ", ex);
                throw ex;
            }
            return cls;
        }

        public async Task<ClassCancelTeacher> CancelClassByTeacher(ClassCancelTeacher cls)
        {
            logger.LogInformation($"UserRepository : CancelClass : Started");

            try
            {
                if (cls.Id == 0)
                {
                    //cls.EndingDate = DateTime.Parse(cls.EndingDate.ToString()).ToUniversalTime();//
                    cls.CreatedOn = DateTime.Now;
                    appDbContext.ClassCancelTeacher.Add(cls);

                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveStudent : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudent ", ex);
                throw ex;
            }
            return cls;
        }

        public async Task<string> GetClassCurrentStatus(int centerId, int teacherId)
        {
            dynamic rootJsonObject = new JObject();
            rootJsonObject.Data = new JArray() as dynamic;
            logger.LogInformation($"UserRepository : CancelClass : Started");
            try
            {

                Holidays holidays = await appDbContext.Holidays.AsNoTracking().Where(x => x.CenterId == centerId && (x.StartDate.Value.Date <= DateTime.Now.Date && x.EndDate.Value.Date >= DateTime.Now.Date)).FirstOrDefaultAsync();

                ClassCancelTeacher classCancelTeacher = await appDbContext.ClassCancelTeacher.AsNoTracking().Where(x => x.UserId == teacherId && (x.StartingDate.Value.Date <= DateTime.Now.Date && x.EndingDate.Value.Date >= DateTime.Now.Date)).FirstOrDefaultAsync();

                var classExists = appDbContext.Class.AsNoTracking().FirstOrDefaultAsync(x => x.StartedDate.Value.Date == DateTime.Now.Date && x.CenterId == centerId && x.Status.Value == 1).Result;

                var classEndExists = appDbContext.Class.AsNoTracking().FirstOrDefaultAsync(x => x.StartedDate.Value.Date == DateTime.Now.Date && x.CenterId == centerId && x.Status.Value == 2).Result;
                dynamic song = new JObject();

                if (holidays != null)
                {
                    song = new JObject();
                    song.Name = holidays.Name;
                    song.Type = 1;
                    song.StartedDate = holidays.StartDate;
                    song.EndDate = holidays.EndDate;
                    rootJsonObject.Data.Add(song);

                }

                if (classCancelTeacher != null)
                {

                    song = new JObject();
                    song.Name = classCancelTeacher.Reason;
                    song.Type = 2;
                    song.StartedDate = classCancelTeacher.StartingDate;
                    song.EndDate = classCancelTeacher.EndingDate;
                    rootJsonObject.Data.Add(song);
                }

                if (classExists != null)
                {
                    song = new JObject();
                    song.Name = "Class is going on";
                    song.Type = 3;
                    song.SubStatus = classExists.SubStatus;
                    song.Id = classExists.Id;
                    song.StartedDate = classExists.StartedDate;
                    song.EndDate = classExists.EndDate;
                    rootJsonObject.Data.Add(song);

                }

                if (classEndExists != null && classEndExists.Status == 2)//class end
                {
                    song = new JObject();
                    song.Name = "Class Ended";
                    song.Type = 4;
                    song.Id = classEndExists.Id;
                    song.StartedDate = classEndExists.StartedDate;
                    song.EndDate = classEndExists.EndDate;
                    rootJsonObject.Data.Add(song);

                }
                rootJsonObject.Status = true;
                logger.LogInformation($"UserRepository : SaveStudent : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveStudent ", ex);
                throw ex;
            }
            return JsonConvert.SerializeObject(rootJsonObject);
        }

        public async Task<Class> UpdateEndClassTime(Class cls)
        {
            logger.LogInformation($"UserRepository : UpdateEndClassTime : Started");

            try
            {
                if (cls.Id > 0)
                {
                    var clasData = appDbContext.Class.AsNoTracking().FirstOrDefaultAsync(x => x.Id == cls.Id).Result;
                    if (clasData != null)
                    {
                        clasData.EndDate = DateTime.Now;
                        clasData.Status = (int)(Constant.ClassStatus.Completed);//completed
                    }
                    appDbContext.Update(clasData);
                    await appDbContext.SaveChangesAsync();

                    //
                    var center = appDbContext.Center.AsNoTracking().FirstOrDefaultAsync(x => x.Id == clasData.CenterId).Result;
                    if (center != null)
                    {
                        List<int?> presentStudentIds = appDbContext.StudentAttendance.Where(x => x.CenterId == (center.Id)).Select(x => x.StudentId).ToList();
                        List<int> studentIds = appDbContext.Student.Where(x => x.CenterId == (center.Id) && presentStudentIds.Contains(x.Id)).Select(x => x.Id).ToList();
                        if (studentIds != null && studentIds.Count > 0)
                        {
                            appDbContext.Student.Where(x => studentIds.Contains(x.Id)).ToList().ForEach(i =>
                            {
                                i.ActiveClassStatus = false;
                            });

                            await appDbContext.SaveChangesAsync();
                        }

                    }

                }


                logger.LogInformation($"UserRepository : UpdateEndClassTime : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : UpdateEndClassTime ", ex);
                throw ex;
            }
            return cls;
        }

        public async Task<Dictionary<int, int>> GetActiveClass()
        {
            logger.LogInformation($"UserRepository : GetActiveClass : Started");

            Dictionary<int, int> ClassData = new Dictionary<int, int>();
            try
            {
                List<Class> classes = await appDbContext.Class.AsNoTracking().ToListAsync();
                List<Class> activeClasses = classes.Where(x => x.Status == (int)Constant.ClassStatus.Active).ToList();

                ClassData.Add(activeClasses.Count(), classes.Count());

                logger.LogInformation($"UserRepository : GetAllClasses : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllClasses", ex);
                throw ex;
            }
            return ClassData;
        }

        public async Task<Class> GetLiveClassDetail(int classId)
        {
            logger.LogInformation($"UserRepository : GetActiveClass : Started");
            Class clas = null;

            try
            {
                clas = await appDbContext.Class.AsNoTracking().Where(x => x.StartedDate.Value.Date == DateTime.Now.Date && x.Id == classId).FirstOrDefaultAsync();


                logger.LogInformation($"UserRepository : GetAllClasses : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllClasses", ex);
                throw ex;
            }
            return clas;
        }

        public async Task<Class> DeleteClassByTeacherId(int classId)
        {
            logger.LogInformation($"UserRepository : GetActiveClass : Started");

            Class classes = null;
            try
            {
                classes = await appDbContext.Class.Where(x => x.Id == classId).FirstOrDefaultAsync();
                if (classes != null)
                {
                    appDbContext.Remove(classes);

                    ClassCancelTeacher classCancelTeacher = appDbContext.ClassCancelTeacher.Where(x => x.UserId == classes.UsersId).FirstOrDefault();
                    if (classCancelTeacher != null)
                    {
                        appDbContext.Remove(classCancelTeacher);
                    }
                    List<StudentAttendance> studentAttendance = await appDbContext.StudentAttendance.Where(x => x.ClassId == classId).ToListAsync();
                    if (studentAttendance != null && studentAttendance.Count > 0)
                    {
                        appDbContext.RemoveRange(studentAttendance);
                    }
                }


                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : GetAllClasses : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllClasses", ex);
                throw ex;
            }
            return classes;
        }

        public async Task<Class> UpdateClassSubStatus(int clsId)
        {
            logger.LogInformation($"UserRepository : UpdateEndClassTime : Started");
            Class cls = null;
            try
            {

                if (clsId > 0)
                {
                    cls = appDbContext.Class.AsNoTracking().FirstOrDefaultAsync(x => x.Id == clsId).Result;
                    if (cls != null)
                    {
                        cls.SubStatus = 1;

                    }
                    appDbContext.Update(cls);
                    await appDbContext.SaveChangesAsync();
                }

                logger.LogInformation($"UserRepository : UpdateEndClassTime : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : UpdateEndClassTime ", ex);
                throw ex;
            }
            return cls;
        }
    }
}
