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

        [Authorize]
        [HttpPost("SaveClass")]
        public async Task<IActionResult> SaveClass([FromForm] ClassDto classDto)
        {
            logger.LogInformation("UserController : LoginSuperAdmin : Started");
            try
            {
                Class cls= ClassConvertor.ConvertClasstoToClass(classDto);
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
                        error = "Class doesn't save",
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
