using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAttendanceApiBLL;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiBLL.Manager;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Repository;
using StudentAttendanceApiDAL.Tables;

namespace StudentAttendanceApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ILogger<SchoolController> logger;
        private readonly ISchoolManager _studentManager;

        public SchoolController(ISchoolManager studentManager, ILogger<SchoolController> logger)
        {
            this.logger = logger;
            this._studentManager = studentManager;
        }

        [Authorize]
        [HttpPost("SaveSchool")]
        public async Task<IActionResult> SaveSchool([FromForm] SchoolDto studentDto)
        {
            logger.LogInformation("UserController : LoginSuperAdmin : Started");
            try
            {
                School teacher= SchoolConvertor.ConvertSchoolDtoToSchool(studentDto);
                var masterAdmin = await _studentManager.SaveSchool(teacher);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "School save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "School doesn't save",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveSuperAdmin ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

       
        [HttpGet("GetAllSchools")]
        public async Task<IActionResult> GetAllSchools()
        {
            logger.LogInformation("UserController : GetUser : Started");
            try
            {
                var student = await _studentManager.GetAllSchools();
                if (student != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = student,
                        message = "school exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = student,
                        message = "school not exists",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : GetStudentById ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

    }
}
