using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using HospitalAllocation.Data.Allocation.Positions;

namespace HospitalAllocation.Data.Allocation.StaffGroups
{
    /// <summary>
    /// An ICU pod, with Nurses allocated to Beds and CentralStaff allocated
    /// to CentralPositions
    /// </summary>
    [DataContract]
    public class Pod : Team
    {
        /// <summary>
        /// Creates a new empty pod of the given type
        /// </summary>
        /// <returns>The empty.</returns>
        /// <param name="type">Type.</param>
        public static Pod CreateEmpty(TeamType type)
        {
            var beds = new Dictionary<int, Position>();
            int bedNum = 0;
            switch (type)
            {
                case TeamType.A:
                case TeamType.D:
                    bedNum = 12;
                    break;

                case TeamType.B:
                case TeamType.C:
                    bedNum = 10;
                    break;

                default:
                    throw new InvalidOperationException("Cannot create non-pod team");
            }

            for (int i = 1; i <= bedNum; i++)
            {
                beds.Add(i, UnidentifiedPosition.Empty);
            }

            return new Pod(type, beds,
                           UnidentifiedPosition.Empty,
                           UnidentifiedPosition.Empty,
                           UnidentifiedPosition.Empty,
                           UnidentifiedPosition.Empty,
                           UnidentifiedPosition.Empty,
                           UnidentifiedPosition.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HospitalAllocation.Data.Allocation.StaffGroups.Pod"/> class.
        /// Sets the beds to an empty dictionary
        /// </summary>
        public Pod(TeamType teamType,
                   IDictionary<int, Position> beds,
                   Position consultant,
                   Position teamLeader,
                   Position registrar,
                   Position resident,
                   Position podCa,
                   Position caCleaner
                   )
            : base(teamType)
        {
            Beds = beds != null ? new Dictionary<int, Position>(beds) : new Dictionary<int, Position>();
            Consultant = consultant;
            TeamLeader = teamLeader;
            Registrar = registrar;
            Resident = resident;
            PodCa = podCa;
            CaCleaner = caCleaner;
        }

        /// <summary>
        /// Returns if all the positions are allocated and the state of pod is valid
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsStateValid
        {
            get
            {
                var valid = true;
                valid &= Consultant != null;
                valid &= TeamLeader != null;
                valid &= Registrar != null;
                valid &= Resident != null;
                valid &= PodCa != null;
                valid &= CaCleaner != null;
                valid &= Beds != null;

                if (!valid)
                {
                    return false;
                }

                valid &= Beds.Values.All(v => v != null);
                return valid;
            }
        }

        /// <summary>
        /// Returns a dictionary which includes <staff_id, count> of any multiple
        /// allocations. 
        /// </summary>
        /// <returns>Dictionary of staff who are allocated to multiple beds</returns>
        public IDictionary<int, int> MultipleAllocation()
        {
            var allocation = new Dictionary<int, int>();
            foreach (var bed in Beds.Values)
            {
                if (bed is IdentifiedPosition)
                {
                    var staff_id = (bed as IdentifiedPosition).StaffId;
                    if (allocation.ContainsKey(staff_id))
                    {
                        allocation[staff_id]++;
                    }
                    else
                    {
                        allocation[staff_id] = 1;
                    }
                }
            }
            return allocation.Where(kvp => kvp.Value > 1)
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        /// <summary>
        /// The bed allocations in this pod
        /// </summary>
        /// <value>The beds.</value>
        [DataMember(Name = "beds")]
        public IDictionary<int, Position> Beds { get; }

        /// <summary>
        /// The consultant allocation
        /// </summary>
        /// <value>The consultant.</value>
        [DataMember(Name = "consultant")]
        public Position Consultant { get; }

        /// <summary>
        /// The allocated team leader
        /// </summary>
        /// <value>The team leader.</value>
        [DataMember(Name = "team_leader")]
        public Position TeamLeader { get; }

        /// <summary>
        /// The allocated registrar
        /// </summary>
        /// <value>The registrar.</value>
        [DataMember(Name = "registrar")]
        public Position Registrar { get; }

        /// <summary>
        /// The allocated resident
        /// </summary>
        /// <value>The resident.</value>
        [DataMember(Name = "resident")]
        public Position Resident { get; }

        /// <summary>
        /// The allocated Pod CA
        /// </summary>
        /// <value>The pod ca.</value>
        [DataMember(Name = "pod_ca")]
        public Position PodCa { get; }

        /// <summary>
        /// The allocated CA Cleaner
        /// </summary>
        /// <value>The ca cleaner.</value>
        [DataMember(Name = "ca_cleaner")]
        public Position CaCleaner { get; }
    }
}
