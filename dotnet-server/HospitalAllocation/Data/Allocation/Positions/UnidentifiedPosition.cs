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
    class UnidentifiedPosition : Position
    {
        public static UnidentifiedPosition Empty
        {
            get => s_emptyPosition ?? (s_emptyPosition = new UnidentifiedPosition(string.Empty, ShiftType.EightHour));
        }
        private static UnidentifiedPosition s_emptyPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HospitalAllocation.Data.Allocation.Positions.UnidentifiedPosition"/> class.
        /// </summary>
        /// <param name="staffName">Staff name.</param>
        /// <param name="shiftType">Shift type.</param>
        public UnidentifiedPosition(String staffName, ShiftType shiftType) : base(shiftType)
        {
            StaffName = staffName;
        }

        /// <summary>
        /// The staff name assigned to this position
        /// </summary>
        [DataMember(Name = "staff_name")]
        public String StaffName { get; }

        /// <summary>
        /// Checks whether this position is empty
        /// </summary>
        /// <returns><c>true</c>, if the position is empty, <c>false</c> otherwise.</returns>
        override public bool IsEmpty() => StaffName == null;
    }
}
