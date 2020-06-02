using System;
using System.Runtime.Serialization;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Data.Allocation
{
    /// <summary>
    /// Denotes a full allocation of staff to positions in the ICU
    /// </summary>
    [DataContract]
    public class IcuAllocation
    {
        /// <summary>
        /// Get an empty ICU allocation object
        /// </summary>
        /// <value>The empty.</value>
        public static IcuAllocation Empty
        {
            get => new IcuAllocation(Pod.CreateEmpty(TeamType.A),
                                     Pod.CreateEmpty(TeamType.B),
                                     Pod.CreateEmpty(TeamType.C),
                                     Pod.CreateEmpty(TeamType.D),
                                     SeniorTeam.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HospitalAllocation.Data.Allocation.IcuAllocation"/> class.
        /// </summary>
        /// <param name="podA">Pod A.</param>
        /// <param name="podB">Pod B.</param>
        /// <param name="podC">Pod C.</param>
        /// <param name="podD">Pod D.</param>
        /// <param name="seniorTeam">Senior team.</param>
        public IcuAllocation(Pod podA, Pod podB, Pod podC, Pod podD, SeniorTeam seniorTeam)
        {
            PodA = podA;
            PodB = podB;
            PodC = podC;
            PodD = podD;
            SeniorTeam = seniorTeam;
        }

        /// <summary>
        /// ICU Pod A
        /// </summary>
        /// <value>The pod a.</value>
        [DataMember(Name = "a")]
        public Pod PodA { get; }

        /// <summary>
        /// ICU Pod B
        /// </summary>
        /// <value>The pod b.</value>
        [DataMember(Name = "b")]
        public Pod PodB { get; }

        /// <summary>
        /// ICU Pod C
        /// </summary>
        /// <value>The pod c.</value>
        [DataMember(Name = "c")]
        public Pod PodC { get; }

        /// <summary>
        /// ICU Pod D
        /// </summary>
        /// <value>The pod d.</value>
        [DataMember(Name = "d")]
        public Pod PodD { get; }

        /// <summary>
        /// The senior ICU team
        /// </summary>
        /// <value>The senior team.</value>
        [DataMember(Name = "senior")]
        public SeniorTeam SeniorTeam { get; }
    }
}
