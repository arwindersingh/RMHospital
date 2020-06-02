using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using HospitalAllocation.Data.StaffMember;
using HospitalAllocation.Messages.Responses;

namespace HospitalAllocation.Messages.Responses.Staff
{
    /// <summary>
    /// Message to return a single staff member value to the caller
    /// </summary>
    [DataContract]
    public class StaffResponse : ApiResponse
    {
        /// <summary>
        /// Create a new staff response message with a staff member value
        /// </summary>
        /// <param name="staffMember"></param>
        public StaffResponse(StaffMember staffMember) : base(ResponseStatus.Success)
        {
            StaffMember = staffMember;
        }

        /// <summary>
        /// The staff member value being returned to the caller
        /// </summary>
        [DataMember(Name = "staff")]
        public StaffMember StaffMember { get; }
    }
}
