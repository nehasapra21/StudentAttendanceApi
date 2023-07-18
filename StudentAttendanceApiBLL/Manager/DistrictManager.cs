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
    public class DistrictManager:IDistrictManager
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
            _logger.LogInformation($"DitrictManager : Bll : GetAllDistrict : Started");
            var district = await _districtRepository.GetAllDistrict();
            _logger.LogInformation($"DitrictManager : Bll : GetAllDistrict : End");
            return district;
        }

        public async Task<District> SaveDistrict(District district)
        {
            _logger.LogInformation($"DitrictManager : Bll : district : Started");

            return await _districtRepository.SaveDistrict(district);
        }

        public async Task<string> CheckDistrictName(string name)
        {
            _logger.LogInformation($"VillageManager : Bll : CheckDistrictName : Started");

            return await _districtRepository.CheckDistrictName(name);
        }

        #endregion
    }
}
