using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// A note or comment attached to a staff member
    /// </summary>
    public class Note
    {
        /// <summary>
        /// The database id of the note
        /// </summary>
        [Key]
        public int NoteId { get; set; }

        /// <summary>
        /// The staff member to whom the note is attached
        /// </summary>
        [Required]
        public int StaffMemberId { get; set; }

        /// <summary>
        /// The time the note was made
        /// </summary>
        [Required]
        public long CreationTime { get; set; }

        /// <summary>
        /// The latest time the note was modified
        /// </summary>
        [Required]
        public long LastModificationTime { get; set; }

        /// <summary>
        /// The actual note itself
        /// </summary>
        [Required]
        public string Contents { get; set; }

        #region Navigation Properties

        /// <summary>
        /// The staff member whose photo this is
        /// </summary>
        [ForeignKey(nameof(StaffMemberId))]
        public StaffMember StaffMember { get; set; }

        #endregion
    }
}
