using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
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
    public class DashboardManager : IDashboardManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly IDashboardRepository _dashboardRepository;

        #endregion

        #region | Controller |

        public DashboardManager(IDashboardRepository dashboardRepository,
                                ILogger<DashboardManager> logger)
        {
            _dashboardRepository = dashboardRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

        public async Task<string> GetClassCountByMonth(int centerId, int month)
        {
            _logger.LogInformation($"DashboardManager : Bll : GetClassCountByMonth : Started");
            string village = await _dashboardRepository.GetClassCountByMonth(centerId, month);
            _logger.LogInformation($"DashboardManager : Bll : GetClassCountByMonth : End");
            return village;
        }

        public async Task<string> GetTotalGenderRatioByCenterId(int centerId)
        {
            _logger.LogInformation($"DashboardManager : Bll : GetTotalGenterRatioByCenterId : Started");
            string village = await _dashboardRepository.GetTotalGenderRatioByCenterId(centerId);
            _logger.LogInformation($"DashboardManager : Bll : GetTotalGenterRatioByCenterId : End");
            return village;
        }

        public async Task<string> GetTotalStudentOfClass(int centerId)
        {
            _logger.LogInformation($"DashboardManager : Bll : GetTotalStudentOfClass : Started");
            string village = await _dashboardRepository.GetTotalStudentOfClass(centerId);
            _logger.LogInformation($"DashboardManager : Bll : GetTotalStudentOfClass : End");
            return village;
        }

        public async Task<string> GetCenterDetailByMonth(int centerId, int month, int year)
        {
            _logger.LogInformation($"DashboardManager : Bll : GetCenterDetailByMonth : Started");
            string data = await _dashboardRepository.GetCenterDetailByMonth(centerId, month, year);
            _logger.LogInformation($"DashboardManager : Bll : GetCenterDetailByMonth : End");
            return data;
        }

        public async Task<string> GetTotalBpl(int centerId, bool BplValue)
        {
            _logger.LogInformation($"DashboardManager : Bll : GetCenterDetailByMonth : Started");
            string data = await _dashboardRepository.GetTotalBpl(centerId, BplValue);
            _logger.LogInformation($"DashboardManager : Bll : GetCenterDetailByMonth : End");
            return data;
        }

        public async Task<string> GetTotalStudentCategoryOfClass(int centerId)
        {
            _logger.LogInformation($"DashboardManager : Bll : GetTotalStudentCategoryOfClass : Started");
            string data = await _dashboardRepository.GetTotalStudentCategoryOfClass(centerId);
            _logger.LogInformation($"DashboardManager : Bll : GetTotalStudentCategoryOfClass : End");
            return data;
        }

        public async Task<string>
        GetUserByFilter(int type, int districtId, int vidhanSabhaId, int panchaytaId, int villageId, DateTime date)
        {
            _logger.LogInformation($"DashboardManager : Bll : GetTotalStudentCategoryOfClass : Started");
            string data = await _dashboardRepository.GetUserByFilter(type, districtId, vidhanSabhaId, panchaytaId, villageId, date);
            _logger.LogInformation($"DashboardManager : Bll : GetTotalStudentCategoryOfClass : End");
            return data;
        }

        #endregion
    }
}
