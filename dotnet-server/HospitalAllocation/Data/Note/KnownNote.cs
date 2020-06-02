using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HospitalAllocation.Data.Note
{
    /// <summary>
    /// Describes a note as a readonly data passing object 
    /// </summary>
    [DataContract]
    public class KnownNote
    {
        /// <summary>
        /// Initializes a note
        /// </summary>
        /// <param name="staffId">Id of the owner staff of the note</param>
        /// <param name="contents">Contents of the note</param>
        [JsonConstructor]
        public KnownNote(int staffId, string contents)
        {
            StaffId = staffId;
            Contents = contents;
            CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            LastModificationTime = CreationTime;
        }

        /// <summary>
        /// Initializes a note
        /// </summary>
        /// <param name="noteId">Note id in the database</param>
        /// <param name="staffId">Staff member id of the note</param>
        /// <param name="contents">Contents of note</param>
        /// <param name="creationTime">Creation time of the note</param>
        /// <param name="lastModificationTime">Last modification time of the note</param>
        public KnownNote(int noteId,
                         int staffId,
                         string contents,
                         long creationTime,
                         long lastModificationTime)
        {
            NoteId = noteId;
            StaffId = staffId;
            Contents = contents;
            CreationTime = creationTime;
            LastModificationTime = lastModificationTime;
        }

        /// <summary>
        /// The database id of the note
        /// </summary>
        [DataMember(Name = "note_id")]
        public int? NoteId { get; }

        /// <summary>
        /// The id of the staff member owning the note
        /// </summary>
        [DataMember(Name = "staff_id")]
        public int StaffId { get; }

        /// <summary>
        /// The contents of the note
        /// </summary>
        [DataMember(Name = "contents")]
        public string Contents { get; }

        /// <summary>
        /// The creation time of the note
        /// </summary>
        [DataMember(Name = "creation_time")]
        public long CreationTime { get; }

        /// <summary>
        /// The lasted time that the note got modified
        /// </summary>
        [DataMember(Name = "last_modification_time")]
        public long LastModificationTime { get; }
    }
}