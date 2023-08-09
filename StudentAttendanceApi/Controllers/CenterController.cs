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
    [Authorize]
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

        [HttpGet("GetCenteryId")]
        public async Task<IActionResult> GetCenteryId(int centeId)
        {
            logger.LogInformation("UserController : GetUser : Started");
            try
            {
                var user = await _centerManager.GetCenteryId(centeId);
                if (user != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = user,
                        message = "center exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {

                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = user,
                        message = "center not exists",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveSuperAdmin ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

        //[Authorize]
        //[HttpPost("CheckCenterName")]
        //public async Task<IActionResult> CheckCenterName(string name)
        //{
        //    logger.LogInformation("UserController : CheckCenterName : Started");
        //    try
        //    {
        //        var mobileNo = await _centerManager.CheckCenterName(name);
        //        if (mobileNo != null)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, new
        //            {
        //                status = true,
        //                message = "Center name already exists",
        //                code = StatusCodes.Status200OK
        //            });
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, new
        //            {
        //                status = false,
        //                error = "Center name doesn't exists",
        //                code = StatusCodes.Status404NotFound
        //            });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, $"UserController : CheckCenterName ", ex);
        //        return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
        //    }
        //}

        [HttpGet("GetAllCenters")]
        public async Task<IActionResult> GetAllCenters()
        {
            logger.LogInformation("UserController : GetAllCenters : Started");
            try
            {
                var allCenters = await _centerManager.GetAllCenters();
                if (allCenters != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data= allCenters,
                        message = "List of centers",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = allCenters,
                        message = "List of centers not exists",
                        code = StatusCodes.Status404NotFound
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : GetAllCenters ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

        [HttpGet("GetAllCentersByStatus")]
        public async Task<IActionResult> GetAllCentersByStatus(int status)
        {
            logger.LogInformation("UserController : GetStudentAttendanceOfCenter : Started");
            try
            {
                var allCenters = await _centerManager.GetStudentAttendanceOfCenter(status);
                if (allCenters != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = allCenters,
                        message = "Student attendance of centers",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = allCenters,
                        message = "No class avilable",
                        code = StatusCodes.Status404NotFound
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : GetAllCenters ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }


        [HttpGet("GetCenterByTeacherId")]
        public async Task<IActionResult> GetCenterByTeacherId(int userId)
        {
            logger.LogInformation("UserController : GetStudentAttendanceOfCenter : Started");
            try
            {
                var center = await _centerManager.GetCenterByUserId(userId);
                if (center != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = center,
                        message = "center detail",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = center,
                        message = "center detail not found",
                        code = StatusCodes.Status404NotFound
                    });
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
