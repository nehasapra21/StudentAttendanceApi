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
        Task<Users> SaveSuperAdmin(Users users);
        Task<Users> GetUserById(int userId,int type);
        Task<string> CheckUserMobileNumber(string mobileNumber);
        Task<List<Users>> GetRegisteredTeachers();
        Task<List<Users>> GetAllRegionalAdmins();
    }
}
