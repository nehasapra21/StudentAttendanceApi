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
    public class StudentAttendanceController : ControllerBase
    {
        private readonly ILogger<StudentAttendanceController> logger;
        private readonly IStudentAttendanceManager _studentAttendanceManager;

        public StudentAttendanceController(IStudentAttendanceManager studentAttendanceManager, ILogger<StudentAttendanceController> logger)
        {
            this.logger = logger;
            this._studentAttendanceManager = studentAttendanceManager;
        }

        [Authorize]
        [HttpPost("SaveStudentAttendance")]
        public async Task<IActionResult> SaveStudentAttendance([FromBody]StudentAttendanceDto studentAttendanceDto)
        {
            logger.LogInformation("UserController : SaveStudentAttendance : Started");
            try
            {
                StudentAttendance studentAttend = StudentAttendanceConvertor.ConvertStudentAttendancetoToStudentAttendance(studentAttendanceDto);
                var studentAttendance = await _studentAttendanceManager.SaveStudentAttendance(studentAttend);
                if (studentAttendance ==-1)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Student attendance already exixts",
                        code = StatusCodes.Status200OK
                    });
                }
                else if(studentAttendance ==0)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Manual attendance already exists with 6 times",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Student attendance applied",
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

        [HttpGet("GetAllStudentWihAvgAttendance")]
        public async Task<IActionResult> GetAllStudentWihAvgAttendance(int centerId)
        {
            logger.LogInformation("UserController : SaveStudentAttendance : Started");
            try
            {
                var students = await _studentAttendanceManager.GetAllStudentWihAvgAttendance(centerId);
                if (students !=null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = students,
                        message = "Student exixts",
                        code = StatusCodes.Status200OK
                    }) ;
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Student  not exists",
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

        [HttpGet("GetAllStudentAttendancStatus")]
        public async Task<IActionResult> GetAllStudentAttendancStatus(int centerId,string classDate)
        {
            logger.LogInformation("UserController : SaveStudentAttendance : Started");
            try
            {
                var students = await _studentAttendanceManager.GetAllStudentAttendancStatus(centerId, classDate);
                if (students != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = students,
                        message = "Student exixts",
                        code = StatusCodes.Status200OK
                    }); ;
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Student  not exists",
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
    }
}
