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
    public class HolidaysRepository : IHolidaysRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public HolidaysRepository(AppDbContext appDbContext, ILogger<HolidaysRepository> logger, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<Holidays> SaveHolidays(Holidays holidays)
        {
            logger.LogInformation($"UserRepository : SaveHolidays : Started");

            try
            {
                if (holidays.Id > 0)
                {
                    appDbContext.Entry(holidays).State = EntityState.Modified;
                }
                else
                {
                    if (holidays.CenterIds.Count > 0)
                    {
                        foreach (var item in holidays.CenterIds)
                        {
                            Holidays hol = new Holidays();
                            hol.StartDate = holidays.StartDate;
                            hol.EndDate = holidays.EndDate;
                            hol.Name = holidays.Name;
                            hol.Status = holidays.Status;
                            hol.CenterId = item;
                            hol.CreatedOn = DateTime.Now;
                            hol.CreatedBy = holidays.CreatedBy;
                            appDbContext.Holidays.Add(hol);
                        }
                    }

                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveHolidays : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveHolidays ", ex);
                throw ex;
            }
            return holidays;
        }

        public async Task<List<Holidays>> GetAllHolidaysByTeacherId(int teacherId)
        {
            logger.LogInformation($"DistrictRepository : GetAllHolidaysByTeacherId : Started");
            List<Holidays> holidays = new List<Holidays>();
            try
            {
                holidays = await (from h in appDbContext.Holidays
                                  join c in appDbContext.Center
                                  on h.CenterId equals c.Id
                                  where c.AssignedTeachers == teacherId &&
                                  (h.StartDate.Value.Date >= DateTime.Now.Date && h.EndDate.Value.Date <= DateTime.Now.Date)
                                  select new Holidays
                                  {
                                      Id = h.Id,
                                      Name = h.Name,
                                      CenterId = c.Id,
                                      Description = h.Description
                                  }).ToListAsync();

                logger.LogInformation($"DistrictRepository : GetAllHolidaysByTeacherId : End");
                return holidays.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictRepository : GetAllHolidaysByTeacherId ", ex);
            }

            return holidays;
        }

        public async Task<List<Holidays>> GetAllHolidaysByYear(int year)
        {
            logger.LogInformation($"DistrictRepository : GetAllHolidaysByYear : Started");
            List<Holidays> holidays = new List<Holidays>();
            try
            {

                holidays = await appDbContext.Holidays.AsNoTracking().Where(x => x.StartDate.Value.Year == year).ToListAsync();

                logger.LogInformation($"DistrictRepository : GetAllHolidaysByYear : End");
                return holidays.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictRepository : GetAllHolidaysByYear ", ex);
            }

            return holidays;
        }

        public async Task<List<Holidays>> GetAllHolidays(int userId, int type)
        {
            logger.LogInformation($"DistrictRepository : GetAllHolidaysByYear : Started");
            List<Holidays> holidays = new List<Holidays>();
            try
            {

                if (type == 1)//Admin
                {
                    holidays = await appDbContext.Holidays.AsNoTracking().Where(x => x.CreatedBy == userId).ToListAsync();
                }
                else
                {
                    holidays = await appDbContext.Holidays.AsNoTracking().ToListAsync();
                }


                logger.LogInformation($"DistrictRepository : GetAllHolidaysByYear : End");
                return holidays.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictRepository : GetAllHolidaysByYear ", ex);
            }

            return holidays;
        }

        public async Task<List<Holidays>> GetAllHolidaysByCenterId(int centerId)
        {
            logger.LogInformation($"DistrictRepository : GetAllHolidaysByCenterId : Started");
            List<Holidays> holidays = new List<Holidays>();
            try
            {

                holidays = await appDbContext.Holidays.AsNoTracking().Where(x => x.CenterId == centerId).ToListAsync();

                logger.LogInformation($"DistrictRepository : GetAllHolidaysByCenterId : End");
                return holidays.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictRepository : GetAllHolidaysByCenterId ", ex);
            }

            return holidays;
        }

    }
}
