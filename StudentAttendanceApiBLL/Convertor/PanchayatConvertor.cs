using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class PanchayatConvertor
    {
        public static Panchayat ConvertPanchayatDtoToPanchayat(PanchayatDto panchayatDto)
        {
            Panchayat panchayat = new Panchayat();
            panchayat.Id = panchayatDto.Id;
            panchayat.PanchayatGuidId = panchayatDto.PanchayatGuidId;
            panchayat.Name = panchayatDto.Name;
            panchayat.Status = panchayatDto.Status;
            panchayat.VidhanSabhaId = panchayatDto.VidhanSabhaId;
            panchayat.DistrictId = panchayatDto.DistrictId;
            panchayat.CreatedBy = panchayatDto.CreatedBy;
            panchayat.CreatedOn = panchayatDto.CreatedOn;

            return panchayat;

        }
    }
}
