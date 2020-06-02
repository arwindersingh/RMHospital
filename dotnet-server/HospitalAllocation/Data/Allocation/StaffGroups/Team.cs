using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HospitalAllocation.Data.Allocation.StaffGroups
{
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TeamType
    {
        /// <summary>
        /// Pod A
        /// </summary>
        [EnumMember(Value = "a")]
        A,

        /// <summary>
        /// Pod B
        /// </summary>
        [EnumMember(Value = "b")]
        B,

        /// <summary>
        /// Pod C
        /// </summary>
        [EnumMember(Value = "c")]
        C,

        /// <summary>
        /// Pod D
        /// </summary>
        [EnumMember(Value = "d")]
        D,

        /// <summary>
        /// The senior team
        /// </summary>
        [EnumMember(Value = "senior")]
        Senior,
    }

    /// <summary>
    /// A nursing team in the ICU
    /// </summary>
    [DataContract]
    [KnownType(typeof(Pod))]
    [KnownType(typeof(SeniorTeam))]
    public abstract class Team
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:HospitalAllocation.Data.Allocation.StaffGroups.Team"/> class.
        /// </summary>
        protected Team(TeamType teamType)
        {
            TeamType = teamType;
        }

        /// <summary>
        /// The name of the team
        /// </summary>
        /// <value>The name of the team.</value>
        [DataMember(Name = "name")]
        public TeamType TeamType { get; }
    }
}
