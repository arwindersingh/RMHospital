using System;
using System.Runtime.Serialization;

namespace HospitalAllocation.Messages.Responses
{
    /// <summary>
    /// Encodes that an error has occurred in an API call
    /// </summary>
    [DataContract]
    public class ErrorResponse : ApiResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:HospitalAllocation.Messages.Responses.ErrorResponse"/> class.
        /// </summary>
        /// <param name="reason">The reason the API call failed</param>
        public ErrorResponse(string reason) : base(ResponseStatus.Failure)
        {
            Reason = reason;
        }

        /// <summary>
        /// The reason the API call failed
        /// </summary>
        /// <value>The reason.</value>
        [DataMember(Name = "reason")]
        public string Reason { get; }
    }
}
