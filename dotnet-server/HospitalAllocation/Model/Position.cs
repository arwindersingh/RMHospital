using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// The possible shift types for an allocation.
    /// </summary>
    public enum ShiftType
    {
        EightHour,
        TwelveHour,
        Closed,
    }

    /// <summary>
    /// A position within an allocation.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// The id of this position.
        /// </summary>
        [Key]
        public int PositionId { get; set; }

        /// <summary>
        /// The id of the <see cref="T:HospitalAllocation.Model.TeamAllocation"/>.
        /// </summary>
        [Required]
        public int? AllocationId { get; set; }

        /// <summary>
        /// The type of this position.
        /// </summary>
        [Required]
        public PositionType? Type { get; set; }

        /// <summary>
        /// The shift type of this position.
        /// </summary>
        [Required]
        public ShiftType ShiftType { get; set; }

        #region Navigation Properties

        /// <summary>
        /// The <see cref="T:HospitalAllocation.Model.StaffPosition"/>
        /// assigned to this position.
        /// </summary>
        public StaffPosition StaffPosition { get; set; }

        /// <summary>
        /// The <see cref="T:HospitalAllocation.Model.TeamAllocation"/>
        /// this position is part of.
        /// </summary>
        [ForeignKey(nameof(AllocationId))]
        public TeamAllocation Allocation { get; set; }

        #endregion /* Navigation Properties */
    }
}
