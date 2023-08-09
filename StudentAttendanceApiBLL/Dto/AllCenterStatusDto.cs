using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public class AllCenterStatusDto
    {
        public int Id { get; set; }

        public string? CenterName { get; set; }

        public string? Date { get; set; }

        public bool? ClassStatus { get; set; }
        public bool? Status { get; set; }
        public string DistrictName { get; set; }

        public string VidhanSabhaName { get; set; }
        public int? TotalPresentStudents { get; set; }
        public int? TotalStudents { get; set; }
        public string PanchayatName { get; set; }
        public int VidhanSabhaId { get; set; }

        public int DistrictId { get; set; }

        public int PanchayatId { get; set; }
        public int? AssignedTeacher { get; set; }
        public string TeacherName { get; set; }


    }

}
