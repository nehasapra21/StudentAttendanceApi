using Microsoft.Extensions.Logging;
using StudentAttendanceApi.FCM;
using StudentAttendanceApiBLL.Dto;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiBLL.NotificationData1;
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
        private readonly ICenterRepository _centerRepository;
        private readonly IUserRepository _userRepository;
        #endregion

        #region | Controller |

        public HolidaysManager(IHolidaysRepository holidaysRepository, ICenterRepository centerRepository,
                                ILogger<HolidaysManager> logger)
        {
            _holidaysRepository = holidaysRepository;
            _logger = logger;
            _centerRepository = centerRepository;
        }

        #endregion

        #region | Public Methods |

    
        public async Task<NotificationModel> SaveHolidays(Holidays holidays)
        {
            _logger.LogInformation($"UserManager : Bll : SaveHolidays : Started");
            NotificationDto dto = new NotificationDto();
            string RegionalToken = string.Empty;
            string TeacherToken = string.Empty;
            Holidays holidays1= await _holidaysRepository.SaveHolidays(holidays);
            Center center = await _centerRepository.GetCenteryId(holidays1.CenterId.Value);
            if (center != null)
            {
                dto.TeacherStatus = true;
                dto.RegionaladminStatus = true;
                RegionalToken = await _userRepository.GetUserDeviceByUserId(center.AssignedRegionalAdmin.Value);
                TeacherToken = await _userRepository.GetUserDeviceByUserId(center.AssignedTeachers.Value);
            }
            dto.CenterId = holidays1.CenterId;
            dto.StartingDate = holidays1.StartDate;
            dto.EndingDate = holidays1.EndDate;
            dto.RegionalToken = RegionalToken;
            dto.TeacherToken = TeacherToken;
            dto.Type = 2;
            dto.SuperAdminStatus = true;

            SendNotificationClass sendNotification = new SendNotificationClass(_userRepository);
            NotificationModel model = await sendNotification.SendNotificationType(dto, false);

            return model;
        }

        public async Task<List<Holidays>> GetAllHolidaysByTeacherId(int teacherId)
        {
            _logger.LogInformation($"UserManager : Bll : SaveHolidays : Started");


            return await _holidaysRepository.GetAllHolidaysByTeacherId(teacherId);
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

        public async Task<List<Holidays>> GetAllHolidays(int userId,int type)
        {
            _logger.LogInformation($"UserManager : Bll : GetAllHolidaysByCenterId : Started");


            return await _holidaysRepository.GetAllHolidays(userId,type);
        }
        #endregion
    }
}
