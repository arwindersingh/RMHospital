using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// The possible position types.
    /// </summary>
    public enum PositionType
    {
        Bed = 1,
        Consultant = 2,
        TeamLeader = 3,
        Registrar = 4,
        Resident = 5,
        PodCa = 6,
        CaCleaner = 7,
        AccessCoordinator = 8,
        Tech = 9,
        Mern = 10,
        CaSupport = 11,
        WardOnCallConsultant = 12,
        TransportRegistrar = 13,
        DonationCoordinator = 14,
        Cnm = 15,
        Cnc = 16,
        Resource = 17,
        InternalRegistrar = 18,
        ExternalRegistrar = 19,
        Educator = 20
    }

    /// <summary>
    /// An entry in the table mapping <see cref="T:HospitalAllocation.Model.PositionType"/>.
    /// </summary>
    public class PositionTypeEntry
    {
        /// <summary>
        /// The id of the position type entry.
        /// </summary>
        [Key]
        public int PositionTypeId { get; set; }

        /// <summary>
        /// The <see cref="T:HospitalAllocation.Model.PositionType"/> the entry represents.
        /// </summary>
        public PositionType Type { get; set; }

        /// <summary>
        /// The name of the <see cref="T:HospitalAllocation.Model.PositionType"/>.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
