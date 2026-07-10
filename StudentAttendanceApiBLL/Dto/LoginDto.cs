using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.Dto
{
    public class LoginDto
    {
        public string? MobileNumber { get; set; }
        public string? Password { get; set; }
    }
}
