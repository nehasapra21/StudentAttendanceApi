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
    public class VidhanSabhaController : ControllerBase
    {
        private readonly ILogger<VidhanSabhaController> logger;
        private readonly IVidhanSabhaManager _vidhanSabhaManager;

        public VidhanSabhaController(IVidhanSabhaManager vidhanSabhaManager, ILogger<VidhanSabhaController> logger)
        {
            this.logger = logger;
            this._vidhanSabhaManager = vidhanSabhaManager;
        }

        [Authorize]
        [HttpGet("GetAllVidhanSabha")]
        public async Task<IActionResult> GetAllVidhanSabha()
        {
            logger.LogInformation("VidhanSabhaController : GetAllVidhanSabha : Started");
            try
            {
                var vidhanSabha = await _vidhanSabhaManager.GetAllVidhanSabha();
                if (vidhanSabha != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status=true,
                        message = "List of vidhanSabha",
                        data = vidhanSabha,
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        message = "List of vidhanSabha not found",
                        data = vidhanSabha,
                        code = StatusCodes.Status404NotFound
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VidhanSabhaController : GetAllVidhanSabha ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpPost("SaveVidhanSabha")]
        public async Task<ActionResult> SaveVidhanSabha([FromForm] VidhanSabhaDto vidhanSabhaDto)
        {
            logger.LogInformation("VidhanSabhaController : SaveVidhanSabha : Started");
            try
            {
                VidhanSabha vidhanSabha = VidhanSabhaConvertor.ConvertVidhanSabhaDtoToVidhanSabha(vidhanSabhaDto);
                var vidhanSabhaVal = await _vidhanSabhaManager.SaveVidhanSabha(vidhanSabha);
                if (vidhanSabhaVal != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = vidhanSabhaVal,
                        message = "VidanSabha save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "VidanSabha doesn't save",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VidhanSabhaController : SaveVidhanSabha ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpGet("GetVidhanSabhaByDistrictId")]
        public async Task<ActionResult> GetVidhanSabhaByDistrictId(int districtId)
        {
            logger.LogInformation("VidhanSabhaController : GetVidhanSabhaByDistrictId : Started");
            try
            {
                var vidhansabaha = await _vidhanSabhaManager.GetVidhanSabhaByDistrictId(districtId);
                if (vidhansabaha != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = vidhansabaha,
                        message = "VidanSabha exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = vidhansabaha,
                        message = "VidanSabha not  exists",
                        code = StatusCodes.Status404NotFound
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VidhanSabhaController : GetUserById ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpPost("CheckVidhanSabhaName")]
        public async Task<IActionResult> CheckVidhanSabhaName(string name)
        {
            logger.LogInformation("UserController : CheckVidhanSabhaName : Started");
            try
            {
                var mobileNo = await _vidhanSabhaManager.CheckVidhanSabhaName(name);
                if (mobileNo != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = false,
                        message = "VidhanSabha name already exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "VidhanSabha name doesn't exists",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : CheckVidhanSabhaName ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

    }
}
