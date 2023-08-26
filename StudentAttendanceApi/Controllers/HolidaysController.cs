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
    public class HolidaysController : ControllerBase
    {
        private readonly ILogger<HolidaysController> logger;
        private readonly IHolidaysManager _holidaysManager;

        public HolidaysController(IHolidaysManager holidaysManager, ILogger<HolidaysController> logger)
        {
            this.logger = logger;
            this._holidaysManager = holidaysManager;
        }

        [HttpPost("SaveHolidays")]
        public async Task<IActionResult> SaveHolidays([FromForm]HolidaysDto holidaysDto)
        {
            logger.LogInformation("UserController : SaveHolidays : Started");
            try
            {
                Holidays holidays= HolidaysConvertor.ConvertHolidaysDtoToHolidays(holidaysDto);
                var masterAdmin = await _holidaysManager.SaveHolidays(holidays);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Holdays save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Holdays doesn't save",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveHolidays ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpGet("GetAllHolidaysByTeacherId")]
        public async Task<IActionResult> GetAllHolidaysByTeacherId(int teacherId)
        {
            logger.LogInformation("DistrictController : GetAllDistrict : Started");
            try
            {
                var allHolidays = await _holidaysManager.GetAllHolidaysByTeacherId(teacherId);
                if (allHolidays != null)
                {

                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data= allHolidays,
                        message = "List of Holidays",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = allHolidays,
                        message = "List of Holidays not found",
                        code = StatusCodes.Status404NotFound
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictController : allHolidays ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpGet("GetAllHolidaysByCenterId")]
        public async Task<IActionResult> GetAllHolidaysByCenterId(int centerId)
        {
            logger.LogInformation("DistrictController : GetAllDistrict : Started");
            try
            {
                var allHolidays = await _holidaysManager.GetAllHolidaysByCenterId(centerId);
                if (allHolidays != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = allHolidays,
                        message = "List of Holidays exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = allHolidays,
                        message = "Holidays not exists",
                        code = StatusCodes.Status404NotFound
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictController : GetAllHolidaysByCenterId ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }


        [HttpGet("GetAllHolidaysByYear")]
        public async Task<IActionResult> GetAllHolidaysByYear(int year)
        {
            logger.LogInformation("DistrictController : GetAllHolidaysByYear : Started");
            try
            {
                var allHolidays = await _holidaysManager.GetAllHolidaysByYear(year);
                if (allHolidays != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = allHolidays,
                        message = "List of Holidays",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = allHolidays,
                        message = "List of Holidays not found",
                        code = StatusCodes.Status404NotFound
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictController : GetAllHolidaysByYear ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

        [HttpGet("GetAllHolidays")]
        public async Task<IActionResult> GetAllHolidays(int userId,int type)
        {
            logger.LogInformation("DistrictController : GetAllHolidaysByYear : Started");
            try
            {
                var allHolidays = await _holidaysManager.GetAllHolidays(userId,type);
                if (allHolidays != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = allHolidays,
                        message = "List of Holidays",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = allHolidays,
                        message = "List of Holidays not found",
                        code = StatusCodes.Status404NotFound
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DistrictController : GetAllHolidaysByYear ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
            }
        }

    }
}
