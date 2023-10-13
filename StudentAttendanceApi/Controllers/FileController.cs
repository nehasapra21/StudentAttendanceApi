
using Api.Utility;
using ExcelDataReader;
using FakeNewApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentAttendanceApi.FCM;
using StudentAttendanceApi.Services;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace StudentAttendanceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileController> logger;
        private readonly INotificationService _notificationService;
        private readonly IUserManager _userManager;

        public FileController(IWebHostEnvironment webHostEnvironment, ILogger<FileController> logger, INotificationService notificationService, IUserManager userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            _notificationService = notificationService;
            this._userManager = userManager;
        }

        [Route("SendNotification")]
        [HttpPost]
        public async Task<IActionResult> SendNotification([FromForm] NotificationModel notificationModel)
        {
            try
            {
                notificationModel.DeviceId = await _userManager.GetUserDeviceByUserId(notificationModel.userId);

                ResponseModel result = await _notificationService.SendNotification(notificationModel);
                if (result != null && result.IsSuccess)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = true,
                        message = result.Message,
                        code = StatusCodes.Status200OK
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        status = false,
                        error = result.Message,
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
        //[HttpPost("UploadProfileImage")]
        //public IActionResult UploadProfileImage(string base64img)
        //{
        //    logger.LogInformation("FileController : UploadProfileImage : Started");
        //    ImagesDto fileUrls = new ImagesDto();

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(base64img))
        //        {
        //            fileUrls = FileApiUtilty.UploadFileInFolder(base64img, _webHostEnvironment);
        //            logger.LogInformation("FileController : UploadProfileImage : End");
        //            if (fileUrls.FilePath != null)
        //            {
        //                return StatusCode(StatusCodes.Status200OK, new
        //                {
        //                    status = true,
        //                    data = fileUrls,
        //                    code = StatusCodes.Status200OK
        //                }); ;
        //            }
        //            else
        //            {
        //                return StatusCode(StatusCodes.Status200OK, new
        //                {
        //                    status = false,
        //                    error = "File url not found",
        //                    code = StatusCodes.Status204NoContent
        //                });
        //            }

        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status200OK, new
        //            {
        //                status = false,
        //                error = "File not found",
        //                code = StatusCodes.Status204NoContent
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, $"FileController : UploadProfileImage ", ex);
        //        return StatusCode(StatusCodes.Status404NotFound, new
        //        {
        //            status = false,
        //            type = ex.GetType().FullName,
        //            error = ex.InnerException.Message,
        //            code = StatusCodes.Status404NotFound
        //        });
        //    }
        //}


        [HttpPost("UploadProfileImage")]
        public IActionResult UploadProfileImage([FromForm] List<IFormFile> files)
        {
            logger.LogInformation("FileController : UploadProfileImage : Started" + _webHostEnvironment);
            logger.LogInformation("FileController : UploadProfileImage : Started");
            ImagesDto fileUrls = new ImagesDto();

            try
            {
                if (files.Count > 0)
                {
                    fileUrls = FileApiUtilty.UploadFileInFolder(files, _webHostEnvironment);
                    logger.LogInformation("FileController : UploadProfileImage : End");
                    if (fileUrls.FilePath != null)
                    {
                        return StatusCode(StatusCodes.Status200OK, new
                        {
                            status = true,
                            data = fileUrls,
                            code = StatusCodes.Status200OK
                        }); ;
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new
                        {
                            status = false,
                            error = "File url not found",
                            code = StatusCodes.Status204NoContent
                        });
                    }

                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        status = false,
                        error = "File not found",
                        code = StatusCodes.Status204NoContent
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"FileController : UploadProfileImage ", ex);
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    status = false,
                    type = ex.GetType().FullName,
                    error = ex.InnerException.Message,
                    code = StatusCodes.Status404NotFound
                });
            }

        }

        //[HttpPost("UploadExcelData")]
        //public async Task<IActionResult> UploadExcelData([FromForm] List<IFormFile> files)
        //{
        //    logger.LogInformation("FileController : UploadExcelData : Started" + _webHostEnvironment);
        //    logger.LogInformation("FileController : UploadExcelData : Started");
        //    ImagesDto fileUrls = new ImagesDto();
        //    List<string> str = new List<string>(0);
        //    try
        //    {
        //        foreach (var file in files)
        //        {
        //            var list = new List<ExcelDto>();
        //            DataSet dsexcelRecords = new DataSet();
        //            IExcelDataReader reader = null;
        //            Stream FileStream = null;

        //            // Create the Directory if it is not exist
        //            string dirPath = Path.Combine(_webHostEnvironment.WebRootPath, "ReceivedReports");
        //            if (!Directory.Exists(dirPath))
        //            {
        //                Directory.CreateDirectory(dirPath);
        //            }

        //            string dataFileName = Path.GetFileName(file.FileName);

        //            string extension = Path.GetExtension(dataFileName);

        //            string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

        //            // Make a Copy of the Posted File from the Received HTTP Request
        //            string saveToPath = Path.Combine(dirPath, dataFileName);

        //            using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //            }

        //            // read the excel file
        //            using (var stream = new FileStream(saveToPath, FileMode.Open))
        //            {
        //                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //                if (extension == ".xls")
        //                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
        //                else
        //                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        //                DataSet ds = new DataSet();
        //                ds = reader.AsDataSet();
        //                reader.Close();

        //                if (ds != null && ds.Tables.Count > 0)
        //                {
        //                    // Read the the Table
        //                    DataTable serviceDetails = ds.Tables[0];
        //                    List<string> name = new List<string>();
        //                    serviceDetails.Rows.RemoveAt(0);
        //                    for (int i = 1; i < serviceDetails.Rows.Count; i++)
        //                    {
        //                        ExcelDto details = new ExcelDto();
        //                        details.Name = serviceDetails.Rows[i][0].ToString();
        //                        details.Password = serviceDetails.Rows[i][1].ToString();
        //                        name.Add(details.Password);
        //                        // Add the record in Database
        //                        // await context.CustomerResponseDetails.AddAsync(details);
        //                        //await context.SaveChangesAsync();
        //                    }
        //                    str = await _userManager.GetPassword(name);
        //                }
        //            }
        //        }
        //        return StatusCode(StatusCodes.Status200OK, new
        //        {
        //            status = false,
        //            data= str,
        //            error = "File url not found",
        //            code = StatusCodes.Status204NoContent
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, $"FileController : UploadProfileImage ", ex);
        //        return StatusCode(StatusCodes.Status404NotFound, new
        //        {
        //            status = false,
        //            type = ex.GetType().FullName,
        //            error = ex.InnerException.Message,
        //            code = StatusCodes.Status404NotFound
        //        });
        //    }
        //}

        //[HttpGet("DownloadFile")]
        //public async Task<byte[]> DownloadFile(string filePath)
        //{
        //    byte[] content = null;
        //    logger.LogInformation("FileController : DownloadFile : Started");
        //    try
        //    {
        //        var fileCompletePath = FileApiUtilty.GetFullPathOfFile(filePath, _webHostEnvironment);
        //        var memory = new MemoryStream();
        //        using (var stream = new FileStream(fileCompletePath, FileMode.Open))
        //        {
        //            await stream.CopyToAsync(memory);
        //        }
        //        memory.Position = 0;
        //        content = await System.IO.File.ReadAllBytesAsync(fileCompletePath);
        //        logger.LogInformation("FileController : DownloadFile : End");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, $"FileController : DownloadFile ", ex);
        //    }
        //    return content;
        //}


        //[HttpGet("DeleteFile")]
        //public async Task<ActionResult> DeleteFile(string filePath)
        //{
        //    logger.LogInformation("FileController : DeleteFile : Started");
        //    try
        //    {
        //        var fileCompletePath = FileApiUtilty.GetFullPathOfFile(filePath, _webHostEnvironment);
        //        System.IO.File.Delete(fileCompletePath);

        //        var isDeleted = _fileManager.DeleteFile(filePath);

        //        logger.LogInformation("FileController : DeleteFile : End");
        //        return new OkObjectResult(isDeleted);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, $"FileController : DeleteFile ", ex);
        //        return StatusCode(StatusCodes.Status501NotImplemented, "file error");
        //    }
        //}
    }
}
