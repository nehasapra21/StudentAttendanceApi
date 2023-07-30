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
    public class ClassController : ControllerBase
    {
        private readonly ILogger<ClassController> logger;
        private readonly IClassManager _classManager;

        public ClassController(IClassManager classManager, ILogger<ClassController> logger)
        {
            this.logger = logger;
            this._classManager = classManager;
        }


        [HttpPost("SaveClass")]
        public async Task<IActionResult> SaveClass([FromForm] ClassDto classDto)
        {
            logger.LogInformation("UserController : SaveClass : Started");
            try
            {
                Class cls = ClassConvertor.ConvertClasstoToClass(classDto);
                var classData = await _classManager.SaveClass(cls);
                if (classData != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = classData,
                        message = "Class save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Class already exists",
                        code = StatusCodes.Status404NotFound
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }


        [HttpPost("CancelClass")]
        public async Task<IActionResult> CancelClass([FromForm] CancelClassDto classDto)
        {
            logger.LogInformation("UserController : CancelClass : Started");
            try
            {
                Class cls = ClassConvertor.ConvertClasstoToClass(classDto);
                var classData = await _classManager.CancelClass(cls);
                if (classData != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Class canceled successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Class not canceled",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : CancelClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpPost("UpdateEndClassTime")]
        public async Task<IActionResult> UpdateEndClassTime([FromForm] EndClassDto classDto)
        {
            logger.LogInformation("UserController : SaveClass : Started");
            try
            {
                Class cls = ClassConvertor.ConvertClasstoToEndClassDto(classDto);
                var classData = await _classManager.UpdateEndClassTime(cls);
                if (classData != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Time updated",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Time not updated",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpGet("GetActiveClass")]
        public async Task<IActionResult> GetActiveClassCount()
        {
            logger.LogInformation("UserController : GetActiveClass : Started");
            try
            {
                var allClasses = await _classManager.GetActiveClass();
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ActiveClasses", allClasses.Keys.ToList());
                data.Add("TotalClasses", allClasses.Values.ToList());
                if (allClasses != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = data ,
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return NotFound();
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
