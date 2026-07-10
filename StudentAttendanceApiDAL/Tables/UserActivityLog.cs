using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendanceApi.ActivityLog
{
    [Table("UserActivityLog")]
    public class UserActivityLog
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Url { get; set; }    
        public string Data { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }
        public DateTime ActivityDate { get; set; }  =DateTime.Now;
    }
}
