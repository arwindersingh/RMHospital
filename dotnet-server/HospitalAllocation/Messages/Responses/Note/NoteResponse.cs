using System.Runtime.Serialization;
using System.Collections.Generic;
using HospitalAllocation.Data.Note;

namespace HospitalAllocation.Messages.Responses.Note
{
    /// <summary>
    /// Message to return a single note value to the caller
    /// </summary>
    [DataContract]
    public class NoteResponse : ApiResponse
    {
        /// <summary>
        /// Creates a new note response message
        /// </summary>
        /// <param name="note">The given note value</param>
        public NoteResponse(KnownNote note) : base(ResponseStatus.Success)
        {
            Note = note;
        }

        /// <summary>
        /// The note value to return
        /// </summary>
        [DataMember(Name = "Note")]
        public KnownNote Note { get; }
    }
}