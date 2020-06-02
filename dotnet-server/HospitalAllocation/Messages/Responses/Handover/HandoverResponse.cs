using System;
using System.Runtime.Serialization;
using HospitalAllocation.Data.Handover;

namespace HospitalAllocation.Messages.Responses.Handover
{
    public class HandoverResponse : ApiResponse
    {
        public HandoverResponse(HandoverDTO handover) : base(ResponseStatus.Success) => Handover = handover;

        [DataMember(Name = "handover")]
        public HandoverDTO Handover { get; }
    }

}
