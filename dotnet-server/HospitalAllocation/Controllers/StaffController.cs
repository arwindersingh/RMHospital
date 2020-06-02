using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalAllocation.Data.StaffMember;
using HospitalAllocation.Providers.Staff.Interface;
using HospitalAllocation.Messages.Responses;
using HospitalAllocation.Messages.Responses.Staff;

namespace HospitalAllocation.Controllers
{
    /// <summary>
    /// Controls the storage and handling of staff data
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]")]
    public class StaffController : Controller
    {
        /// <summary>
        /// The period before the current time we consider to be recent.
        /// </summary>
        private const long RecentDoublePeriodSeconds = 7776000;

        // The underlying provider for staff data storage
        private readonly IStaffProvider _staffProvider;

        /// <summary>
        /// Construct a new staff controller around a given staff provider
        /// </summary>
        /// <param name="staffProvider"></param>
        public StaffController(IStaffProvider staffProvider)
        {
            _staffProvider = staffProvider;
        }

        /// <summary>
        /// Get all staff which pass the query constraints.
        /// </summary>
        /// <param name="ids">The IDs to query.</param>
        /// <param name="rosterOnIds">The RosterOn IDs to query.</param>
        /// <param name="skills">The skills required.</param>
        /// <param name="staffTypes">The types of staff to query.</param>
        /// <param name="namePrefix">The prefix of the staff names.</param>
        /// <param name="maxDouble">The latest date a staff member has had a double allocation.</param>
        /// <returns>The staff which pass the query constraints.</returns>
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery(Name = "id")] int[] ids,
            [FromQuery(Name = "rosteron_id")] int[] rosterOnIds,
            [FromQuery(Name = "skill")] string[] skills,
            [FromQuery(Name = "type")] string[] staffTypes,
            [FromQuery(Name = "prefix")] string namePrefix,
            [FromQuery(Name = "double_before")] long? maxDouble)
        {
            using (IStaffQuery query = _staffProvider.NewQuery())
            {
                IEnumerable<IdentifiedStaffMember> staffMembers = query.StaffMembers.ToList();

                // Staff need their ID requested
                if (ids.Length != 0)
                {
                    staffMembers = staffMembers.Where(sm => ids.Contains(sm.StaffId));
                }

                // Staff need their RosterOn ID requested
                if (rosterOnIds.Length != 0)
                {
                    staffMembers = staffMembers.Where(sm => sm.StaffMember.RosterOnId.HasValue &&
                        rosterOnIds.Contains(sm.StaffMember.RosterOnId.Value));
                }

                // Staff need to have all the skills in the request
                if (skills.Length != 0)
                {
                    skills = skills.Select(s => s.Trim()).ToArray();
                    staffMembers = staffMembers.Where(sm => skills
                        .All(s => sm.StaffMember.Skills.Contains(s, StringComparer.OrdinalIgnoreCase)));
                }

                // Staff need to be of a designation that was requested
                if (staffTypes.Length != 0)
                {
                    staffTypes = staffTypes.Select(s => s.Trim()).ToArray();
                    staffMembers = staffMembers.Where(sm => staffTypes.Contains(sm.StaffMember.Designation,
                        StringComparer.OrdinalIgnoreCase));
                }

                // Staff need to have a name with the prefix requested
                if (!string.IsNullOrWhiteSpace(namePrefix))
                {
                    namePrefix = namePrefix.TrimStart();
                    staffMembers = staffMembers.Where(sm => sm.StaffMember.FirstName.StartsWith(namePrefix,
                        StringComparison.OrdinalIgnoreCase));
                }

                // Staff must have not had a double allocation since the date requested
                if (maxDouble != null)
                {
                    staffMembers = staffMembers.Where(sm =>
                        sm.StaffMember.LastDouble == null || sm.StaffMember.LastDouble.Value <= maxDouble);
                }
                return Json(new StaffListResponse(staffMembers.ToList()));
            }
        }

        /// <summary>
        /// Get a single staff member by their storage ID
        /// </summary>
        /// <param name="id">the ID of the staff member to retrieve</param>
        /// <returns></returns>
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            StaffMember staffMember = _staffProvider.Get(
                id,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds() - RecentDoublePeriodSeconds
            );
            if (staffMember != null)
            {
                return Json(new StaffResponse(staffMember));
            }

            return BadRequest(Json(new ErrorResponse("No staff member known with ID: " + id.ToString())));
        }

        /// <summary>
        /// Create a new staff member and return a message containing its allocated ID
        /// </summary>
        /// <param name="staffMember">the staff member to store</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] StaffMember staffMember)
        {
            if (staffMember == null)
            {
                return BadRequest(Json(new ErrorResponse("Bad request body")));
            }

            int staffId;
            try
            {
                staffId = _staffProvider.Create(staffMember);
            }
            catch (ArgumentException e)
            {
                return BadRequest(Json(new ErrorResponse(e.Message)));
            }
            return Json(new StaffIdResponse(1));
        }

        /// <summary>
        /// Update an existing staff member with a given ID with the new values in the body
        /// </summary>
        /// <param name="id">the ID of the staff member to update</param>
        /// <param name="staffMember">the new values to update the staff member with</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateExisting(int id, [FromBody] StaffMember staffMember)
        {
            if (staffMember == null)
            {
                return BadRequest(Json(new ErrorResponse("Bad request body")));
            }

            StaffMember newStaff;
            try
            {
                newStaff = _staffProvider.Update(id, staffMember);
            }
            catch (ArgumentException e)
            {
                return BadRequest(Json(new ErrorResponse(e.Message)));
            }

            return Json(new StaffResponse(newStaff));
        }

        /// <summary>
        /// Delete the staff member with the given ID from storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_staffProvider.Delete(id))
            {
                return Json(new StaffIdResponse(id));
            }
            return BadRequest(Json(new ErrorResponse("No staff member known with ID: " + id.ToString())));
        }
    }
}
