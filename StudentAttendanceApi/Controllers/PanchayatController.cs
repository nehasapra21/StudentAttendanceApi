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
    public class PanchayatController : ControllerBase
    {
        private readonly ILogger<PanchayatController> logger;
        private readonly IPanchayatManager _panchayatManager;

        public PanchayatController(IPanchayatManager panchayatManager, ILogger<PanchayatController> logger)
        {
            this.logger = logger;
            this._panchayatManager = panchayatManager;
        }

        [HttpGet("GetAllPanchayat")]
        public async Task<IActionResult> GetAllPanchayat()
        {
            logger.LogInformation("PanchayatController : GetAllPanchayat : Started");
            try
            {
                return Ok(await _panchayatManager.GetAllPanchayat());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"PanchayatController : GetAllPanchayat ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }

        [HttpPost("SavePanchayat")]
        public async Task<ActionResult> SaveDistrict([FromBody] PanchayatDto panchayatDto)
        {
            logger.LogInformation("PanchayatController : SavePanchayat : Started");
            try
            {
                Panchayat panchayat = PanchayatConvertor.ConvertPanchayatDtoToPanchayat(panchayatDto);
                var panchayatVal = await _panchayatManager.SavePanchayat(panchayat);
                logger.LogInformation("PanchayatController : SavePanchayat : End");
                if (panchayatVal == null)
                    return BadRequest(new { message = "panchayatVal" });
                return new OkObjectResult(panchayatVal);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"PanchayatController : SavePanchayat ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }

        [HttpGet("GetPanchayatByDistrictAndVidhanSabhaId")]
        public async Task<ActionResult> GetPanchayatByDistrictAndVidhanSabhaId(int districtId, int vidhanSabhaId)
        {
            logger.LogInformation("VidhanSabhaController : GetPanchayatByDistrictAndVidhanSabhaId : Started");
            try
            {
                return Ok(await _panchayatManager.GetPanchayatByDistrictAndVidhanSabhaId(districtId, vidhanSabhaId));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VidhanSabhaController : GetPanchayatByDistrictAndVidhanSabhaId ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "error");
            }
        }
    }
}
