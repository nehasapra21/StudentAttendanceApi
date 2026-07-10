using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IPanchayatRepository
    {
        Task<List<Panchayat>> GetAllPanchayat(int offset, int limit);
        Task<Panchayat> SavePanchayat(Panchayat panchayat);
        Task<Panchayat> GetPanchayatByDistrictAndVidhanSabhaId(int districtId, int vidhanSabhaId);
        Task<string> CheckPanchayatName(string name);
    }
}
