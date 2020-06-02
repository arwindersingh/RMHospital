using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using HospitalAllocation.JsonConverters;

namespace HospitalAllocation.Data.Allocation.Positions
{
    /// <summary>
    /// How long a shift goes for
    /// </summary>
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShiftType
    {
        [EnumMember(Value = "8")]
        EightHour,
        [EnumMember(Value = "12")]
        TwelveHour,
        [EnumMember(Value = "closed")]
        Closed,
    }

    /// <summary>
    /// A position to which a staff member can be allocated, restricted by
    /// the type of staff member who may be allocated to that position
    /// </summary>
    [DataContract]
    [JsonConverter(typeof(PositionConverter))]
    abstract public class Position
    {
        protected Position(ShiftType shiftType)
        {
            ShiftType = shiftType;
        }

        /// <summary>
        /// The type of shift the staff member in this position takes
        /// </summary>
        /// <value></value>
        [DataMember(Name = "shift_type", IsRequired = true)]
        public ShiftType ShiftType { get; }

        /// <summary>
        /// Checks whether this position is empty
        /// </summary>
        /// <returns><c>true</c>, if the position is empty, <c>false</c> otherwise.</returns>
        public abstract bool IsEmpty();
    }
}
