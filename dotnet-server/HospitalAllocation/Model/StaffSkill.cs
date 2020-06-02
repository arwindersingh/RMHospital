using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// Associative table to associate StaffMembers and Skills
    /// </summary>
    public class StaffSkill
    {
        /// <summary>
        /// Database key of this staff-skill entry
        /// </summary>
        [Key]
        public int StaffSkillId { get; set; }

        /// <summary>
        /// The ID of the staff member with the skill
        /// </summary>
        [Required]
        public int StaffMemberId { get; set; }

        /// <summary>
        /// The ID of the skill the staff member has
        /// </summary>
        [Required]
        public int SkillId { get; set; }

        #region Navigation Properties

        /// <summary>
        /// The staff member with the skill
        /// </summary>
        [ForeignKey(nameof(StaffMemberId))]
        public StaffMember StaffMember { get; set; }

        /// <summary>
        /// The skill that the staff member has
        /// </summary>
        [ForeignKey(nameof(SkillId))]
        public Skill Skill { get; set; }

        #endregion
    }
}
