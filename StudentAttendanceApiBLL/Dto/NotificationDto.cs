using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.Dto
{
    public class NotificationDto
    {
        public int? CenterId { get; set; }
        public string? Name { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public string? RegionalToken { get; set; }
        public string? TeacherToken { get; set; }
        public int? Type { get; set; }
        public bool RegionaladminStatus { get; set; }
        public bool SuperAdminStatus { get; set; }
        public bool TeacherStatus { get; set; }

    }
}
