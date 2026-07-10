using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.Dto.Dashboard
{
    public class DashboardGenderRatioDto
    {
        public int TotalStudents { get; set; }
      
        public int FeMaleCount { get; set; }
      
        public int MaleCount { get; set; }
     

    }
    
}
