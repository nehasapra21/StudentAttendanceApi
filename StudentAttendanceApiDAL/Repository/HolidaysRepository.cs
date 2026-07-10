using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static StudentAttendanceApiDAL.Constant;

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

                    // Get the existing holiday records for the same holiday ID
                    var existingHolidays = appDbContext.Holidays
                        .Where(h => h.Name == holidays.Name)
                        .ToList();

                    // Remove existing holiday entries if CenterIds are removed
                    var centerIdsToRemove = existingHolidays
                        .Where(h => !holidays.CenterIds.Contains(h.CenterId.Value))
                        .ToList();

                    if (centerIdsToRemove.Count > 0)
                    {
                        appDbContext.Holidays.RemoveRange(centerIdsToRemove);
                    }

                    //appDbContext.Entry(holidays).State = EntityState.Modified;
                    // Add new holiday entries if new CenterIds are added
                    var centerIdsToAdd = holidays.CenterIds
                        .Where(id => !existingHolidays.Any(h => h.CenterId == id))
                        .ToList();

                    foreach (var centerId in centerIdsToAdd)
                    {
                        Holidays hol = new Holidays
                        {
                            StartDate = holidays.StartDate,
                            EndDate = holidays.EndDate,
                            Name = holidays.Name,
                            Status = holidays.Status,
                            CenterId = centerId,
                            CreatedOn = DateTime.Now,
                            CreatedBy = holidays.CreatedBy
                        };
                        appDbContext.Holidays.Add(hol);
                    }

                    // Update the existing records that match the remaining CenterIds
                    foreach (var existingHoliday in existingHolidays)
                    {
                        if (holidays.CenterIds.Contains(existingHoliday.CenterId.Value))
                        {
                            existingHoliday.StartDate = holidays.StartDate;
                            existingHoliday.EndDate = holidays.EndDate;
                            existingHoliday.Name = holidays.Name;
                            existingHoliday.Status = holidays.Status;
                            appDbContext.Entry(existingHoliday).State = EntityState.Modified;
                        }
                    }

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

        public async Task<List<Holidays>> GetAllHolidays(int holidayStatus, int userId = 0)
        {
            logger.LogInformation($"DistrictRepository : GetAllHolidaysByYear : Started");
            List<Holidays> holidays = new List<Holidays>();
            try
            {
                if (userId == 0)//SuperAdmin
                {
                    if (holidayStatus == 1)
                    {
                        holidays = await (from h in appDbContext.Holidays
                                          join c in appDbContext.Center
                                          on h.CenterId equals c.Id
                                          select new Holidays
                                          {
                                              Id = h.Id,
                                              Name = h.Name,
                                              StartDate = h.StartDate,
                                              EndDate = h.EndDate,
                                              CreatedBy = h.CreatedBy,
                                              CreatedOn = h.CreatedOn,
                                              CenterId = c.Id,
                                              CenterName = c.CenterName,
                                          }).AsNoTracking().OrderBy(x => x.StartDate).ToListAsync();
                    }
                    else
                    {
                        holidays = await (from h in appDbContext.Holidays
                                          join c in appDbContext.Center
                                          on h.CenterId equals c.Id
                                          where h.StartDate >=                                      DateTime.Today
                                          select new Holidays
                                          {
                                              Id = h.Id,
                                              Name = h.Name,
                                              StartDate = h.StartDate,
                                              EndDate = h.EndDate,
                                              CreatedBy = h.CreatedBy,
                                              CreatedOn = h.CreatedOn,
                                              CenterId = c.Id,
                                              CenterName = c.CenterName,
                                          }).AsNoTracking().OrderBy(x => x.StartDate).ToListAsync();

                    }
                }
                else
                {
                    if (holidayStatus == 1)//All holidays
                    {
                        holidays = await (from h in appDbContext.Holidays
                                          join c in appDbContext.Center
                                          on h.CenterId equals c.Id
                                          where h.CreatedBy == userId
                                          select new Holidays
                                          {
                                              Id = h.Id,
                                              Name = h.Name,
                                              StartDate = h.StartDate,
                                              EndDate = h.EndDate,
                                              CreatedBy = h.CreatedBy,
                                              CreatedOn = h.CreatedOn,
                                              CenterId = c.Id,
                                              CenterName = c.CenterName,
                                          }).AsNoTracking().OrderBy(x => x.StartDate).ToListAsync();
                    }
                    else
                    {
                        //upcoming
                        holidays = await (from h in appDbContext.Holidays
                                          join c in appDbContext.Center
                                          on h.CenterId equals c.Id
                                          where h.CreatedBy == userId
                                          && h.StartDate >= DateTime.Today
                                          select new Holidays
                                          {
                                              Id = h.Id,
                                              Name = h.Name,
                                              StartDate = h.StartDate,
                                              EndDate = h.EndDate,
                                              CreatedBy = h.CreatedBy,
                                              CreatedOn = h.CreatedOn,
                                              CenterId = c.Id,
                                              CenterName = c.CenterName,
                                          }).AsNoTracking().OrderBy(x => x.StartDate).ToListAsync();
                    }

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

        public async Task<Holidays> DeleteHolidayById(int id)
        {
            logger.LogInformation($"UserRepository : GetActiveClass : Started");

            Holidays holidays = null;
            try
            {
                holidays = await appDbContext.Holidays.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (holidays != null)
                {
                    appDbContext.Remove(holidays);
                }

                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : GetAllClasses : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllClasses", ex);
                throw ex;
            }
            return holidays;
        }


    }
}
