using System;
using System.Collections.Immutable;
using System.Linq;
using HospitalAllocation.Data.Designation;

namespace HospitalAllocation.Providers.Designation.Interface
{
    /// <summary>
    /// Interface which defines the set of functions that any designation provider class must implement
    /// </summary>
    public interface IDesignationProvider
    {
        /// <summary>
        /// Create a new designation
        /// </summary>
        /// <param name="designation">New designation to store in the database</param>
        /// <returns>The database identifier key of the newly created designation</returns>
        int Create(KnownDesignation designation);

        /// <summary>
        /// Lists the entire collection of designations
        /// </summary>
        /// <returns>ImmutableList of all the designations in the database</returns>
        ImmutableList<KnownDesignation> List();
        
        /// <summary>
        /// Get a designation matching the Id
        /// </summary>
        /// <param name="designationId">The id with which to query the database</param>
        /// <returns>The matching designation</returns>
        KnownDesignation Get(int designationId);

        /// <summary>
        /// Update an existing designation
        /// </summary>
        /// <param name="DesignationId">The id of existing designation</param>
        /// <param name="designation">The designation to update to</param>
        /// <returns>Success status</returns>
        bool Update(int DesignationId, KnownDesignation designation);

        /// <summary>
        /// Check if a designation exists
        /// </summary>
        /// <param name="name">The given name</param>
        /// <returns>If it exists</returns>
        bool Exists(string name);

        /// <summary>
        /// Deletes a designation if not associated with any staff
        /// </summary>
        /// <param name="designationId">The id of the designation to be deleted</param>
        /// <returns>Status</returns>
        bool Delete(int designationId);
    }
}