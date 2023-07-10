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
            masterAdmin.FullName = masterAdminDto.FullName;
            masterAdmin.FullAddress = masterAdminDto.FullAddress;
            masterAdmin.Status = masterAdminDto.Status;
            masterAdmin.Age = masterAdminDto.Age;
            masterAdmin.Gender = masterAdminDto.Gender;
            masterAdmin.DateOfBirth = masterAdminDto.DateOfBirth;
            masterAdmin.PhoneNumber = masterAdminDto.PhoneNumber;
            masterAdmin.WhatsApp = masterAdminDto.WhatsApp;
            masterAdmin.Email = masterAdminDto.Email;
            masterAdmin.Contact = masterAdminDto.Contact;
            masterAdmin.RoleId = masterAdminDto.RoleId;
            masterAdmin.Picture = masterAdminDto.Picture;
            masterAdmin.LastLoginTime = masterAdminDto.LastLoginTime;
            masterAdmin.Password = masterAdminDto.Password;
            masterAdmin.Type = masterAdminDto.Type;
            masterAdmin.CreatedBy = masterAdminDto.CreatedBy;
            masterAdmin.CreatedOn = masterAdminDto.CreatedOn;

            return masterAdmin;

        }
    }
}
