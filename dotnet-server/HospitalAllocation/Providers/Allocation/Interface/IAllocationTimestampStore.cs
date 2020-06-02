using HospitalAllocation.Data.Allocation.StaffGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAllocation.Providers.Allocation.Interface
{
    /// <summary>
    /// Provides an interface to the allocation and access to past allocations.
    /// </summary>
    public interface IAllocationTimestampStore : IAllocationStore
    {
        /// <summary>
        /// Search for the allocation at a certain time for a certain pod.
        /// </summary>
        /// <param name="time">The time to search for in seconds since UNIX epoch.</param>
        /// <param name="pod">The pod to search for.</param>
        /// <returns>The allocation at the time for the pod if it exists, null otherwise.</returns>
        Pod GetPastPod(long time, TeamType pod);

        /// <summary>
        /// Search for the allocation at a certain time for the senior team.
        /// </summary>
        /// <param name="time">The time to search for in seconds since UNIX epoch.</param>
        /// <returns>The senior team allocation at the time if it exists, null otherwise.</returns>
        SeniorTeam GetPastSeniorTeam(long time);
    }
}
