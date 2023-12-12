﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAttendanceApiBLL;
using StudentAttendanceApiBLL.Dto;
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

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
        {
            logger.LogInformation("UserController : LoginUser : Started");
            try
            {
                var masterAdmin = await _userManager.LoginUser(loginDto.MobileNumber, loginDto.Password);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "Login successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "invalid credential",
                        code = StatusCodes.Status404NotFound
                    });
                }
                //Dictionary<int, object> userData = await _userManager.LoginUser(loginDto.MobileNumber, loginDto.Password);
                //Dictionary<string, object> data = new Dictionary<string, object>();
                //data.Add("Key", userData.Keys.ToList());
                //data.Add("Value", userData.Values.ToList());
                //if (data!=null)
                //{
                //    return StatusCode(StatusCodes.Status200OK, new
                //    {
                //        status = true,
                //        data = data,
                //        code = StatusCodes.Status200OK
                //    });
                //}
                //else
                //{
                //    return StatusCode(StatusCodes.Status404NotFound, new
                //    {
                //        status = false,
                //        error = "error",
                //        code = StatusCodes.Status404NotFound
                //    });
                //}

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : LoginUser ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpPost("SaveSuperAdmin")]
        public async Task<IActionResult> SaveSuperAdmin([FromForm] SuperAdminDto superAdminDto)
        {
            logger.LogInformation("UserController : SaveSuperAdmin : Started");
            try
            {
                UserDto userDto = UserConvertor.ConvertSuperAdminUsertoToUserDto(superAdminDto);
                var masterAdmin = await _userManager.SaveLogin(userDto);
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
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }


        [Authorize]
        [HttpPost("UpdateDeviceId")]
        public async Task<IActionResult> UpdateDeviceId([FromForm] UserDeviceDto userDto)
        {
            logger.LogInformation("UserController : SaveSuperAdmin : Started");
            try
            {
                var masterAdmin = await _userManager.UpdateDeviceId(userDto.userId, userDto.DeviceId);
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
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }


        [Authorize]
        [HttpPost("SaveUser")]
        public async Task<IActionResult> SaveUser([FromForm] UserDto userDto)
        {
            logger.LogInformation("UserController : SaveUser : Started");
            try
            {
                if (userDto.Type == 2 && userDto.ListOfPanchayatIds == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "ListOfPanchayatIds Parameter is missing",
                        code = StatusCodes.Status404NotFound
                    });
                }

             
                var masterAdmin = await _userManager.SaveLogin(userDto);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "Data save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "data doesn't save",
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

        [Authorize]
        [HttpPost("UpdateSuperAdminUser")]
        public async Task<IActionResult> UpdateSuperAdminUser([FromForm] UserDto userDto)
        {
            logger.LogInformation("UserController : SaveUser : Started");
            try
            {
                var masterAdmin = await _userManager.UpdateSuperAdminUser(userDto);
                if (masterAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = masterAdmin,
                        message = "Data save successfully",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = "data doesn't save",
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

        [Authorize]
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            logger.LogInformation("UserController : GetUser : Started");
            try
            {
                var user = await _userManager.GetUserById(userId);
                if (user != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = user,
                        message = "user exists",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = user,
                        message = "user not exists",
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
        //[HttpPost("CheckUserMobileNumber")]
        //public async Task<IActionResult> CheckUserMobileNumber(string mobileNumber)
        //{
        //    logger.LogInformation("UserController : CheckUserMobileNumber : Started");
        //    try
        //    {
        //        var mobileNo = await _userManager.CheckUserMobileNumber(mobileNumber);
        //        if (mobileNo != null)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, new
        //            {
        //                status = false,
        //                message = "Mobile number already exists",
        //                code = StatusCodes.Status200OK
        //            });
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, new
        //            {
        //                status = true,
        //                error = "Mobile number doesn't exists",
        //                code = StatusCodes.Status404NotFound
        //            });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, $"UserController : SaveSuperAdmin ", ex);
        //        return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
        //    }
        //}

        [Authorize]
        [HttpGet("GetAllTeachers")]
        public async Task<IActionResult> GetAllTeachers(int userId = 0)
        {
            logger.LogInformation("UserController : GetRegisteredTeachers : Started");
            try
            {
                var allTeachers = await _userManager.GetAllTeachers(userId);
                if (allTeachers != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = allTeachers,
                        message = "List of assigned teachers",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = allTeachers,
                        message = "Asigned teachers not found",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : GetRegisteredTeachers ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpGet("GetAllUnAssignedTeacher")]
        public async Task<IActionResult> GetAllUnAssignedTeacher()
        {
            logger.LogInformation("UserController : GetRegisterTeachers : Started");
            try
            {
                var allTeachers = await _userManager.GetAllUnAssignedTeacher();
                if (allTeachers != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = allTeachers,
                        message = "List of teachers",
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = allTeachers,
                        message = "List of teachers not found",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : GetRegisteredTeachers ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpGet("GetAllRegionalAdmins")]
        public async Task<IActionResult> GetAllRegionalAdmins()
        {
            logger.LogInformation("UserController : GetAllRegionalAdmins : Started");
            try
            {
                var allRegionalAdmin = await _userManager.GetAllRegionalAdmins();
                if (allRegionalAdmin != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = allRegionalAdmin,
                        message = "List of regional admins",
                        code = StatusCodes.Status200OK
                    });

                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        data = allRegionalAdmin,
                        message = "List of regional admins not found",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : GetAllRegionalAdmins ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpGet("SearchData")]
        public async Task<IActionResult> SearchData(string type, string queryString)
        {
            logger.LogInformation("UserController : SearchData : Started");
            try
            {
                var searchedData = await _userManager.SearchData(type,queryString);
                if (searchedData != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        data = searchedData,
                        message = "List of search data",
                        code = StatusCodes.Status200OK
                    });

                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        message = "List of search data not found",
                        code = StatusCodes.Status404NotFound
                    });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserController : GetAllRegionalAdmins ", ex);
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

    }
}
