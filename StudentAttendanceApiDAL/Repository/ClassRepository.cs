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
                var classExists = appDbContext.Class.FirstOrDefaultAsync(x => x.ClassEnrolmentId == cls.ClassEnrolmentId && x.StartedDate.Value.Date == DateTime.UtcNow.Date).Result;
                if (classExists != null)
                {
                    return null;
                }
                else
                {
                    cls.StartedDate = DateTime.UtcNow;
                    cls.Status = (int)Constant.ClassStatus.Active;
                    appDbContext.Class.Add(cls);
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
                Class clasVal =await appDbContext.Class.FirstOrDefaultAsync(x => x.Id == cls.Id);
                if (clasVal != null)
                {
                    clasVal.Reason = cls.Reason;
                    clasVal.CancelBy = cls.CancelBy;
                    clasVal.CancelDate = DateTime.UtcNow;
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



        public async Task<Class> UpdateEndClassTime(Class cls)
        {
            logger.LogInformation($"UserRepository : UpdateEndClassTime : Started");

            try
            {
                if (cls.Id > 0)
                {
                    var clasData=appDbContext.Class.FirstOrDefaultAsync(x=>x.Id == cls.Id).Result;
                    if(clasData != null)
                    {
                        clasData.EndDate= DateTime.UtcNow;
                        clasData.Status = (int)(Constant.ClassStatus.Completed);//completed
                    }
                    appDbContext.Update(clasData);
                    

                    //
                    var studentAttendane=appDbContext.StudentAttendance.FirstOrDefaultAsync(x=>x.ClassId==cls.Id).Result;
                    if (studentAttendane != null)
                    {
                        var student=appDbContext.Student.FirstOrDefaultAsync(x=>x.Id==(studentAttendane.StudentId)).Result;
                        if(student!=null)
                        {
                            student.ActiveClassStatus = false;
                            appDbContext.Student.Update(student);
                        }
                        appDbContext.SaveChangesAsync();
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


      
        public async Task<Dictionary<int,int>> GetActiveClass()
        {
            logger.LogInformation($"UserRepository : GetActiveClass : Started");

            Dictionary<int, int> ClassData = new Dictionary<int, int>();
            try
            {
                List<Class> classes = await appDbContext.Class.ToListAsync();
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
    }
}
