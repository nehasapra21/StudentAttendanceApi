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
    public class TeacherManager : ITeacherManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly ITeacherRepository _teacherRepository;

        #endregion

        #region | Controller |

        public TeacherManager(ITeacherRepository teacherRepository,
                                ILogger<TeacherManager> logger)
        {
            _teacherRepository = teacherRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

        public async Task<Teacher> LoginTeacher(string name, string password)
        {
            _logger.LogInformation($"UserManager : Bll : LoginSuperAdmin : Started");

            string pass = EncryptionUtility.GetHashPassword(password);

            return await _teacherRepository.LoginTeacher(name, pass);
        }

        public async Task<Teacher> SaveTeacher(Teacher teacher)
        {
            _logger.LogInformation($"UserManager : Bll : SaveSuperAdmin : Started");

            string pass = EncryptionUtility.GetHashPassword(teacher.Password);
            teacher.Password = pass;
            return await _teacherRepository.SaveTeacher(teacher);
        }
        #endregion
    }
}
