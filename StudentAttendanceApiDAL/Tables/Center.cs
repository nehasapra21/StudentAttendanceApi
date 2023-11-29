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
    [Table("Center")]
    public class Center
    {
        [Key]
        public int Id { get; set; }
        public string? CenterGuidId { get; set; }
        public string? CenterName { get; set; }

        public bool? ClassStatus { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? StartedDate { get; set; }
        public int? AssignedTeachers { get; set; }
        public int? AssignedRegionalAdmin { get; set; }
        public int VidhanSabhaId { get; set; }
        public int DistrictId { get; set; }
        public int PanchayatId { get; set; }
        public int? VillageId { get; set; }
        [NotMapped]
        public int TotalCenterCount { get; set; }
        [NotMapped]
        public string RegionalAdminName { get; set; }
        [NotMapped]
        public string DistrictName { get; set; }
        [NotMapped]
        public string VidhanSabhaName { get; set; }
        [NotMapped]
        public string VillageName { get; set; }
        [NotMapped]
        public string PanchayatName { get; set; }
        public ICollection<CenterAssignUser> CenterAssignUser { get; set; }
        public District District { get; set; }
        public VidhanSabha VidhanSabha { get; set; }
        public Panchayat Panchayat { get; set; }
        public Village Village { get; set; }
        [NotMapped]
        public int? TotalStudents { get; set; }
        [NotMapped]
        public string TeacherName { get; set; }
        [NotMapped]
        public string RegionalAdminId { get; set; }

        [NotMapped]
        public int? TotalActiveStudents { get; set; }
        [NotMapped]
        public int? TotalPresentStudents { get; set; }
        [NotMapped]
        public DateTime? ClassStartDate { get; set; }
        [NotMapped]
        public DateTime? ClassEndDate{ get; set; }
        [NotMapped]

        public Users User { get; set; }
        public ICollection<ClassCancelTeacher> ClassCancelTeacher { get; set; }
    }

}
