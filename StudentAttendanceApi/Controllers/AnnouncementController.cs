using Api.Utility;
using FakeNewApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly ILogger<AnnouncementController> logger;
        private readonly IAnnouncementManager _announcementManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AnnouncementController(IWebHostEnvironment webHostEnvironment,IAnnouncementManager announcementManager, Logger<AnnouncementController> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            this._announcementManager = announcementManager;
        }

        [HttpPost("SaveAnnouncement")]
        public async Task<IActionResult> SaveAnnouncement([FromForm] AnnouncementDto announcementDto)
        {
            ImagesDto fileUrls = new ImagesDto();
            logger.LogInformation("UserController : SaveHolidays : Started");
            try
            {
                if (announcementDto.ImageFile.Count > 0)
                {
                    fileUrls = FileApiUtilty.UploadFileInFolder(announcementDto.ImageFile, _webHostEnvironment);
                    announcementDto.Image = fileUrls.FilePath;
                }
               
                Announcement announcement = AnnouncementConvertor.ConvertAnnouncementtoToAnnouncement(announcementDto);
                var announcementVal = await _announcementManager.SaveAnnouncement(announcement);
                if (announcementVal != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = announcementVal,
                        message = "Announcement save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "Announcement doesn't save",
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

        [HttpGet("GetAnnouncement")]
        public async Task<IActionResult> GetAnnouncement()
        {
            logger.LogInformation("DistrictController : GetAnnouncement : Started");
            try
            {
                var allAnnouncements = await _announcementManager.GetAnnouncement();
                if (allAnnouncements != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = allAnnouncements,
                        message = "List of allAnnouncements",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = allAnnouncements,
                        message = "List of allAnnouncements not found",
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

    }
}
