using System;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Providers.Allocation.Memory.StaffGroups
{
    /// <summary>
    /// Stores an in-memory representation of Pod D
    /// </summary>
    public class PodDMemoryStore : PodMemoryStore
    {
        // The number of beds in this pod
        private const int BED_CAPACITY = 12;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Memory.StaffGroups.PodDMemoryStore"/> class.
        /// </summary>
        public PodDMemoryStore() : base(TeamType.D, BED_CAPACITY)
        {
        }
    }
}
