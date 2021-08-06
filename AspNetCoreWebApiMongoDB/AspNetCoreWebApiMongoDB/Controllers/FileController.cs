using AspNetCoreWebApiMongoDB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {

        private CrewService _crewService;


        public FileController(CrewService svc)
        {
            this._crewService = svc;
        }

        [HttpPut()]
        [DisableRequestSizeLimit]
        public void Upload(IFormFile file)
        {
            this._crewService.SaveCrewFile(file);
        }

        [HttpPut("versioned/{version}")]
        [DisableRequestSizeLimit]
        public void UploadNewFileVersion(IFormFile formFile, string version)
        {
            this._crewService.StoreNewFileVersion(formFile, version);
        }

        // https://codeburst.io/download-files-using-web-api-ae1d1025f0a9
        [HttpGet("{fileName}")]
        public async Task<ActionResult> DownloadFile(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var file = File(this._crewService.DownloadFile(fileName), contentType);
            return file;
        }


        [HttpGet("open-stream/{fileName}")]
        public async Task<ActionResult> DownloadFileOpenStream(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var file = File(this._crewService.DownloadFile(fileName), contentType);
            return file;
        }

        [HttpGet("open-stream-versioned/{fileName}/{version}")]
        public async Task<ActionResult> DownloadFileOpenStreamVersionedFile(string fileName, string version)
        {

            var fileInfo = this._crewService.DownloadVersionedFile(fileName, version);
            return File(fileInfo.Stream, fileInfo.FileInfo.ContentType, fileInfo.FileInfo.FileName);
        }
    }
}
