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
    public class SchoolManager : ISchoolManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly ISchoolRepository _teacherRepository;

        #endregion

        #region | Controller |

        public SchoolManager(ISchoolRepository teacherRepository,
                                ILogger<SchoolManager> logger)
        {
            _teacherRepository = teacherRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

        public async Task<School> SaveSchool(School school)
        {
            _logger.LogInformation($"UserManager : Bll : LoginSuperAdmin : Started");


            return await _teacherRepository.SaveSchool(school);
        }

        public async Task<List<School>> GetAllSchools()
        {
            _logger.LogInformation($"UserManager : Bll : SaveSuperAdmin : Started");

         
            return await _teacherRepository.GetAllSchools();
        }
        #endregion
    }
}
