﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.Tables
{
    [Table("Student")]
    public class Student
    {
        public int Id { get; set; }
        public string? EnrollmentId { get; set; }
        public string? FullName { get; set; }   
       
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Contact { get; set; }
        public bool? Status { get; set; }
        public bool? ActiveClassStatus { get; set; }
        public int? Counter{ get; set; }
        public int? ManualAttendance { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Remarks { get; set; }
        public string? Grade { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImage { get; set; }
        public string? WhatsApp { get; set; }
        public string? LastClass { get; set; }
        public string? FullAddress { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int VidhanSabhaId { get; set; }
        public int DistrictId { get; set; }
        public int PanchayatId { get; set; }
        public int CenterId { get; set; }
        public int? CreatedBy { get; set; }
        public int? VillageId { get; set; }
        public string? Education { get; set; }

        public string? Category { get; set; }
        public string? FatherMobileNumber { get; set; }
        public string? FatherOccupation { get; set; }
        public string? MotherMobileNumber { get; set; }
        public string? MotherOccupation { get; set; }
        public bool? Bpl { get; set; }
        public int? SchoolId { get; set; }
     

        public ICollection<StudentAttendance> StudentAttendances { get; set; }
        [NotMapped]
        public string? SchoolName { get; set; }
        [NotMapped]
        public decimal AvgAttendance { get; set; }
        [NotMapped]
        public string? StudentStaus { get; set; }
        [NotMapped]
        public int? value{ get; set; }
        [NotMapped]
        public int? classcount { get; set; }
        [NotMapped]
        public List<int> ListOfStudentIds { get; set; }
        [NotMapped]
        public string CenterName { get; set; }
        [NotMapped]
        public string TeacherName { get; set; }
        [NotMapped]
        public int TotalStudentCount { get; set; }


        //[NotMapped]
        //public string DistrictName { get; set; }
        //[NotMapped]
        //public string VidhanSabhaName { get; set; }
        //[NotMapped]
        //public string VillageName { get; set; }
        //[NotMapped]
        //public string PanchayatName { get; set; }



    }
    
}
