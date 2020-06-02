using System;
using System.Collections.Generic;
using HospitalAllocation.Data.Allocation;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Providers.Allocation.Interface
{
    /// <summary>
    /// Provides an interface to the allocation to be manipulated through the API
    /// </summary>
    public interface IAllocationStore
    {
        /// <summary>
        /// The interface to Pod A
        /// </summary>
        /// <value>The pod a.</value>
        IPodStore PodA { get; }

        /// <summary>
        /// The interface to Pod B
        /// </summary>
        /// <value>The pod b.</value>
        IPodStore PodB { get; }

        /// <summary>
        /// Interfaces Pod C
        /// </summary>
        /// <value>The pod c.</value>
        IPodStore PodC { get; }

        /// <summary>
        /// Interfaces Pod D
        /// </summary>
        /// <value>The pod d.</value>
        IPodStore PodD { get; }

        /// <summary>
        /// Provides the interface to the senior team
        /// </summary>
        /// <value>The senior team.</value>
        ISeniorTeamStore SeniorTeam { get; }

        /// <summary>
        /// Commit changes made to the interface to its underlying store
        /// </summary>
        /// <param name="teamType">The team to commit.</param>
        void Commit(TeamType team);
    }
}
