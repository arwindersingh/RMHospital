using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using HospitalAllocation.Model;
using HospitalAllocation.Providers.Staff.Interface;

using DataObject = HospitalAllocation.Data.StaffMember;

namespace HospitalAllocation.Providers.Staff.Database
{
    /// <summary>
    /// Provides staff data storage backed by a relational database
    /// </summary>
    public class DbStaffProvider : IStaffProvider
    {
        /// <summary>
        /// The time interval we consider for calculating doubles
        /// </summary>
        private const long DoubleIntervalSeconds = 43200;

        // The database configuration options that we can construct a DB context (connection) from
        private readonly DbContextOptions<AllocationContext> _dbOptions;

        /// <summary>
        /// Construct a new staff provider around a database, as configured by a DbContextOptions object
        /// </summary>
        /// <param name="dbOptions"></param>
        public DbStaffProvider(DbContextOptions<AllocationContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        /// <summary>
        /// Create a new staff member from a given staff member object and return their new allocted database ID
        /// </summary>
        /// <param name="staffMember">the staff member data to save in the database</param>
        /// <returns>the integer identifier of the new staff entry</returns>
        public int Create(DataObject.StaffMember staffMember)
        {
            using (var dbContext = new AllocationContext(_dbOptions))
            {
                // Find if this staff member's designation is already known
                Model.Designation designation = dbContext.Designations
                    .SingleOrDefault(d => String.Equals(d.Name, staffMember.Designation, StringComparison.OrdinalIgnoreCase));

                if (designation == null)
                {
                    throw new ArgumentException(String.Format("Unrecognised designation: {0}", staffMember.Designation));
                }

                // Now dig out the skills we already know about. We could ask for only the skills that this 
                // staff member has, but then we need to check those skills against the staff member's with
                // case insensitive comparison on the DB's side. It's probably both faster and simpler to read 
                // in all the skills and then filter on our side
                IDictionary<string, Model.Skill> dbSkills = new Dictionary<string, Model.Skill>(dbContext.Skills.ToDictionary(s => s.Name, s => s),
                    StringComparer.OrdinalIgnoreCase);

                // Create the new staff member entry
                StaffMember dbStaffMember = new StaffMember()
                {
                    FirstName = staffMember.FirstName,
                    LastName = staffMember.LastName,
                    Alias = staffMember.Alias,
                    Designation = designation,
                    LastDouble = staffMember.LastDouble,
                    PhotoId = staffMember.PhotoId,
                    RosterOnId = staffMember.RosterOnId,
                };

                // Now generate their skill entries
                var skills = new List<StaffSkill>();
                foreach (string skillName in staffMember.Skills)
                {
                    if (!dbSkills.ContainsKey(skillName))
                    {
                        throw new ArgumentException(String.Format("Unrecognised skill: {0}", skillName));
                    }

                    skills.Add(new StaffSkill() { StaffMember = dbStaffMember, Skill = dbSkills[skillName] });
                }
                dbStaffMember.StaffSkills = skills;

                // Finally, stick them in the database
                dbContext.StaffMembers.Add(dbStaffMember);
                dbContext.SaveChanges();

                // EF Core should magically populate this property after the commit
                return dbStaffMember.StaffMemberId;
            }
        }

        /// <summary>
        /// Retrieve an existing staff member from the database by their database ID
        /// </summary>
        /// <param name="staffId">The database ID of the staff member.</param>
        /// <returns>A data object representation of the retrieved staff member.</returns>
        public DataObject.StaffMember Get(int staffId)
        {
            using (var dbContext = new AllocationContext(_dbOptions))
            {
                // Retrieve the staff member by ID, including all fields
                StaffMember staffMember = dbContext.StaffMembers
                    .Include(sm => sm.Designation)
                    .Include(sm => sm.StaffSkills)
                        .ThenInclude(ss => ss.Skill)
                    .SingleOrDefault(sm => sm.StaffMemberId == staffId);

                // If we got nothing, bail out
                if (staffMember == null)
                {
                    return null;
                }

                // Turn the skills back into skill names
                ICollection<string> skills = staffMember.StaffSkills
                    .Select(ss => ss.Skill.Name)
                    .ToList();

                // Return a fresh data passing object
                return new DataObject.StaffMember(staffMember.FirstName,
                    staffMember.LastName,
                    staffMember.Alias,
                    staffMember.Designation.Name,
                    skills,
                    staffMember.LastDouble,
                    null,
                    staffMember.PhotoId,
                    staffMember.RosterOnId);
            }
        }

        /// <summary>
        /// Retrieve an existing staff member from the database by their database ID
        /// </summary>
        /// <param name="staffId">The database ID of the staff member.</param>
        /// <param name="recentDoubleTime">The earliest double time considered recent.</param>
        /// <returns>A data object representation of the retrieved staff member.</returns>
        public DataObject.StaffMember Get(int staffId, long recentDoubleTime)
        {
            var member = Get(staffId);

            if (member == null)
            {
                return null;
            }

            using (var dbContext = new AllocationContext(_dbOptions))
            {
                var recentDoubles = GenerateRecentDoubles(dbContext, staffId, recentDoubleTime);

                return new DataObject.StaffMember(member.FirstName,
                    member.LastName,
                    member.Alias,
                    member.Designation,
                    member.Skills.ToList(),
                    member.LastDouble,
                    recentDoubles,
                    member.PhotoId,
                    member.RosterOnId);
            }
        }

        /// <summary>
        /// Update an existing staff member in the database with the new values assigned
        /// </summary>
        /// <param name="staffId">the database ID of the staff member to update</param>
        /// <param name="updatedStaffMember">the new values to assign to the given staff member</param>
        /// <returns></returns>
        public DataObject.StaffMember Update(int staffId, DataObject.StaffMember updatedStaffMember)
        {
            using (var dbContext = new AllocationContext(_dbOptions))
            {
                StaffMember staffMember = dbContext.StaffMembers
                    .Include(sm => sm.Designation)
                    .Include(sm => sm.StaffSkills)
                        .ThenInclude(ss => ss.Skill)
                    .SingleOrDefault(sm => sm.StaffMemberId == staffId);

                // Save a lot of work if we don't recognise the ID
                if (staffMember == null)
                {
                    throw new ArgumentException(String.Format("Unrecognised staff ID: {0}", staffId));
                }

                staffMember.FirstName = updatedStaffMember.FirstName;
                staffMember.LastName = updatedStaffMember.LastName;
                staffMember.Alias = updatedStaffMember.Alias;
                staffMember.LastDouble = updatedStaffMember.LastDouble;
                staffMember.RosterOnId = updatedStaffMember.RosterOnId;
                staffMember.PhotoId = updatedStaffMember.PhotoId;

                // If we have a new designation, we need to ask the database
                if (updatedStaffMember.Designation != staffMember.Designation.Name)
                {
                    Model.Designation designation = dbContext.Designations
                        .SingleOrDefault(d => String.Equals(d.Name, updatedStaffMember.Designation, StringComparison.OrdinalIgnoreCase));
                    staffMember.Designation = designation ?? throw new ArgumentException(String.Format("Unrecognised designation: {0}", updatedStaffMember.Designation));
                }

                // Work out what skills have been changed
                var currentSkills = new HashSet<string>(staffMember.StaffSkills.Select(ss => ss.Skill.Name));
                var updatedSkills = new HashSet<string>(updatedStaffMember.Skills);

                // Skills that this staff member has that they didn't before
                IEnumerable<string> addedSkills = updatedSkills.Except(currentSkills, StringComparer.OrdinalIgnoreCase);
                // Skills this staff member used to have but now doesn't
                IEnumerable<string> deletedSkills = currentSkills.Except(updatedSkills, StringComparer.OrdinalIgnoreCase);

                // Remove all the deleted skills from the staff member
                staffMember.StaffSkills
                    .RemoveAll(s => deletedSkills.Contains(s.Skill.Name, StringComparer.OrdinalIgnoreCase));

                // Add the new skills
                if (addedSkills.Any())
                {
                    // Query the database for all the skills (figure this is faster than a query per skill)
                    IDictionary<string, Model.Skill> skills = new Dictionary<string, Model.Skill>(dbContext.Skills.ToDictionary(s => s.Name, s => s),
                        StringComparer.OrdinalIgnoreCase);

                    // If there are any skills we're trying to add by name that aren't in the database,
                    // we have a problem and cannot update
                    IEnumerable<string> unknownSkills = addedSkills.Except(skills.Keys, StringComparer.OrdinalIgnoreCase);
                    if (unknownSkills.Any())
                    {
                        throw new ArgumentException(String.Format("Unrecognised skills: {0}", String.Join(",", unknownSkills)));
                    }

                    // Finally add all the new skills in with new relations
                    staffMember.StaffSkills.AddRange(addedSkills.Select(s => new StaffSkill()
                    {
                        Skill = skills[s],
                        StaffMember = staffMember
                    }));
                }

                // If we've gotten this far, we commit and are done
                dbContext.SaveChanges();

                return new DataObject.StaffMember(staffMember.FirstName,
                    staffMember.LastName,
                    staffMember.Alias,
                    staffMember.Designation.Name,
                    staffMember.StaffSkills.Select(ss => ss.Skill.Name).ToList(),
                    staffMember.LastDouble,
                    null,
                    staffMember.PhotoId,
                    staffMember.RosterOnId);
            }
        }

        /// <summary>
        /// Delete a staff member from the database by their ID
        /// </summary>
        /// <param name="staffId">the database ID of the staff member entry</param>
        /// <returns>true if the staff member was found and deleted, false otherwise</returns>
        public bool Delete(int staffId)
        {
            using (var dbContext = new AllocationContext(_dbOptions))
            {
                StaffMember staffMember = dbContext.StaffMembers
                    .Include(sm => sm.Positions)
                        .ThenInclude(sp => sp.Position)
                    .SingleOrDefault(sm => sm.StaffMemberId == staffId);

                if (staffMember == null)
                {
                    return false;
                }

                // Replace allocations with static variant
                foreach (var pos in staffMember.Positions)
                {
                    pos.Position.StaffPosition = new UnknownStaffPosition
                    {
                        StaffName = staffMember.FirstName,
                    };
                }

                dbContext.StaffMembers.Remove(staffMember);
                dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Get a new query object over the staff database
        /// </summary>
        /// <returns></returns>
        public IStaffQuery NewQuery()
        {
            return new DbStaffQuery(_dbOptions);
        }

        /// <summary>
        /// Generates the recent doubles for a staff member.
        /// </summary>
        /// <param name="context">The database context to use.</param>
        /// <param name="staffId">The ID of the staff member.</param>
        /// <param name="recentDoubleTime">The earliest time considered recent.</param>
        /// <returns>The recent doubles for the staff member.</returns>
        private static ICollection<long> GenerateRecentDoubles(
            AllocationContext context, int staffId, long recentDoubleTime)
        {
            // Find recent doubles for the staff member
            var doubles = context.StaffMembers
                .Include(s => s.Positions)
                    .ThenInclude(sp => sp.Position)
                        .ThenInclude(p => p.Allocation)
                .Single(s => s.StaffMemberId == staffId)
                    .Positions
                        .Where(p => p.Position.Allocation.Time >= recentDoubleTime)
                        .GroupBy(p => p.Position.Allocation.Time)
                            .Where(g => g.Count() > 1)
                            .Select(g => g.Key)
                            .ToList();

            // Add the stored last double value if it is relevant
            var staff = context.StaffMembers.Find(staffId);
            if (staff.LastDouble.HasValue && staff.LastDouble.Value >= recentDoubleTime)
            {
                doubles.Add(staff.LastDouble.Value);
            }

            if (doubles.Count == 0)
            {
                return doubles;
            }

            // Find and remove the last doubles that occur within a time interval
            doubles.Sort();
            long intervalStart = doubles.Min();
            var removeList = new List<int>();
            for (int i = 1; i < doubles.Count; i++)
            {
                if (doubles[i] > intervalStart + DoubleIntervalSeconds)
                {
                    intervalStart = doubles[i];
                }
                else
                {
                    removeList.Add(i);
                }
            }

            removeList.Reverse();
            foreach (var index in removeList)
            {
                doubles.RemoveAt(index);
            }

            return doubles;
        }
    }
}
