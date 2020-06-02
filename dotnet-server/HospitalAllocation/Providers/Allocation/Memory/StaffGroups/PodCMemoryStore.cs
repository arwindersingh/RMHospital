using System;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Providers.Allocation.Memory.StaffGroups
{
    /// <summary>
    /// Stores a representation of Pod C in memory
    /// </summary>
    public class PodCMemoryStore : PodMemoryStore
    {
        // The number of beds in this pod
        private const int BED_CAPACITY = 10;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Memory.StaffGroups.PodCMemoryStore"/> class.
        /// </summary>
        public PodCMemoryStore() : base(TeamType.C, BED_CAPACITY)
        {
        }
    }
}
