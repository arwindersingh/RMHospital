using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Messages.Responses.Allocation
{
    /// <summary>
    /// Lists summaries of teams
    /// </summary>
    [DataContract]
    public class TeamListResponse : ApiResponse
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Messages.Responses.Allocation.TeamListResponse"/> class.
        /// </summary>
        /// <param name="teams">Teams.</param>
        public TeamListResponse(IReadOnlyList<TeamType> teams) : base(ResponseStatus.Success)
        {
            Teams = teams;
        }

        /// <summary>
        /// The summaries of the teams to list
        /// </summary>
        /// <value>The teams.</value>
        [DataMember(Name = "teams")]
        public IReadOnlyList<TeamType> Teams { get; }
    }
}
