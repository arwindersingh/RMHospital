using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HospitalAllocation.Data.Allocation.Positions
{
    /// <summary>
    /// A position to which a staff member can be allocated, restricted by
    /// the type of staff member who may be allocated to that position
    /// </summary>
    [DataContract]
    class IdentifiedPosition : Position
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:HospitalAllocation.Data.Allocation.Positions.IdentifiedPosition"/> class.
        /// </summary>
        /// <param name="staffId">Staff Id.</param>
        /// <param name="shiftType">Shift type.</param>
        public IdentifiedPosition(int staffId, ShiftType shiftType) : base(shiftType)
        {
            StaffId = staffId;
        }

        /// <summary>
        /// The staff id assigned to this position
        /// </summary>
        [DataMember(Name = "staff_id")]
        public int StaffId { get; }

        /// <summary>
        /// Checks whether this position is empty
        /// </summary>
        /// <returns><c>true</c>, if the position is empty, <c>false</c> otherwise.</returns>
        override public bool IsEmpty() => StaffId == 0;
    }
}
