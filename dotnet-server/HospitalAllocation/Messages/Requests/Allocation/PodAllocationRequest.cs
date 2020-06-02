using System;
using System.Runtime.Serialization;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Messages.Requests.Allocation
{
    /// <summary>
    /// Format of a request sent to the API to allocate a pod
    /// </summary>
    [DataContract]
    public class PodAllocationRequest
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Messages.Requests.Allocation.PodAllocationRequest"/> class.
        /// </summary>
        /// <param name="pod">Pod.</param>
        public PodAllocationRequest(Pod pod)
        {
            Pod = pod;
        }

        /// <summary>
        /// The new allocation of the pod to allocate
        /// </summary>
        /// <value>The pod.</value>
        [DataMember(Name = "team")]
        public Pod Pod { get; }
    }
}
