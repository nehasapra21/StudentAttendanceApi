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
    public class HolidaysDto
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        [Required]
        public DateTime? SelectedDate { get; set; }
        public int? CreatedBy { get; set; }
        [Required]
        public int CenterId { get; set; }
        public List<int> CenterIds { get; set; }
    }

}
