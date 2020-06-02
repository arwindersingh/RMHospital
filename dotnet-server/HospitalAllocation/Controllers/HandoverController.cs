using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalAllocation.Data.Handover;
using HospitalAllocation.Messages.Responses;
using HospitalAllocation.Messages.Responses.Handover;
using HospitalAllocation.Providers.Handover.Inteface;
using Microsoft.AspNetCore.Mvc;


namespace HospitalAllocation.Controllers
{
    /// <summary>
    /// Handover controller
    /// 
    /// An API for operating handover notes
    ///  
    /// Controller Follows the HTTP1.1 Protocal
    /// 
    /// https://www.w3.org/Protocols/rfc2616/rfc2616.html
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]")]
    public class HandoverController : Controller
    {
        
        // The underlying provider for staff data storage
        private readonly IHandoverProvider _handoverProvider;

        /// <summary>
        /// Construct a new handover controller around a given handover provider
        /// </summary>
        /// <param name="handoverProvider"></param>
        public HandoverController(IHandoverProvider handoverProvider)
        {
            _handoverProvider = handoverProvider;
        }

        /// <summary>
        /// Create the specified handover.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="handover">Handover.</param>
        [HttpPost]
        public IActionResult Create([FromBody] HandoverDTO handover)
        {
            if (handover == null){
                return BadRequest(Json(new ErrorResponse("Form Error")));
            }
            int handoverId;
            handoverId = _handoverProvider.Create(handover);
            /// 201 for successful create
            return CreatedAtAction(handoverId.ToString(),new {id=handoverId});
        }

        /// <summary>
        /// Get the specified id.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="id">Identifier.</param>
        [HttpGet("{id}",Name = "GetById")]
        public IActionResult Get(int id)
        {
            try
            {
                HandoverDTO handover = _handoverProvider.GetHandover(id);
                if (handover != null)
                {
                    return Json(new HandoverResponse(handover));
                }
            }
            catch(Exception)
            {
                return NotFound();
            }
            return NotFound();
        }

        /// <summary>
        /// Update the specified id and handover.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="handover">Handover.</param>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]HandoverDTO handover)
        {
            if (handover == null)
            {
                return BadRequest(Json(new ErrorResponse("Form Error")));
            }
            bool update_result = _handoverProvider.Update(id,handover);
            if(update_result == false){
                return NotFound();
            }
            /// 204 for successful update
            return NoContent();
        }

        /// <summary>
        /// Delete the specified id.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="id">Identifier.</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool delete = _handoverProvider.Delete(id);
            if (delete == false)
            {
                return NotFound();
            }
            /// 204 for successful delete
            return NoContent();
        }
    }
}
