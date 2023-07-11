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
    public class DistrictController : ControllerBase
    {
        private readonly ILogger<DistrictController> logger;
        private readonly IDistrictManager _districtManager;

        public DistrictController(IDistrictManager districtManager, ILogger<DistrictController> logger)
        {
            this.logger = logger;
            this._districtManager = districtManager;
        }

        [HttpGet("GetAllDistrict")]
        public async Task<IActionResult> GetAllDistrict()
        {
            logger.LogInformation("DistrictController : GetAllDistrict : Started");
            try
            {
                return Ok(await _districtManager.GetAllDistrict());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictController : GetAllDistrict ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }

        
        [HttpPost("SaveDistrict")]
        public async Task<ActionResult> SaveDistrict([FromBody] DistrictDto districtDto)
        {
            logger.LogInformation("DistrictController : SaveDistrict : Started");
            try
            {
                District district = DistrictConvertor.ConvertDistrictDtoToDistrict(districtDto);
                var iplStudentEmi = await _districtManager.SaveDistrict(district);
                logger.LogInformation("DistrictController : SaveDistrict : End");
                if (iplStudentEmi == null)
                    return BadRequest(new { message = "username" });
                return new OkObjectResult(iplStudentEmi);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictController : SaveDistrict ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }
    }
}
