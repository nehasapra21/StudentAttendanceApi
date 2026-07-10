using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StudentAttendanceApiDAL.Tables
{
    public class AnnouncementDto
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public List<IFormFile> ImageFile { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

    }
    
}
