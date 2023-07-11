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
    public class RegionalAdminManager:IRegionalAdminManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly IRegionalAdminRepository _masterAdminRepository;

        #endregion

        #region | Controller |

        public RegionalAdminManager(IRegionalAdminRepository masterAdminRepository,
                                ILogger<RegionalAdminManager> logger)
        {
            _masterAdminRepository = masterAdminRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

        public async Task<List<RegionalAdmin>> GetAllRegionalAdmin()
        {
            _logger.LogInformation($"MasterAdminManager : Bll : GetAllMasterAdmin : Started");
            var masterAdmins = await _masterAdminRepository.GetAllRegionalAdmin();
            _logger.LogInformation($"MasterAdminManager : Bll : GetAllMasterAdmin : End");
            return masterAdmins;
        }

        public async Task<RegionalAdmin> SaveRegionalAdmin(RegionalAdmin regionalAdmin)
        {
            _logger.LogInformation($"MasterAdminManager : Bll : SaveMasterAdmin : Started");
            string pass = EncryptionUtility.GetHashPassword(regionalAdmin.Password);
            regionalAdmin.Password = pass;
            return await _masterAdminRepository.SaveRegionalAdmin(regionalAdmin);
        }
        
        #endregion
    }
}
