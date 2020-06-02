using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HospitalAllocation.Providers.Designation.Interface;
using HospitalAllocation.Data.Designation;
using HospitalAllocation.Messages.Responses.Designation;
using HospitalAllocation.Messages.Responses;

namespace HospitalAllocation.Controllers
{
    /// <summary>
    /// Handles the calls made to the designation API
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]")]
    public class DesignationController : Controller
    {
        /// <summary>
        /// Designation provider object which provides access to the data.
        /// </summary>
        private readonly IDesignationProvider _designationProvider;

        /// <summary>
        /// Initialize an object with the supplied designation provider
        /// </summary>
        /// <param name="designationProvider">the supplied designation provider</param>
        public DesignationController(IDesignationProvider designationProvider)
        {
            _designationProvider = designationProvider;
        }

        /// <summary>
        /// Handle GET calls to API root to return list of designations
        /// </summary>
        /// <returns>All designations</returns>
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new DesignationListResponse(_designationProvider.List()));
        }

        /// <summary>
        /// Handles POST calls to API and creates a new designation
        /// </summary>
        /// <param name="designation">Designation object to create</param>
        /// <returns>The newly created designation</returns>
        [HttpPost]
        public IActionResult Create([FromBody] KnownDesignation designation)
        {
            if (designation == null)
            {
                return Json(BadRequest(new ErrorResponse("Invalid Request Format.")));
            }
            if (designation.Name == null || designation.Name == "")
            {
                return Json(BadRequest(new ErrorResponse("Designation name is required.")));
            }
            return Json(new DesignationIdResponse(_designationProvider.Create(designation)));
        }

        /// <summary>
        /// Handles PUT calls to update the designation specified by the parameters
        /// </summary>
        /// <param name="id">The ID of the designation to update</param>
        /// <param name="designation">The new value to set the existing object to</param>
        /// <returns>The updated designation</returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] KnownDesignation designation)
        {
            if (_designationProvider.Get(id) == null)
            {
                return Json(BadRequest(new ErrorResponse("Designation id doesn't exist")));
            }
            if (designation == null)
            {
                return Json(BadRequest(new ErrorResponse("Invalid Request Format.")));
            }
            if (designation.Name == null || designation.Name == "")
            {
                return Json(BadRequest(new ErrorResponse("Designation name is required.")));
            }
            if (_designationProvider.Exists(designation.Name))
            {
                return Json(BadRequest(new ErrorResponse(String.Format("Designation named {0} already exists.", designation.Name))));
            }
            bool updateSuccessful = _designationProvider.Update(id, designation);
            if (updateSuccessful)
            {
                KnownDesignation knownDesignation = _designationProvider.Get(id);
                if (knownDesignation != null)
                {
                    return Json(new DesignationResponse(knownDesignation));
                }
            }
            return Json(BadRequest(new ErrorResponse("Designation update failed")));
        }

        /// <summary>
        /// Handles DELETE calls to delete a specified designation
        /// </summary>
        /// <param name="id">The ID of designation to delete</param>
        /// <returns>Response status</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_designationProvider.Get(id) == null)
            {
                return Json(BadRequest(new ErrorResponse(String.Format("Designation with {0} does not exist", id))));
            }
            if (_designationProvider.Delete(id))
            {
                return Json(new DesignationIdResponse(id));
            }
            return Json(BadRequest(new ErrorResponse(String.Format("Designation with {0} can not be deleted", id))));
        }
    }
}