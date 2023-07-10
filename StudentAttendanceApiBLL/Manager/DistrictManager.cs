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
    public class DistrictManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly IDistrictRepository _districtRepository;

        #endregion

        #region | Controller |

        public DistrictManager(IDistrictRepository districtRepository,
                                ILogger<DistrictManager> logger)
        {
            _districtRepository = districtRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

        public async Task<List<District>> GetAllDistrict()
        {
            _logger.LogInformation($"MasterAdminManager : Bll : GetAllDistrict : Started");
            var district = await _districtRepository.GetAllDistrict();
            _logger.LogInformation($"MasterAdminManager : Bll : GetAllDistrict : End");
            return district;
        }
        #endregion
    }
}
