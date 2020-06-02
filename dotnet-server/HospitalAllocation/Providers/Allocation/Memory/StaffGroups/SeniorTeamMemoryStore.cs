using System;
using System.Collections.Generic;
using System.Linq;
using HospitalAllocation.Data.Allocation.StaffGroups;
using HospitalAllocation.Data.Allocation.Positions;
using HospitalAllocation.Providers.Allocation.Interface;
using HospitalAllocation.Providers.Allocation.Interface.Positions;
using HospitalAllocation.Providers.Allocation.Memory.Positions;

namespace HospitalAllocation.Providers.Allocation.Memory.StaffGroups
{
    /// <summary>
    /// A memory store of the senior team allocation
    /// </summary>
    public class SeniorTeamMemoryStore : ISeniorTeamStore
    {
        // The name of this team
        private const string TEAM_NAME = "senior";

        // Internal store of the access coordinator allocation
        private readonly PositionMemoryStore _accessCoordinator;

        // Internal store of the tech allocation
        private readonly PositionMemoryStore _tech;

        // Internal store of the mern allocation
        private readonly PositionMemoryStore _mern;

        // Internal store of the CA support allocation
        private readonly PositionMemoryStore _caSupport;

        // Internal store of the ward/on-call consultant allocation
        private readonly PositionMemoryStore _wardOnCallConsultant;

        // Internal store of the transport registrar
        private readonly PositionMemoryStore _transportRegistrar;

        // Internal store of the donation coordinator allocation
        private readonly PositionMemoryStore _donationCoordinator;

        // Internal store of the CNM allocation
        private readonly SeniorListMemoryStore _cnm;

        // Internal store of the CNC allocation
        private readonly SeniorListMemoryStore _cnc;

        // Internal store of the resource allocation
        private readonly SeniorListMemoryStore _resource;

        // Internal store of the internal registrar allocation
        private readonly SeniorListMemoryStore _internalRegistrar;

        // Internal store of the external registrar allocation
        private readonly SeniorListMemoryStore _externalRegistrar;

        // Internal store of the educator allocation
        private readonly SeniorListMemoryStore _educator;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Memory.StaffGroups.SeniorTeamMemoryStore"/> class.
        /// </summary>
        public SeniorTeamMemoryStore()
        {
            _accessCoordinator = new PositionMemoryStore("access_coordinator");
            _tech = new PositionMemoryStore("tech");
            _mern = new PositionMemoryStore("mern");
            _caSupport = new PositionMemoryStore("ca_support");
            _wardOnCallConsultant = new PositionMemoryStore("ward_consultant");
            _transportRegistrar = new PositionMemoryStore("transport");
            _donationCoordinator = new PositionMemoryStore("donation");
            _cnm = new SeniorListMemoryStore();
            _cnc = new SeniorListMemoryStore();
            _resource = new SeniorListMemoryStore();
            _internalRegistrar = new SeniorListMemoryStore();
            _externalRegistrar = new SeniorListMemoryStore();
            _educator = new SeniorListMemoryStore();
        }

        /// <summary>
        /// Provides a snapshot of the current state of the senior team allocation
        /// </summary>
        /// <value>The senior team.</value>
        public SeniorTeam AsSeniorTeam => new SeniorTeam(_accessCoordinator.Position,
                                                         _tech.Position,
                                                         _mern.Position,
                                                         _caSupport.Position,
                                                         _wardOnCallConsultant.Position,
                                                         _transportRegistrar.Position,
                                                         _donationCoordinator.Position,
                                                         _cnm.Array,
                                                         _cnc.Array,
                                                         _resource.Array,
                                                         _internalRegistrar.Array,
                                                         _externalRegistrar.Array,
                                                         _educator.Array
                                                        );

        /// <summary>
        /// Get the interface to the access coordinator allocation
        /// </summary>
        /// <value>The access coordinator.</value>
        public IPositionStore AccessCoordinator => _accessCoordinator;

        /// <summary>
        /// Get the interface to the tech allocation
        /// </summary>
        /// <value>The tech.</value>
        public IPositionStore Tech => _tech;

        /// <summary>
        /// Get the interface to the mern allocation
        /// </summary>
        /// <value>The mern.</value>
        public IPositionStore Mern => _mern;

        /// <summary>
        /// Get the interface to the CA support allocation
        /// </summary>
        /// <value>The ca support.</value>
        public IPositionStore CaSupport => _caSupport;

        /// <summary>
        /// Get the interface to the ward/on-call consultant allocation
        /// </summary>
        /// <value>The ward on call consultant.</value>
        public IPositionStore WardOnCallConsultant => _wardOnCallConsultant;

        /// <summary>
        /// Get the interface to the transport registrar allocation
        /// </summary>
        /// <value>The transport registrar.</value>
        public IPositionStore TransportRegistrar => _transportRegistrar;

        /// <summary>
        /// Get the interface to the donation coordinator allocation
        /// </summary>
        /// <value>The donation coordinator.</value>
        public IPositionStore DonationCoordinator => _donationCoordinator;

        /// <summary>
        /// Get the interface to the CNM allocation
        /// </summary>
        /// <value>The cnm.</value>
        public ISeniorListStore Cnm => _cnm;

        /// <summary>
        /// Get the interface to the CNC allocation
        /// </summary>
        /// <value>The cnc.</value>
        public ISeniorListStore Cnc => _cnc;

        /// <summary>
        /// Get the interface to the resource allocation
        /// </summary>
        /// <value>The resource.</value>
        public ISeniorListStore Resource => _resource;

        /// <summary>
        /// Get the interface to the internal registrar allocation
        /// </summary>
        /// <value>The internal registrar.</value>
        public ISeniorListStore InternalRegistrar => _internalRegistrar;

        /// <summary>
        /// Get the interface to the external registrar allocation
        /// </summary>
        /// <value>The external registrar.</value>
        public ISeniorListStore ExternalRegistrar => _externalRegistrar;

        /// <summary>
        /// Get the interface to the educator allocation
        /// </summary>
        /// <value>The educator.</value>
        public ISeniorListStore Educator => _educator;
    }
}
