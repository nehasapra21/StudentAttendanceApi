using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IVidhanSabhaManager
    {
        Task<List<VidhanSabha>> GetAllVidhanSabha();
        Task<VidhanSabha> SaveVidhanSabha(VidhanSabha vidhanSabha);
    }
}
