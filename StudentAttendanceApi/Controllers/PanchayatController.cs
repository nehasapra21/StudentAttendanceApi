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
    public class PanchayatController : ControllerBase
    {
        private readonly ILogger<PanchayatController> logger;
        private readonly IPanchayatManager _panchayatManager;

        public PanchayatController(IPanchayatManager panchayatManager, ILogger<PanchayatController> logger)
        {
            this.logger = logger;
            this._panchayatManager = panchayatManager;
        }

        [Authorize]
        [HttpGet("GetAllPanchayat")]
        public async Task<IActionResult> GetAllPanchayat()
        {
            logger.LogInformation("PanchayatController : GetAllPanchayat : Started");
            try
            {
                var panchayat = await _panchayatManager.GetAllPanchayat();
                if (panchayat != null)
                {
                    return Ok(panchayat);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"PanchayatController : GetAllPanchayat ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpPost("SavePanchayat")]
        public async Task<ActionResult> SaveDistrict([FromForm] PanchayatDto panchayatDto)
        {
            logger.LogInformation("PanchayatController : SavePanchayat : Started");
            try
            {
                Panchayat panchayat = PanchayatConvertor.ConvertPanchayatDtoToPanchayat(panchayatDto);
                var panchayatVal = await _panchayatManager.SavePanchayat(panchayat);
                if (panchayatVal != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = panchayatVal,
                        message = "Panchayat save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Panchayat doesn't save",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"PanchayatController : SavePanchayat ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpGet("GetPanchayatByDistrictAndVidhanSabhaId")]
        public async Task<ActionResult> GetPanchayatByDistrictAndVidhanSabhaId(int districtId, int vidhanSabhaId)
        {
            logger.LogInformation("VidhanSabhaController : GetPanchayatByDistrictAndVidhanSabhaId : Started");
            try
            {
                var panchayat=await _panchayatManager.GetPanchayatByDistrictAndVidhanSabhaId(districtId, vidhanSabhaId);
                if (panchayat != null)
                {
                    return Ok(panchayat);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VidhanSabhaController : GetPanchayatByDistrictAndVidhanSabhaId ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpPost("CheckPanchayatName")]
        public async Task<IActionResult> CheckPanchayatName(string name)
        {
            logger.LogInformation("UserController : CheckPanchayatName : Started");
            try
            {
                var mobileNo = await _panchayatManager.CheckPanchayatName(name);
                if (mobileNo != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = false,
                        message = "Panchayat name already exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = true,
                        error = "Panchayat name doesn't exists",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : CheckPanchayatName ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }
    }
}
