using Microsoft.AspNetCore.Mvc;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IUserRepository
    {
        Task<Users?> LoginUser(string name, string password);
        Task<Users> SaveLogin(Users user);
        Task<Users> GetOnlyUserById(int userId);
        Task<Users> SaveSuperAdmin(Users users);
        Task<Users> GetUserById(int userId);
        Task<string> GetUserDeviceByUserId(int userId);
        Task<string> CheckUserMobileNumber(string mobileNumber);
        Task<List<Users>> GetAllTeachers(int userId);
        Task<List<Users>> GetAllUnAssignedTeacher();
        Task<List<Users>> GetAllRegionalAdmins();
        Task<Users> UpdateDeviceId(int userId, string deviceId);
        Task<List<object>> SearchData(string type, string queryString);
        //string GetUserTokenByUserId(int userId);
        Task<List<string>> GetAllSuperAdminToken();

    }
}
