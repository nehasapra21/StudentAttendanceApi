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
    public class StudentActiveDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Status{ get; set; }
    }
    
}
