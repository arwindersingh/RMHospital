using System.Runtime.Serialization;

namespace HospitalAllocation.Messages.Responses.Designation
{
    /// <summary>
    /// A response message with an id of the chosen designation
    /// </summary>
    [DataContract]
    public class DesignationIdResponse : ApiResponse
    {
        /// <summary>
        /// The integer id which represents the designation
        /// </summary>
        [DataMember(Name = "designation_id")]
        public int DesignationId { get; }

        /// <summary>
        /// Constructs an object with the supplied designation id
        /// </summary>
        /// <param name="designationId">The supplied designation id</param>
        public DesignationIdResponse(int designationId) : base(ResponseStatus.Success)
        {
            DesignationId = designationId;
        }
    }
}