using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentAttendanceApi.FCM;
using StudentAttendanceApi.Services;
using StudentAttendanceApiBLL;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiBLL.Manager;
using StudentAttendanceApiBLL.NotificationData;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Repository;
using StudentAttendanceApiDAL.Tables;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Web.Http.Results;
using INotificationService = StudentAttendanceApi.Services.INotificationService;

namespace StudentAttendanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly ILogger<ClassController> logger;
        private readonly IClassManager _classManager;
        private readonly INotificationService _notificationService;

        public ClassController(IClassManager classManager, INotificationService notificationService, ILogger<ClassController> logger)
        {
            this.logger = logger;
            this._classManager = classManager;
            _notificationService = notificationService;
        }


        [HttpPost("SaveClass")]
        public async Task<IActionResult> SaveClass([FromForm] ClassDto classDto)
        {
            logger.LogInformation("UserController : SaveClass : Started");
            try
            {
                Class cls = ClassConvertor.ConvertClasstoToClass(classDto);
                var classData = await _classManager.SaveClass(cls);
                if (classData != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = classData,
                        message = "Class save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status409Conflict, new
                    {
                        status = false,
                        error = "Class already exists",
                        code = StatusCodes.Status409Conflict
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }


        [HttpPost("CancelClass")]
        public async Task<IActionResult> CancelClass([FromForm] CancelClassDto classDto)
        {
            logger.LogInformation("UserController : CancelClass : Started");
            try
            {
                Class cls = ClassConvertor.ConvertClasstoToClass(classDto);
                var classData = await _classManager.CancelClass(cls);
                if (classData != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Class canceled successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Class not canceled",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : CancelClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpPost("UpdateEndClassTime")]
        public async Task<IActionResult> UpdateEndClassTime([FromForm] EndClassDto classDto)
        {
            logger.LogInformation("UserController : SaveClass : Started");
            try
            {
                Class cls = ClassConvertor.ConvertClasstoToEndClassDto(classDto);
                var classData = await _classManager.UpdateEndClassTime(cls);
                if (classData != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Time updated",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Time not updated",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }


        [HttpPost("UpdateClassSubStatus")]
        public async Task<IActionResult> UpdateClassSubStatus([FromForm] UpdateClassSubStatusDto classDto)
        {
            logger.LogInformation("UserController : SaveClass : Started");
            try
            {
                var classData = await _classManager.UpdateClassSubStatus(classDto);
                if (classData != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "Status updated",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Status not updated",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpPost("CancelClassByTeacher")]
        public async Task<IActionResult> CancelClassByTeacher([FromForm] ClassCancelTeacherDto classDto)
        {
            logger.LogInformation("UserController : SaveClass : Started");
            try
            {
                ClassCancelTeacher cls = ClassConvertor.ConvertClassToClassCancelTeacherDto(classDto);
                NotificationModel model = await _classManager.CancelClassByTeacher(cls);
                if (model != null)
                {
                    ResponseModel response = await _notificationService.SendNotification(model);
                    if (response != null && response.IsSuccess)
                    {
                        return StatusCode(StatusCodes.Status200OK, new
                        {
                            status = true,
                            message = "Class cancelled",
                            Notification = response.Message,
                            code = StatusCodes.Status200OK
                        }); ;
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new
                        {
                            status = true,
                            message = "Class cancelled",
                            Notification = response.Message,
                            code = StatusCodes.Status200OK
                        });

                    }

                  
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Class not cancelled",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpPost("DeleteClassByTeacherId")]
        public async Task<IActionResult> DeleteClassByClassId(int classId)
        {
            //var response = this.Request.CreateResponse(HttpStatusCode.OK);
            logger.LogInformation("UserController : SaveClass : Started");
            try
            {

                Class classData = await _classManager.DeleteClassByTeacherId(classId);

                if (classData != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = "class deleted",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "class  not deleted",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpGet("GetClassCurrentStatus")]
        public async Task<IActionResult> GetClassCurrentStatus(int centerId, int teacherId)
        {
            //var response = this.Request.CreateResponse(HttpStatusCode.OK);
            logger.LogInformation("UserController : SaveClass : Started");
            try
            {

                string classData = await _classManager.GetClassCurrentStatus(centerId, teacherId);
                ContentResult contentResult = new ContentResult();
                contentResult.Content = classData;
                contentResult.ContentType = "application/json";

                return contentResult;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }


        [HttpGet("GetLiveClassDetail")]
        public async Task<IActionResult> GetLiveClassDetail(int classId)
        {
            //var response = this.Request.CreateResponse(HttpStatusCode.OK);
            logger.LogInformation("UserController : SaveClass : Started");
            try
            {

                var classData = await _classManager.GetLiveClassDetail(classId);

                if (classData != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = classData,
                        message = "class detail exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Class detail not exists",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }
        //[HttpGet("GetActiveClass")]
        //public async Task<IActionResult> GetActiveClassCount()
        //{
        //    logger.LogInformation("UserController : GetActiveClass : Started");
        //    try
        //    {
        //        var allClasses = await _classManager.GetActiveClass();
        //        Dictionary<string, object> data = new Dictionary<string, object>();
        //        data.Add("ActiveClasses", allClasses.Keys.ToList());
        //        data.Add("TotalClasses", allClasses.Values.ToList());
        //        if (allClasses != null)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, new
        //            {
        //                status = true,
        //                data = data ,
        //                code = StatusCodes.Status200OK
        //            });
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, $"UserController : GetAllClasses ", ex);
        //        return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
        //    }
        //}

    }
}
