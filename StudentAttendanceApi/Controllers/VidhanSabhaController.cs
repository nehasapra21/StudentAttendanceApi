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
    public class VidhanSabhaController : ControllerBase
    {
        private readonly ILogger<VidhanSabhaController> logger;
        private readonly IVidhanSabhaManager _vidhanSabhaManager;

        public VidhanSabhaController(IVidhanSabhaManager vidhanSabhaManager, ILogger<VidhanSabhaController> logger)
        {
            this.logger = logger;
            this._vidhanSabhaManager = vidhanSabhaManager;
        }

        [HttpGet("GetAllVidhanSabha")]
        public async Task<IActionResult> GetAllVidhanSabha()
        {
            logger.LogInformation("VidhanSabhaController : GetAllVidhanSabha : Started");
            try
            {
                return Ok(await _vidhanSabhaManager.GetAllVidhanSabha());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VidhanSabhaController : GetAllVidhanSabha ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }

        [HttpPost("SaveVidhanSabha")]
        public async Task<ActionResult> SaveVidhanSabha([FromBody] VidhanSabhaDto vidhanSabhaDto)
        {
            logger.LogInformation("VidhanSabhaController : SaveVidhanSabha : Started");
            try
            {
                VidhanSabha vidhanSabha = VidhanSabhaConvertor.ConvertVidhanSabhaDtoToVidhanSabha(vidhanSabhaDto);
                var vidhanSabhaVal = await _vidhanSabhaManager.SaveVidhanSabha(vidhanSabha);
                logger.LogInformation("VidhanSabhaController : SaveVidhanSabha : End");
                if (vidhanSabhaVal == null)
                    return BadRequest(new { message = "panchayatVal" });
                return new OkObjectResult(vidhanSabhaVal);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VidhanSabhaController : SaveVidhanSabha ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }

        [HttpGet("GetVidhanSabhaByDistrictId")]
        public async Task<ActionResult> GetVidhanSabhaByDistrictId(int districtId)
        {
            logger.LogInformation("VidhanSabhaController : GetVidhanSabhaByDistrictId : Started");
            try
            {
                return Ok(await _vidhanSabhaManager.GetVidhanSabhaByDistrictId(districtId));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VidhanSabhaController : GetUserById ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "error");
            }
        }

    
    }
}
