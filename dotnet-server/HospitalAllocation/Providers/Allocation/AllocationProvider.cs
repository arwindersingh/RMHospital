using System;
using System.Collections.Generic;
using System.Linq;
using HospitalAllocation.Data.Allocation.Positions;
using HospitalAllocation.Data.Allocation.StaffGroups;
using HospitalAllocation.Providers.Allocation.Interface.Positions;
using HospitalAllocation.Providers.Allocation.Interface;

namespace HospitalAllocation.Providers.Allocation
{
    /// <summary>
    /// Manages the interface between incoming requests to get and set values and the
    /// backing stores themselves
    /// </summary>
    public class AllocationProvider
    {
        // The backing store of the allocation
        private readonly IAllocationStore _allocationStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HospitalAllocation.Providers.Allocation.AllocationProvider"/> class.
        /// </summary>
        /// <param name="allocationStore">The storage object to manage allocation storage</param>
        public AllocationProvider(IAllocationStore allocationStore)
        {
            _allocationStore = allocationStore;
        }

        /// <summary>
        /// Get a value representing the current allocation of Pod A
        /// </summary>
        /// <value>The pod a.</value>
        public Pod PodA { get => _allocationStore.PodA.AsPod; }

        /// <summary>
        /// Set the new allocation of pod A from the non-null fields of the given value
        /// </summary>
        /// <param name="podAllocation">Pod allocation.</param>
        public void SetPodAAllocation(Pod podAllocation)
        {
            SetPodAllocation(_allocationStore.PodA, podAllocation);
        }

        /// <summary>
        /// Get a value representing the current allocation of Pod B
        /// </summary>
        /// <value>The pod b.</value>
        public Pod PodB { get => _allocationStore.PodB.AsPod; }

        /// <summary>
        /// Set the stored allocation of Pod B based on the non-null fields of the given allocation object
        /// </summary>
        /// <param name="podAllocation">Pod allocation.</param>
        public void SetPodBAllocation(Pod podAllocation)
        {
            SetPodAllocation(_allocationStore.PodB, podAllocation);
        }

        /// <summary>
        /// Get a value representing the current allocation of Pod C
        /// </summary>
        /// <value>The pod c.</value>
        public Pod PodC { get => _allocationStore.PodC.AsPod; }

        /// <summary>
        /// Set the stored allocation of Pod C from the non-null fields of the given pod value
        /// </summary>
        /// <param name="podAllocation">Pod allocation.</param>
        public void SetPodCAllocation(Pod podAllocation)
        {
            SetPodAllocation(_allocationStore.PodC, podAllocation);
        }

        /// <summary>
        /// Get a value representing the current allocation of Pod D
        /// </summary>
        /// <value>The pod d.</value>
        public Pod PodD { get => _allocationStore.PodD.AsPod; }

        /// <summary>
        /// Set the allocation stored for Pod D from the non-null fields of the given pod value
        /// </summary>
        /// <param name="podAllocation">Pod allocation.</param>
        public void SetPodDAllocation(Pod podAllocation)
        {
            SetPodAllocation(_allocationStore.PodD, podAllocation);
        }

        /// <summary>
        /// Get a value representing the current allocation of the senior team
        /// </summary>
        /// <value>The senior team.</value>
        public SeniorTeam SeniorTeam { get => _allocationStore.SeniorTeam.AsSeniorTeam; }

        /// <summary>
        /// Set the allocation stored for the senior team from the non-null
        /// fields of the given senior team object
        /// </summary>
        /// <param name="seniorAllocation">Senior allocation.</param>
        public void SetSeniorTeam(SeniorTeam seniorAllocation)
        {
            SetSeniorTeamAllocation(_allocationStore.SeniorTeam, seniorAllocation);
        }

        /// <summary>
        /// Commit the set allocation in the underlying allocation store
        /// </summary>
        /// <param name="teamType">The team to commit.</param>
        public void Commit(TeamType team)
        {
            _allocationStore.Commit(team);
        }

        /// <summary>
        /// Set the allocation of a senior team store from the non-null fields of
        /// a given senior team value
        /// </summary>
        /// <param name="seniorStore">Senior store.</param>
        /// <param name="seniorAllocation">Senior allocation.</param>
        public static void SetSeniorTeamAllocation(ISeniorTeamStore seniorStore, SeniorTeam seniorAllocation)
        {
            SetPosition(seniorStore.AccessCoordinator, seniorAllocation.AccessCoordinator);

            SetPosition(seniorStore.CaSupport, seniorAllocation.CaSupport);

            SetPosition(seniorStore.Mern, seniorAllocation.Mern);

            SetPosition(seniorStore.Tech, seniorAllocation.Tech);

            SetPosition(seniorStore.TransportRegistrar, seniorAllocation.TransportRegistrar);

            SetPosition(seniorStore.WardOnCallConsultant, seniorAllocation.WardOnCallConsultant);

            SetPosition(seniorStore.DonationCoordinator, seniorAllocation.DonationCoordinator);

            SetSeniorList(seniorStore.Cnc, seniorAllocation.Cnc);

            SetSeniorList(seniorStore.Cnm, seniorAllocation.Cnm);

            SetSeniorList(seniorStore.Educator, seniorAllocation.Educator);

            SetSeniorList(seniorStore.ExternalRegistrar, seniorAllocation.ExternalRegistrar);

            SetSeniorList(seniorStore.InternalRegistrar, seniorAllocation.InternalRegistrar);

            SetSeniorList(seniorStore.Resource, seniorAllocation.Resource);
        }

        /// <summary>
        /// Set the given pod allocation store from the non-null fields of a given
        /// pod allocation object
        /// </summary>
        /// <param name="pod">Pod.</param>
        /// <param name="podAllocation">Pod allocation.</param>
        public static void SetPodAllocation(IPodStore pod, Pod podAllocation)
        {
            if (podAllocation.Beds != null && podAllocation.Beds.Count > 0)
            {
                foreach (KeyValuePair<int, Position> bed in podAllocation.Beds)
                {
                    SetPosition(pod.BedSet[bed.Key], bed.Value);
                }
            }

            SetPosition(pod.CaCleaner, podAllocation.CaCleaner);

            SetPosition(pod.Consultant, podAllocation.Consultant);

            SetPosition(pod.TeamLeader, podAllocation.TeamLeader);

            SetPosition(pod.PodCa, podAllocation.PodCa);

            SetPosition(pod.Registrar, podAllocation.Registrar);

            SetPosition(pod.Resident, podAllocation.Resident);
        }

        /// <summary>
        /// Set a given position with the given position allocation value
        /// </summary>
        /// <param name="positionStore">The position store to set the value of</param>
        /// <param name="newPositionAllocation">The position allocation value to set the store with</param>
        private static void SetPosition(IPositionStore positionStore, Position newPositionAllocation)
        {
            if (!(newPositionAllocation?.IsEmpty() ?? true))
            {
                positionStore.Position = newPositionAllocation;
                positionStore.ShiftType = newPositionAllocation.ShiftType;
            }
        }

        /// <summary>
        /// Set the given senior list store with the given list of senior position values  
        /// </summary>
        /// <param name="listStore">List store.</param>
        /// <param name="listAllocation">List allocation.</param>
        private static void SetSeniorList(ISeniorListStore listStore, IReadOnlyList<Position> listAllocation)
        {
            if (listAllocation != null)
            {
                // We want to go through the list and
                //   1. Eliminate null entries
                //   2. Turn whitespace entries into the empty string
                var reducedEntries = listAllocation
                    .Where(sp => !(sp?.IsEmpty() ?? true))
                    .ToArray();

                // Now we sort the array by name, but putting empty names to the back
                Array.Sort(reducedEntries, (x, y) =>
                {
                    if (x.IsEmpty())
                    {
                        return -1;
                    }

                    if (y.IsEmpty())
                    {
                        return 1;
                    }

                    return 0;
                });

                listStore.Array = reducedEntries;
            }
        }

        /// <summary>
        /// Search for the allocation at a certain time for a certain pod.
        /// </summary>
        /// <param name="time">The time to search for in seconds since UNIX epoch.</param>
        /// <param name="pod">The pod to search for.</param>
        /// <returns>The allocation at the time for the pod if it exists, null otherwise.</returns>
        public Pod GetPastPod(long time, TeamType pod)
        {
            IAllocationTimestampStore store = _allocationStore as IAllocationTimestampStore;
            return store?.GetPastPod(time, pod);
        }

        /// <summary>
        /// Search for the allocation at a certain time for the senior team.
        /// </summary>
        /// <param name="time">The time to search for in seconds since UNIX epoch.</param>
        /// <returns>The senior team allocation at the time if it exists, null otherwise.</returns>
        public SeniorTeam GetPastSeniorTeam(long time)
        {
            IAllocationTimestampStore store = _allocationStore as IAllocationTimestampStore;
            return store?.GetPastSeniorTeam(time);
        }
    }
}
