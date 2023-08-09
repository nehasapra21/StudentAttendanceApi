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
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> logger;
        private readonly IStudentManager _studentManager;

        public StudentController(IStudentManager studentManager, ILogger<StudentController> logger)
        {
            this.logger = logger;
            this._studentManager = studentManager;
        }

        [Authorize]
        [HttpPost("SaveStudent")]
        public async Task<IActionResult> SaveStudent([FromForm] StudentDto studentDto)
        {
            logger.LogInformation("UserController : LoginSuperAdmin : Started");
            try
            {
                Student teacher= StudentConvertor.ConvertStudenttoToStudent(studentDto);
                var masterAdmin = await _studentManager.SaveStudent(teacher);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "Student save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Student doesn't save",
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

        //[HttpGet("GetStudentByCenterId")]
        //public async Task<IActionResult> GetStudentByCenterId(int centerId)
        //{
        //    logger.LogInformation("UserController : GetStudentByCenterId : Started");
        //    try
        //    {
        //        var student = await _studentManager.GetStudentByCenterId(centerId);
        //        if (student != null)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, new
        //            {
        //                status = false,
        //                data = student,
        //                message = "student exists",
        //                code = StatusCodes.Status200OK
        //            });
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, $"UserController : GetStudentByCenterId ", ex);
        //        return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
        //    }
        //}

        [HttpGet("GetStudentById")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            logger.LogInformation("UserController : GetUser : Started");
            try
            {
                var student = await _studentManager.GetStudentById(studentId);
                if (student != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = student,
                        message = "student exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = student,
                        message = "student not exists",
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

        [HttpPost("UpdateStudentActiveOrInactive")]
        public async Task<IActionResult> UpdateStudentActiveOrInactive(int id,int  status)
        {
            logger.LogInformation("UserController : UpdateStudentActiveOrInactive : Started");
            try
            {
                var student = await _studentManager.UpdateStudentActiveOrInactive(id,status);
                if (student != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = student,
                        message = "Status updated",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = student,
                        message = "Status not updated",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : UpdateStudentActiveOrInactive ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

        [HttpGet("GetTotalStudentPresent")]
        public async Task<IActionResult> GetTotalStudentPresent()
        {
            logger.LogInformation("UserController : GetTotalStudentPresent : Started");
            try
            {
                var allClasses = await _studentManager.GetTotalStudentPresent();
              
                if (allClasses != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data= allClasses,
                        message = "Total count",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = allClasses,
                        message = "Not found",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : GetAllClasses ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }
    }
}
