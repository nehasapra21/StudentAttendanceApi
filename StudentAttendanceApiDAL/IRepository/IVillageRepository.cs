using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IVillageRepository
    {
        Task<List<Village>> GetAllVillage();
        Task<Village> SaveVillage(Village village);
    }
}
