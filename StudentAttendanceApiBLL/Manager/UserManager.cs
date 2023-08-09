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
            Users user=await _userRepository.LoginUser(name, pass);
            if(user!=null)
            {

                user.Password = null;
            }
            return user;
        }

        public async Task<UserDto> SaveLogin(Users user)
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
                string pass = string.Empty;
                pass = EncryptionUtility.GetHashPassword(user.Password);
                user.Password = pass;
            }
            
           
            Users saveUser= await _userRepository.SaveLogin(user);
            UserDto userDto = new UserDto();
            if(saveUser!=null)
            {
                userDto=UserConvertor.ConvertUserToUserDto(saveUser);
            }

            return userDto;
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
                if(user.Type==(int)Constant.Type.Teacher)
                {
                    teacherDetailDto = new TeacherDetailDto();
                    teacherDetailDto = UserConvertor.ConvertUserToTeacherDetailDto(user);
                  
                    return teacherDetailDto;
                }
                else if(user.Type == (int)Constant.Type.RegionalAdmin)
                {
                    regionalAdminDetailDto = new RegionalAdminDetailDto();
                    regionalAdminDetailDto = UserConvertor.ConvertUserToRegionalAdminDetailDto(user);
                    return regionalAdminDetailDto;
                }
                else
                {
                    superAdminDetailDto=new SuperAdminDetailDto();
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

        public async Task<List<TeacherDto>> GetAllTeachers()
        {
            _logger.LogInformation($"UserManager : Bll : GetAssignedTeachers : Started");
            List<Users> users = await _userRepository.GetAllTeachers();
            List<TeacherDto> teacher = null;
            if (users!=null)
            {
                teacher = new List<TeacherDto>();
                foreach (var item in users)
                {
                    TeacherDto teacherDto = new TeacherDto();
                    teacherDto.Id = item.Id;
                    teacherDto.Name = item.Name;
                    teacherDto.Profile = item.Picture;
                    teacherDto.Assigned = item.AssignedTeacherStatus==null?false:item.AssignedTeacherStatus;
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
        #endregion
    }
}
