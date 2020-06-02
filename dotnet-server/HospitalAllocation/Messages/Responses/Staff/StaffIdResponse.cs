using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using HospitalAllocation.Messages.Responses;

namespace HospitalAllocation.Messages.Responses.Staff
{
    /// <summary>
    /// A message to confirm the successful creation of a staff member and inform
    /// the caller of the staff member entry's addressing ID
    /// </summary>
    [DataContract]
    public class StaffIdResponse : ApiResponse
    {
        /// <summary>
        /// Create a new staff creation response message
        /// </summary>
        /// <param name="staffId">the ID of the newly created staff member</param>
        public StaffIdResponse(int staffId) : base(ResponseStatus.Success)
        {
            StaffId = staffId;
        }

        /// <summary>
        /// The ID of the newly created staff member
        /// </summary>
        [DataMember(Name = "staff_id")]
        public int StaffId { get; }
    }
}
