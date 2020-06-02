using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// A staff designation, describing the kind of nurse or
    /// staff member a person is
    /// </summary>
    public class Designation
    {
        /// <summary>
        /// The database key of the designation
        /// </summary>
        [Key]
        public int DesignationId { get; set; }

        /// <summary>
        /// The short name of this designation (e.g. ANUM)
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// A longer description of the designation
        /// </summary>
        public string Description { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Lists the staff members with this designation
        /// </summary>
        public List<StaffMember> StaffMembers { get; set; }

        #endregion /* Navigation Properties */
    }
}
