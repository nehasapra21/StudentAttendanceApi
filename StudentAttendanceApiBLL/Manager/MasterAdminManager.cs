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
    public class MasterAdminManager:IMasterAdminManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly IMasterAdminRepository _masterAdminRepository;

        #endregion

        #region | Controller |

        public MasterAdminManager(IMasterAdminRepository masterAdminRepository,
                                ILogger<MasterAdminManager> logger)
        {
            _masterAdminRepository = masterAdminRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

        public async Task<List<MasterAdmin>> GetAllMasterAdmin()
        {
            _logger.LogInformation($"MasterAdminManager : Bll : GetAllMasterAdmin : Started");
            var masterAdmins = await _masterAdminRepository.GetAllMasterAdmin();
            _logger.LogInformation($"MasterAdminManager : Bll : GetAllMasterAdmin : End");
            return masterAdmins;
        }

        public async Task<MasterAdmin> SaveMasterAdmin(MasterAdmin masterAdmin)
        {
            _logger.LogInformation($"MasterAdminManager : Bll : SaveMasterAdmin : Started");

            return await _masterAdminRepository.SaveMasterAdmin(masterAdmin);
        }
        public async Task<MasterAdmin> LoginSuperAdmin(string name,string password)
        {
            _logger.LogInformation($"MasterAdminManager : Bll : LoginSuperAdmin : Started");

            return await _masterAdminRepository.LoginSuperAdmin(name,password);
        }

        #endregion
    }
}
