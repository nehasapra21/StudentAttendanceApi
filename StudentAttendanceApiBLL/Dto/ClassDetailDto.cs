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
    public class ClassDetailDto
    {
        public int? Completed { get; set; }
       
        public int? NotStarted { get; set; }
       
        public int NotEnded { get; set; }
       
        public int? NoAttendance { get; set; }

    }

}
