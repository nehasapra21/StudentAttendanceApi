using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using StudentAttendanceApiBLL;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiBLL.Manager;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Tables;
using System.Net.Http;
using System.Text;
using System;
using System.Text.Json.Nodes;
using System.Threading;

namespace StudentAttendanceApi.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> logger;
        private readonly IDashboardManager _dashboardManager;

        public DashboardController(IDashboardManager dashboardManager, ILogger<DashboardController> logger)
        {
            this.logger = logger;
            this._dashboardManager = dashboardManager;
        }

        [HttpGet("GetClassCountByMonth")]
        public async Task<IActionResult> GetClassCountByMonth(int centerId,int month)
        {
            logger.LogInformation("VillageController : GetAllVillage : Started");
            try
            {
                string classCountByMonth = await _dashboardManager.GetClassCountByMonth(centerId, month);
                ContentResult contentResult = new ContentResult();
                contentResult.Content = classCountByMonth;
                contentResult.ContentType = "application/json";

                return contentResult;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetAllVillage ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }
      
        [HttpGet("GetTotalGenterRatioByCenterId")]
        public async Task<IActionResult> GetTotalGenterRatioByCenterId(int centerId)
        {
            logger.LogInformation("VillageController : GetAllVillage : Started");
            try
            {
                string GenderRatio = await _dashboardManager.GetTotalGenderRatioByCenterId(centerId);
                ContentResult contentResult = new ContentResult();
                contentResult.Content = GenderRatio;
                contentResult.ContentType = "application/json";

                return contentResult;
               
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetAllVillage ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpGet("GetTotalStudentOfClass")]
        public async Task<IActionResult> GetTotalStudentOfClass(int centerId)
        {
            logger.LogInformation("VillageController : GetTotalStudentOfClass : Started");
            try
            {
                string GenderRatio = await _dashboardManager.GetTotalStudentOfClass(centerId);
                ContentResult contentResult = new ContentResult();
                contentResult.Content = GenderRatio;
                contentResult.ContentType = "application/json";

                return contentResult;
               
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetTotalStudentOfClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpGet("GetCenterDetailByMonth")]
        public async Task<IActionResult> GetCenterDetailByMonth(int centerId, int month, int year)
        {
            logger.LogInformation("VillageController : GetCenterDetailByMonth : Started");
            try
            {
                string GenderRatio = await _dashboardManager.GetCenterDetailByMonth(centerId,month,year);
                ContentResult contentResult = new ContentResult();
                contentResult.Content = GenderRatio;
                contentResult.ContentType = "application/json";

                return contentResult;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetTotalStudentOfClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpGet("GetTotalBpl")]
        public async Task<IActionResult> GetTotalBpl(int centerId, bool BplValue)
        {
            logger.LogInformation("VillageController : GetTotalBpl : Started");
            try
            {
                string BplData = await _dashboardManager.GetTotalBpl(centerId, BplValue);
                ContentResult contentResult = new ContentResult();
                contentResult.Content = BplData;
                contentResult.ContentType = "BplData/json";

                return contentResult;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetTotalBpl ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [HttpGet("GetTotalStudentCategoryOfClass")]
        public async Task<IActionResult> GetTotalStudentCategoryOfClass(int centerId)
        {
            logger.LogInformation("VillageController : GetTotalBpl : Started");
            try
            {
                string categoryData = await _dashboardManager.GetTotalStudentCategoryOfClass(centerId);
                ContentResult contentResult = new ContentResult();
                contentResult.Content = categoryData;
                contentResult.ContentType = "BplData/json";

                return contentResult;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetTotalBpl ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }



        [HttpGet("GetUserByFilter")]
        public async Task<IActionResult> GetUserByFilter(int type, int districtId,int vidhanSabhaId,int panchaytaId,int villageId, DateTime date)
        {
            logger.LogInformation("VillageController : GetCenterDetailByMonth : Started");
            try
            {
                string GenderRatio = await _dashboardManager.GetUserByFilter(type, districtId, vidhanSabhaId, panchaytaId, villageId, date);
                ContentResult contentResult = new ContentResult();
                contentResult.Content = GenderRatio;
                contentResult.ContentType = "application/json";

                return contentResult;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"VillageController : GetTotalStudentOfClass ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }
    }
}
