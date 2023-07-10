using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class VidhanSabhaConvertor
    {
        public static VidhanSabha ConvertVidhanSabhaDtoToVidhanSabha(VidhanSabhaDto vidhanSabhaDto)
        {
            VidhanSabha vidhanSabha = new VidhanSabha();
            vidhanSabha.Id = vidhanSabhaDto.Id;
            vidhanSabha.VidhanSabhaGuidId = vidhanSabhaDto.VidhanSabhaGuidId;
            vidhanSabha.Name = vidhanSabhaDto.Name;
            vidhanSabha.Status = vidhanSabhaDto.Status;
            vidhanSabha.DistrictId = vidhanSabhaDto.DistrictId;
            vidhanSabha.CreatedBy = vidhanSabhaDto.CreatedBy;
            vidhanSabha.CreatedOn = vidhanSabhaDto.CreatedOn;

            return vidhanSabha;

        }
    }
}
