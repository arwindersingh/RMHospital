using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using HospitalAllocation.Messages.Responses;

namespace HospitalAllocation.Messages.Responses.CSVFile
{
    /// <summary>
    /// A message to confirm the successful upload of a CSV and inform
    /// the caller of the ID
    /// </summary>
    [DataContract]
    public class CSVFileIdResponse : ApiResponse
    {
        /// <summary>
        /// Create a new CSV upload response message
        /// </summary>
        /// <param name="id">the ID of the newly uploaded CSV file</param>
        public CSVFileIdResponse(int id) : base(ResponseStatus.Success)
        {
            Id = id;
        }

        /// <summary>
        /// The ID of the newly uploaded CSV file
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; }
    }
}
