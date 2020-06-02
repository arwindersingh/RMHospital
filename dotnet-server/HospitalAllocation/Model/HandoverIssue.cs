using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// Handover issue
    /// There can be multiple issues in a handover. This is an entry of the
    /// table in the handover 
    /// </summary>
    public class HandoverIssue
    {
        /// <summary>
        /// The database Id of a issue of a handover
        /// </summary>
        /// <value>The handover issue identifier.</value>
        [Key]
        public int HandoverIssueId { get; set; }

        /// <summary>
        /// The issue number in the card
        /// </summary>
        /// <value>The issue number.</value>
        public int IssueNumber { get; set; }
        /// <summary>
        /// Gets or sets the IssueDescription (Current Issue Column of Handover)
        /// </summary>
        /// <value>The issue description.</value>
        public string CurrentIssue { get; set; }

        /// <summary>
        /// Gets or sets the management.
        /// </summary>
        /// <value>The management.</value>
        public string Management { get; set; }

        /// <summary>
        /// Gets or sets the follow up.
        /// </summary>
        /// <value>The follow up.</value>
        public string FollowUp { get; set; }

        /// <summary>
        /// Gets or sets the handover.
        /// </summary>
        /// <value>The handover.</value>
        public Handover Handover { get; internal set; }
    }
}
