using System.ComponentModel.DataAnnotations;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// The teams in the ICU.
    /// </summary>
    public enum TeamType
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        Senior = 5
    }

    /// <summary>
    /// An entry in the table mapping <see cref="T:HospitalAllocation.Model.TeamType"/>.
    /// </summary>
    public class TeamTypeEntry
    {
        /// <summary>
        /// The id of the team type entry.
        /// </summary>
        [Key]
        public int TeamTypeId { get; set; }

        /// <summary>
        /// The <see cref="T:HospitalAllocation.Model.TeamType"/> the entry represents.
        /// </summary>
        public TeamType Type { get; set; }

        /// <summary>
        /// The name of the <see cref="T:HospitalAllocation.Model.TeamType"/>.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
