using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// A position filled by a staff member known to the database.
    /// </summary>
    public class KnownStaffPosition : StaffPosition
    {
        /// <summary>
        /// The id of the <see cref="T:HospitalAllocation.Model.StaffMember"/> in this position.
        /// </summary>
        [Required]
        public int? StaffMemberId { get; set; }

        #region Navigation Properties

        /// <summary>
        /// The <see cref="T:HospitalAllocation.Model.StaffMember"/> in this position.
        /// </summary>
        [ForeignKey(nameof(StaffMemberId))]
        public StaffMember StaffMember { get; set; }

        #endregion /* Navigation Properties */
    }
}
