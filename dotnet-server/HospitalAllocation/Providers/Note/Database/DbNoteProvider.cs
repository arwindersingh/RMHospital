using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using HospitalAllocation.Model;
using HospitalAllocation.Providers.Note.Interface;
using HospitalAllocation.Data.Note;

namespace HospitalAllocation.Providers.Note.Database
{
    public class DbNoteProvider : INoteProvider
    {
        /// <summary>
        /// The database context option used to construct a database connection
        /// </summary>
        private readonly DbContextOptions<AllocationContext> _dbContextOptions;

        /// <summary>
        /// Constructs a database note provider object
        /// </summary>
        /// <param name="dbContextOptions"></param>
        public DbNoteProvider(DbContextOptions<AllocationContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        /// <summary>
        /// Creates a new note
        /// </summary>
        /// <param name="newNote">The new note supplied</param>
        /// <returns>The database identifier key of the newly created note</returns>
        public int Create(KnownNote newNote)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var modelNote = new Model.Note()
            {
                StaffMemberId = newNote.StaffId,
                Contents = newNote.Contents,
                CreationTime = now,
                LastModificationTime = now
            };

            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                if (dbContext.StaffMembers.Find(modelNote.StaffMemberId) != null)
                {
                    dbContext.Notes.Add(modelNote);
                    dbContext.SaveChanges();
                    return modelNote.NoteId;
                } 
                return 0;
            }
        }

        public ImmutableList<KnownNote> GetNotes()
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                ImmutableList<KnownNote> notes = dbContext.Notes
                    .Select(n => new KnownNote(
                        n.NoteId, 
                        n.StaffMemberId, 
                        n.Contents,
                        n.CreationTime,
                        n.LastModificationTime))
                    .ToImmutableList();
                return notes;
            }
        }

        /// <summary>
        /// Lists the notes of a staff member
        /// </summary>
        /// <param name="StaffMemberId">Given Id of the staff member</param>
        /// <returns>The notes of that staff member</returns>
        public ImmutableList<KnownNote> GetStaffNotes(int id)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                ImmutableList<KnownNote> notes = dbContext.Notes
                    .Where(n => n.StaffMemberId == id)
                    .Select(n => new KnownNote(
                        n.NoteId, 
                        n.StaffMemberId, 
                        n.Contents,
                        n.CreationTime,
                        n.LastModificationTime))
                    .OrderBy(n => n.CreationTime)
                    .ToImmutableList();
                return notes;
            }
        }

        /// <summary>
        /// Gets a note
        /// </summary>
        /// <param name="noteId">The given note id</param>
        /// <returns>The note matching the id</returns>
        public KnownNote GetNote(int noteId)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Note note = dbContext.Notes.Find(noteId);
                if (note == null)
                {
                    return null;
                }
                return new KnownNote(
                    note.NoteId, 
                    note.StaffMemberId, 
                    note.Contents,
                    note.CreationTime,
                    note.LastModificationTime);
            }
        }

        /// <summary>
        /// Updates an existing note
        /// </summary>
        /// <param name="noteId">Id of the existing note</param>
        /// <param name="newNote">The new note</param>
        /// <returns>Success status</returns>
        public bool Update(int noteId, KnownNote newNote)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Note note = dbContext.Notes.Find(noteId);
                if (note == null)
                {
                    return false;
                }
                note.Contents = newNote.Contents;
                note.LastModificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>   
        /// Deletes a note
        /// </summary>
        /// <param name="noteId">The given note id</param>
        /// <returns>Success status</returns>
        public bool Delete(int noteId)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Note note = dbContext.Notes.Find(noteId);
                if (note == null)
                {
                    return false;
                }
                dbContext.Notes.Remove(note);
                dbContext.SaveChanges();
                return true;
            }
        }
    }
}