using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAttendanceApiBLL;
using StudentAttendanceApiBLL.Dto;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiBLL.Manager;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Repository;
using StudentAttendanceApiDAL.Tables;

namespace StudentAttendanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ILogger<TeacherController> logger;
        private readonly ITeacherManager _teacherManager;

        public TeacherController(ITeacherManager teacherManager, ILogger<TeacherController> logger)
        {
            this.logger = logger;
            this._teacherManager = teacherManager;
        }

        [HttpPost("LoginTeacher")]
        public async Task<IActionResult> LoginTeacher(LoginDto loginDto)
        {
            logger.LogInformation("UserController : LoginSuperAdmin : Started");
            try
            {
                var masterAdmin = await _teacherManager.LoginTeacher(loginDto.Name, loginDto.Password);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "Teacher Login successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Teacher doesn't exists",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : LoginSuperAdmin ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }

        [Authorize]
        [HttpPost("SaveTeacher")]
        public async Task<IActionResult> SaveTeacher([FromForm] TeacherDto teacherDto)
        {
            logger.LogInformation("UserController : LoginSuperAdmin : Started");
            try
            {
                Teacher teacher= TeacherConvertor.ConvertTeachertoToTeacher(teacherDto);
                var masterAdmin = await _teacherManager.SaveTeacher(teacher);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "Teacher save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Teacher doesn't save",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveSuperAdmin ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }
    }
}
