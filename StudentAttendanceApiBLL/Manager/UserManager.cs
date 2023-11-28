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
        private readonly ICenterRepository _centerRepository;

        #endregion

        #region | Controller |

        public UserManager(IUserRepository userRepository, ICenterRepository centerRepository,
                                ILogger<UserManager> logger)
        {
            _userRepository = userRepository;
            _centerRepository = centerRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

        public async Task<Dictionary<int,object>> LoginUser(string name, string password)
        {
            _logger.LogInformation($"UserManager : Bll : LoginSuperAdmin : Started");

            Dictionary<int, object> userData = new Dictionary<int, object>();
            string message = string.Empty;
            string pass = EncryptionUtility.GetHashPassword(password);
            Users user = await _userRepository.LoginUser(name, pass);
            if (user != null)
            {
                if (user.Type == 2)//teacher
                {
                    //check center active or not
                    bool centerStatus = await _centerRepository.CheckCenterStatusByUserId(user.Id);
                    if (centerStatus)
                    {
                        //if center active 
                        userData.Add(1, user);//Login succesfully
                    }
                    else
                    {
                        userData.Add(2, null);//User inactive                    }
                    }
                }
                else
                {
                    userData.Add(1, user);
                }
                user.Password = null;
            }
            else
            {
                userData.Add(3, null);//"invalid credential"
            }
            
            return userData;
        }

        public async Task<Users> UpdateDeviceId(int userId, string deviceId)
        {
            _logger.LogInformation($"UserManager : Bll : LoginSuperAdmin : Started");

            return await _userRepository.UpdateDeviceId(userId, deviceId);
        }

        public async Task<List<string>> GetPassword(List<string> names)
        {
            List<string> strings = new List<string>();
            if (names.Count > 0)
            {
                foreach (var item in names)
                {
                    string pass = EncryptionUtility.GetHashPassword(item);
                    strings.Add(pass);
                }
            }
            return strings;
        }
        public async Task<string> GetPasswordVal(string password)
        {
            List<string> strings = new List<string>();

            string pass = EncryptionUtility.GetHashPassword(password);

            return password;
        }

        public async Task<UserDto> SaveLogin(UserDto userDto)
        {
            _logger.LogInformation($"UserManager : Bll : SaveSuperAdmin : Started");

            Users user = null;
            //Users user = UserConvertor.ConvertUsertoToUser(userDto);
            if (userDto.Id == 0)
            {
                userDto.EnrolmentRollId = userDto.Name.Substring(0, 2) + "-" + userDto.DateOfBirth + "-";
                if (userDto.Gender == Constant.Gender.Male.ToString())
                {
                    userDto.EnrolmentRollId = userDto.EnrolmentRollId + "M";
                }
                else
                {
                    userDto.EnrolmentRollId = userDto.EnrolmentRollId + "F";
                }
                string pass = EncryptionUtility.GetHashPassword(userDto.Password);

                userDto.Password = pass;

                user = UserConvertor.ConvertUsertoToUser(userDto);
            }
            else
            {
                user = await _userRepository.GetOnlyUserById(userDto.Id);
                if (user != null)
                {
                    userDto.Type = user.Type;
                    if (userDto.Type == 1)//superadmin can chnage anything
                    {
                        userDto.EnrolmentRollId = user.EnrolmentRollId;
                        userDto.Password = user.Password;
                        userDto.CreatedOn = user.CreatedOn;
                        userDto.Status = user.Status;
                        user = UserConvertor.ConvertUsertoToUser(userDto);
                    }
                    else
                    {
                        user = UserConvertor.ConvertUpdateUsertoToUser(userDto, user);
                    }

                }

            }

            Users saveUser = await _userRepository.SaveLogin(user);
            UserDto saveUserDto = new UserDto();
            if (saveUserDto != null)
            {
                saveUserDto = UserConvertor.ConvertUserToUserDto(saveUser);
            }

            return saveUserDto;
        }

        public async Task<UserDto> UpdateSuperAdminUser(UserDto userDto)
        {
            _logger.LogInformation($"UserManager : Bll : SaveSuperAdmin : Started");

            Users user = null;
            //Users user = UserConvertor.ConvertUsertoToUser(userDto);

            user = await _userRepository.GetOnlyUserById(userDto.Id);
            if (user != null)
            {
                userDto.Type = user.Type;
                userDto.EnrolmentRollId = user.EnrolmentRollId;
                userDto.Password = user.Password;
                userDto.CreatedOn = user.CreatedOn;
                userDto.Status = user.Status;
                user = UserConvertor.ConvertUsertoToUser(userDto);
            }

            Users saveUser = await _userRepository.SaveLogin(user);
            UserDto saveUserDto = new UserDto();
            if (saveUserDto != null)
            {
                saveUserDto = UserConvertor.ConvertUserToUserDto(saveUser);
            }

            return saveUserDto;
        }

        //public async Task<Users> SaveSuperAdmin(Users user)
        //{
        //    _logger.LogInformation($"UserManager : Bll : SaveSuperAdmin : Started");

        //    if (user.Id == 0)
        //    {
        //        user.EnrolmentRollId = user.Name.Substring(0, 2) + "-" + user.DateOfBirth + "-";
        //        if (user.Gender == Constant.Gender.Male.ToString())
        //        {
        //            user.EnrolmentRollId = user.EnrolmentRollId + "M";
        //        }
        //        else
        //        {
        //            user.EnrolmentRollId = user.EnrolmentRollId + "F";
        //        }
        //    }
        //    string pass = string.Empty;
        //    if (user.Id == 0)
        //    {
        //        pass = EncryptionUtility.GetHashPassword(user.Password);
        //        user.Password = pass;
        //    }
        //    return await _userRepository.SaveLogin(user);
        //}

        public async Task<object> GetUserById(int userId)
        {
            _logger.LogInformation($"UserManager : Bll : GetUserById : Started");

            SuperAdminDetailDto superAdminDetailDto = null;
            RegionalAdminDetailDto regionalAdminDetailDto = null;
            TeacherDetailDto teacherDetailDto = null;
            Users user = await _userRepository.GetUserById(userId);

            if (user != null)
            {
                if (user.Type == (int)Constant.Type.Teacher)
                {
                    teacherDetailDto = new TeacherDetailDto();
                    teacherDetailDto = UserConvertor.ConvertUserToTeacherDetailDto(user);

                    return teacherDetailDto;
                }
                else if (user.Type == (int)Constant.Type.RegionalAdmin)
                {
                    regionalAdminDetailDto = new RegionalAdminDetailDto();
                    regionalAdminDetailDto = UserConvertor.ConvertUserToRegionalAdminDetailDto(user);
                    return regionalAdminDetailDto;
                }
                else
                {
                    superAdminDetailDto = new SuperAdminDetailDto();
                    superAdminDetailDto = UserConvertor.ConvertUserToSuperAdminDetailDto(user);
                    return superAdminDetailDto;
                }

                //userDto.CenterEnrollmentDate = user.Center!=null?user.Center.StartedDate:null;
            }
            return null;
        }

        public async Task<string> CheckUserMobileNumber(string mobileNumber)
        {
            _logger.LogInformation($"UserManager : Bll : CheckUserMobileNumber : Started");


            return await _userRepository.CheckUserMobileNumber(mobileNumber);
        }
        public async Task<string> GetUserDeviceByUserId(int userId)
        {
            _logger.LogInformation($"UserManager : Bll : CheckUserMobileNumber : Started");


            return await _userRepository.GetUserDeviceByUserId(userId);

        }
        public async Task<List<TeacherDto>> GetAllTeachers(int userId)
        {
            _logger.LogInformation($"UserManager : Bll : GetAssignedTeachers : Started");
            List<Users> users = await _userRepository.GetAllTeachers(userId);
            List<TeacherDto> teacher = null;
            if (users != null)
            {
                teacher = new List<TeacherDto>();
                foreach (var item in users)
                {
                    TeacherDto teacherDto = new TeacherDto();
                    teacherDto.Id = item.Id;
                    teacherDto.Name = item.Name;
                    teacherDto.Profile = item.Picture;
                    teacherDto.PhoneNumber = item.PhoneNumber;
                    teacherDto.Assigned = item.AssignedTeacherStatus == null ? false : item.AssignedTeacherStatus;
                    teacher.Add(teacherDto);
                }
            };

            return teacher;
        }


        public async Task<List<TeacherDto>> GetAllUnAssignedTeacher()
        {
            _logger.LogInformation($"UserManager : Bll : GetAllTeachers : Started");
            List<TeacherDto> teacher = new List<TeacherDto>();
            List<Users> users = await _userRepository.GetAllUnAssignedTeacher();
            foreach (var item in users)
            {
                TeacherDto teacherDto = new TeacherDto();
                teacherDto.Id = item.Id;
                teacherDto.Name = item.Name;
                teacherDto.Profile = item.Picture;
                teacherDto.Assigned = item.AssignedTeacherStatus;
                teacherDto.PhoneNumber = item.PhoneNumber;
                teacher.Add(teacherDto);
            }
            return teacher;
        }
        public async Task<List<RegionalAdminDto>> GetAllRegionalAdmins()
        {
            _logger.LogInformation($"UserManager : Bll : GetAllRegionalAdmins : Started");
            List<RegionalAdminDto> regionalAdminDtos = new List<RegionalAdminDto>();
            List<Users> users = await _userRepository.GetAllRegionalAdmins();
            foreach (var item in users)
            {
                RegionalAdminDto regionalAdminDto = new RegionalAdminDto();
                regionalAdminDto.Id = item.Id;
                regionalAdminDto.Name = item.Name;
                regionalAdminDto.Profile = item.Picture;
                regionalAdminDtos.Add(regionalAdminDto);
            }
            return regionalAdminDtos;
        }


        public async Task<List<object>> SearchData(string type, string queryString)
        {
            _logger.LogInformation($"UserManager : Bll : SearchData : Started");

            return await _userRepository.SearchData(type, queryString);
        }
        #endregion
    }
}
