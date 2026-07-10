using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAttendanceApi.FCM;
using StudentAttendanceApi.Services;
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
        private readonly INotificationService _notificationService;

        public HolidaysController(IHolidaysManager holidaysManager, INotificationService notificationService, ILogger<HolidaysController> logger)
        {
            this.logger = logger;
            this._holidaysManager = holidaysManager;
            _notificationService = notificationService;
        }

        [HttpPost("SaveHolidays")]
        public async Task<IActionResult> SaveHolidays([FromForm] HolidaysDto holidaysDto)
        {
            logger.LogInformation("UserController : SaveHolidays : Started");
            try
            {
                Holidays holidays = HolidaysConvertor.ConvertHolidaysDtoToHolidays(holidaysDto);
                var model = await _holidaysManager.SaveHolidays(holidays);
                //  NotificationModel model = await _classManager.CancelClassByTeacher(cls);
                if (model != null)
                {
                    //ResponseModel response = await _notificationService.SendNotification(model);
                    string message = string.Empty;
                    if (holidaysDto.Id>0)
                    {
                        message = "Holdays update successfully";
                    }
                    else
                    {
                        message = "Holdays save successfully";
                    }
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = message,
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
public async Task<IActionResult> GetAllHolidays(int status,int userId = 0)
{
    logger.LogInformation("DistrictController : GetAllHolidaysByYear : Started");
    try
    {
        var allHolidays = await _holidaysManager.GetAllHolidays(status,userId);
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
            return StatusCode(StatusCodes.Status200OK, new
            {
                status = false,
                data = new object(),
                message = "List of Holidays not found",
                code = StatusCodes.Status200OK
            });
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"DistrictController : GetAllHolidaysByYear ", ex);
        return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
    }
}

[HttpPost("DeleteHolidayById")]
public async Task<IActionResult> DeleteHolidayById(int id)
{
    //var response = this.Request.CreateResponse(HttpStatusCode.OK);
    logger.LogInformation("UserController : SaveClass : Started");
    try
    {

        Holidays holiday = await _holidaysManager.DeleteHolidayById(id);

        if (holiday != null)
        {
            return StatusCode(StatusCodes.Status200OK, new
            {
                status = true,
                message = "holiday deleted",
                code = StatusCodes.Status200OK
            });
        }
        else
        {
            return StatusCode(StatusCodes.Status200OK, new
            {
                status = false,
                error = "holiday  not deleted",
                code = StatusCodes.Status200OK
            });
        }

    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"UserController : SaveClass ", ex);
        return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
    }
}
        //[HttpPost("UpdateHoliday")]
        //public async Task<IActionResult> UpdateHoliday([FromForm] HolidaysDto holidaysDto)
        //{
        //    logger.LogInformation("UserController : SaveClass : Started");
        //    try
        //    {
        //        Holidays holidays = HolidaysConvertor.ConvertHolidaysDtoToHolidays(holidaysDto);
        //        var model = await _holidaysManager.SaveHolidays(holidays);
        //        if (classData != null)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, new
        //            {
        //                status = true,
        //                message = "Time updated",
        //                code = StatusCodes.Status200OK
        //            });
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, new
        //            {
        //                status = false,
        //                error = "Time not updated",
        //                code = StatusCodes.Status404NotFound
        //            });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, $"UserController : SaveClass ", ex);
        //        return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
        //    }
        //}



    }
}
