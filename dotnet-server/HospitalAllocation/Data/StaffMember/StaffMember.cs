using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace HospitalAllocation.Data.StaffMember
{
    /// <summary>
    /// Describes a staff member as a readonly data passing object
    /// </summary>
    [DataContract]
    public class StaffMember
    {
        /// <summary>
        /// Construct a new staff member data passing object
        /// </summary>
        /// <param name="fName">the staff member's name</param>
        /// <param name="lName">the staff member's last name</param>
        /// <param name="alias">the staff member's alias</param>
        /// <param name="designation">the designation of this staff member</param>
        /// <param name="skills">the skills this staff member has</param>
        /// <param name="lastDouble">the last double shift the staff member worked</param>
        /// <param name="recentDoubles">The double allocations assigned to the staff member recently.</param>
        /// <param name="photo">the numerical ID of this staff member's photo in the database</param>
        /// <param name="rosteron_id">the numerical ID corresponding to this staff member's entry in the RMH RosterOn database</param>
        public StaffMember(string first_name, 
            string last_name,
            string alias,
            string staff_type,
            ICollection<string> skills,
            long? last_double,
            ICollection<long> recent_doubles,
            int? photo,
            int? rosteron_id)
        {
            FirstName = first_name;
            LastName = last_name;
            Alias = alias;
            Designation = staff_type;
            Skills = new List<string>(skills);
            LastDouble = last_double;
            PhotoId = photo;
            RosterOnId = rosteron_id;

            if (recent_doubles == null)
            {
                RecentDoubles = null;
            }
            else
            {
                RecentDoubles = new List<long>(recent_doubles);
            }
        }

        /// <summary>
        /// The first name of this person
        /// </summary>
        [DataMember(Name = "first_name")]
        public string FirstName { get; }


        /// <summary>
        /// The last name of this person
        /// </summary>
        [DataMember(Name = "last_name")]
        public string LastName { get; }

        /// <summary>
        /// The last name of this person
        /// </summary>
        [DataMember(Name = "alias")]
        public string Alias { get; }

        /// <summary>
        /// The designation that this staff member has
        /// </summary>
        [DataMember(Name = "staff_type")]
        public string Designation { get; }

        /// <summary>
        /// The nursing or staff skills of this staff member
        /// </summary>
        [DataMember(Name = "skills")]
        public IReadOnlyCollection<string> Skills { get; }

        /// <summary>
        /// The last double allocation assigned to the staff member, if any
        /// </summary>
        [DataMember(Name = "last_double")]
        public long? LastDouble { get; }

        /// <summary>
        /// The double allocations assigned to the staff member recently.
        /// </summary>
        [DataMember(Name = "recent_doubles", EmitDefaultValue = false)]
        public IReadOnlyCollection<long> RecentDoubles { get; }

        /// <summary>
        /// The database ID of the staff member's photo, if any
        /// </summary>
        [DataMember(Name = "photo")]
        public int? PhotoId { get; }

        /// <summary>
        /// The ID of this staff member in the RMH RosterOn database
        /// </summary>
        [DataMember(Name = "rosteron_id")]
        public int? RosterOnId { get; }
    }
}
