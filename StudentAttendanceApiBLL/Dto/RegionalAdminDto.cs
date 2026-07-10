using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace StudentAttendanceApiBLL
{
    public class RegionalAdminDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Profile { get; set; }
    }

}
