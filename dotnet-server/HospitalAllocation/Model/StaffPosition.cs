using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// A staff member filling a position.
    /// </summary>
    public abstract class StaffPosition
    {
        /// <summary>
        /// The id of this staff position.
        /// </summary>
        [Key]
        public int StaffPositionId { get; set; }

        /// <summary>
        /// The id of the <see cref="T:HospitalAllocation.Model.Position"/>.
        /// </summary>
        [Required]
        public int? PositionId { get; set; }

        #region Navigation Properties

        /// <summary>
        /// The <see cref="T:HospitalAllocation.Model.Position"/> being filled.
        /// </summary>
        [ForeignKey(nameof(PositionId))]
        public Position Position { get; set; }

        #endregion /* Navigation Properties */
    }
}
