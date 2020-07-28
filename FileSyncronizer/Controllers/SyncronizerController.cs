using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FileSyncronizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncronizerController : ControllerBase
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        FileContext db;
        public SyncronizerController(IWebHostEnvironment environment, FileContext _db)
        {
            hostingEnvironment = environment;
            db = _db;
        }

        [AllowAnonymous]
        [RequestSizeLimit(100_000_000)]
        [HttpPost("FileUpload")]
        public async Task<IActionResult> Upload (IFormFile file,string EncryptionKey)
        {
            
            try
            {
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "FileDb");
                var File = new FileSyncronizer.Entities.File();
                File.CreatedDate = DateTime.Now;
                File.EncryptionKey = EncryptionKey;
                File.Extention = System.IO.Path.GetExtension(file.FileName);
                File.Stream = GetByteArrayFromImage(file);
                db.File.Add(File);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return new JsonResult(ex); 
            }
        

            return new JsonResult("Uploaded");


        }
        [AllowAnonymous]
        [HttpGet("Search")]

        public IActionResult SearchFile(string FileName)
        {
            var Search =  db.File.Where(x => x.FileName.Contains(FileName)).Select(x=> new { 
            Id= x.Id,
            FileName = x.FileName,
            Extention = x.Extention
            });
            return new JsonResult(Search);
        }

        [HttpGet("FileDownload")]
        public async Task<FileStreamResult> DownloadFile (int Id, string Key)
        {
            var files = (await db.File.SingleOrDefaultAsync(x => x.Id == Id));
            var fileName = ((await db.File.SingleOrDefaultAsync(x => x.Id == Id)).FileName);
            var mimeType = "";
            if (files.Extention== ".abw")
            {
                mimeType = "application/x-abiword";
            }
            else if (files.Extention == ".arc")
            {
                mimeType = "application/x-freearc";
            }
            else if (files.Extention == ".bin")
            {
                mimeType = "application/octet-stream";
            }
            else if (files.Extention == ".bmp")
            {
                mimeType = "image/bmp";
            }
            else if (files.Extention == ".bz")
            {
                mimeType = "application/x-bzip";
            }
            else if (files.Extention == ".bz2")
            {
                mimeType = "application/x-bzip2";
            }
            else if (files.Extention == ".csh")
            {
                mimeType = "application/x-csh";
            }
            else if (files.Extention == ".css")
            {
                mimeType = "text/css";
            }
            else if (files.Extention == ".csv")
            {
                mimeType = "text/csv";
            }
            else if (files.Extention == ".doc")
            {
                mimeType = "application/msword";
            }
            else if (files.Extention == ".docx")
            {
                mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }
            else if (files.Extention == ".eot")
            {
                mimeType = "application/vnd.ms-fontobject";
            }
            else if (files.Extention == ".epub")
            {
                mimeType = "application/epub+zip";
            }
            else if (files.Extention == ".gz")
            {
                mimeType = "application/gzip";
            }
            else if (files.Extention == ".gif")
            {
                mimeType = "image/gif";
            }
            else if (files.Extention == ".html")
            {
                mimeType = "text/html";
            }
            else if (files.Extention == ".ico")
            {
                mimeType = "image/vnd.microsoft.icon";
            }
            else if (files.Extention == ".mp3")
            {
                mimeType = "audio/mpeg";
            }
            else if (files.Extention == ".php")
            {
                mimeType = "application/x-httpd-php";
            }
            else if (files.Extention == ".ppt")
            {
                mimeType = "application/vnd.ms-powerpoint";
            }
            else if (files.Extention == ".pptx")
            {
                mimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            }

            var file = (await db.File.SingleOrDefaultAsync(x => x.Id == Id)).Stream;
            Stream stream =  new MemoryStream(file);

            return new FileStreamResult(stream, mimeType)
            {
                FileDownloadName = fileName
            };
        }


        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
               
                return target.ToArray();
            }
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        [HttpGet("GetThreadInfo")]
        public IActionResult ThreadInfo()
             => new JsonResult(GetThreadInfo());
        private dynamic GetThreadInfo()
        {
            int availableWorkerThreads;
            int availableAsyncIOThreads;
            ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableAsyncIOThreads);
            return new { AvailableAsyncIOThreads = availableAsyncIOThreads, AvailableWorkerThreads = availableWorkerThreads };
        }
    }
}
