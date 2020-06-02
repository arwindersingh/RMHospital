using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalAllocation.Data.StaffMember;

namespace HospitalAllocation.Providers.Staff.Interface
{
    /// <summary>
    /// Interface describing a provider that manages staff member data
    /// </summary>
    public interface IStaffProvider
    {
        /// <summary>
        /// Create a new staff member and return their allocated ID
        /// </summary>
        /// <param name="staffMember">the staff member to create in the datastore</param>
        /// <returns>the integer identifier with which the staff data may be accessed</returns>
        int Create(StaffMember staffMember);

        /// <summary>
        /// Retrieve a staff member from the staff datastore by ID
        /// </summary>
        /// <param name="staffId">the integer identifier of the staff member data to get</param>
        /// <returns></returns>
        StaffMember Get(int staffId);

        /// <summary>
        /// Retrieve a staff member from the staff datastore by ID
        /// </summary>
        /// <param name="staffId">The database ID of the staff member.</param>
        /// <param name="recentDoubleTime">The earliest double time considered recent.</param>
        /// <returns>A data object representation of the retrieved staff member.</returns>
        StaffMember Get(int staffId, long recentDoubleTime);

        /// <summary>
        /// Update an existing staff member entry in the datastore
        /// </summary>
        /// <param name="staffId">the ID of the staff member to update</param>
        /// <param name="staffMember">the new data to associate with the staff profile</param>
        /// <returns>the new data object value of the staff member in the database (largely redundant)</returns>
        StaffMember Update(int staffId, StaffMember staffMember);

        /// <summary>
        /// Delete an existing staff member entry in the datastore
        /// </summary>
        /// <param name="staffId">the integer identifier of the staff data object to delete</param>
        /// <returns>true if the entry was found and deleted, false otherwise</returns>
        bool Delete(int staffId);

        /// <summary>
        /// Get a disposable query reference to the staff database
        /// </summary>
        /// <returns></returns>
        IStaffQuery NewQuery();
    }

    /// <summary>
    /// A query object to represent a query over the staff provider
    /// </summary>
    public interface IStaffQuery : IDisposable
    {
        IQueryable<IdentifiedStaffMember> StaffMembers { get; }
    }
}
