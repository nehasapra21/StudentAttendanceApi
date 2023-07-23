using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.Tables
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string? ClassEnrolmentId { get; set; }
        public string Name { get; set; }
        public int CenterId { get; set; }
        public bool? Status { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TotalStudents { get; set; }
        public int? AvilableStudents { get; set; }
        public string? Reason { get; set; }
        public int? CancelBy { get; set; }

    }

}
