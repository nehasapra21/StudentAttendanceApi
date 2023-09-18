﻿using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IVidhanSabhaManager
    {
        Task<List<VidhanSabha>> GetAllVidhanSabha(int offset, int limit);
        Task<VidhanSabha> SaveVidhanSabha(VidhanSabha vidhanSabha);
        Task<VidhanSabha> GetVidhanSabhaByDistrictId(int districtId);
        Task<string> CheckVidhanSabhaName(string name);
    }
}
