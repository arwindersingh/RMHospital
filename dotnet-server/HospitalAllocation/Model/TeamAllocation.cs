using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// An allocation of an ICU team.
    /// </summary>
    public class TeamAllocation
    {
        /// <summary>
        /// The id of the allocation.
        /// </summary>
        [Key]
        public int TeamAllocationId { get; set; }

        /// <summary>
        /// The team the allocation is for.
        /// </summary>
        [Required]
        public TeamType? Type { get; set; }

        /// <summary>
        /// The time of the allocation.
        /// </summary>
        public long Time { get; set; }

        #region Navigation Properties

        /// <summary>
        /// The allocations for this team.
        /// </summary>
        [InverseProperty(nameof(Position.Allocation))]
        public ICollection<Position> Positions { get; set; }

        #endregion /* Navigation Properties */
    }
}
