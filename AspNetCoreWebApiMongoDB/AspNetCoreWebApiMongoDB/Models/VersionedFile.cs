using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Models
{
    public class VersionedFile
    {

        public VersionedFile(Stream stream, VersionedFileInfo fileInfo)
        {
            Stream = stream;
            FileInfo = fileInfo;
        }


        public Stream Stream { get; }


        public VersionedFileInfo FileInfo { get; }
    }
}
