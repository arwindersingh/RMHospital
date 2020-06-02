using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HospitalAllocation.Providers.CSVFile.Interface;
using HospitalAllocation.Messages.Responses;
using HospitalAllocation.Messages.Responses.CSVFile;
using HospitalAllocation.Model;

namespace HospitalAllocation.Controllers
{
    /// <summary>
    /// Provides an API for the CSVFile storage and retrieval from the backend
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]")]
    public class CsvfileController : Controller
    {
        /// <summary>
        /// CSVFile provider object which provide access to the data only through
        /// constructor.
        /// </summary>
        private readonly ICSVFileProvider _csvfileProvider;

        /// <summary>
        /// Initialize an object with the supplied csvfile provider.
        /// </summary>
        /// <param name="csvfileProvider">the supplied csvfile provider</param>
        public CsvfileController(ICSVFileProvider csvfileProvider)
        {
            _csvfileProvider = csvfileProvider;
        }

        /// <summary>
        /// Retrieve most recent CSV
        /// </summary>
        /// <returns>The csv file given by timestamp</returns>
        [HttpGet]
        public IActionResult RetrieveMostRecentCSVFile()
        {
            ICSVFileStream csvfile = _csvfileProvider.GetMostRecentCSVFile();

            if (csvfile == null)
            {
                return BadRequest(new ErrorResponse("No such csv file"));
            }

            // We need to copy the csvfile to an internal memory stream in order
            // to transfer it out with a byte array. This is probably inefficient
            // since we copy once to the filesystem to memory and then from
            // memory to HTTP - worth refactoring later
            var dataStream = new MemoryStream();
            csvfile.CopyTo(dataStream);
            return File(dataStream.ToArray(), csvfile.Format.ContentTypeString());
        }

        /// <summary>
        /// Retrieve a CSVFile from the data store given its id
        /// </summary>
        /// <param name="id">The unique numerical timestamp of the csv file</param>
        /// <returns>The csv file given by timestamp</returns>
        [HttpGet("{id}")]
        public IActionResult RetrieveCSVFile(int id)
        {
            ICSVFileStream csvfile = _csvfileProvider.GetCSVFile(id);

            if (csvfile == null)
            {
                return BadRequest(new ErrorResponse("No such CSV file"));
            }

            // We need to copy the csvfile to an internal memory stream in order
            // to transfer it out with a byte array. This is probably inefficient
            // since we copy once to the filesystem to memory and then from
            // memory to HTTP - worth refactoring later
            var dataStream = new MemoryStream();
            csvfile.CopyTo(dataStream);

            return File(dataStream.ToArray(), csvfile.Format.ContentTypeString());
        }
       
        /// <summary>
        /// Upload a csv file to be stored by the backend
        /// </summary>
        /// <param name="file">the csv file uploaded in the HTTP stream</param>
        /// <returns>A message containing the TimeStamp assigned to the uploaded csv file</returns>
        [HttpPost]
        public IActionResult UploadCSVFile(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest(new ErrorResponse("Bad request format."));
            }

            ICSVFileStream csvfile = HttpCSVFileStream.CreateFromIFormFile(file);
            if (csvfile == null)
            {
                return BadRequest(new ErrorResponse("Invalid CSVFile or unsupported format"));
            }

            try
            {
                CSVFileStorage savedCSV = _csvfileProvider.CreateCSVFile(csvfile);
                return Json(new CSVFileIdResponse(savedCSV.CSVFileID));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }
    }
}