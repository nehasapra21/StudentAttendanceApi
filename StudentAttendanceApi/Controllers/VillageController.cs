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

        [HttpPost("SaveVillage")]
        public async Task<ActionResult> SaveVillage([FromBody] VillageDto villageDto)
        {
            logger.LogInformation("VillageController : SaveVillage : Started");
            try
            {
                Village village = VillageConvertor.ConvertVillageDtoToVillage(villageDto);
                var villageVal = await _villageManager.SaveVillage(village);
                logger.LogInformation("VillageController : SaveVillage : End");
                if (villageVal == null)
                    return BadRequest(new { message = "villageVal" });
                return new OkObjectResult(villageVal);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : SaveVillage ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }

        [HttpGet("GetVillageByDistrictVidhanSabhaAndPanchId")]
        public async Task<ActionResult> GetVillageByDistrictVidhanSabhaAndPanchId(int districtId, int vidhanSabhaId, int panchayatId)
        {
            logger.LogInformation("VillageController : GetVillageByDistrictVidhanSabhaAndPanchId : Started");
            try
            {
                return Ok(await _villageManager.GetVillageByDistrictVidhanSabhaAndPanchId(districtId,vidhanSabhaId, panchayatId));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetVillageByDistrictVidhanSabhaAndPanchId ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "error");
            }
        }

    }
}
