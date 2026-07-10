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
    public class CenterDto
    {
        public int Id { get; set; }
        public string? CenterGuidId { get; set; }
        [Required]
        public string? CenterName { get; set; }
       
     
        [Required]
        public int? AssignedTeachers { get; set; }
        [Required]
        public int? AssignedRegionalAdmin { get; set; }
      
        public DateTime? StartedDate { get; set; }
       
        [Required]
        public int VidhanSabhaId { get; set; }
        [Required]
        public int DistrictId { get; set; }
        [Required]
        public int PanchayatId { get; set; }
        
        public int? VillageId { get; set; }
       

    }
    
}
