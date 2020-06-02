using System;
using System.Collections.Generic;
using HospitalAllocation.Data.Allocation.Positions;
using HospitalAllocation.Data.Allocation.StaffGroups;
using HospitalAllocation.Providers.Allocation.Interface;
using HospitalAllocation.Providers.Allocation.Interface.Positions;
using HospitalAllocation.Providers.Allocation.Memory.Positions;

namespace HospitalAllocation.Providers.Allocation.Memory.StaffGroups
{
    /// <summary>
    /// Stores an in-memory representation of a pod allocation
    /// </summary>
    public abstract class PodMemoryStore : IPodStore
    {
        // Internal store of bed allocations
        private readonly BedSetMemoryStore _beds;

        // Internal store of the consultant allocation
        private readonly PositionMemoryStore _consultant;

        // Internal store of the team leader allocation
        private readonly PositionMemoryStore _teamLeader;

        // Internal store of the registrar allocation
        private readonly PositionMemoryStore _registrar;

        // Internal store of the resident allocation
        private readonly PositionMemoryStore _resident;

        // Internal store of the pod CA allocation
        private readonly PositionMemoryStore _podCa;

        // Internal store of the CA cleaner allocation
        private readonly PositionMemoryStore _caCleaner;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Memory.StaffGroups.PodMemoryStore"/> class.
        /// </summary>
        /// <param name="podName">Pod name.</param>
        /// <param name="bedCapacity">Bed capacity.</param>
        protected PodMemoryStore(TeamType teamType, int bedCapacity)
        {
            TeamType = teamType;
            _beds = new BedSetMemoryStore(bedCapacity);
            _consultant = new PositionMemoryStore("consultant");
            _teamLeader = new PositionMemoryStore("team_leader");
            _registrar = new PositionMemoryStore("registrar");
            _resident = new PositionMemoryStore("resident");
            _podCa = new PositionMemoryStore("pod_ca");
            _caCleaner = new PositionMemoryStore("ca_cleaner");
        }

        /// <summary>
        /// Gets a snapshot of this pod's allocation
        /// </summary>
        /// <value>The pod.</value>
        public Pod AsPod => new Pod(TeamType,
                                    _beds.Beds,
                                    _consultant.Position,
                                    _teamLeader.Position,
                                    _registrar.Position,
                                    _resident.Position,
                                    _podCa.Position,
                                    _caCleaner.Position);

        /// <summary>
        /// Gets the name of this pod
        /// </summary>
        /// <value>The name.</value>
        public TeamType TeamType { get; }

        /// <summary>
        /// Get the interface to the bed store
        /// </summary>
        /// <value>The bed set.</value>
        public IBedSetStore BedSet => _beds;

        /// <summary>
        /// Get the interface to the consultant store
        /// </summary>
        /// <value>The consultant.</value>
        public IPositionStore Consultant => _consultant;

        /// <summary>
        /// Get the interface to the team leader store
        /// </summary>
        /// <value>The team leader.</value>
        public IPositionStore TeamLeader => _teamLeader;

        /// <summary>
        /// Get the interface to the registrar store
        /// </summary>
        /// <value>The registrar.</value>
        public IPositionStore Registrar => _registrar;

        /// <summary>
        /// Get the interface to the resident store
        /// </summary>
        /// <value>The resident.</value>
        public IPositionStore Resident => _resident;

        /// <summary>
        /// Get the interface to the pod CA store
        /// </summary>
        /// <value>The pod ca.</value>
        public IPositionStore PodCa => _podCa;

        /// <summary>
        /// Get the interface to the CA cleaner store
        /// </summary>
        /// <value>The ca cleaner.</value>
        public IPositionStore CaCleaner => _caCleaner;
    }
}
