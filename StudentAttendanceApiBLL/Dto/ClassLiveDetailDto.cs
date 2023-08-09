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
    public class ClassLiveDetailDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Status{ get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? TotalStudents { get; set; }
        public int? AvilableStudents { get; set; }
        public int? SubStatus { get; set; }

    }

}
