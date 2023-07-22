using Microsoft.Extensions.Logging;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiDAL;
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

        public async Task<Users> LoginUser(string name, string password)
        {
            _logger.LogInformation($"UserManager : Bll : LoginSuperAdmin : Started");

            string pass = EncryptionUtility.GetHashPassword(password);

            return await _userRepository.LoginUser(name, pass);
        }

        public async Task<Users> SaveLogin(Users user)
        {
            _logger.LogInformation($"UserManager : Bll : SaveSuperAdmin : Started");
           
            if (user.Id == 0)
            {
                user.EnrolmentRollId = user.Name.Substring(0, 2) + "-" + user.DateOfBirth + "-";
                if (user.Gender == Constant.Gender.Male.ToString())
                {
                    user.EnrolmentRollId = user.EnrolmentRollId + "M";
                }
                else
                {
                    user.EnrolmentRollId = user.EnrolmentRollId + "F";
                }
            }
            
            string pass = EncryptionUtility.GetHashPassword(user.Password);
            user.Password = pass;
            return await _userRepository.SaveLogin(user);
        }

        public async Task<Users> SaveSuperAdmin(Users user)
        {
            _logger.LogInformation($"UserManager : Bll : SaveSuperAdmin : Started");
           
            if (user.Id == 0)
            {
                user.EnrolmentRollId = user.Name.Substring(0, 2) + "-" + user.DateOfBirth + "-";
                if (user.Gender == Constant.Gender.Male.ToString())
                {
                    user.EnrolmentRollId = user.EnrolmentRollId + "M";
                }
                else
                {
                    user.EnrolmentRollId = user.EnrolmentRollId + "F";
                }
            }

            string pass = EncryptionUtility.GetHashPassword(user.Password);
            user.Password = pass;
            return await _userRepository.SaveLogin(user);
        }

        public async Task<Users> GetUserById(int userId)
        {
            _logger.LogInformation($"UserManager : Bll : GetUser : Started");

            return await _userRepository.GetUserById(userId);
        }

        public async Task<string> CheckUserMobileNumber(string mobileNumber)
        {
            _logger.LogInformation($"UserManager : Bll : CheckUserMobileNumber : Started");

            return await _userRepository.CheckUserMobileNumber(mobileNumber);
        }

        public async Task<List<Users>> GetRegisteredTeachers()
        {
            _logger.LogInformation($"UserManager : Bll : GetRegisteredTeachers : Started");

            return await _userRepository.GetRegisteredTeachers();
        }

        public async Task<List<Users>> GetAllRegionalAdmins()
        {
            _logger.LogInformation($"UserManager : Bll : GetAllRegionalAdmins : Started");

            return await _userRepository.GetAllRegionalAdmins();
        }
        #endregion
    }
}
