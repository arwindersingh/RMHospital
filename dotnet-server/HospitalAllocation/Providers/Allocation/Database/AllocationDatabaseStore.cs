using System;
using System.Linq;
using HospitalAllocation.Providers.Allocation.Interface;
using HospitalAllocation.Model;
using HospitalAllocation.Providers.Allocation.Memory;
using Microsoft.EntityFrameworkCore;
using HospitalAllocation.Data.Allocation.StaffGroups;
using System.Collections.Generic;

namespace HospitalAllocation.Providers.Allocation.Database
{
    /// <summary>
    /// Provides access to the allocation backed by a database.
    /// </summary>
    public class AllocationDatabaseStore : IAllocationTimestampStore
    {
        /// <summary>
        /// The memory store used to store the latest allocation along with pending changes.
        /// </summary>
        private readonly AllocationMemoryStore _allocation;

        /// <summary>
        /// The options used to configure the context used to access the database.
        /// </summary>
        private readonly DbContextOptions<AllocationContext> _dbOptions;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Database.AllocationDatabaseStore"/>
        /// class using the specified options.
        /// </summary>
        /// <param name="dbOptions">The options used to access the database.</param>
        public AllocationDatabaseStore(DbContextOptions<AllocationContext> dbOptions)
        {
            _dbOptions = dbOptions;

            using (var context = new AllocationContext(dbOptions))
            {
                // Fill the memory store with the latest allocations, if any exist
                _allocation = new AllocationMemoryStore();
                if (context.TeamAllocations.Where(t => t.Type == Model.TeamType.A).Any())
                {
                    AllocationProvider.SetPodAllocation(_allocation.PodA,
                        ModelConverter.PodFromModel(GetLatestAllocation(Model.TeamType.A, context)));
                }
                if (context.TeamAllocations.Where(t => t.Type == Model.TeamType.B).Any())
                {
                    AllocationProvider.SetPodAllocation(_allocation.PodB,
                        ModelConverter.PodFromModel(GetLatestAllocation(Model.TeamType.B, context)));
                }
                if (context.TeamAllocations.Where(t => t.Type == Model.TeamType.C).Any())
                {
                    AllocationProvider.SetPodAllocation(_allocation.PodC,
                        ModelConverter.PodFromModel(GetLatestAllocation(Model.TeamType.C, context)));
                }
                if (context.TeamAllocations.Where(t => t.Type == Model.TeamType.D).Any())
                {
                    AllocationProvider.SetPodAllocation(_allocation.PodD,
                        ModelConverter.PodFromModel(GetLatestAllocation(Model.TeamType.D, context)));
                }
                if (context.TeamAllocations.Where(t => t.Type == Model.TeamType.Senior).Any())
                {
                    AllocationProvider.SetSeniorTeamAllocation(_allocation.SeniorTeam,
                        ModelConverter.SeniorTeamFromModel(GetLatestAllocation(Model.TeamType.Senior, context)));
                }
            }
        }

        /// <summary>
        /// Pod A.
        /// </summary>
        public IPodStore PodA => _allocation.PodA;

        /// <summary>
        /// Pod B.
        /// </summary>
        public IPodStore PodB => _allocation.PodB;

        /// <summary>
        /// Pod C.
        /// </summary>
        public IPodStore PodC => _allocation.PodC;

        /// <summary>
        /// Pod D.
        /// </summary>
        public IPodStore PodD => _allocation.PodD;

        /// <summary>
        /// The senior team.
        /// </summary>
        public ISeniorTeamStore SeniorTeam => _allocation.SeniorTeam;

        /// <summary>
        /// Commit changes made to the interface to the database.
        /// </summary>
        /// <param name="team">The team to commit.</param>
        public void Commit(Data.Allocation.StaffGroups.TeamType team)
        {
            using (var context = new AllocationContext(_dbOptions))
            {
                var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var modelConverter = ModelConverter.FromDb(_dbOptions);

                var oldAllocations = context.TeamAllocations
                    .Include(a => a.Positions)
                    .Where(a => a.Type == ModelConverter.TeamTypeToModel(team))
                    .Where(a => a.Time == currentTime);

                // Determine the team to insert
                Pod pod;
                switch (team)
                {
                    case Data.Allocation.StaffGroups.TeamType.A:
                        pod = PodA.AsPod;
                        break;
                    case Data.Allocation.StaffGroups.TeamType.B:
                        pod = PodB.AsPod;
                        break;
                    case Data.Allocation.StaffGroups.TeamType.C:
                        pod = PodC.AsPod;
                        break;
                    case Data.Allocation.StaffGroups.TeamType.D:
                        pod = PodD.AsPod;
                        break;
                    case Data.Allocation.StaffGroups.TeamType.Senior:
                        pod = null;
                        break;
                    default:
                        throw new ArgumentException("Unknown team type");
                }

                // Insert a pod team
                if (pod != null)
                {
                    InsertAllocation(context, currentTime, oldAllocations, modelConverter.PodToModel(pod));

                    // Check and update double for the staff
                    var multipleAllocations = new Dictionary<int, int>();
                    multipleAllocations = pod
                                            .MultipleAllocation()
                                            .GroupBy(d => d.Key)
                                            .ToDictionary(d => d.Key, d => d.First().Value);
                    foreach (int staffId in multipleAllocations.Keys)
                    {
                        StaffMember staffMember = context.StaffMembers.
                                                SingleOrDefault(staff => staff.StaffMemberId == staffId);
                        staffMember.LastDouble = currentTime;
                    }
                }
                // Insert a senior team
                else
                {
                    InsertAllocation(context, currentTime, oldAllocations,
                        modelConverter.SeniorTeamToModel(SeniorTeam.AsSeniorTeam));
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Search for the allocation at a certain time for a certain pod.
        /// </summary>
        /// <param name="time">The time to search for in seconds since UNIX epoch.</param>
        /// <param name="pod">The pod to search for.</param>
        /// <returns>The allocation at the time for the pod if it exists, null otherwise.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// Thrown when the <see cref="T:HospitalAllocation.Data.Allocation.StaffGroups.TeamType"/> isn't a pod.
        /// </exception>
        public Pod GetPastPod(long time, Data.Allocation.StaffGroups.TeamType pod)
        {
            using (var context = new AllocationContext(_dbOptions))
            {
                var allocations = context.TeamAllocations
                    .Include(t => t.Positions)
                        .ThenInclude(p => p.StaffPosition)
                    .Where(p => p.Type == ModelConverter.TeamTypeToModel(pod))
                    .Where(p => time >= p.Time);

                var allocation = allocations.FirstOrDefault(p => p.Time == allocations.Max(q => q.Time));
                if (allocation == null)
                {
                    return null;
                }
                else
                {
                    foreach (var pos in allocation.Positions.Where(p => p.StaffPosition is KnownStaffPosition))
                    {
                        context.Entry(pos.StaffPosition as KnownStaffPosition)
                            .Reference(sp => sp.StaffMember)
                            .Load();
                    }

                    return ModelConverter.PodFromModel(allocation);
                }
            }
        }

        /// <summary>
        /// Search for the allocation at a certain time for the senior team.
        /// </summary>
        /// <param name="time">The time to search for in seconds since UNIX epoch.</param>
        /// <returns>The senior team allocation at the time if it exists, null otherwise.</returns>
        public SeniorTeam GetPastSeniorTeam(long time)
        {
            using (var context = new AllocationContext(_dbOptions))
            {
                var allocations = context.TeamAllocations
                    .Include(t => t.Positions)
                        .ThenInclude(p => p.StaffPosition)
                    .Where(p => p.Type == Model.TeamType.Senior)
                    .Where(p => time >= p.Time);

                var allocation = allocations.FirstOrDefault(p => p.Time == allocations.Max(q => q.Time));
                if (allocation == null)
                {
                    return null;
                }
                else
                {
                    foreach (var pos in allocation.Positions.Where(p => p.StaffPosition is KnownStaffPosition))
                    {
                        context.Entry(pos.StaffPosition as KnownStaffPosition)
                            .Reference(sp => sp.StaffMember)
                            .Load();
                    }

                    return ModelConverter.SeniorTeamFromModel(allocation);
                }
            }
        }

        /// <summary>
        /// Gets the latest allocation for a team from the database.
        /// </summary>
        /// <param name="type">The team queried.</param>
        /// <param name="context">The database context.</param>
        /// <returns>The latest allocation for the team.</returns>
        /// <remarks>There must be an allocation for the team.</remarks>
        private TeamAllocation GetLatestAllocation(Model.TeamType team, AllocationContext context)
        {
            if (!context.TeamAllocations.Where(t => t.Type == team).Any())
            {
                throw new Exception(string.Format("No allocations for team {0}.", team));
            }

            var allocation = context.TeamAllocations
                .Include(t => t.Positions)
                    .ThenInclude(p => p.StaffPosition)
                .Single(p => p.Type == team &&
                    p.Time == context.TeamAllocations
                        .Where(q => q.Type == team)
                        .Max(q => q.Time));

            foreach (var pos in allocation.Positions.Select(p => p.StaffPosition).OfType<KnownStaffPosition>())
            {
                context.Entry(pos)
                    .Reference(sp => sp.StaffMember)
                    .Load();
            }

            return allocation;
        }

        /// <summary>
        /// Inserts an allocation into the database by updating the existing allocation if an
        /// allocation already exists at that time, and creating a new allocation otherwise.
        /// </summary>
        /// <param name="context">The context to insert into.</param>
        /// <param name="time">The time of the allocation in UNIX time seconds.</param>
        /// <param name="oldAllocations">The allocations at time.</param>
        /// <param name="allocation">The allocation to insert.</param>
        private static void InsertAllocation(AllocationContext context, long time,
            IQueryable<TeamAllocation> oldAllocations, TeamAllocation allocation)
        {
            var oldAllocation = oldAllocations.SingleOrDefault(a => a.Type == allocation.Type);
            if (oldAllocation != null)
            {
                oldAllocation.Positions = allocation.Positions;
                context.Update(oldAllocation);
            }
            else
            {
                allocation.Time = time;
                context.TeamAllocations.Add(allocation);
            }
        }
    }
}
