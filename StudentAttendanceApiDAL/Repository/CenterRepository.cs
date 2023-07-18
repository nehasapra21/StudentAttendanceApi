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
                    appDbContext.Entry(center).State = EntityState.Modified;
                }
                else
                {
                    center.CreatedDate = DateTime.UtcNow;
                    appDbContext.Center.Add(center);

                    //update assigned teacher status
                    Users user = await appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(center.AssignedTeachers));
                   
                    if (user != null)
                    {
                        user.AssignedTeacherStatus = true;
                    }
                    appDbContext.Users.Update(user);

                }
                await appDbContext.SaveChangesAsync();

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

        public async Task<List<Center>> GetAllCenters()
        {
            logger.LogInformation($"UserRepository : GetAllCenters : Started");

            List<Center> centers = new List<Center>();
            try
            {
                centers = await (from c in appDbContext.Center
                                 join d in appDbContext.District
                                 on c.DistrictId equals d.Id
                                 join v in appDbContext.VidhanSabha
                                 on c.VidhanSabhaId equals v.Id
                                 join p in appDbContext.Panchayat
                                  on c.PanchayatId equals p.Id
                                 select new Center
                                 {
                                     Id = c.Id,
                                     CenterName = c.CenterName,
                                     DistrictName = d.Name,
                                     VidhanSabhaName = v.Name,
                                     PanchayatName = p.Name,
                                     AssignedTeachers = c.AssignedTeachers,
                                     TeacherName = appDbContext.Users.FirstOrDefault(x => x.Id ==             c.AssignedTeachers).Name,
                                     TotalStudents = appDbContext.Student.Where(x => x.CenterId ==                    c.Id).AsNoTracking().ToList().Count
                                     }).ToListAsync();

                
                logger.LogInformation($"UserRepository : GetAllCenters : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllCenters", ex);
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
