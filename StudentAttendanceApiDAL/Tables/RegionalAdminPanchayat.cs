using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceApiDAL.Tables
{
    [Table("RegionalAdminPanchayat")]
    public class RegionalAdminPanchayat
    {
        [Key]
        public int Id { get; set; }
        public int UsersId { get; set; }
        public int PanchayatId { get; set; }
        public string? PanchayatName { get; set; }
    }
}
