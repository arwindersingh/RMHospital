using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HospitalAllocation.Messages.Responses
{
    /// <summary>
    /// Encodes the status of an API response
    /// </summary>
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResponseStatus
    {
        /// <summary>
        /// The API call was successfully processed
        /// </summary>
        [EnumMember(Value = "success")]
        Success,

        /// <summary>
        /// The API call did not process successfully
        /// </summary>
        [EnumMember(Value = "failure")]
        Failure,
    }

    /// <summary>
    /// A parent class for all response messages from the API
    /// </summary>
    [DataContract]
    public abstract class ApiResponse
    {
        protected ApiResponse(ResponseStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// The status of the response
        /// </summary>
        /// <value>The status.</value>
        [DataMember(Name = "status")]
        public ResponseStatus Status { get; }
    }
}
