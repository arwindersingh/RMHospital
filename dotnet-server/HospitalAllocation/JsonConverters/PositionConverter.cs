using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HospitalAllocation.Data.Allocation.Positions;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace HospitalAllocation.JsonConverters
{
    /// <summary>
    /// Implements a JSON converter for Position abstract class. It parses the 
    /// JSON to either of two concrete sub classes of Position.
    /// 
    /// If there was an error on parsing, it returns an empty UnidentifiedPosition
    /// </summary>
    class PositionConverter : AbstractConverter<Position>
    {
        /// <summary>
        /// Dictionary from JSON strings to their corresponding ShiftType values
        /// </summary>
        private static readonly Dictionary<string, ShiftType> s_shiftTypes;

        /// <summary>
        /// Static constructor. Currently used to:
        ///   - Build the enum name/value dictionary from EnumMemberAttributes given to ShiftType values
        /// </summary>
        static PositionConverter()
        {
            Type shiftEnum = typeof(ShiftType);
            s_shiftTypes = new Dictionary<string, ShiftType>();

            // For each value in the ShiftType enum, go through and get the name assigned in the EnumMemberAttribute
            foreach (ShiftType shiftType in Enum.GetValues(shiftEnum))
            { 
                // Ugly array index because we get an array back :(
                MemberInfo enumValInfo = shiftEnum.GetMember(shiftType.ToString())[0];
                EnumMemberAttribute enumValAttr = enumValInfo.GetCustomAttribute<EnumMemberAttribute>();
                s_shiftTypes.Add(enumValAttr.Value, shiftType);
            }
        }

        /// <summary>
        /// Indicates JSONConverter should not use this class to write JSON
        /// That behaviour is provided by default
        /// </summary>
        public override bool CanWrite
        {
            get => false;
        }

        /// <summary>
        /// Parses the JSON object and creates an appropiate instance of a 
        /// subclass of Position
        /// </summary>
        /// <param name="objectType">Expected object type</param>
        /// <param name="jObject">The object containing input JSON</param>
        /// <returns>Returns an object of a subclass of Postion type</returns>
        protected override Position Create(Type objectType, JObject jObject)
        {
            var staff_name = jObject.Property("staff_name");
            var staff_id = jObject.Property("staff_id");
            var shift_type = jObject.Property("shift_type");

            // If both staff_name and staff_id are set, return UnidentifiedPosition
            // If both staff_name and staff_id are not set, return UnidentifiedPosition
            // If shift_type is not set return UnidentifiedPostion
            if (!(staff_name == null ^ staff_id == null) || shift_type == null)
            {
                return default(Position);
            }

            // Look up the value by the EnumMemberAttribute name given to it
            if (!s_shiftTypes.TryGetValue(jObject["shift_type"].Value<string>(), out ShiftType shiftType))
            {
                return default(Position);
            }

            if (staff_id != null)
            {
                int id = jObject["staff_id"].Value<int>();
                return new IdentifiedPosition(id, shiftType);
            }

            else if (staff_name != null)
            {
                String name = jObject["staff_name"].Value<string>();
                return new UnidentifiedPosition(name, shiftType);
            }
            return new UnidentifiedPosition(null, 0);
        }
    }
}