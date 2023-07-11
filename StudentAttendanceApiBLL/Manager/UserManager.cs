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
    public class UserManager : IUserManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;

        #endregion

        #region | Controller |

        public UserManager(IUserRepository userRepository,
                                ILogger<UserManager> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

        public async Task<Users> LoginSuperAdmin(string name, string password)
        {
            _logger.LogInformation($"UserManager : Bll : LoginSuperAdmin : Started");

            string pass = EncryptionUtility.GetHashPassword(password);

            return await _userRepository.LoginSuperAdmin(name, pass);
        }

        public async Task<Users> SaveSuperAdmin(Users user)
        {
            _logger.LogInformation($"UserManager : Bll : SaveSuperAdmin : Started");

            string pass = EncryptionUtility.GetHashPassword(user.Password);
            user.Password = pass;
            return await _userRepository.SaveSuperAdmin(user);
        }
        #endregion
    }
}
