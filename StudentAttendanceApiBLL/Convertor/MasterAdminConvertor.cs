using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class MasterAdminConvertor
    {
        public static MasterAdmin ConvertMasterAdminDtoToMasterAdmin(MasterAdminDto masterAdminDto)
        {
            MasterAdmin masterAdmin = new MasterAdmin();
            masterAdmin.Id = masterAdminDto.Id;
            masterAdmin.MasterAdminGuidId = masterAdminDto.MasterAdminGuidId;
            masterAdmin.FullAddress = masterAdminDto.FullAddress;
            masterAdmin.Status = masterAdminDto.Status;
            masterAdmin.CreatedBy = masterAdminDto.CreatedBy;
            masterAdmin.CreatedOn = masterAdminDto.CreatedOn;

            return masterAdmin;

        }
    }
}
