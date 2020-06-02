using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using HospitalAllocation.Model;
using HospitalAllocation.Providers.Designation.Interface;
using HospitalAllocation.Data.Designation;

namespace HospitalAllocation.Providers.Designation.Database
{
    /// <summary>
    /// Designation provider which is backed up by a database
    /// </summary>
    public class DbDesignationProvider : IDesignationProvider
    {
        /// <summary>
        /// The database context option used to construct a database connection
        /// </summary>
        private readonly DbContextOptions<AllocationContext> _dbContextOptions;

        /// <summary>
        /// Construct the provider object with the supplied context options
        /// </summary>
        /// <param name="dbContextOptions">The database context option used to construct a database connection</param>
        public DbDesignationProvider(DbContextOptions<AllocationContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        /// <summary>
        /// Create a new designation
        /// </summary>
        /// <param name="newDesignation">New designation to store in the database</param>
        /// <returns>The database identifier key of newly created designation</returns>
        public int Create(KnownDesignation newDesignation)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Designation designation = dbContext.Designations.SingleOrDefault(
                    d => String.Equals(d.Name, newDesignation.Name, StringComparison.OrdinalIgnoreCase)
                );
                if (designation == null) // No existing designation was found
                {
                    designation = new Model.Designation() { Name = newDesignation.Name };
                    dbContext.Designations.Add(designation);
                    dbContext.SaveChanges();
                }
                return designation.DesignationId;
            }
        }

        /// <summary>
        /// Lists the entire collection of designations
        /// </summary>
        /// <returns>Immutable list of all the designations in the database</returns>
        public ImmutableList<KnownDesignation> List()
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                return dbContext.Designations
                            .Select(x =>
                                new KnownDesignation(x.DesignationId, x.Name))
                            .ToImmutableList();
            }
        }

        /// <summary>
        /// Get a designation matching the Id
        /// </summary>
        /// <param name="designationId">The id with which to query the databse</param>
        /// <returns>The matching designation</returns>
        public KnownDesignation Get(int id)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Designation designation = dbContext.Designations.Find(id);
                if (designation == null)
                {
                    return null;
                }
                return new KnownDesignation(designation.DesignationId, designation.Name);
            }
        }

        /// <summary>
        /// Update an existing designation
        /// </summary>
        /// <param name="designationId">The id of existing designation</param>
        /// <param name="designation">The new designation to update to</param>
        /// <returns>Success status</returns>
        public bool Update(int designationId, KnownDesignation designation)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Designation dbDesignation = dbContext.Designations.Find(designationId);
                if (dbDesignation == null)
                {
                    return false;
                }
                dbDesignation.Name = designation.Name;
                dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Check if a designation exists
        /// </summary>
        /// <param name="name">The given name</param>
        /// <returns>If it exists</returns>
        public bool Exists(string name)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                // If there is any existing desigation in the database that has the same name.
                return dbContext.Designations.Any(d => String.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Deletes a designation if not associated with any staff
        /// </summary>
        /// <param name="designationID">The id of designation to delete</param>
        /// <returns>Success status</returns>
        public bool Delete(int designationId)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Designation designation = dbContext.Designations
                                                    .Include(d => d.StaffMembers)
                                                    .SingleOrDefault(d => d.DesignationId == designationId);
                // Only delete a skill that exists and is not associated with other staff
                if (designation == null || designation.StaffMembers?.Any() == true)
                {
                    return false;
                }
                dbContext.Designations.Remove(designation);
                dbContext.SaveChanges();
                return true;
            }
        }
    }
}