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
    [Table("Teacher")]
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        public Guid TeacherGuidId { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        [NotMapped]
        public string? Token { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public bool? Status { get; set; }
        public int? Count { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Picture { get; set; }
        public string? WhatsApp { get; set; }
        public string? LastLoginTime { get; set; }
        public string? FullAddress { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int VidhanSabhaId { get; set; }
        public int DistrictId { get; set; }
        public int PanchayatId { get; set; }
        public int CenterId { get; set; }
        public int? CreatedBy { get; set; }
        public int VillageId { get; set; }
        public string? Education { get; set; }



    }
    
}