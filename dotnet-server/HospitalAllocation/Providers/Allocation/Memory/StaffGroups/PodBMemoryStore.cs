using System;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Providers.Allocation.Memory.StaffGroups
{
    /// <summary>
    /// Stores a representation of Pod B in memory
    /// </summary>
    public class PodBMemoryStore : PodMemoryStore
    {
        // The number of beds in this pod
        private const int BED_CAPACITY = 10;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Memory.StaffGroups.PodBMemoryStore"/> class.
        /// </summary>
        public PodBMemoryStore() : base(TeamType.B, BED_CAPACITY)
        {
        }
    }
}
