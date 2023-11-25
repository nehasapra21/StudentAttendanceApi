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

        public async  Task<dynamic> GetClassCountByMonth(int centerId, int month)
        {
            _logger.LogInformation($"DashboardManager : Bll : GetClassCountByMonth : Started");
            string village = await _dashboardRepository.GetClassCountByMonth(centerId, month);
            _logger.LogInformation($"DashboardManager : Bll : GetClassCountByMonth : End");
            return village;
        }

        public async Task<dynamic> GetTotalGenderRatioByCenterId(int centerId)
        {
            _logger.LogInformation($"DashboardManager : Bll : GetTotalGenterRatioByCenterId : Started");
            dynamic village = await _dashboardRepository.GetTotalGenderRatioByCenterId(centerId);
            _logger.LogInformation($"DashboardManager : Bll : GetTotalGenterRatioByCenterId : End");
            return village;
        }

        public async Task<dynamic> GetTotalStudentOfClass(int centerId)
        {
            _logger.LogInformation($"DashboardManager : Bll : GetTotalStudentOfClass : Started");
            dynamic village = await _dashboardRepository.GetTotalStudentOfClass(centerId);
            _logger.LogInformation($"DashboardManager : Bll : GetTotalStudentOfClass : End");
            return village;
        }


        #endregion
    }
}
