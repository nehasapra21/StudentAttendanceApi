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
    [Authorize]
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

        [Authorize]
        [HttpGet("GetAllVillage")]
        public async Task<IActionResult> GetAllVillage()
        {
            logger.LogInformation("VillageController : GetAllVillage : Started");
            try
            {
               var villageVal=await _villageManager.GetAllVillage();
                if (villageVal != null)
                {
                    return Ok(villageVal);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetAllVillage ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpPost("SaveVillage")]
        public async Task<ActionResult> SaveVillage([FromForm] VillageDto villageDto)
        {
            logger.LogInformation("VillageController : SaveVillage : Started");
            try
            {
                Village village = VillageConvertor.ConvertVillageDtoToVillage(villageDto);
                var villageVal = await _villageManager.SaveVillage(village);
                if (villageVal != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = villageVal,
                        message = "Village save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Village doesn't save",
                        code = StatusCodes.Status404NotFound
                    });
                }


            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : SaveVillage ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpGet("GetVillageByDistrictVidhanSabhaAndPanchId")]
        public async Task<ActionResult> GetVillageByDistrictVidhanSabhaAndPanchId(int districtId, int vidhanSabhaId, int panchayatId)
        {
            logger.LogInformation("VillageController : GetVillageByDistrictVidhanSabhaAndPanchId : Started");
            try
            {
                var village = await _villageManager.GetVillageByDistrictVidhanSabhaAndPanchId(districtId, vidhanSabhaId, panchayatId);
                if (village != null)
                {
                    return Ok(village);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetVillageByDistrictVidhanSabhaAndPanchId ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpPost("CheckVillageName")]
        public async Task<IActionResult> CheckVillageName(string name)
        {
            logger.LogInformation("UserController : CheckVillageName : Started");
            try
            {
                var mobileNo = await _villageManager.CheckVillageName(name);
                if (mobileNo != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = false,
                        message = "Village name already exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = true,
                        error = "Village name doesn't exists",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : CheckVillageName ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

    }
}
