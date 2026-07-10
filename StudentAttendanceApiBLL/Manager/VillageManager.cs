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
    public class VillageManager:IVillageManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly IVillageRepository _villageRepository;

        #endregion

        #region | Controller |

        public VillageManager(IVillageRepository villageRepository,
                                ILogger<VillageManager> logger)
        {
            _villageRepository = villageRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

        public async Task<List<Village>> GetAllVillage(int offset, int limit)
        {
            _logger.LogInformation($"VillageManager : Bll : GetAllVillage : Started");
            var village = await _villageRepository.GetAllVillage(offset,limit);
            _logger.LogInformation($"VillageManager : Bll : GetAllVillage : End");
            return village;
        }

        public async Task<Village> SaveVillage(Village village)
        {
            _logger.LogInformation($"VillageManager : Bll : SaveVillage : Started");

            return await _villageRepository.SaveVillage(village);
        }

        public async Task<Village> GetVillageByDistrictVidhanSabhaAndPanchId(int districtId, int vidhanSabhaId, int panchayatId)
        {
            _logger.LogInformation($"VillageManager : Bll : GetVillageByDistrictVidhanSabhaAndPanchId : Started");

            return await _villageRepository.GetVillageByDistrictVidhanSabhaAndPanchId(districtId, vidhanSabhaId, panchayatId);
        }

        public async Task<string> CheckVillageName(string name)
        {
            _logger.LogInformation($"VillageManager : Bll : CheckVillageName : Started");

            return await _villageRepository.CheckVillageName(name);
        }
        #endregion
    }
}
