﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAttendanceApiBLL;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiBLL.Manager;
using StudentAttendanceApiDAL.IRepository;

namespace StudentAttendanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterAdminController : ControllerBase
    {
        private readonly ILogger<MasterAdminController> logger;
        private readonly IMasterAdminManager _masterAdminManager;
        

        public MasterAdminController(IMasterAdminManager masterAdminManager, ILogger<MasterAdminController> logger)
        {
            this.logger = logger;
            this._masterAdminManager = masterAdminManager;
        }

        [HttpGet("GetAllMasterAdmin")]
        public async Task<IActionResult> GetAllMasterAdmin()
        {
            logger.LogInformation("MasterAdminController : GetAllMasterAdmin : Started");
            try
            {
                return Ok(await _masterAdminManager.GetAllMasterAdmin());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"MasterAdminController : GetAllMasterAdmin ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }

        [HttpPost("SaveMasterAdmin")]
        public async Task<ActionResult> SaveMasterAdmin([FromBody] MasterAdminDto masterAdminDto)
        {
            logger.LogInformation("MasterAdminController : SaveMasterAdmin : Started");
            try
            {
                var masterAdmin = MasterAdminConvertor.ConvertMasterAdminDtoToMasterAdmin(masterAdminDto);
                var masterAdminVal = await _masterAdminManager.SaveMasterAdmin(masterAdmin);
                logger.LogInformation("MasterAdminController : SaveMasterAdmin : End");
                if (masterAdminVal == null)
                    return BadRequest(new { message = "masterAdmin" });
                return new OkObjectResult(masterAdminVal);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"MasterAdminController : SaveMasterAdmin ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }
    }
}
