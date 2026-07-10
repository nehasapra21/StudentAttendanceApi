using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.Model
{
    public class CenterResult
    {
        public int CenterId { get; set; }
        public Center Center { get; set; }
        public int Type { get; set; }
        public int NotStarted { get; set; }
        public int EndDateWithAttendance { get; set; }
        public int EndDateWithNoAttendance { get; set; }
        public int CompletedWithAttendance { get; set; }
        public int NoAttendance { get; set; }
      
    }
}
