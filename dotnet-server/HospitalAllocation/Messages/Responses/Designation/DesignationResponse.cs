using System.Runtime.Serialization;
using System.Collections.Generic;
using HospitalAllocation.Data.Designation;

namespace HospitalAllocation.Messages.Responses.Designation
{
    /// <summary>
    /// Message containing the details of a single designation
    /// </summary>
    [DataContract]
    public class DesignationResponse : ApiResponse
    {
        /// <summary>
        /// The value of a designation used to construct the message
        /// </summary>
        [DataMember(Name = "designation")]
        public KnownDesignation Designation { get; }

        /// <summary>
        /// Constructs a Message with the supplied designation
        /// </summary>
        /// <param name="designation">The supplied designation</param>
        public DesignationResponse(KnownDesignation designation) : base(ResponseStatus.Success)
        {
            Designation = designation;
        }
    }
}