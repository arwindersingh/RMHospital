using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using HospitalAllocation.Data.Allocation.Positions;

namespace HospitalAllocation.Data.Allocation.StaffGroups
{
    /// <summary>
    /// The senior staff team in the ICU, not bound to a pod
    /// </summary>
    [DataContract]
    public class SeniorTeam : Team
    {
        /// <summary>
        /// Gets an empty senior team
        /// </summary>
        /// <value>The empty.</value>
        public static SeniorTeam Empty
        {
            get => new SeniorTeam(UnidentifiedPosition.Empty,
                                  UnidentifiedPosition.Empty,
                                  UnidentifiedPosition.Empty,
                                  UnidentifiedPosition.Empty,
                                  UnidentifiedPosition.Empty,
                                  UnidentifiedPosition.Empty,
                                  UnidentifiedPosition.Empty,
                                  new Position[0],
                                  new Position[0],
                                  new Position[0],
                                  new Position[0],
                                  new Position[0],
                                  new Position[0]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HospitalAllocation.Data.Allocation.StaffGroups.SeniorTeam"/> class.
        /// </summary>
        /// <param name="accessCoordinator">Access coordinator.</param>
        /// <param name="tech">Tech.</param>
        /// <param name="mern">Mern.</param>
        /// <param name="caSupport">Ca support.</param>
        /// <param name="wardOnCallConsultant">Ward on call consultant.</param>
        /// <param name="transportRegistrar">Transport registrar.</param>
        /// <param name="cnm">Cnm.</param>
        /// <param name="cnc">Cnc.</param>
        /// <param name="resource">Resource.</param>
        /// <param name="internalRegistrar">Internal registrar.</param>
        /// <param name="externalRegistrar">External registrar.</param>
        /// <param name="educator">Educator.</param>
        public SeniorTeam(Position accessCoordinator,
                          Position tech,
                          Position mern,
                          Position caSupport,
                          Position wardOnCallConsultant,
                          Position transportRegistrar,
                          Position donationCoordinator,
                          IReadOnlyList<Position> cnm,
                          IReadOnlyList<Position> cnc,
                          IReadOnlyList<Position> resource,
                          IReadOnlyList<Position> internalRegistrar,
                          IReadOnlyList<Position> externalRegistrar,
                          IReadOnlyList<Position> educator
                          )
            : base(TeamType.Senior)
        {
            AccessCoordinator = accessCoordinator;
            Tech = tech;
            Mern = mern;
            CaSupport = caSupport;
            WardOnCallConsultant = wardOnCallConsultant;
            TransportRegistrar = transportRegistrar;
            DonationCoordinator = donationCoordinator;
            Cnm = cnm;
            Cnc = cnc;
            Resource = resource;
            InternalRegistrar = internalRegistrar;
            ExternalRegistrar = externalRegistrar;
            Educator = educator;
        }

        /// <summary>
        /// Returns if all the positions are allocated and the state of senior is valid
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsStateValid
        {
            get
            {
                var valid = true;
                valid &= AccessCoordinator != null;
                valid &= Tech != null;
                valid &= Mern != null;
                valid &= CaSupport != null;
                valid &= WardOnCallConsultant != null;
                valid &= TransportRegistrar != null;
                valid &= DonationCoordinator != null;

                valid &= Cnm != null;
                valid &= Cnc != null;
                valid &= Resource != null;
                valid &= InternalRegistrar != null;
                valid &= ExternalRegistrar != null;
                valid &= Educator != null;

                if (!valid)
                {
                    return false;
                }
                valid &= Cnm.All(v => v != null);
                valid &= Cnc.All(v => v != null);
                valid &= Resource.All(v => v != null);
                valid &= InternalRegistrar.All(v => v != null);
                valid &= ExternalRegistrar.All(v => v != null);
                valid &= Educator.All(v => v != null);

                return valid;
            }
        }
        
        /// <summary>
        /// The allocated access coordinator
        /// </summary>
        /// <value>The access coordinator.</value>
        [DataMember(Name = "access_coordinator")]
        public Position AccessCoordinator { get; }

        /// <summary>
        /// The allocated tech
        /// </summary>
        /// <value>The tech.</value>
        [DataMember(Name = "tech")]
        public Position Tech { get; }

        /// <summary>
        /// The allocated mern
        /// </summary>
        /// <value>The mern.</value>
        [DataMember(Name = "mern")]
        public Position Mern { get; }

        /// <summary>
        /// The allocated CA Support
        /// </summary>
        /// <value>The ca support.</value>
        [DataMember(Name = "ca_support")]
        public Position CaSupport { get; }

        /// <summary>
        /// The allocated Ward/On-call Consultant
        /// </summary>
        /// <value>The ward on call consultant.</value>
        [DataMember(Name = "ward_consultant")]
        public Position WardOnCallConsultant { get; }

        /// <summary>
        /// The allocated transport registrar
        /// </summary>
        /// <value>The transport registrar.</value>
        [DataMember(Name = "transport")]
        public Position TransportRegistrar { get; }

        /// <summary>
        /// The allocated donation coordinator
        /// </summary>
        /// <value>The donation coordinator.</value>
        [DataMember(Name = "donation")]
        public Position DonationCoordinator { get; }

        /// <summary>
        /// The allocated CNMs
        /// </summary>
        /// <value>The cnm.</value>
        [DataMember(Name = "cnm")]
        public IReadOnlyList<Position> Cnm { get; }

        /// <summary>
        /// The allocated CNCs
        /// </summary>
        /// <value>The cnc.</value>
        [DataMember(Name = "cnc")]
        public IReadOnlyList<Position> Cnc { get; }

        /// <summary>
        /// The allocated Resources
        /// </summary>
        /// <value>The resource.</value>
        [DataMember(Name = "resource")]
        public IReadOnlyList<Position> Resource { get; }

        /// <summary>
        /// The allocated Internal Registrars
        /// </summary>
        /// <value>The internal registrar.</value>
        [DataMember(Name = "internal")]
        public IReadOnlyList<Position> InternalRegistrar { get; }

        /// <summary>
        /// The allocated External Registrars
        /// </summary>
        /// <value>The external registrar.</value>
        [DataMember(Name = "external")]
        public IReadOnlyList<Position> ExternalRegistrar { get; }

        /// <summary>
        /// The allocated Educators
        /// </summary>
        /// <value>The educator.</value>
        [DataMember(Name = "educator")]
        public IReadOnlyList<Position> Educator { get; }
    }
}
