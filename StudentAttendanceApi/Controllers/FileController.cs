
using Api.Utility;

using FakeNewApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace StudentAttendanceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileController> logger;

        public FileController(IWebHostEnvironment webHostEnvironment, ILogger<FileController> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            this.logger = logger;
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
            logger.LogInformation("FileController : UploadProfileImage : Started");
            ImagesDto fileUrls = new ImagesDto();

            try
            {
                if (files.Count > 0)
                {
                    fileUrls =  FileApiUtilty.UploadFileInFolder(files, _webHostEnvironment);
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
