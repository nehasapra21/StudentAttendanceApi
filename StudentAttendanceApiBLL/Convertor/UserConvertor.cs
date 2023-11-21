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
            masterAdmin.EnrolmentRollId = masterAdminDto.EnrolmentRollId;
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
            masterAdmin.DistrictId = masterAdminDto.DistrictId;
            masterAdmin.VidhanSabhaId = masterAdminDto.VidhanSabhaId;
            // masterAdmin.PanchayatId = masterAdminDto.PanchayatId;
            masterAdmin.VillageId = masterAdminDto.VillageId;
            masterAdmin.AssignedRegionalAdminStatus = masterAdminDto.AssignedRegionalAdminStatus;
            masterAdmin.AssignedTeacherStatus = masterAdminDto.AssignedTeacherStatus;
            masterAdmin.EnrollmentDate = masterAdminDto.EnrollmentDate;
            masterAdmin.GuardianName = masterAdminDto.GuardianName;
            masterAdmin.GuardianNumber = masterAdminDto.GuardianNumber;
            masterAdmin.Education = masterAdminDto.Education;
            if (!string.IsNullOrEmpty(masterAdminDto.ListOfPanchayatIds))
            {
                masterAdmin.ListOfPanchayatId = masterAdminDto.ListOfPanchayatIds.Split(',').Select(int.Parse).ToList();
            }
            return masterAdmin;

        }

        public static Users ConvertUpdateUsertoToUser(UserDto masterAdminDto)
        {
            Users masterAdmin = new Users();
            masterAdmin.Id = masterAdminDto.Id;
            masterAdmin.DateOfBirth = masterAdminDto.DateOfBirth;
            masterAdmin.GuardianName = masterAdminDto.GuardianName;
            masterAdmin.GuardianNumber = masterAdminDto.GuardianNumber;
            masterAdmin.Email = masterAdminDto.Email;
            return masterAdmin;
        }

        public static Users ConvertSuperAdminUsertoToSuperAdminUser(SuperAdminDto masterAdminDto)
        {
            Users masterAdmin = new Users();
            masterAdmin.Id = masterAdminDto.Id;
            masterAdmin.EnrolmentRollId = masterAdminDto.EnrolmentRollId;
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
            masterAdmin.EnrollmentDate = masterAdminDto.EnrollmentDate;
            //masterAdmin.DistrictId = masterAdminDto.DistrictId;
            //masterAdmin.VidhanSabhaId = masterAdminDto.VidhanSabhaId;
            //masterAdmin.PanchayatId = masterAdminDto.PanchayatId;
            //masterAdmin.VillageId = masterAdminDto.VillageId;
            //masterAdmin.CenterId = masterAdminDto.CenterId;
            return masterAdmin;

        }

        public static UserDto ConvertSuperAdminUsertoToUserDto(SuperAdminDto masterAdminDto)
        {
            UserDto masterAdmin = new UserDto();
            masterAdmin.Id = masterAdminDto.Id;
            masterAdmin.EnrolmentRollId = masterAdminDto.EnrolmentRollId;
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
            masterAdmin.EnrollmentDate = masterAdminDto.EnrollmentDate;
            //masterAdmin.DistrictId = masterAdminDto.DistrictId;
            //masterAdmin.VidhanSabhaId = masterAdminDto.VidhanSabhaId;
            //masterAdmin.PanchayatId = masterAdminDto.PanchayatId;
            //masterAdmin.VillageId = masterAdminDto.VillageId;
            //masterAdmin.CenterId = masterAdminDto.CenterId;
            return masterAdmin;

        }


        public static UserDto ConvertUserToUserDto(Users masterAdminDto)
        {
            UserDto masterAdmin = new UserDto();
            masterAdmin.Id = masterAdminDto.Id;
            masterAdmin.EnrolmentRollId = masterAdminDto.EnrolmentRollId;
            masterAdmin.Name = masterAdminDto.Name;
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
            masterAdmin.Type = masterAdminDto.Type;
            masterAdmin.CreatedBy = masterAdminDto.CreatedBy;
            masterAdmin.CreatedOn = masterAdminDto.CreatedOn;
            masterAdmin.DistrictId = masterAdminDto.DistrictId;
            masterAdmin.VidhanSabhaId = masterAdminDto.VidhanSabhaId;
            // masterAdmin.PanchayatId = masterAdminDto.PanchayatId;
            masterAdmin.VillageId = masterAdminDto.VillageId;
            masterAdmin.AssignedRegionalAdminStatus = masterAdminDto.AssignedRegionalAdminStatus;
            masterAdmin.AssignedTeacherStatus = masterAdminDto.AssignedTeacherStatus;
            masterAdmin.EnrollmentDate = masterAdminDto.EnrollmentDate;
            masterAdmin.GuardianName = masterAdminDto.GuardianName;
            masterAdmin.GuardianNumber = masterAdminDto.GuardianNumber;
            masterAdmin.Education = masterAdminDto.Education;
            //masterAdmin.ListOfPanchayatId = masterAdminDto.ListOfPanchayatId;
            masterAdmin.DistrictId = masterAdminDto.DistrictId;
            masterAdmin.VidhanSabhaId = masterAdminDto.VidhanSabhaId;
            //masterAdmin.PanchayatId = masterAdminDto.PanchayatId;
            masterAdmin.VillageId = masterAdminDto.VillageId;
            //masterAdmin.DistrictName = masterAdminDto.District != null ? masterAdminDto.District.Name : string.Empty;

            //masterAdmin.VidhanSabhaName = (masterAdminDto.District != null && masterAdminDto.District.VidhanSabha != null) ? masterAdminDto.District.VidhanSabha.Name : string.Empty;
            //masterAdmin.PanchayatName = (masterAdminDto.District != null && masterAdminDto.District.VidhanSabha != null && masterAdminDto.District.VidhanSabha.Panchayat != null) ? masterAdminDto.District.VidhanSabha.Panchayat.Name : string.Empty;
            //masterAdmin.VillageName = (masterAdminDto.District != null && masterAdminDto.District.VidhanSabha != null && masterAdminDto.District.VidhanSabha.Panchayat != null && masterAdminDto.District.VidhanSabha.Panchayat.Village != null) ? masterAdminDto.District.VidhanSabha.Panchayat.Village.Name : string.Empty;
            //if (masterAdminDto.RegionalAdminPanchayat != null)
            //{
            //    masterAdmin.ListOfPanchayatName = new List<string>();
            //    masterAdmin.ListOfPanchayatId = new List<int>();
            //    foreach (var item in masterAdminDto.RegionalAdminPanchayat)
            //    {
            //        masterAdmin.ListOfPanchayatName.Add(item.PanchayatName);
            //        masterAdmin.ListOfPanchayatId.Add(item.PanchayatId.Value);
            //    }
            //}
            return masterAdmin;

        }

        public static SuperAdminDetailDto ConvertUserToSuperAdminDetailDto(Users masterAdminDto)
        {
            SuperAdminDetailDto masterAdmin = new SuperAdminDetailDto();
            masterAdmin.Id = masterAdminDto.Id;
            masterAdmin.EnrolmentRollId = masterAdminDto.EnrolmentRollId;
            masterAdmin.Name = masterAdminDto.Name;
            masterAdmin.FullAddress = masterAdminDto.FullAddress;
            masterAdmin.Status = masterAdminDto.Status;
            masterAdmin.Age = masterAdminDto.Age;
            masterAdmin.Gender = masterAdminDto.Gender;
            masterAdmin.DateOfBirth = masterAdminDto.DateOfBirth;
            masterAdmin.PhoneNumber = masterAdminDto.PhoneNumber;
            masterAdmin.WhatsApp = masterAdminDto.WhatsApp;
            masterAdmin.Email = masterAdminDto.Email;
            masterAdmin.Contact = masterAdminDto.Contact;
            masterAdmin.Picture = masterAdminDto.Picture;
            masterAdmin.LastLoginTime = masterAdminDto.LastLoginTime;
            masterAdmin.Type = masterAdminDto.Type;
            masterAdmin.CreatedBy = masterAdminDto.CreatedBy;
            masterAdmin.CreatedOn = masterAdminDto.CreatedOn;
            masterAdmin.EnrollmentDate = masterAdminDto.EnrollmentDate;
            return masterAdmin;

        }


        public static RegionalAdminDetailDto ConvertUserToRegionalAdminDetailDto(Users masterAdminDto)
        {
            RegionalAdminDetailDto masterAdmin = new RegionalAdminDetailDto();
            masterAdmin.Id = masterAdminDto.Id;
            masterAdmin.EnrolmentRollId = masterAdminDto.EnrolmentRollId;
            masterAdmin.Name = masterAdminDto.Name;
            masterAdmin.FullAddress = masterAdminDto.FullAddress;
            masterAdmin.Status = masterAdminDto.Status;
            masterAdmin.Age = masterAdminDto.Age;
            masterAdmin.Gender = masterAdminDto.Gender;
            masterAdmin.DateOfBirth = masterAdminDto.DateOfBirth;
            masterAdmin.PhoneNumber = masterAdminDto.PhoneNumber;
            masterAdmin.WhatsApp = masterAdminDto.WhatsApp;
            masterAdmin.Email = masterAdminDto.Email;
            masterAdmin.Contact = masterAdminDto.Contact;
            masterAdmin.Picture = masterAdminDto.Picture;
            masterAdmin.LastLoginTime = masterAdminDto.LastLoginTime;
            masterAdmin.Type = masterAdminDto.Type;
            masterAdmin.CreatedBy = masterAdminDto.CreatedBy;
            masterAdmin.CreatedOn = masterAdminDto.CreatedOn;
            masterAdmin.DistrictId = masterAdminDto.DistrictId;
            masterAdmin.VidhanSabhaId = masterAdminDto.VidhanSabhaId;
            masterAdmin.VillageId = masterAdminDto.VillageId;
            masterAdmin.AssignedRegionalAdminStatus = masterAdminDto.AssignedRegionalAdminStatus;
            masterAdmin.AssignedTeacherStatus = masterAdminDto.AssignedTeacherStatus;
            masterAdmin.EnrollmentDate = masterAdminDto.EnrollmentDate;
            masterAdmin.GuardianName = masterAdminDto.GuardianName;
            masterAdmin.GuardianNumber = masterAdminDto.GuardianNumber;
            masterAdmin.Education = masterAdminDto.Education;
            masterAdmin.DistrictId = masterAdminDto.DistrictId;
            masterAdmin.VidhanSabhaId = masterAdminDto.VidhanSabhaId;
            masterAdmin.VillageId = masterAdminDto.VillageId;

            masterAdmin.DistrictName = masterAdminDto.District != null ? masterAdminDto.District.Name : string.Empty;

            masterAdmin.VidhanSabhaName = (masterAdminDto.VidhanSabha != null) ? masterAdminDto.VidhanSabha.Name : string.Empty;

            masterAdmin.VillageName = (masterAdminDto.Village != null) ? masterAdminDto.Village.Name : string.Empty;
            if (masterAdminDto.CenterAssignUser != null)
            {
                masterAdmin.AssignedDate = masterAdminDto.CenterAssignUser.Date;
            }
            if (masterAdminDto.RegionalAdminPanchayat != null)
            {
                masterAdmin.ListOfPanchayat = new List<PanchayatDto>();
                foreach (var item in masterAdminDto.RegionalAdminPanchayat)
                {
                    PanchayatDto panchayat = new PanchayatDto();
                    panchayat.Id = item.PanchayatId;
                    panchayat.Name = item.PanchayatName;
                    masterAdmin.ListOfPanchayat.Add(panchayat);
                }
            }
            masterAdmin.ListOfCenters = new List<CenterDto>();
            foreach (var item in masterAdminDto.Centers)
            {
                CenterDto center = new CenterDto();
                center.Id = item.Id;
                center.CenterName = item.CenterName;
                masterAdmin.ListOfCenters.Add(center);
            }
            return masterAdmin;

        }

        public static TeacherDetailDto ConvertUserToTeacherDetailDto(Users masterAdminDto)
        {
            TeacherDetailDto masterAdmin = new TeacherDetailDto();
            masterAdmin.Id = masterAdminDto.Id;
            masterAdmin.EnrolmentRollId = masterAdminDto.EnrolmentRollId;
            masterAdmin.Name = masterAdminDto.Name;
            masterAdmin.FullAddress = masterAdminDto.FullAddress;
            masterAdmin.Status = masterAdminDto.Status;
            masterAdmin.Age = masterAdminDto.Age;
            masterAdmin.Gender = masterAdminDto.Gender;
            masterAdmin.DateOfBirth = masterAdminDto.DateOfBirth;
            masterAdmin.PhoneNumber = masterAdminDto.PhoneNumber;
            masterAdmin.WhatsApp = masterAdminDto.WhatsApp;
            masterAdmin.Email = masterAdminDto.Email;
            masterAdmin.Contact = masterAdminDto.Contact;
            masterAdmin.Picture = masterAdminDto.Picture;
            masterAdmin.LastLoginTime = masterAdminDto.LastLoginTime;
            masterAdmin.Type = masterAdminDto.Type;
            masterAdmin.CreatedBy = masterAdminDto.CreatedBy;
            masterAdmin.CreatedOn = masterAdminDto.CreatedOn;
            masterAdmin.DistrictId = masterAdminDto.DistrictId;
            masterAdmin.VidhanSabhaId = masterAdminDto.VidhanSabhaId;
            masterAdmin.VillageId = masterAdminDto.VillageId;
            masterAdmin.AssignedRegionalAdminStatus = masterAdminDto.AssignedRegionalAdminStatus;
            masterAdmin.AssignedTeacherStatus = masterAdminDto.AssignedTeacherStatus;
            masterAdmin.EnrollmentDate = masterAdminDto.EnrollmentDate;
            masterAdmin.GuardianName = masterAdminDto.GuardianName;
            masterAdmin.GuardianNumber = masterAdminDto.GuardianNumber;
            masterAdmin.Education = masterAdminDto.Education;
            masterAdmin.PanchayatId = masterAdminDto.PanchayatId;
            masterAdmin.DistrictId = masterAdminDto.DistrictId;
            masterAdmin.VidhanSabhaId = masterAdminDto.VidhanSabhaId;
            masterAdmin.VillageId = masterAdminDto.VillageId;
            masterAdmin.DistrictName = masterAdminDto.District != null ? masterAdminDto.District.Name : string.Empty;
            masterAdmin.PanchayatName = (masterAdminDto.Panchayat != null) ? masterAdminDto.Panchayat.Name : string.Empty;
            masterAdmin.VidhanSabhaName = (masterAdminDto.VidhanSabha != null) ? masterAdminDto.VidhanSabha.Name : string.Empty;

            masterAdmin.VillageName = (masterAdminDto.Village != null) ? masterAdminDto.Village.Name : string.Empty;
            if (masterAdminDto.CenterAssignUser != null)
            {
                masterAdmin.AssignedDate = masterAdminDto.CenterAssignUser.Date;
            }
            if (masterAdminDto.Center != null)
            {
                masterAdmin.Center = new Center();
                masterAdmin.Center.Id = masterAdminDto.Center.Id;
                masterAdmin.Center.Status = masterAdminDto.Center.Status;
                masterAdmin.Center.ClassStatus = masterAdminDto.Center.ClassStatus;
                masterAdmin.Center.CenterName = masterAdminDto.Center.CenterName;
                masterAdmin.Center.StartedDate = masterAdminDto.Center.StartedDate;
            }
            return masterAdmin;

        }
    }
}
