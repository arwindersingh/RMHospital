using System;
using System.Runtime.Serialization;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Messages.Responses.Allocation
{
    /// <summary>
    /// Contains a summary of a single team
    /// </summary>
    [DataContract]
    public class TeamSummaryResponse : ApiResponse
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Messages.Responses.Allocation.TeamSummaryResponse"/> class.
        /// </summary>
        /// <param name="team">Team.</param>
        public TeamSummaryResponse(Team team) : base(ResponseStatus.Success)
        {
            Team = team;
        }

        /// <summary>
        /// The team about which information was requested
        /// </summary>
        /// <value>The team.</value>
        [DataMember(Name = "team")]
        public Team Team { get; }
    }
}
