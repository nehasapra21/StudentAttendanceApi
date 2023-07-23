using Microsoft.Extensions.Logging;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Repository;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.Manager
{
    public class HolidaysManager : IHolidaysManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly IHolidaysRepository _holidaysRepository;

        #endregion

        #region | Controller |

        public HolidaysManager(IHolidaysRepository holidaysRepository,
                                ILogger<HolidaysManager> logger)
        {
            _holidaysRepository = holidaysRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

    
        public async Task<Holidays> SaveHolidays(Holidays holidays)
        {
            _logger.LogInformation($"UserManager : Bll : SaveHolidays : Started");


            return await _holidaysRepository.SaveHolidays(holidays);
        }

        public async Task<List<Holidays>> GetAllHolidaysByTeacherId(int teacherId, string selecteddate)
        {
            _logger.LogInformation($"UserManager : Bll : SaveHolidays : Started");


            return await _holidaysRepository.GetAllHolidaysByTeacherId(teacherId, selecteddate);
        }

        public async Task<List<Holidays>> GetAllHolidaysByYear(int year)
        {
            _logger.LogInformation($"UserManager : Bll : GetAllHolidaysByYear : Started");


            return await _holidaysRepository.GetAllHolidaysByYear(year);
        }


        public async Task<List<Holidays>> GetAllHolidaysByCenterId(int centerId)
        {
            _logger.LogInformation($"UserManager : Bll : GetAllHolidaysByCenterId : Started");


            return await _holidaysRepository.GetAllHolidaysByCenterId(centerId);
        }
        #endregion
    }
}
