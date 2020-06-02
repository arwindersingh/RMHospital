using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HospitalAllocation.Model
{
    public class Skill
    {
        /// <summary>
        /// The database key of the ID
        /// </summary>
        [Key]
        public int SkillId { get; set; }

        /// <summary>
        /// The short form name of this skill
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// A long form description of this skill
        /// </summary>
        public string Description { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Link to the association table to list all staff with this skill
        /// </summary>
        public List<StaffSkill> StaffSkills { get; set; }

        #endregion /* Navigation Properties */
    }
}
