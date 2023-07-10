using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IDistrictRepository
    {
       public Task<List<District>> GetAllDistrict();
       Task<District> SaveDistrict(District district);
    }
}
