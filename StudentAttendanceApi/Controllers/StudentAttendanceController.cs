using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private IConfiguration _configuration;
        private readonly ILogger<StudentAttendanceController> logger;
        private readonly IStudentAttendanceManager _studentAttendanceManager;
        //private IOptions<ConnectionString> option;

        public StudentAttendanceController(IStudentAttendanceManager studentAttendanceManager, ILogger<StudentAttendanceController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this._studentAttendanceManager = studentAttendanceManager;
            _configuration = configuration;
        }

        [HttpPost("SaveStudentAttendance")]
        public async Task<IActionResult> SaveStudentAttendance([FromForm] StudentAttendanceDto studentAttendanceDto)
        {
            logger.LogInformation("UserController : SaveStudentAttendance : Started");
            try
            {
                StudentAttendance studentAttend = StudentAttendanceConvertor.ConvertStudentAttendancetoToStudentAttendance(studentAttendanceDto);
                var studentAttendance = await _studentAttendanceManager.SaveStudentAttendance(studentAttend);
                if (studentAttendance == -1)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Student attendance already exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else if (studentAttendance == 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Student already inactive",
                        code = StatusCodes.Status200OK
                    });
                }
                else if (studentAttendance == -2)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "student not exists in center",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Student attendance applied",
                        code = StatusCodes.Status200OK
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveSuperAdmin ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }
        [HttpPost("SaveAutomaticStudentAttendance")]
        public async Task<IActionResult> SaveAutomaticStudentAttendance([FromForm]StudentAttendanceDto studentAttendanceDto)
        {
            logger.LogInformation("UserController : SaveStudentAttendance : Started");
            try
            {
                StudentAttendance studentAttend = StudentAttendanceConvertor.ConvertStudentAttendancetoToStudentAttendance(studentAttendanceDto);
                var studentAttendance = await _studentAttendanceManager.SaveAtuomaticStudentAttendance(studentAttend);
                if (studentAttendance ==-1)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Student attendance already exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else if(studentAttendance ==0)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Student already inactive",
                        code = StatusCodes.Status200OK
                    });
                }
                else if (studentAttendance == -2)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "student not exists in center",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Student attendance applied",
                        code = StatusCodes.Status200OK
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveSuperAdmin ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpPost("SaveManualStudentAttendance")]
        public async Task<IActionResult> SaveManualStudentAttendance([FromForm] StudentAttendanceDto studentAttendanceDto)
        {
            logger.LogInformation("UserController : SaveStudentAttendance : Started");
            try
            {
                StudentAttendance studentAttend = StudentAttendanceConvertor.ConvertStudentAttendancetoToStudentAttendance(studentAttendanceDto);
                var studentAttendance = await _studentAttendanceManager.SaveManualStudentAttendance(studentAttend);
                if (studentAttendance == -1)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Student attendance already exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else if (studentAttendance == 0)
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
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Student attendance applied",
                        code = StatusCodes.Status200OK
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
                        message = "Students exists",
                        code = StatusCodes.Status200OK
                    }) ;
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Students not exists",
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


        [HttpGet("GetAllAbsentAttendance")]
        public async Task<IActionResult> GetAllAbsentAttendance(int centerId)
        {
            logger.LogInformation("UserController : SaveStudentAttendance : Started");
            try
            {
                var students = await _studentAttendanceManager.GetAllAbsentAttendance(centerId);
               
                if (students != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = students,
                        message = "List of all active students exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Active students not exists",
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
        public async Task<IActionResult> GetAllStudentAttendancStatus(int centerId,string scanDate)
        {
            logger.LogInformation("UserController : SaveStudentAttendance : Started");
            try
            {
                var students = await _studentAttendanceManager.GetAllStudentAttendancStatus(centerId, scanDate);
                if (students != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = students,
                        message = "Student status exists",
                        code = StatusCodes.Status200OK
                    }); ;
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Student status not exists",
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

        [HttpGet("GetAllStudentAttendancByMonth")]
        public async Task<IActionResult> GetAllStudentAttendancByMonth(int centerId,int studentId, int month,int year)
        {
            logger.LogInformation("UserController : SaveStudentAttendance : Started");
            try
            {
                var students = await _studentAttendanceManager.GetAllStudentAttendancByMonth(centerId, studentId, month,year);
                if (students != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = students,
                        message = "Student exists",
                        code = StatusCodes.Status200OK
                    }); ;
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Student not exists",
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
