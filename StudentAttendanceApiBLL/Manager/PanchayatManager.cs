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
    public class PanchayatManager:IPanchayatManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly IPanchayatRepository _panchayatRepository;

        #endregion

        #region | Controller |

        public PanchayatManager(IPanchayatRepository panchayatRepository,
                                ILogger<PanchayatManager> logger)
        {
            _panchayatRepository = panchayatRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

        public async Task<List<Panchayat>> GetAllPanchayat()
        {
            _logger.LogInformation($"PanchayatManager : Bll : GetAllPanchayat : Started");
            var panchayat = await _panchayatRepository.GetAllPanchayat();
            _logger.LogInformation($"PanchayatManager : Bll : GetAllPanchayat : End");
            return panchayat;
        }

        public async Task<Panchayat> SavePanchayat(Panchayat panchayat)
        {
            _logger.LogInformation($"PanchayatManager : Bll : SavePanchayat : Started");

            return await _panchayatRepository.SavePanchayat(panchayat);
        }

        public async Task<Panchayat> GetPanchayatByDistrictAndVidhanSabhaId(int districtId, int vidhanSabhaId)
        {
            _logger.LogInformation($"PanchayatManager : Bll : GetPanchayatByDistrictAndVidhanSabhaId : Started");

            return await _panchayatRepository.GetPanchayatByDistrictAndVidhanSabhaId(districtId, vidhanSabhaId);
        }

        public async Task<string> CheckPanchayatName(string name)
        {
            _logger.LogInformation($"VillageManager : Bll : CheckPanchayatName : Started");

            return await _panchayatRepository.CheckPanchayatName(name);
        }


        #endregion
    }
}
