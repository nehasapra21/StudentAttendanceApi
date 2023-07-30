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
    public class TeacherDto
    {
        
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? Assigned { get; set; }
        public string? Profile { get; set; }
    }

}
