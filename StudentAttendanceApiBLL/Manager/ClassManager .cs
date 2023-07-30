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
    public class ClassManager : IClassManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly IClassRepository _classRepository;

        #endregion

        #region | Controller |

        public ClassManager(IClassRepository classRepository,
                                ILogger<ClassManager> logger)
        {
            _classRepository = classRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

    
        public async Task<Class> SaveClass(Class cls)
        {
            _logger.LogInformation($"UserManager : Bll : SaveClass : Started");

            return await _classRepository.SaveClass(cls);
        }


        public async Task<Class> UpdateEndClassTime(Class cls)
        {
            _logger.LogInformation($"UserManager : Bll : SaveClass : Started");

            return await _classRepository.UpdateEndClassTime(cls);
        }

        public async Task<Dictionary<int, int>> GetActiveClass()
        {
            _logger.LogInformation($"UserManager : Bll : GetActiveClass : Started");

            return await _classRepository.GetActiveClass();
        }

        public async Task<Class> CancelClass(Class cls)
        {
            _logger.LogInformation($"UserManager : Bll : CancelClass : Started");

            return await _classRepository.CancelClass(cls);
        }

        #endregion
    }
}
