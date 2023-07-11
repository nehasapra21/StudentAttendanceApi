using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAttendanceApiBLL;
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

        [HttpPost("SaveRegionalAdmin")]
        public async Task<IActionResult> SaveRegionalAdmin(RegionalAdminDto regionalAdminDto)
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
