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

            CenterDetailDto centerDetailDto = null;
            Center center = await _centerRepository.GetCenteryId(centerId);
            if (center != null)
            {
                centerDetailDto = new CenterDetailDto();
                centerDetailDto = CenterConvertor.ConvertCentertoToCenterDetailDto(center);
            }

            return centerDetailDto;
        }
        public async Task<string> CheckCenterName(string name)
        {
            _logger.LogInformation($"VillageManager : Bll : CheckCenterName : Started");

            return await _centerRepository.CheckCenterName(name);
        }

        public async Task<List<AllCenterDto>> GetAllCenters(int userId, int type)
        {
            _logger.LogInformation($"VillageManager : Bll : GetAllCenters : Started");
            List<AllCenterDto> list = null;
            List<Center> centers = await _centerRepository.GetAllCenters(userId,type);
            if (centers != null && centers.Count > 0)
            {
                list=new List<AllCenterDto>();
                foreach (var item in centers)
                {
                    AllCenterDto allCenterDto = CenterConvertor.ConvertCenterToAllCenterDto(item);
                    list.Add(allCenterDto);
                }
            }
            return list;
        }

        public async Task<List<AllCenterStatusDto>> GetStudentAttendanceOfCenter(int status, int userId, int type)
        {
            _logger.LogInformation($"VillageManager : Bll : GetStudentAttendanceOfCenter : Started");
            List<AllCenterStatusDto> list = null;
            List<Center> centers= await _centerRepository.GetStudentAttendanceOfCenter(status,userId,type);
            if (centers != null && centers.Count > 0)
            {
                list = new List<AllCenterStatusDto>();
                foreach (var item in centers)
                {
                    AllCenterStatusDto allCenterDto = CenterConvertor.ConvertCenterToAllCenterStatusDto(item);
                    list.Add(allCenterDto);
                }
            }
            return list;
        }

        public async Task<List<Center>> GetAllCentersById(int districtId, int vidhanSabhaId, int panchayatId, int villageId)
        {
            _logger.LogInformation($"VillageManager : Bll : GetAllCentersById : Started");

            return await _centerRepository.GetAllCentersById(districtId, vidhanSabhaId, panchayatId, villageId);
        }
        public async Task<CenterDetailDto> GetCenterByUserId(int userId)
        {
            _logger.LogInformation($"VillageManager : Bll : GetAllCentersById : Started");
            CenterDetailDto centerDetailDto = null;
            Center center = await _centerRepository.GetCenterByUserId(userId);
            if (center != null)
            {
                centerDetailDto=new CenterDetailDto();
                centerDetailDto = CenterConvertor.ConvertCentertoToCenterDetailDto(center);
            }

            return centerDetailDto;
        }
        #endregion
    }
}
