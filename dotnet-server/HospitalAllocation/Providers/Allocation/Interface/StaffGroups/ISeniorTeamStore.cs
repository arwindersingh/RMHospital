using System;
using HospitalAllocation.Data.Allocation.StaffGroups;
using HospitalAllocation.Providers.Allocation.Interface.Positions;

namespace HospitalAllocation.Providers.Allocation.Interface
{
    /// <summary>
    /// Provides an interface onto a data store for the senior team allocation
    /// </summary>
    public interface ISeniorTeamStore
    {
        /// <summary>
        /// Interface to allocate the access coordinator
        /// </summary>
        /// <value>The access coordinator.</value>
        IPositionStore AccessCoordinator { get; }

        /// <summary>
        /// Interface to allocate the tech
        /// </summary>
        /// <value>The tech.</value>
        IPositionStore Tech { get; }

        /// <summary>
        /// Interface to allocate the mern
        /// </summary>
        /// <value>The mern.</value>
        IPositionStore Mern { get; }

        /// <summary>
        /// Interface to allocate the CA support
        /// </summary>
        /// <value>The ca support.</value>
        IPositionStore CaSupport { get; }

        /// <summary>
        /// Interface to allocate the ward/on-call consultant
        /// </summary>
        /// <value>The ward on call consultant.</value>
        IPositionStore WardOnCallConsultant { get; }

        /// <summary>
        /// Interface to allocate the transport registrar
        /// </summary>
        /// <value>The transport registrar.</value>
        IPositionStore TransportRegistrar { get; }

        /// <summary>
        /// Interface to allocate the donation coordinator
        /// </summary>
        /// <value>The donation coordinator.</value>
        IPositionStore DonationCoordinator { get; }

        /// <summary>
        /// Interface to allocate CNMs
        /// </summary>
        /// <value>The cnm.</value>
        ISeniorListStore Cnm { get; }

        /// <summary>
        /// Interface to allocate CNCs
        /// </summary>
        /// <value>The cnc.</value>
        ISeniorListStore Cnc { get; }

        /// <summary>
        /// Interface to allocate resources
        /// </summary>
        /// <value>The resource.</value>
        ISeniorListStore Resource { get; }

        /// <summary>
        /// Interface to allocate internal registrars
        /// </summary>
        /// <value>The internal registrar.</value>
        ISeniorListStore InternalRegistrar { get; }

        /// <summary>
        /// Interface to allocate external registrars
        /// </summary>
        /// <value>The external registrar.</value>
        ISeniorListStore ExternalRegistrar { get; }

        /// <summary>
        /// Interface to allocate educators
        /// </summary>
        /// <value>The educator.</value>
        ISeniorListStore Educator { get; }

        /// <summary>
        /// Return a common data representation of the senior team allocation
        /// in this store
        /// </summary>
        /// <value>The senior team.</value>
        SeniorTeam AsSeniorTeam { get; }
    }
}
