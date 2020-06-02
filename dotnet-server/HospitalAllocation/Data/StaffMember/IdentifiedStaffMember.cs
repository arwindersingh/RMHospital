using System;
using System.Runtime.Serialization;

namespace HospitalAllocation.Data.StaffMember
{
    /// <summary>
    /// A staff member paired with their identifier
    /// </summary>
    [DataContract]
    public class IdentifiedStaffMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:HospitalAllocation.Data.StaffMember.IdentifiedStaffMember"/> class.
        /// </summary>
        /// <param name="staffId">The database identifier of the staff member</param>
        /// <param name="staffMember">The staff member value</param>
        public IdentifiedStaffMember(int staffId, StaffMember staffMember)
        {
            StaffId = staffId;
            StaffMember = staffMember;
        }

        /// <summary>
        /// The database identifier for this staff member
        /// </summary>
        /// <value>The staff identifier.</value>
        [DataMember(Name = "id")]
        public int StaffId { get; }

        /// <summary>
        /// The staff member data object with all relevant information
        /// </summary>
        /// <value>The staff member.</value>
        [DataMember(Name = "staff")]
        public StaffMember StaffMember { get; }
    }
}
