using System;
using System.Runtime.Serialization;

namespace HospitalAllocation.Messages.Responses.Handover
{
    public class HandoverIdResponse : ApiResponse
    {

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Messages.Responses.Handover.HandoverIdResponse"/> class.
        /// </summary>
        /// <param name="handoverId">Handover identifier.</param>
        public HandoverIdResponse(int handoverId) : base(ResponseStatus.Success) => HandoverId = handoverId;


        /// <summary>
        /// The integer id which represents the skill
        /// </summary>
        [DataMember(Name = "handover_id")]
        public int HandoverId{ get; }


    }
}
