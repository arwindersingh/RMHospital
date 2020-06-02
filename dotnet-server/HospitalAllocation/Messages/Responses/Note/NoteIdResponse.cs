using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using HospitalAllocation.Messages.Responses;

namespace HospitalAllocation.Messages.Responses.Note
{
    /// <summary>
    /// A message to confirm the successful creation of a note and inform
    /// the caller of the note entry's addressing id
    /// </summary>
    [DataContract]
    public class NoteIdResponse : ApiResponse
    {
        /// <summary>
        /// Constructs a new note id response message
        /// </summary>
        /// <param name="noteId">The given note id</param>
        public NoteIdResponse(int noteId) : base(ResponseStatus.Success)
        {
            NoteId = noteId;
        }

        /// <summary>
        /// The id of the newly created note
        /// </summary>
        [DataMember(Name = "note_id")]
        public int NoteId { get; }
    }
}