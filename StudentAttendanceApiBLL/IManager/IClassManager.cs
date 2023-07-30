using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IClassManager
    {
        Task<Class> SaveClass(Class classval);
        Task<Class> UpdateEndClassTime(Class cls);
        Task<Dictionary<int, int>> GetActiveClass();
        Task<Class> CancelClass(Class cls);
    }

}
