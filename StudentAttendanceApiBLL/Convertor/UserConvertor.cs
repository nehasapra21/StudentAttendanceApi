using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class UserConvertor
    {
        public static Users ConvertUsertoToUser(UserDto masterAdminDto)
        {
            Users masterAdmin = new Users();
            masterAdmin.Id = masterAdminDto.Id;
            masterAdmin.UserGuidId = masterAdminDto.UserGuidId;
            masterAdmin.Name = masterAdminDto.Name;
            masterAdmin.Token = masterAdminDto.Token;
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
