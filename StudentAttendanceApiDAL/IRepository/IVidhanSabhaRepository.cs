using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IVidhanSabhaRepository
    {
        Task<List<VidhanSabha>> GetAllVidhanSabha();
        Task<VidhanSabha> SaveVidhanSabha(VidhanSabha vidhanSabha);
        Task<VidhanSabha> GetVidhanSabhaByDistrictId(int districtId);
    }
}
