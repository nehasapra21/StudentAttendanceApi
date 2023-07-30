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
                    center.CreatedDate = DateTime.UtcNow;
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
                centerAssignUser.Date = DateTime.UtcNow;
                list.Add(centerAssignUser);

                CenterAssignUser centerAssignAdmin = new CenterAssignUser();
                centerAssignAdmin.CenterId = center.Id;
                centerAssignAdmin.UsersId = center.AssignedRegionalAdmin.Value;
                centerAssignAdmin.Date = DateTime.UtcNow;

                list.Add(centerAssignAdmin);

                appDbContext.AddRange(list);
                await  appDbContext.SaveChangesAsync();
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

        public async Task<List<Center>> GetAllCenters()
        {
            logger.LogInformation($"UserRepository : GetAllCenters : Started");

            List<Center> centers = new List<Center>();
            try
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
                                 select new Center
                                 {
                                     Id = c.Id,
                                     CenterGuidId =c.CenterGuidId,
                                     CenterName=c.CenterName,
                                     AssignedTeachers=c.AssignedTeachers,
                                     Status=c.Status,
                                     PanchayatId = p.Id,
                                     DistrictId = d.Id,
                                     VidhanSabhaId = v.Id,
                                     PanchayatName = p.Name,
                                     DistrictName = d.Name,
                                     VidhanSabhaName = v.Name,
                                     TotalStudents = appDbContext.Student.Where(x => x.CenterId == c.Id).AsNoTracking().ToList().Count
                                 }).ToListAsync();
                //centers = await (from c in appDbContext.Center
                //                 join d in appDbContext.District
                //                 on c.DistrictId equals d.Id
                //                 join v in appDbContext.VidhanSabha
                //                 on c.VidhanSabhaId equals v.Id
                //                 join p in appDbContext.Panchayat
                //                  on c.PanchayatId equals p.Id
                //                 into center
                //                 from Village in center.DefaultIfEmpty()
                //                     // join vi in appDbContext.Village
                //                     //on c.VillageId equals vi.Id
                //                 select new Center
                //                 {
                //                     Id = c.Id,
                //                     CenterName = c.CenterName,
                //                     DistrictId = d.Id,
                //                     VidhanSabhaId = v.Id,
                //                     PanchayatId = c.PanchayatId,
                //                     VillageId = Village.Id,

                                     //                     //   DistrictName = d.Name,
                                     //                     //  VidhanSabhaName = v.Name,
                                     //                     StartedDate = c.StartedDate,
                                     //                     AssignedTeachers = c.AssignedTeachers,
                                     //                     AssignedRegionalAdmin = c.AssignedRegionalAdmin,
                                     //                     TeacherName = appDbContext.Users.FirstOrDefault(x => x.Id == c.AssignedTeachers).Name,
                                     //                     RegionalAdminName = appDbContext.Users.FirstOrDefault(x => x.Id == c.AssignedRegionalAdmin).Name,
                                     //                     TotalStudents = appDbContext.Student.Where(x => x.CenterId == c.Id).AsNoTracking().ToList().Count
                                     //                 }).ToListAsync();


                logger.LogInformation($"UserRepository : GetAllCenters : End");


                //if (centers != null && centers.Count > 0)
                //{
                //    foreach (var item in centers)
                //    {
                //        Panchayat pac = appDbContext.Panchayat.AsNoTracking()
                //                          .FirstOrDefaultAsync(x => x.Id == item.PanchayatId).Result;
                //        if (pac != null)
                //        {
                //            item.PanchayatName = pac.Name;
                //        }

                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllCenters", ex);
                throw ex;
            }
            return centers;
        }

        public async Task<List<Center>> GetStudentAttendanceOfCenter(int status)
        {
            logger.LogInformation($"UserRepository : GetAllClasses : Started");

            List<Center> centers = new List<Center>();
            try
            {
                if (status != (int)(Constant.ClassStatus.Upcoming))
                {
                    centers = await (from c in appDbContext.Class
                                     join cen in appDbContext.Center
                                     on c.CenterId equals cen.Id
                                     //join s in appDbContext.Student
                                     //on cen.Id equals s.CenterId
                                     where c.Status == status && c.StartedDate.Value.Date == DateTime.UtcNow.Date
                                     select new Center
                                     {
                                         Id = c.Id,
                                         CenterName = cen.CenterName,
                                         TeacherName = appDbContext.Users.FirstOrDefault(x => x.Id == cen.AssignedTeachers).Name,
                                         TotalPresentStudents = c.AvilableStudents,
                                         TotalActiveStudents = c.TotalStudents,
                                         TotalStudents = appDbContext.Student.Where(x => x.CenterId == c.Id).AsNoTracking().ToList().Count
                                     }).ToListAsync();


                }
                else
                {
                    List<int> centerIds=appDbContext.Class.Where(x=>x.StartedDate==DateTime.UtcNow.Date).Select(x=>x.CenterId).ToList();

                    centers=await appDbContext.Center.Where(x=> !centerIds.Contains(x.Id)).ToListAsync();

                    centers = await (from c in appDbContext.Center
                                     join cen in appDbContext.Class
                                     on c.Id equals cen.CenterId
                                     where !centerIds.Contains(c.Id)
                                     select new Center
                                     {
                                         Id = c.Id,
                                         CenterName = c.CenterName,
                                         //TeacherName = appDbContext.Users.FirstOrDefault(x => x.Id == cen.AssignedTeachers).Name,
                                         //TotalPresentStudents = c.AvilableStudents,
                                         //TotalActiveStudents = c.TotalStudents,
                                         //TotalStudents = appDbContext.Student.Where(x => x.CenterId == c.Id).AsNoTracking().ToList().Count
                                     }).ToListAsync();
                }

                logger.LogInformation($"UserRepository : GetAllClasses : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllClasses", ex);
                throw ex;
            }
            return centers;
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
