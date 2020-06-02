using System;
using System.Collections.Generic;
using HospitalAllocation.Data.Allocation.StaffGroups;
using HospitalAllocation.Providers.Allocation.Interface.Positions;

namespace HospitalAllocation.Providers.Allocation.Interface
{
    /// <summary>
    /// Provides an interface to data store for a pod
    /// </summary>
    public interface IPodStore
    {
        /// <summary>
        /// Interface to the beds in the pod
        /// </summary>
        /// <value>The bed set.</value>
        IBedSetStore BedSet { get; }

        /// <summary>
        /// Interface to allocate the consultant
        /// </summary>
        /// <value>The consultant.</value>
        IPositionStore Consultant { get; }

        /// <summary>
        /// Interface to allocate the team leader
        /// </summary>
        /// <value>The team leader.</value>
        IPositionStore TeamLeader { get; }

        /// <summary>
        /// Interface to allocate the registrar
        /// </summary>
        /// <value>The registrar.</value>
        IPositionStore Registrar { get; }

        /// <summary>
        /// Interface to allocate the resident
        /// </summary>
        /// <value>The resident.</value>
        IPositionStore Resident { get; }

        /// <summary>
        /// Interface to allocate the pod CA
        /// </summary>
        /// <value>The pod ca.</value>
        IPositionStore PodCa { get; }

        /// <summary>
        /// Interface to allocate the CA cleaner
        /// </summary>
        /// <value>The ca cleaner.</value>
        IPositionStore CaCleaner { get; }

        /// <summary>
        /// Get a representation of the underlying pod in the common pod format
        /// </summary>
        /// <value>The pod.</value>
        Pod AsPod { get; }
    }
}
