
using FakeNewApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Utility
{
    public static class FileApiUtilty
    {
        public static ImagesDto UploadFileInFolder(string base64img, IWebHostEnvironment webHostEnvironment)
        {
            ImagesDto imagesDto = new ImagesDto();

            if (base64img != null)
            {
                string folder = @"UploadProfileImage\";

                //Getting FileName
                var fileName = Path.GetFileName("test.jpg");

                //Assigning Unique Filename (Guid)
                var myUniqueFileName = Convert.ToString(Guid.NewGuid());


                folder += myUniqueFileName + "_" + fileName;

                string filePath = Path.Combine(webHostEnvironment.WebRootPath, folder).Replace(@"\\", @"\"); ;

                imagesDto.ImageName = fileName;
                imagesDto.FilePath = filePath;

                string toBeSearched = "wwwroot";
                int ix = filePath.IndexOf(toBeSearched);
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string folderPathUrl= Path.Combine(filePath, fileName);
                System.IO.File.WriteAllBytes(folderPathUrl, Convert.FromBase64String(base64img));


                //using (FileStream fs = File.Create(filePath))
                //    {
                //        file.CopyTo(fs);
                //        fs.Flush();
                //    }

                // string fileUrl = string.Empty;
                //if (ix != -1)
                //{
                //    fileUrl = filePath.Substring(ix + toBeSearched.Length);
                //}
                imagesDto.FilePath = folderPathUrl;
                imagesDto.ImageName = fileName;
            }

            return imagesDto;
        }

        public static string GetFullPathOfFile(string fileName, IWebHostEnvironment webHostEnvironment)
        {
            return $"{webHostEnvironment.WebRootPath}\\UploadFiles\\{fileName}";
        }

        public static Dictionary<string, string> GetMimeType()
        {
            return new Dictionary<string, string>
            {
                {".txt","text/plain"},
                {".png","image/png" },
                {".jpg","image/jpg" }
            };
        }
    }
}
