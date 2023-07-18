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
    public class CommonController : ControllerBase
    {
        private readonly ILogger<CommonController> logger;
        private readonly ICenterManager _centerManager;

        public CommonController(ICenterManager centerManager, ILogger<CommonController> logger)
        {
            this.logger = logger;
            this._centerManager = centerManager;
        }

     

        [HttpPost("CheckUserMobileNumber")]
        public async Task<IActionResult> SaveCenter([FromForm] CenterDto centerDto)
        {
            logger.LogInformation("UserController : LoginSuperAdmin : Started");
            try
            {
                Center center= CenterConvertor.ConvertCentertoToCenter(centerDto);
                var masterAdmin = await _centerManager.SaveCenter(center);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "Center save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Center doesn't save",
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
