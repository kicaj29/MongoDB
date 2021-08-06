using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Models
{
    public class VersionedFileInfo
    {
        public VersionedFileInfo(string fileId, string version, string contentType, string name, long size, DateTime creationTime)
        {
            FileId = fileId;
            VersionName = version;
            ContentType = contentType;
            FileName = Path.GetFileName(name);
            FileSize = size;
            CreationTime = creationTime;
        }

        public string FileId { get; set; }

        public string VersionName { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public long FileSize { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
