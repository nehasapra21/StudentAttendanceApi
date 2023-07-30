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
using System.Xml.Linq;

namespace StudentAttendanceApiBLL.Manager
{
    public class CenterManager : ICenterManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly ICenterRepository _centerRepository;

        #endregion

        #region | Controller |

        public CenterManager(ICenterRepository centerRepository,
                                ILogger<CenterManager> logger)
        {
            _centerRepository = centerRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

    
        public async Task<Center> SaveCenter(Center center)
        {
            _logger.LogInformation($"UserManager : Bll : SaveSuperAdmin : Started");

            return await _centerRepository.SaveCenter(center);
        }

        public async Task<CenterDetailDto> GetCenteryId(int centerId)
        {
            _logger.LogInformation($"UserManager : Bll : LoginSuperAdmin : Started");

            CenterDetailDto centerDetailDto = new CenterDetailDto();
            Center center = await _centerRepository.GetCenteryId(centerId);
            if (center != null)
            {
                centerDetailDto=CenterConvertor.ConvertCentertoToCenterDetailDto(center);
            }

            return centerDetailDto;
        }
        public async Task<string> CheckCenterName(string name)
        {
            _logger.LogInformation($"VillageManager : Bll : CheckCenterName : Started");

            return await _centerRepository.CheckCenterName(name);
        }

        public async Task<List<Center>> GetAllCenters()
        {
            _logger.LogInformation($"VillageManager : Bll : GetAllCenters : Started");
            return await _centerRepository.GetAllCenters();
        }

        public async Task<List<Center>> GetStudentAttendanceOfCenter(int status)
        {
            _logger.LogInformation($"VillageManager : Bll : GetStudentAttendanceOfCenter : Started");

            return await _centerRepository.GetStudentAttendanceOfCenter(status);
        }

        public async Task<List<Center>> GetAllCentersById(int districtId, int vidhanSabhaId, int panchayatId, int villageId)
        {
            _logger.LogInformation($"VillageManager : Bll : GetAllCentersById : Started");

            return await _centerRepository.GetAllCentersById( districtId,  vidhanSabhaId,  panchayatId,  villageId);
        }

        #endregion
    }
}
