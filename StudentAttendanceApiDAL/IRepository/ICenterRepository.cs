﻿using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface ICenterRepository
    {
        Task<Center> SaveCenter(Center center);
        Task<string> CheckCenterName(string name);
        Task<Center> GetCenteryId(int centerId);
        Task<List<Center>> GetAllCenters(int userId,int type);
        Task<List<Center>> GetStudentAttendanceOfCenter(int status, int userId, int type);
        Task<Center> GetCenterByUserId(int userId);
        Task<CenterLog> UpdateCenterActiveOrDeactive(CenterLog centerlog);
        Task<bool> CheckCenterStatusByUserId(int userId);
    }
}
