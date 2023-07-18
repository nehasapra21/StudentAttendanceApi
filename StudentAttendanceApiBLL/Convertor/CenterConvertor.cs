using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class CenterConvertor
    {
        public static Center ConvertCentertoToCenter(CenterDto centerDto)
        {
            Center center= new Center();
            center.Id = centerDto.Id;
            center.CenterGuidId = centerDto.CenterGuidId;
            center.CenterName = centerDto.CenterName;
            center.AssignedRegionalAdmin = centerDto.AssignedRegionalAdmin;
            center.AssignedTeachers = centerDto.AssignedTeachers;
            center.CreatedDate = centerDto.CreatedDate;
            center.DistrictId = centerDto.DistrictId;
            center.VidhanSabhaId = centerDto.VidhanSabhaId;
            center.VillageId = centerDto.VillageId;
            center.PanchayatId = centerDto.PanchayatId;
            center.ClassStatus = centerDto.ClassStatus;
            center.Status = centerDto.Status;
            ;

            return center;

        }
    }
}
