using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HospitalAllocation.Data.Note;
using HospitalAllocation.Messages.Responses;

namespace HospitalAllocation.Messages.Responses.Note
{
    /// <summary>
    /// Message to return a list of notes
    /// </summary>
    [DataContract]
    public class NoteListResponse : ApiResponse
    {
        /// <summary>
        /// Constructs a new note list response
        /// </summary>
        /// <param name="notes">The given list of notes</param>
        public NoteListResponse(IReadOnlyCollection<KnownNote> notes) : base(ResponseStatus.Success)
        {
            Notes = notes;
        }

        /// <summary>
        /// The list of notes to return
        /// </summary>
        [DataMember(Name = "note_list")]
        public IReadOnlyCollection<KnownNote> Notes { get; }
    }  
}
