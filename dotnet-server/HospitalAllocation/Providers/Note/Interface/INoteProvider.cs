using System;
using System.Collections.Immutable;
using System.Linq;
using HospitalAllocation.Data.Note;

namespace HospitalAllocation.Providers.Note.Interface
{
    /// <summary>
    /// Interface which defines the set of functions that any note provider class must implement
    /// </summary>
    public interface INoteProvider
    {
        /// <summary>
        /// Creates a new note
        /// </summary>
        /// <param name="note">The new note to store in the database</param>
        /// <returns>The database identifier key of the newly created note</returns>
        int Create(KnownNote note);

        /// <summary>
        /// Gets all existing notes
        /// </summary>
        /// <returns>A list of all existing notes</returns>
        ImmutableList<KnownNote> GetNotes();

        /// <summary>
        /// Lists the notes of a staff member
        /// </summary>
        /// <param name="StaffMemberId">Given Id of the staff member</param>
        /// <returns>The notes of that staff member</returns>
        ImmutableList<KnownNote> GetStaffNotes(int StaffMemberId);

        /// <summary>
        /// Gets a note
        /// </summary>
        /// <param name="noteId">The given Id</param>
        /// <returns>The note matching the Id</returns>
        KnownNote GetNote(int noteId);

        /// <summary>
        /// Updates an existing note
        /// </summary>
        /// <param name="noteId">Id of the existing note</param>
        /// <param name="note">The new note to be updated</param>
        /// <returns>Success status</returns>
        bool Update(int noteId, KnownNote note);

        /// <summary>
        /// Deletes a note
        /// </summary>
        /// <param name="noteId">The given note id</param>
        /// <returns>Success status</returns>
        bool Delete(int noteId);
    }
}