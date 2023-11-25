using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Collections;
using System.Runtime.ConstrainedExecution;

namespace StudentAttendanceApiDAL.Repository
{
    public class CenterRepository : ICenterRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public CenterRepository(AppDbContext appDbContext, ILogger<CenterRepository> logger, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<Center> SaveCenter(Center center)
        {
            logger.LogInformation($"UserRepository : SaveCenter : Started");

            try
            {
                if (center.Id > 0)
                {
                    var centerVal = appDbContext.Center.AsNoTracking().FirstOrDefaultAsync(x => x.Id == center.Id).Result;
                    if (centerVal != null)
                    {
                        center.Status = centerVal.Status;
                        center.ClassStatus = centerVal.ClassStatus;
                    }
                    appDbContext.Update(center);
                    await appDbContext.SaveChangesAsync();
                }
                else
                {
                    center.Status = true;
                    center.ClassStatus = false;
                    center.CreatedDate = DateTime.Now;
                    appDbContext.Center.Add(center);

                    await appDbContext.SaveChangesAsync();
                }

                #region  update assigned teacher status

                List<int> userIds = new List<int>();
                userIds.Add(center.AssignedRegionalAdmin.Value);
                userIds.Add(center.AssignedTeachers.Value);

                List<Users> user = await appDbContext.Users.Where(x => userIds.Contains(x.Id)).AsNoTracking().ToListAsync();

                appDbContext.Users.Where(x => userIds.Contains(x.Id)).ToList().ForEach(i =>
                {
                    i.AssignedTeacherStatus = true;
                    i.AssignedRegionalAdminStatus = true;
                }
                );

                #endregion


                #region save history of user assign
                List<CenterAssignUser> list = new List<CenterAssignUser>();
                CenterAssignUser centerAssignUser = new CenterAssignUser();
                centerAssignUser.CenterId = center.Id;
                centerAssignUser.UsersId = center.AssignedTeachers.Value;
                centerAssignUser.Date = DateTime.Now;
                list.Add(centerAssignUser);

                CenterAssignUser centerAssignAdmin = new CenterAssignUser();
                centerAssignAdmin.CenterId = center.Id;
                centerAssignAdmin.UsersId = center.AssignedRegionalAdmin.Value;
                centerAssignAdmin.Date = DateTime.Now;

                list.Add(centerAssignAdmin);

                appDbContext.AddRange(list);
                await appDbContext.SaveChangesAsync();
                #endregion

                logger.LogInformation($"UserRepository : SaveCenter : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveCenter ", ex);
                throw ex;
            }
            return center;
        }

        public async Task<string> CheckCenterName(string name)
        {
            logger.LogInformation($"UserRepository : CheckCenterName : Started");

            Center center = new Center();
            try
            {
                center = appDbContext.Center.AsNoTracking().FirstOrDefaultAsync(x => x.CenterName == name).Result;

                logger.LogInformation($"UserRepository : CheckCenterName : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : CheckCenterName", ex);
                throw ex;
            }
            return center == null ? null : center.CenterName;
        }

        public async Task<Center> GetCenteryId(int centerId)
        {
            logger.LogInformation($"UserRepository : CheckDistrictName : Started");

            Center center = new Center();
            try
            {
                center = await appDbContext.Center.Include(x => x.District)
                                                  .Include(x => x.VidhanSabha)
                                                  .Include(x => x.Panchayat)
                                                  .Include(x => x.Village)
                                                  .Where(x => x.Id == centerId).FirstOrDefaultAsync();

                if (center != null)
                {
                    center.RegionalAdminName = appDbContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == center.AssignedRegionalAdmin).Name;

                    center.User = appDbContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == center.AssignedTeachers);

                    center.TotalStudents = appDbContext.Student.Where(x => x.CenterId == center.Id).AsNoTracking().ToList().Count();

                    logger.LogInformation($"UserRepository : CheckDistrictName : End");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : CheckDistrictName", ex);
            }
            return center;
        }

        public async Task<List<Center>> GetAllCenters(int userId, int type)
        {
            logger.LogInformation($"UserRepository : GetAllCenters : Started");

            List<Center> centers = new List<Center>();
            try
            {
                if (userId == 0 && type == 0)
                {
                    centers = await (from c in appDbContext.Center
                                     join u in appDbContext.Users
                                     on c.AssignedTeachers equals u.Id
                                     join d in appDbContext.District
                                     on c.DistrictId equals d.Id
                                     join v in appDbContext.VidhanSabha
                                     on c.VidhanSabhaId equals v.Id
                                     join p in appDbContext.Panchayat
                                      on c.PanchayatId equals p.Id
                                     // join vi in appDbContext.Village
                                     //on c.VillageId equals vi.Id
                                     join vi in appDbContext.Village on c.VillageId equals vi.Id into villageGroup
                                     from village in villageGroup.DefaultIfEmpty()
                                     select new Center
                                     {
                                         Id = c.Id,
                                         StartedDate = c.StartedDate,
                                         ClassStatus = c.ClassStatus,
                                         CenterGuidId = c.CenterGuidId,
                                         CenterName = c.CenterName,
                                         AssignedTeachers = c.AssignedTeachers,
                                         AssignedRegionalAdmin = c.AssignedRegionalAdmin,
                                         Status = c.Status,
                                         PanchayatId = p.Id,
                                         DistrictId = d.Id,
                                         VidhanSabhaId = v.Id,
                                         PanchayatName = p.Name,
                                         DistrictName = d.Name,
                                         VidhanSabhaName = v.Name,
                                         VillageName = village.Name,
                                         VillageId = village.Id,
                                         TeacherName = appDbContext.Users.Where(x => x.Id == c.AssignedTeachers).FirstOrDefault().Name,
                                         RegionalAdminName = appDbContext.Users.Where(x => x.Id == c.AssignedRegionalAdmin).FirstOrDefault().Name,
                                         TotalStudents = appDbContext.Student.Where(x => x.CenterId == c.Id).AsNoTracking().ToList().Count,

                                     }).OrderByDescending(x => x.Id).ToListAsync();

                }
                else
                {
                    centers = await (from c in appDbContext.Center
                                     join u in appDbContext.Users
                                     on c.AssignedTeachers equals u.Id
                                     join d in appDbContext.District
                                     on c.DistrictId equals d.Id
                                     join v in appDbContext.VidhanSabha
                                     on c.VidhanSabhaId equals v.Id
                                     join p in appDbContext.Panchayat
                                      on c.PanchayatId equals p.Id
                                     //   join vi in appDbContext.Village
                                     //on c.VillageId equals vi.Id
                                     join vi in appDbContext.Village on c.VillageId equals vi.Id into villageGroup
                                     from village in villageGroup.DefaultIfEmpty()
                                     where c.AssignedRegionalAdmin == userId
                                     select new Center
                                     {
                                         Id = c.Id,
                                         StartedDate = c.StartedDate,
                                         ClassStatus = c.ClassStatus,
                                         CenterGuidId = c.CenterGuidId,
                                         CenterName = c.CenterName,
                                         AssignedTeachers = c.AssignedTeachers,
                                         AssignedRegionalAdmin = c.AssignedRegionalAdmin,
                                         Status = c.Status,
                                         PanchayatId = p.Id,
                                         DistrictId = d.Id,
                                         VidhanSabhaId = v.Id,
                                         PanchayatName = p.Name,
                                         DistrictName = d.Name,
                                         VidhanSabhaName = v.Name,
                                         VillageName = village.Name,
                                         VillageId = village.Id,
                                         TeacherName = appDbContext.Users.Where(x => x.Id == c.AssignedTeachers).FirstOrDefault().Name,
                                         RegionalAdminName = appDbContext.Users.Where(x => x.Id == c.AssignedTeachers).FirstOrDefault().Name,
                                         TotalStudents = appDbContext.Student.Where(x => x.CenterId == c.Id).AsNoTracking().ToList().Count,

                                     }).OrderByDescending(x => x.Id).ToListAsync();
                }

                //if(centers!=null)
                //{
                //    centers = (from cen in centers
                //               join v in appDbContext.Village on cen.VillageId equals v.Id into villageGroup
                //               from village in villageGroup.DefaultIfEmpty()
                //               select new Center
                //               {
                //                   vil
                //               }).ToList();
                //}
                logger.LogInformation($"UserRepository : GetAllCenters : End");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllCenters", ex);
                throw ex;
            }
            return centers;
        }

        public async Task<Center> GetCenterByUserId(int userId)
        {
            logger.LogInformation($"UserRepository : GetCenterByUserId : Started");

            Center center = new Center();
            try
            {

                center = await appDbContext.Center.Include(x => x.District)
                                              .Include(x => x.VidhanSabha)
                                              .Include(x => x.Panchayat)
                                              .Include(x => x.Village)
                                              .Include(x => x.CenterAssignUser)
                                              .Where(x => x.AssignedTeachers == userId).FirstOrDefaultAsync();


                if (center != null)
                {
                    center.RegionalAdminName = appDbContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == center.AssignedRegionalAdmin).Name;

                    center.TotalStudents = appDbContext.Student.Where(x => x.CenterId == center.Id).AsNoTracking().ToList().Count();
                    
                    logger.LogInformation($"UserRepository : CheckDistrictName : End");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetUserById", ex);
                throw ex;
            }
            return center;
        }

        //public async Task<List<Center>> GetTodayClassStatistics(int status)
        //{
        //    logger.LogInformation($"UserRepository : GetAllClasses : Started");
        //    List<Center> allCenters = new List<Center>();
        //    List<Center> centers = new List<Center>();
        //}

        public async Task<List<Center>> GetStudentAttendanceOfCenter(int status, int userId, int type)
        {
            logger.LogInformation($"UserRepository : GetAllClasses : Started");
            List<Center> allCenters = new List<Center>();
            List<Center> centers = new List<Center>();
            try
            {

                //default case
                if (userId == 0 && type == 0)
                {
                    if (status == (int)(Constant.ClassStatus.Active) || status == (int)(Constant.ClassStatus.Completed))
                    {
                        List<Class> classes = appDbContext.Class.AsNoTracking().Where(x => x.StartedDate.Value.Date == DateTime.Now.Date && x.Status.Value == status).ToList();
                        List<int> centerIds = classes.Select(x => x.CenterId).ToList();

                        centers = await appDbContext.Center.AsNoTracking().Include(x => x.District)
                                                  .Include(x => x.VidhanSabha)
                                                  .Include(x => x.Panchayat).Where(x => centerIds.Contains(x.Id)).ToListAsync();
                        foreach (var item in centers)
                        {
                            Center center = new Center();
                            center.Id = item.Id;
                            center.CenterName = item.CenterName;
                            center.Status = item.Status;
                            center.StartedDate = item.StartedDate;
                            if (classes != null && classes.Count > 0)
                            {
                                Class cls = classes.Where(x => x.CenterId == item.Id && x.StartedDate.Value.Date == DateTime.Now.Date).FirstOrDefault();
                                if (cls != null)
                                {
                                    center.ClassStartDate = classes.Where(x => x.CenterId == item.Id && x.StartedDate.Value.Date == DateTime.Now.Date).FirstOrDefault().StartedDate;
                                    center.ClassEndDate = classes.Where(x => x.CenterId == item.Id && x.StartedDate.Value.Date == DateTime.Now.Date).FirstOrDefault().EndDate;
                                }

                                center.TotalPresentStudents = classes.Where(x => x.StartedDate.Value.Date == DateTime.Now.Date && x.CenterId == item.Id).FirstOrDefault().AvilableStudents;
                                //center.TotalActiveStudents = classes.Where(x => x.StartedDate.Value.Date == DateTime.Now.Date && x.CenterId == item.Id).FirstOrDefault().AvilableStudents;
                            }
                            else
                            {
                                center.TotalPresentStudents = 0;
                                center.TotalActiveStudents = 0;
                            }
                            if (item.AssignedTeachers != null)
                            {
                                center.AssignedTeachers = item.AssignedTeachers;
                                center.TeacherName = appDbContext.Users.FirstOrDefault(x => x.Id == item.AssignedTeachers).Name;
                            }
                            if (item.AssignedRegionalAdmin != null)
                            {
                                center.AssignedRegionalAdmin = item.AssignedRegionalAdmin;
                                center.RegionalAdminName = appDbContext.Users.FirstOrDefault(x => x.Id == item.AssignedRegionalAdmin).Name;
                            }
                            if (item.VillageId > 0)
                            {
                                Village village = appDbContext.Village.FirstOrDefault(x => x.Id == item.VillageId);
                                if (village != null)
                                {
                                    center.VillageName = village.Name;
                                }
                            }
                            center.TotalStudents = appDbContext.Student.Where(x => x.CenterId == item.Id && x.Status.Value).AsNoTracking().ToList().Count;
                            center.PanchayatId = item.PanchayatId;
                            center.VidhanSabhaId = item.VidhanSabhaId;
                            center.DistrictId = item.DistrictId;
                            center.ClassStatus = item.ClassStatus;
                            center.CreatedDate = item.CreatedDate;
                            center.VillageId = item.VillageId;
                           // center.VillageName = item.Village != null ? item.Village.Name : string.Empty;
                            center.PanchayatName = item.Panchayat != null ? item.Panchayat.Name : string.Empty;
                            center.DistrictName = item.District != null ? item.District.Name : string.Empty;
                            center.VidhanSabhaName = item.VidhanSabha != null ? item.VidhanSabha.Name : string.Empty;
                            allCenters.Add(center);
                        }
                    }
                    else if (status == (int)(Constant.ClassStatus.Cancel))
                    {

                        centers = await (from cen in appDbContext.Center
                                         join c in appDbContext.ClassCancelTeacher
                                         on cen.Id equals c.Id
                                         join d in appDbContext.District
                                         on cen.DistrictId equals d.Id
                                         join v in appDbContext.VidhanSabha
                                         on cen.VidhanSabhaId equals v.Id
                                         join p in appDbContext.Panchayat
                                          on cen.PanchayatId equals p.Id
                                         where (c.StartingDate.Value.Date <= DateTime.Now.Date && c.EndingDate.Value.Date >= DateTime.Now.Date)
                                         select cen).ToListAsync();

                        foreach (var item in centers)
                        {
                            Center center = new Center();
                            center.Id = item.Id;
                            center.CenterName = item.CenterName;
                            center.Status = item.Status;
                            center.StartedDate = item.StartedDate;

                            if (item.AssignedTeachers != null)
                            {
                                center.AssignedTeachers = item.AssignedTeachers;
                                center.TeacherName = appDbContext.Users.FirstOrDefault(x => x.Id == item.AssignedTeachers).Name;
                            }
                            if (item.AssignedRegionalAdmin != null)
                            {
                                center.AssignedRegionalAdmin = item.AssignedRegionalAdmin;
                                center.RegionalAdminName = appDbContext.Users.FirstOrDefault(x => x.Id == item.AssignedRegionalAdmin).Name;
                            }
                            if (item.VillageId > 0)
                            {
                                Village village = appDbContext.Village.FirstOrDefault(x => x.Id == item.VillageId);
                                if (village != null)
                                {
                                    center.VillageName = village.Name;
                                }
                            }
                            center.TotalStudents = appDbContext.Student.Where(x => x.CenterId == item.Id && x.Status.Value).AsNoTracking().ToList().Count;
                            center.PanchayatId = item.PanchayatId;
                            center.VidhanSabhaId = item.VidhanSabhaId;
                            center.DistrictId = item.DistrictId;
                            center.ClassStatus = item.ClassStatus;
                            center.CreatedDate = item.CreatedDate;
                            center.VillageId = item.VillageId;
                            //center.VillageName = item.Village != null ? item.Village.Name : string.Empty;
                            center.PanchayatName = item.Panchayat != null ? item.Panchayat.Name : string.Empty;
                            center.DistrictName = item.District != null ? item.District.Name : string.Empty;
                            center.VidhanSabhaName = item.VidhanSabha != null ? item.VidhanSabha.Name : string.Empty;
                            allCenters.Add(center);
                        }
                    }
                    else
                    {
                        //upcoming
                        List<Class> classes = appDbContext.Class.AsNoTracking().Where(x => x.StartedDate.Value.Date == DateTime.Now.Date).ToList();
                        List<int> centerIds = classes.Select(x => x.CenterId).ToList();

                        centers = await appDbContext.Center.AsNoTracking().Include(x => x.District)
                                                  .Include(x => x.VidhanSabha)
                                                  .Include(x => x.Panchayat).Where(x => !centerIds.Contains(x.Id)).ToListAsync();
                        foreach (var item in centers)
                        {
                            Center center = new Center();
                            center.Id = item.Id;
                            center.CenterName = item.CenterName;
                            center.Status = item.Status;
                            center.StartedDate = item.StartedDate;

                            if (item.AssignedTeachers != null)
                            {
                                center.AssignedTeachers = item.AssignedTeachers;
                                if (item.AssignedTeachers != 0)
                                {
                                    center.TeacherName = appDbContext.Users.FirstOrDefault(x => x.Id == item.AssignedTeachers).Name;
                                }
                                else
                                {
                                    center.TeacherName = string.Empty;
                                }
                            }
                            if (item.AssignedRegionalAdmin != null)
                            {
                                center.AssignedRegionalAdmin = item.AssignedRegionalAdmin;
                                center.RegionalAdminName = appDbContext.Users.FirstOrDefault(x => x.Id == item.AssignedRegionalAdmin).Name;
                            }
                            if (item.VillageId > 0)
                            {
                                Village village = appDbContext.Village.FirstOrDefault(x => x.Id == item.VillageId);
                                if (village != null)
                                {
                                    center.VillageName = village.Name;
                                }
                            }
                            center.TotalStudents = appDbContext.Student.Where(x => x.CenterId == item.Id && x.Status.Value).AsNoTracking().ToList().Count;
                            center.PanchayatId = item.PanchayatId;
                            center.VidhanSabhaId = item.VidhanSabhaId;
                            center.DistrictId = item.DistrictId;
                            center.ClassStatus = item.ClassStatus;
                            center.CreatedDate = item.CreatedDate;
                            center.VillageId = item.VillageId;
                           // center.VillageName = item.Village != null ? item.Village.Name : string.Empty;
                            center.PanchayatName = item.Panchayat != null ? item.Panchayat.Name : string.Empty;
                            center.DistrictName = item.District != null ? item.District.Name : string.Empty;
                            center.VidhanSabhaName = item.VidhanSabha != null ? item.VidhanSabha.Name : string.Empty;
                            allCenters.Add(center);
                        }


                    }
                }
                else
                {
                    if (status == (int)(Constant.ClassStatus.Active) || status == (int)(Constant.ClassStatus.Completed))
                    {
                        List<Class> classes = appDbContext.Class.AsNoTracking().Where(x => x.StartedDate.Value.Date == DateTime.Now.Date && x.Status.Value == status && x.UsersId == userId).ToList();
                        List<int> centerIds = classes.Select(x => x.CenterId).ToList();

                        centers = await appDbContext.Center.AsNoTracking().Include(x => x.District)
                                                  .Include(x => x.VidhanSabha)
                                                  .Include(x => x.Panchayat).Where(x => centerIds.Contains(x.Id)).Where(x => x.AssignedRegionalAdmin == userId).ToListAsync();
                        foreach (var item in centers)
                        {
                            Center center = new Center();
                            center.Id = item.Id;
                            center.CenterName = item.CenterName;
                            center.Status = item.Status;
                            center.StartedDate = item.StartedDate;
                            if (classes != null && classes.Count > 0)
                            {
                                Class cls = classes.Where(x => x.CenterId == item.Id && x.StartedDate.Value.Date == DateTime.Now.Date).FirstOrDefault();
                                if (cls != null)
                                {
                                    center.CreatedDate = classes.Where(x => x.CenterId == item.Id && x.StartedDate.Value.Date == DateTime.Now.Date).FirstOrDefault().StartedDate;
                                    center.ClassEndDate = classes.Where(x => x.CenterId == item.Id && x.StartedDate.Value.Date == DateTime.Now.Date).FirstOrDefault().EndDate;
                                }

                                center.TotalPresentStudents = classes.Where(x => x.StartedDate.Value.Date == DateTime.Now.Date && x.CenterId == item.Id).FirstOrDefault().TotalStudents;
                                //center.TotalActiveStudents = classes.Where(x => x.StartedDate.Value.Date == DateTime.Now.Date && x.CenterId == item.Id).FirstOrDefault().AvilableStudents;
                            }
                            else
                            {
                                center.TotalPresentStudents = 0;
                                center.TotalActiveStudents = 0;
                            }
                            if (item.AssignedTeachers != null)
                            {
                                center.AssignedTeachers = item.AssignedTeachers;
                                center.TeacherName = appDbContext.Users.FirstOrDefault(x => x.Id == item.AssignedTeachers).Name;
                            }
                            if (item.VillageId > 0)
                            {
                                Village village = appDbContext.Village.FirstOrDefault(x => x.Id == item.VillageId);
                                if (village != null)
                                {
                                    center.VillageName = village.Name;
                                }
                            }
                            center.TotalStudents = appDbContext.Student.Where(x => x.CenterId == item.Id && x.Status.Value).AsNoTracking().ToList().Count;
                            center.PanchayatId = item.PanchayatId;
                            center.VidhanSabhaId = item.VidhanSabhaId;
                            center.DistrictId = item.DistrictId;
                            center.ClassStatus = item.ClassStatus;
                            center.CreatedDate = item.CreatedDate;
                            center.PanchayatName = item.Panchayat != null ? item.Panchayat.Name : string.Empty;
                            center.DistrictName = item.District != null ? item.District.Name : string.Empty;
                            center.VidhanSabhaName = item.VidhanSabha != null ? item.VidhanSabha.Name : string.Empty;
                            allCenters.Add(center);
                        }
                    }
                    else if (status == (int)(Constant.ClassStatus.Cancel))
                    {

                        centers = await (from cen in appDbContext.Center
                                         join c in appDbContext.ClassCancelTeacher
                                         on cen.Id equals c.Id
                                         join d in appDbContext.District
                                         on cen.DistrictId equals d.Id
                                         join v in appDbContext.VidhanSabha
                                         on cen.VidhanSabhaId equals v.Id
                                         join p in appDbContext.Panchayat
                                          on cen.PanchayatId equals p.Id
                                         where (cen.AssignedRegionalAdmin == userId && c.StartingDate.Value.Date <= DateTime.Now.Date && c.EndingDate.Value.Date >= DateTime.Now.Date)
                                         select cen).ToListAsync();

                        foreach (var item in centers)
                        {
                            Center center = new Center();
                            center.Id = item.Id;
                            center.CenterName = item.CenterName;
                            center.Status = item.Status;
                            center.StartedDate = item.StartedDate;

                            if (item.AssignedTeachers != null)
                            {
                                center.AssignedTeachers = item.AssignedTeachers;
                                center.TeacherName = appDbContext.Users.FirstOrDefault(x => x.Id == item.AssignedTeachers).Name;
                            }
                            if (item.VillageId > 0)
                            {
                                Village village = appDbContext.Village.FirstOrDefault(x => x.Id == item.VillageId);
                                if (village != null)
                                {
                                    center.VillageName = village.Name;
                                }
                            }
                            center.TotalStudents = appDbContext.Student.Where(x => x.CenterId == item.Id && x.Status.Value).AsNoTracking().ToList().Count;
                            center.PanchayatId = item.PanchayatId;
                            center.VidhanSabhaId = item.VidhanSabhaId;
                            center.DistrictId = item.DistrictId;
                            center.ClassStatus = item.ClassStatus;
                            center.CreatedDate = item.CreatedDate;
                            center.PanchayatName = item.Panchayat != null ? item.Panchayat.Name : string.Empty;
                            center.DistrictName = item.District != null ? item.District.Name : string.Empty;
                            center.VidhanSabhaName = item.VidhanSabha != null ? item.VidhanSabha.Name : string.Empty;
                            allCenters.Add(center);
                        }
                    }
                    else
                    {
                        //upcoming
                        List<Class> classes = appDbContext.Class.AsNoTracking().Where(x => x.StartedDate.Value.Date == DateTime.Now.Date).ToList();
                        List<int> centerIds = classes.Select(x => x.CenterId).ToList();

                        centers = await appDbContext.Center.AsNoTracking().Include(x => x.District)
                                                  .Include(x => x.VidhanSabha)
                                                  .Include(x => x.Panchayat).Where(x => !centerIds.Contains(x.Id) && x.AssignedRegionalAdmin == userId).ToListAsync();
                        foreach (var item in centers)
                        {
                            Center center = new Center();
                            center.Id = item.Id;
                            center.CenterName = item.CenterName;
                            center.Status = item.Status;
                            center.StartedDate = item.StartedDate;

                            if (item.AssignedTeachers != null)
                            {
                                center.AssignedTeachers = item.AssignedTeachers;
                                center.TeacherName = appDbContext.Users.FirstOrDefault(x => x.Id == item.AssignedTeachers).Name;
                            }
                            if (item.VillageId > 0)
                            {
                                Village village = appDbContext.Village.FirstOrDefault(x => x.Id == item.VillageId);
                                if (village != null)
                                {
                                    center.VillageName = village.Name;
                                }
                            }
                            center.TotalStudents = appDbContext.Student.Where(x => x.CenterId == item.Id && x.Status.Value).AsNoTracking().ToList().Count;
                            center.PanchayatId = item.PanchayatId;
                            center.VidhanSabhaId = item.VidhanSabhaId;
                            center.DistrictId = item.DistrictId;
                            center.ClassStatus = item.ClassStatus;
                            center.CreatedDate = item.CreatedDate;
                            center.PanchayatName = item.Panchayat != null ? item.Panchayat.Name : string.Empty;
                            center.DistrictName = item.District != null ? item.District.Name : string.Empty;
                            center.VidhanSabhaName = item.VidhanSabha != null ? item.VidhanSabha.Name : string.Empty;
                            allCenters.Add(center);
                        }


                    }
                }

                logger.LogInformation($"UserRepository : GetAllClasses : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllClasses", ex);
                throw ex;
            }
            return allCenters;
        }

        public async Task<List<Center>> GetAllCentersById(int districtId, int vidhanSabhaId, int panchayatId, int villageId)
        {
            logger.LogInformation($"UserRepository : GetAllCentersById : Started");

            List<Center> centers = new List<Center>();
            try
            {
                // string? query = CreateSqlQuery(districtId, vidhanSabhaId, panchayatId, villageId);
                //if (!string.IsNullOrEmpty(query))
                //{
                //    centers = await appDbContext.Center.AsNoTracking().ToListAsync();
                //}
                //else
                //{
                //    centers = await appDbContext.Center.AsNoTracking().ToListAsync();
                //}
                centers = await appDbContext.Center.AsNoTracking().ToListAsync();
                logger.LogInformation($"UserRepository : GetAllCentersById : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllCentersById", ex);
                throw ex;
            }
            return centers;
        }


        //private async string CreateSqlQuery(int districtId, int vidhanSabhaId, int panchayatId, int villageId)
        //{
        //    string query = string.Empty;
        //    List<Center> center = await appDbContext.Center.AsNoTracking().ToListAsync();
        //    if (districtId > 0)
        //    {
        //        query = center.Where(x => x.DistrictId == districtId + "&&").ToString();
        //    }
        //    if (vidhanSabhaId > 0)
        //    {
        //        query = center.Where(x => x.DistrictId == districtId).ToString();
        //    }
        //    if (panchayatId > 0)
        //    {
        //        query = query + "x.PanchayatId ==" + panchayatId + "&&";
        //    }
        //    if (villageId > 0)
        //    {
        //        query = query + "x.VillageId ==" + villageId";
        //    }


        //    return query;
        //}
    }
}
