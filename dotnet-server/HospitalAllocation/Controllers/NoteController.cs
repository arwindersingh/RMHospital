using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HospitalAllocation.Providers.Note.Interface;
using HospitalAllocation.Providers.Staff.Interface;
using HospitalAllocation.Data.Note;
using HospitalAllocation.Messages.Responses.Note;
using HospitalAllocation.Messages.Responses;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace HospitalAllocation.Controllers
{
    /// <summary>
    /// Controls the note api
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]")]
    public class NoteController : Controller
    {
        /// <summary>
        /// Note provider that provides access to the data in the database
        /// </summary>
        private readonly INoteProvider _noteProvider;

        /// <summary>
        /// Staff provider that provides access to the data in the database
        /// </summary>
        private readonly IStaffProvider _staffProvider;

        /// <summary>
        /// Constructs a note controller
        /// </summary>
        /// <param name="noteProvider">The given note provider</param>
        /// <param name="staffProvider">The given staff provider</param>
        public NoteController(INoteProvider noteProvider, IStaffProvider staffProvider)
        {
            _noteProvider = noteProvider;
            _staffProvider = staffProvider;
        }

        /// <summary>
        /// Creates a new note
        /// </summary>
        /// <param name="newNote">The given note</param>
        /// <returns>The newly created note id</returns>
        [HttpPost]
        public IActionResult Create([FromBody] KnownNote newNote)
        {
            if (newNote.Contents == null)
            {
                return BadRequest(Json(new ErrorResponse("Contents can't be empty.")));
            }
            int newNoteId = _noteProvider.Create(newNote);
            if (newNoteId != 0)
            {
                return Json(new NoteIdResponse(newNoteId));
            }
            return BadRequest(Json(new ErrorResponse(String.Format("Staff member with ID {0} does not exist.", newNote.StaffId))));
        }

        /// <summary>
        /// Gets a note
        /// </summary>
        /// <param name="noteId">The given note id</param>
        /// <returns>The note with given note id</returns>
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("{noteId}")]
        public IActionResult GetNote(int noteId)
        {
            KnownNote note = _noteProvider.GetNote(noteId);
            if (note != null)
            {
                return Json(new NoteResponse(note));
            }
            return BadRequest(Json(new ErrorResponse(String.Format("Note with note ID {0} does not exist", noteId))));
        }

        /// <summary>
        /// Get all the notes constrained by the query
        /// </summary>
        /// <param name="staffId">The given staff member id</param>
        /// <returns>The list of notes constrained by the query</returns>
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult GetStaffNotes([FromQuery(Name = "staff_id")] int? staffId)
        {
            // If there is no staff member id specified in the query, return all existing notes.
            if (staffId == null)
            {
                ImmutableList<KnownNote> allNotes = _noteProvider.GetNotes();
                return Json(new NoteListResponse(allNotes));
            }
            if (_staffProvider.Get(staffId.Value) == null)
            {
                return BadRequest(Json(new ErrorResponse(String.Format("Staff member with ID {0} does not exist.", staffId)))); 
            }
            return Json(new NoteListResponse(_noteProvider.GetStaffNotes(staffId.Value)));
        }

        /// <summary>
        /// Updates a note
        /// </summary>
        /// <param name="noteId">The given note id</param>
        /// <param name="newNote">The new note value</param>
        /// <returns>The newly updated note value</returns>
        [HttpPut("{noteId}")]
        public IActionResult UpdateExistingNote(int noteId, [FromBody] KnownNote newNote)
        {
            if (newNote == null)
            {
                return BadRequest(Json(new ErrorResponse("Invalid Request")));
            }
            if (newNote.Contents == null)
            {
                return BadRequest(Json(new ErrorResponse("Contents can't be empty.")));
            }
            if (_noteProvider.Update(noteId, newNote))
            {
                return Json(new NoteResponse(_noteProvider.GetNote(noteId)));
            }
            return BadRequest(Json(new ErrorResponse(String.Format("Note with ID {0} does not exist.", noteId))));
        }

        /// <summary>
        /// Deletes the note
        /// </summary>
        /// <param name="id">The given note id</param>
        /// <returns>Success Status</returns>
        [HttpDelete("{noteId}")]
        public IActionResult DeleteNote(int noteId)
        {
            if (_noteProvider.Delete(noteId))
            {
                return Json(new NoteIdResponse(noteId));
            }
            return BadRequest(Json(new ErrorResponse(String.Format("Note with ID {0} does not exist.", noteId))));
        }
    }
}