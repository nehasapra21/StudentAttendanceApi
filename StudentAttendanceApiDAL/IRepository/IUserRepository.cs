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
        Task<Users?> LoginSuperAdmin(string name, string password);
        Task<Users> SaveSuperAdmin(Users user);
    }
}
