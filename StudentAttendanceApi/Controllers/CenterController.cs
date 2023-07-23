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
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CenterController : ControllerBase
    {
        private readonly ILogger<CenterController> logger;
        private readonly ICenterManager _centerManager;

        public CenterController(ICenterManager centerManager, ILogger<CenterController> logger)
        {
            this.logger = logger;
            this._centerManager = centerManager;
        }


        [Authorize]
        [HttpPost("SaveCenter")]
        public async Task<IActionResult> SaveCenter([FromForm] CenterDto centerDto)
        {
            logger.LogInformation("UserController : LoginSuperAdmin : Started");
            try
            {
                Center center = CenterConvertor.ConvertCentertoToCenter(centerDto);
                var masterAdmin = await _centerManager.SaveCenter(center);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "Center save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Center doesn't save",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveSuperAdmin ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpPost("CheckCenterName")]
        public async Task<IActionResult> CheckCenterName(string name)
        {
            logger.LogInformation("UserController : CheckCenterName : Started");
            try
            {
                var mobileNo = await _centerManager.CheckCenterName(name);
                if (mobileNo != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = false,
                        message = "Center name already exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = true,
                        error = "Center name doesn't exists",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : CheckCenterName ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpGet("GetAllCenters")]
        public async Task<IActionResult> GetAllCenters()
        {
            logger.LogInformation("UserController : GetAllCenters : Started");
            try
            {
                var allCenters = await _centerManager.GetAllCenters();
                if (allCenters != null)
                {
                    return Ok(allCenters);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : GetAllCenters ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

        //[Authorize]
        //[HttpGet("GetAllCentersById")]
        //public async Task<IActionResult> GetAllCentersById(int districtId, int vidhanSabhaId, int panchayatId, int villageId)
        //{
        //    logger.LogInformation("UserController : GetAllCentersById : Started");
        //    try
        //    {
        //        var allCenters = await _centerManager.GetAllCentersById(districtId, vidhanSabhaId, panchayatId, villageId);
        //        if (allCenters != null)
        //        {
        //            return Ok(allCenters);
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, $"UserController : GetAllCentersById ", ex);
        //        return StatusCode(StatusCodes.Status501NotImplemented, "error");
        //    }
        //}

    }
}
