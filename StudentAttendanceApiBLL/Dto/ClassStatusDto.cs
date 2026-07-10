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
    public class ClassStatusDto
    {
        public int Type { get; set; }

        public bool? Status{ get; set; }

        public string? Name{ get; set; }


    }

}
