using Microsoft.AspNetCore.Authorization;
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
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> logger;
        private readonly IDashboardManager _dashboardManager;

        public DashboardController(IDashboardManager dashboardManager, ILogger<DashboardController> logger)
        {
            this.logger = logger;
            this._dashboardManager = dashboardManager;
        }

        [HttpGet("GetClassCountByMonth")]
        public async Task<IActionResult> GetClassCountByMonth(int centerId,int month)
        {
            logger.LogInformation("VillageController : GetAllVillage : Started");
            try
            {
                dynamic classCountByMonth = await _dashboardManager.GetClassCountByMonth(centerId, month);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    status = true,
                    message = "List of Gender ratio",
                    data = classCountByMonth,
                    code = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetAllVillage ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }
      
        [HttpGet("GetTotalGenterRatioByCenterId")]
        public async Task<IActionResult> GetTotalGenterRatioByCenterId(int centerId)
        {
            logger.LogInformation("VillageController : GetAllVillage : Started");
            try
            {
                dynamic GenderRatio = await _dashboardManager.GetTotalGenderRatioByCenterId(centerId);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    status = true,
                    message = "List of Gender ratio",
                    data = GenderRatio,
                    code = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetAllVillage ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpGet("GetTotalStudentOfClass")]
        public async Task<IActionResult> GetTotalStudentOfClass(int centerId)
        {
            logger.LogInformation("VillageController : GetTotalStudentOfClass : Started");
            try
            {
                dynamic GenderRatio = await _dashboardManager.GetTotalStudentOfClass(centerId);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    status = true,
                    message = "List of Gender ratio",
                    data = GenderRatio,
                    code = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetTotalStudentOfClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

    }
}
