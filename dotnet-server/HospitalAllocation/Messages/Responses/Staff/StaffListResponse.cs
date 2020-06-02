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
    /// Message communicating a list of staff members as queried
    /// </summary>
    [DataContract]
    public class StaffListResponse : ApiResponse
    {
        /// <summary>
        /// Construct a new staff list response from a list of staff members
        /// </summary>
        /// <param name="staffMembers"></param>
        public StaffListResponse(IReadOnlyCollection<IdentifiedStaffMember> staffMembers) : base(ResponseStatus.Success)
        {
            StaffMembers = staffMembers;
        }

        /// <summary>
        /// The list of staff members to return
        /// </summary>
        [DataMember(Name = "staff_list")]
        public IReadOnlyCollection<IdentifiedStaffMember> StaffMembers { get; }
    }
}
