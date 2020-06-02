using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HospitalAllocation.Model;
using HospitalAllocation.Providers.Staff.Interface;
using DataObject = HospitalAllocation.Data.StaffMember;

namespace HospitalAllocation.Providers.Staff.Database
{
    /// <summary>
    /// Query object to provide a query interface to map the underlying relational
    /// database to the staff member data objects while also meeting the need to
    /// perform complex queries on the data
    /// </summary>
    public class DbStaffQuery : IStaffQuery
    {
        // The database context object backing the query
        private readonly AllocationContext _dbContext;

        /// <summary>
        /// Construct a new database query from given allocation database context options
        /// </summary>
        /// <param name="dbOptions"></param>
        public DbStaffQuery(DbContextOptions<AllocationContext> dbOptions)
        {
            _dbContext = new AllocationContext(dbOptions);
        }

        /// <summary>
        /// Create a query with the necessary inclusions over all staff
        /// </summary>
        public IQueryable<DataObject.IdentifiedStaffMember> StaffMembers
        {
            get
            {
                return _dbContext.StaffMembers
                    .Include(sm => sm.Designation)
                    .Include(sm => sm.StaffSkills)
                        .ThenInclude(ss => ss.Skill)
                    .Select(s =>
                        new DataObject.IdentifiedStaffMember(
                            s.StaffMemberId,
                            new DataObject.StaffMember(
                                s.FirstName,
                                s.LastName,
                                s.Alias,
                                s.Designation.Name,
                                s.StaffSkills.Select(ss => ss.Skill.Name).ToList(),
                                s.LastDouble,
                                null,
                                s.PhotoId,
                                s.RosterOnId)));
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
