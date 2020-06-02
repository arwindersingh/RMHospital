using System;
using System.Runtime.Serialization;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Messages.Responses.Allocation
{
    /// <summary>
    /// An API response to display the allocation of a single team
    /// </summary>
    [DataContract]
    public class AllocationResponse : ApiResponse
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Messages.Responses.Allocation.AllocationResponse"/> class.
        /// </summary>
        /// <param name="teamAllocation">Team allocation.</param>
        public AllocationResponse(Team teamAllocation) : base(ResponseStatus.Success)
        {
            Allocation = teamAllocation;
        }

        /// <summary>
        /// Contains the allocation of the given team
        /// </summary>
        /// <value>The allocation.</value>
        [DataMember(Name = "team")]
        public Team Allocation { get; }
    }
}
