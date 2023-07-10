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
    public class VidhanSabhaManager:IVidhanSabhaManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly IVidhanSabhaRepository _vidhanRepository;

        #endregion

        #region | Controller |

        public VidhanSabhaManager(IVidhanSabhaRepository vidhanRepository,
                                ILogger<VidhanSabhaManager> logger)
        {
            _vidhanRepository = vidhanRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

        public async Task<List<VidhanSabha>> GetAllVidhanSabha()
        {
            _logger.LogInformation($"VidhanSabhaManager : Bll : GetAllVidhanSabha : Started");
            var vidhanSabha = await _vidhanRepository.GetAllVidhanSabha();
            _logger.LogInformation($"VidhanSabhaManager : Bll : GetAllVidhanSabha : End");
            return vidhanSabha;
        }

        public async Task<VidhanSabha> SaveVidhanSabha(VidhanSabha vidhanSabha)
        {
            _logger.LogInformation($"VidhanSabhaManager : Bll : SaveVidhanSabha : Started");

            return await _vidhanRepository.SaveVidhanSabha(vidhanSabha);
        }

        public async Task<VidhanSabha> GetVidhanSabhaByDistrictId(int districtId)
        {
            _logger.LogInformation($"VidhanSabhaManager : Bll : GetVidhanSabhaByDistrictId : Started");

            return await _vidhanRepository.GetVidhanSabhaByDistrictId(districtId);
        }


        #endregion

    }
}
