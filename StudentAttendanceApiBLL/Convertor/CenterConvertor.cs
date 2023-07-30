﻿using StudentAttendanceApiDAL.Tables;
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
            center.StartedDate = centerDto.StartedDate;
            //  center.CreatedDate = centerDto.CreatedDate;
            center.DistrictId = centerDto.DistrictId;
            center.VidhanSabhaId = centerDto.VidhanSabhaId;
            center.VillageId = centerDto.VillageId;
            center.PanchayatId = centerDto.PanchayatId;
          //  center.ClassStatus = centerDto.ClassStatus;
            //center.Status = centerDto.Status;
            ;

            return center;

        }

        public static CenterDetailDto ConvertCentertoToCenterDetailDto(Center centerDto)
        {
            CenterDetailDto center = new CenterDetailDto();
            center.Id = centerDto.Id;
            center.CenterName = centerDto.CenterName;
            center.EnrollmentDate = centerDto.CreatedDate;
            center.DistrictId = centerDto.DistrictId;
            center.VidhanSabhaId = centerDto.VidhanSabhaId;
            center.VillageId = centerDto.VillageId;
            center.ClassStatus = centerDto.ClassStatus;
            center.Status = centerDto.Status;
            center.PanchayatId = centerDto.PanchayatId;
            center.DistrictId = centerDto.DistrictId;
            center.VidhanSabhaId = centerDto.VidhanSabhaId;
            center.VillageId = centerDto.VillageId;
            center.RegionalAdminId = centerDto.AssignedRegionalAdmin;
            center.DistrictName = centerDto.District != null ? centerDto.District.Name : string.Empty;
            center.VidhanSabhaName = (centerDto.VidhanSabha != null) ? centerDto.VidhanSabha.Name : string.Empty;
            center.PanchayatName = (centerDto.Panchayat != null) ? centerDto.Panchayat.Name : string.Empty;
            center.VillageName = (centerDto.Village != null) ? centerDto.Village.Name : string.Empty;
            center.TotalStudents = centerDto.TotalStudents;
            center.RegionalAdminName = centerDto.RegionalAdminName;
            if(centerDto.User!=null)
            {
                center.Teacher=UserConvertor.ConvertUserToUserDto(centerDto.User);
            }
            return center;

        }
    }
}
