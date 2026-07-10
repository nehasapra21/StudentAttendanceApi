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
    public class CenterLogDto
    {
        public int Id { get; set; }
        public int? CenterId { get; set; }
        public int? UserId { get; set; }
        public string? Reason { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public bool? Status { get; set; }

    }
    
}
