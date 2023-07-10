using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiBLL.Manager;
using StudentAttendanceApiDAL.IRepository;

namespace StudentAttendanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillageController : ControllerBase
    {
        private readonly ILogger<VillageController> logger;
        private readonly IVillageManager _villageManager;

        public VillageController(IVillageManager villageManager, ILogger<VillageController> logger)
        {
            this.logger = logger;
            this._villageManager = villageManager;
        }

        [HttpGet("GetAllVillage")]
        public async Task<IActionResult> GetAllVillage()
        {
            logger.LogInformation("VillageController : GetAllVillage : Started");
            try
            {
                return Ok(await _villageManager.GetAllVillage());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetAllVillage ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }
    }
}
