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
        Task<Users> LoginSuperAdmin(string name, string password);
        Task<Users> SaveSuperAdmin(Users user);
    }

}
