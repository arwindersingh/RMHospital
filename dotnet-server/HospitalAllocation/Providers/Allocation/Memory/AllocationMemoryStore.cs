using System;
using HospitalAllocation.Providers.Allocation.Interface;
using HospitalAllocation.Providers.Allocation.Memory.StaffGroups;
using HospitalAllocation.Data.Allocation;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Providers.Allocation.Memory
{
    /// <summary>
    /// A naive allocation store that just keeps an in-memory
    /// record of the allocation and cannot be persisted
    /// </summary>
    public class AllocationMemoryStore : IAllocationStore
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Memory.AllocationMemoryStore"/> class.
        /// </summary>
        public AllocationMemoryStore()
        {
            PodA = new PodAMemoryStore();
            PodB = new PodBMemoryStore();
            PodC = new PodCMemoryStore();
            PodD = new PodDMemoryStore();
            SeniorTeam = new SeniorTeamMemoryStore();
        }

        /// <summary>
        /// Provides Pod A
        /// </summary>
        /// <value>The pod a.</value>
        public IPodStore PodA { get; }

        /// <summary>
        /// Provides Pod B
        /// </summary>
        /// <value>The pod b.</value>
        public IPodStore PodB { get; }

        /// <summary>
        /// Provides Pod C
        /// </summary>
        /// <value>The pod c.</value>
        public IPodStore PodC { get; }

        /// <summary>
        /// Provides Pod D
        /// </summary>
        /// <value>The pod d.</value>
        public IPodStore PodD { get; }

        /// <summary>
        /// Provides the senior team allocation
        /// </summary>
        /// <value>The senior team.</value>
        public ISeniorTeamStore SeniorTeam { get; }

        /// <summary>
        /// Does nothing, since this store only exists in memory
        /// </summary>
        /// <param name="team">The team to commit.</param>
        /// <remarks>The team parameter is ignored and all teams are committed.</remarks>
        public void Commit(TeamType team)
        {
            // Do nothing because this is weak and only stores in memory
        }

        /// <summary>
        /// Return a snapshot representation of the state of this allocation
        /// </summary>
        /// <value>The allocation.</value>
        public IcuAllocation Allocation
        {
            get
            {
                return new IcuAllocation(PodA.AsPod, PodB.AsPod, PodC.AsPod, PodD.AsPod, SeniorTeam.AsSeniorTeam);
            }
        }
    }
}
