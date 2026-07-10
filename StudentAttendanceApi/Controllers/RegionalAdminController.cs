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
using StudentAttendanceApiDAL.Tables;

namespace StudentAttendanceApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class RegionalAdminController : ControllerBase
    {
        private readonly ILogger<RegionalAdminController> logger;
        private readonly IRegionalAdminManager _masterAdminManager;
        

        public RegionalAdminController(IRegionalAdminManager masterAdminManager, ILogger<RegionalAdminController> logger)
        {
            this.logger = logger;
            this._masterAdminManager = masterAdminManager;
        }

        [Authorize]
        [HttpGet("GetAllRegionalAdmin")]
        public async Task<IActionResult> GetAllRegionalAdmin()
        {
            logger.LogInformation("RegionalAdminController : GetAllMasterAdmin : Started");
            try
            {
                return Ok(await _masterAdminManager.GetAllRegionalAdmin());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"RegionalAdminController : GetAllMasterAdmin ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }

        [HttpPost("LoginRegionalAdmin")]
        public async Task<IActionResult> LoginRegionalAdmin(LoginDto loginDto)
        {
            logger.LogInformation("UserController : LoginRegionalAdmin : Started");
            try
            {
                var masterAdmin = await _masterAdminManager.LoginRegionalAdmin(loginDto.Name, loginDto.Password);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "Regional admin Login successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Regional admin doesn't exists",
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
        [HttpPost("SaveRegionalAdmin")]
        public async Task<IActionResult> SaveRegionalAdmin([FromForm] RegionalAdminDto regionalAdminDto)
        {
            logger.LogInformation("RegionalAdminController : SaveRegionalAdmin : Started");
            try
            {
                RegionalAdmin regionalAdmin = RegionalAdminConvertor.ConvertMasterAdminDtoToMasterAdmin(regionalAdminDto);
                var masterAdmin = await _masterAdminManager.SaveRegionalAdmin(regionalAdmin);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "SuperAdmin save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "SuperAdmin doesn't save",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"RegionalAdminController : SaveRegionalAdmin ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }


    }
}
