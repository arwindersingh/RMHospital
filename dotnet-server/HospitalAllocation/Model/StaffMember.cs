using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// A StaffMember who may be allocated to beds or team positions
    /// </summary>
    public class StaffMember
    {
        /// <summary>
        /// The database ID of this staff member
        /// </summary>
        [Key]
        public int StaffMemberId { get; set; }

        /// <summary>
        /// The first name of this person
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of this person
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// The staff member's chosen alias
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// The ID of this staff member's designation
        /// </summary>
        public int? DesignationId { get; set; }

        /// <summary>
        /// The staff member's ID in the RosterOn database
        /// </summary>
        public int? RosterOnId { get; set; }

        /// <summary>
        /// The ID of the staff member's photo -- if they have one
        /// </summary>
        public int? PhotoId { get; set; }

        /// <summary>
        /// The date of the last double allocation this person had
        /// </summary>
        public long? LastDouble { get; set; }

        #region Navigation Properties

        /// <summary>
        /// The staff member's photo
        /// </summary>
        [ForeignKey(nameof(PhotoId))]
        public Photo Photo { get; set; }

        /// <summary>
        /// The designation of this staff member
        /// </summary>
        [ForeignKey(nameof(DesignationId))]
        public Designation Designation { get; set; }

        /// <summary>
        /// Any notes associated with this staff member
        /// </summary>
        public List<Note> Notes { get; set; }

        /// <summary>
        /// Association property linking all the skills this staff member has
        /// </summary>
        public List<StaffSkill> StaffSkills { get; set; }

        /// <summary>
        /// The positions this staff member has filled.
        /// </summary>
        public List<KnownStaffPosition> Positions { get; set; }

        #endregion /* Navigation Properties */
    }
}
