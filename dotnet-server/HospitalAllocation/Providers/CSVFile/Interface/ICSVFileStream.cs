using System;
using System.IO;
using Microsoft.AspNetCore.Http;


namespace HospitalAllocation.Providers.CSVFile.Interface
{
    /// <summary>
    /// Describes a data source for a CSVFile
    /// </summary>
    public interface ICSVFileStream
    {
        CSVFileFormat Format { get; }

        long Length {get;}

        IFormFile File { get; }

        DateTime TimeStamp { get; }

        void CopyTo(Stream stream);
    }
}
