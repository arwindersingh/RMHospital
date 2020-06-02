using System.Runtime.Serialization;
using System.Collections.Immutable;
using HospitalAllocation.Data.Designation;

namespace HospitalAllocation.Messages.Responses.Designation
{
    /// <summary>
    /// A response message containing a list of designations
    /// </summary>
    [DataContract]
    public class DesignationListResponse : ApiResponse
    {
        /// <summary>
        /// The Immutable List containing the list of designations to construct the message
        /// </summary>
        [DataMember(Name = "designation_list")]
        public ImmutableList<KnownDesignation> Designations { get; }

        /// <summary>
        /// Construct a object with the provided list of designations
        /// </summary>
        /// <param name="designations">The provided list of designations</param>
        public DesignationListResponse(ImmutableList<KnownDesignation> designations) : base(ResponseStatus.Success)
        {
            Designations = designations;
        }
    }
}