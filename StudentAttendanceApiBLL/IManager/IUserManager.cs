using Microsoft.AspNetCore.Mvc;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IUserManager
    {
        Task<Users> LoginUser(string name, string password);
        Task<UserDto> SaveLogin(Users user);
        //Task<Users> SaveSuperAdmin(Users users);
        Task<object> GetUserById(int userId);
        Task<string> GetUserDeviceByUserId(int userId);
        Task<string> CheckUserMobileNumber(string mobileNumber);
        Task<List<TeacherDto>> GetAllTeachers();
        Task<List<TeacherDto>> GetAllUnAssignedTeacher();
        Task<List<RegionalAdminDto>> GetAllRegionalAdmins();
        Task<Users> UpdateDeviceId(int userId,string deviceId);
    }

}
