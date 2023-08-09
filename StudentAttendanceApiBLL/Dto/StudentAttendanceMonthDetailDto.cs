﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public class StudentAttendanceMonthDetailDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? AttendanceStatus { get; set; }
        public DateTime? Date { get; set; }

    }
    
}
