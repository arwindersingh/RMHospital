using System;
using System.Runtime.Serialization;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Messages.Requests.Allocation
{
    /// <summary>
    /// Format of requests to the API to allocate a new senior team
    /// </summary>
    [DataContract]
    public class SeniorAllocationRequest
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Messages.Requests.Allocation.SeniorAllocationRequest"/> class.
        /// </summary>
        /// <param name="seniorTeam">Senior team.</param>
        public SeniorAllocationRequest(SeniorTeam seniorTeam)
        {
            SeniorTeam = seniorTeam;
        }

        /// <summary>
        /// The new allocation of the senior team
        /// </summary>
        /// <value>The senior team.</value>
        [DataMember(Name = "team")]
        public SeniorTeam SeniorTeam { get; }
    }
}
