using System;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Providers.Allocation.Memory.StaffGroups
{
    /// <summary>
    /// Stores a representation of Pod A in memory
    /// </summary>
    public class PodAMemoryStore : PodMemoryStore
    {
        // The number of beds in this pod
        private const int BED_CAPACITY = 12;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Memory.StaffGroups.PodAMemoryStore"/> class.
        /// </summary>
        public PodAMemoryStore() : base(TeamType.A, BED_CAPACITY)
        {
        }
    }
}
