using System;
using System.Runtime.Serialization;

namespace HospitalAllocation.Data.Handover
{


    [DataContract]
    public class HandoverIssueDTO
    {
        
        public HandoverIssueDTO(int issueNumber,
                                string currentIssue,
                                string management,
                                string followUp)
        {
            IssueNumber = issueNumber;
            CurrentIssue = currentIssue;
            Management = management;
            FollowUp = followUp;
        }

        [DataMember(Name = "issue_number")]
        public int IssueNumber { get; }

        [DataMember(Name = "current_issue")]
        public string CurrentIssue { get; }

        [DataMember(Name = "management")]
        public string Management { get; }

        [DataMember(Name = "follow_up")]
        public string FollowUp { get; }
    }
}
