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
    [Table("CenterAssignUser")]
    public class CenterAssignUser
    {
        [Key]
        public int Id { get; set; }
        public int UsersId { get; set; }
        public int CenterId { get; set; }
        public int? Type{ get; set; }
        public DateTime? Date { get; set; }
    }
    
}
