﻿using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IDistrictManager
    {
        Task<List<DistrictDto>> GetAllDistrict();
        Task<District> SaveDistrict(District district);
    }

}
