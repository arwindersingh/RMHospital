using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using HospitalAllocation.Providers.CSVFile.Interface;

namespace HospitalAllocation.Providers.CSVFile.Database
{
    public class MemoryCSVFileStream : ICSVFileStream
    {
        private readonly byte[] _csvfileData;

        public MemoryCSVFileStream(byte[] csvfileData , DateTime timestamp, CSVFileFormat format )
        {
            _csvfileData = csvfileData;
            TimeStamp = timestamp;
            Format = format;
        }

        public CSVFileFormat Format {get;}

        public long Length {get;}
        public IFormFile File { get; }

        public DateTime TimeStamp {get; }

        public void CopyTo(Stream stream)
        {
            var byteStream = new MemoryStream(_csvfileData);
            byteStream.CopyTo(stream);
        }
    }
}
