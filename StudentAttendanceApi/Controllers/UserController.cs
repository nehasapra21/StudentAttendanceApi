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
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager, ILogger<UserController> logger)
        {
            this.logger = logger;
            this._userManager = userManager;
        }

        [HttpPost("LoginSuperAdmin")]
        public async Task<IActionResult> LoginSuperAdmin(UserDto userDto)
        {
            logger.LogInformation("UserController : LoginSuperAdmin : Started");
            try
            {
                var masterAdmin = await _userManager.LoginSuperAdmin(userDto.Name, userDto.Password);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "SuperAdmin Login successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "SuperAdmin doesn't exists",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : LoginSuperAdmin ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }

        [HttpPost("SaveSuperAdmin")]
        public async Task<IActionResult> SaveSuperAdmin(UserDto userDto)
        {
            logger.LogInformation("UserController : LoginSuperAdmin : Started");
            try
            {
                Users user=UserConvertor.ConvertUsertoToUser(userDto);
                var masterAdmin = await _userManager.SaveSuperAdmin(user);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "SuperAdmin save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "SuperAdmin doesn't save",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : SaveSuperAdmin ", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, "error");
            }
        }
    }
}
