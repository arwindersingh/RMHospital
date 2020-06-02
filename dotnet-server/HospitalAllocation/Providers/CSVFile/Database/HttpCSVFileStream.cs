using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.AzureAppServices.Internal;

namespace HospitalAllocation.Providers.CSVFile.Interface
{
    public class HttpCSVFileStream : ICSVFileStream
    {
        private static readonly IDictionary<string, CSVFileFormat> s_contentTypeFormats;

        /// <summary>
        /// Initializes static variables used by the 
        /// <see cref="T:HospitalAllocation.Providers.Image.Interface.HttpCSVFileStream"/> class.
        /// </summary>
        static HttpCSVFileStream()
        {
            s_contentTypeFormats = new Dictionary<string, CSVFileFormat>()
            {
                { "csvfile/csv", CSVFileFormat.csv },
                { "text/csv", CSVFileFormat.csv }
            };
        }

        public static HttpCSVFileStream CreateFromIFormFile(IFormFile formFile)
        {
            if (!s_contentTypeFormats.TryGetValue(formFile.ContentType, out CSVFileFormat format))
            {
                return null;
            }

            return new HttpCSVFileStream(formFile, format);
        }

        private readonly IFormFile _httpFileStream;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:HostpitalAllocation.Providers.CSVFile.Interface.HttpCSVFileStream"/> class.
        /// 
        /// </summary>
        /// <param name="formFile">Form file.</param>
        /// <param name="format">Format.</param>
        private HttpCSVFileStream(IFormFile formFile, CSVFileFormat format)
        {
            _httpFileStream = formFile;
            Format = format;
            
        }

        public CSVFileFormat Format { get; }

        public long Length { get => _httpFileStream.Length;  }

        public DateTime TimeStamp { get => DateTime.Now; }

        public IFormFile File { get => _httpFileStream;  }

        public void CopyTo(Stream stream)
        {
            _httpFileStream.CopyTo(stream);
        }
    }
}
