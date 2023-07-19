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
    [Authorize]
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

        [Authorize]
        [HttpGet("GetAllDistrict")]
        public async Task<IActionResult> GetAllDistrict()
        {
            logger.LogInformation("DistrictController : GetAllDistrict : Started");
            try
            {
                var allDistricts = await _districtManager.GetAllDistrict();
                if (allDistricts != null)
                {
                    return Ok(allDistricts);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictController : GetAllDistrict ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpPost("SaveDistrict")]
        public async Task<ActionResult> SaveDistrict([FromForm] DistrictDto districtDto)
        {
            logger.LogInformation("DistrictController : SaveDistrict : Started");
            try
            {
                District district = DistrictConvertor.ConvertDistrictDtoToDistrict(districtDto);
                var districtVal = await _districtManager.SaveDistrict(district);
                if (districtVal != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = districtVal,
                        message = "District save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "District doesn't save",
                        code = StatusCodes.Status404NotFound
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictController : SaveDistrict ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

        [Authorize]
        //[HttpPost("CheckDistrictName")]
        //public async Task<IActionResult> CheckDistrictName(string name)
        //{
        //    logger.LogInformation("UserController : CheckUserMobileNumber : Started");
        //    try
        //    {
        //        var mobileNo = await _districtManager.CheckDistrictName(name);
        //        if (mobileNo != null)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, new
        //            {
        //                status = false,
        //                message = "District name already exists",
        //                code = StatusCodes.Status200OK
        //            });
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, new
        //            {
        //                status = true,
        //                error = "District name doesn't exists",
        //                code = StatusCodes.Status404NotFound
        //            });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, $"UserController : SaveSuperAdmin ", ex);
        //        return StatusCode(StatusCodes.Status501NotImplemented, "error");
        //    }
        //}
    }
}
