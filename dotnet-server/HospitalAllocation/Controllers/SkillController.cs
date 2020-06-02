using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HospitalAllocation.Providers.Skill.Interface;
using HospitalAllocation.Data.Skill;
using HospitalAllocation.Messages.Responses.Skill;
using HospitalAllocation.Messages.Responses;

namespace HospitalAllocation.Controllers
{
    /// <summary>
    /// Handles the calls made to staff API
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]")]
    public class SkillController : Controller
    {
        /// <summary>
        /// Skill provider provides access to the data in the database
        /// </summary>
        private readonly ISkillProvider _skillsProvider;

        /// <summary>
        /// Initialize an object with the supplied skill provider
        /// </summary>
        /// <param name="skillProvider">Skills provider that provides access to the data in the database</param>
        public SkillController(ISkillProvider skillProvider)
        {
            _skillsProvider = skillProvider;
        }

        /// <summary>
        /// Handle GET calls to API root to return list of skills
        /// </summary>
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new SkillListResponse(_skillsProvider.List()));
        }

        /// <summary>
        /// Handles POST calls to API and creates a new skill
        /// </summary>
        /// <param name="skill">Skill object to create</param>
        [HttpPost]
        public IActionResult Create([FromBody] KnownSkill skill)
        {
            if (skill == null)
            {
                return BadRequest(Json(new ErrorResponse("Bad request body")));
            }
            if (String.IsNullOrEmpty(skill.Name))
            {
                return Json(BadRequest(new ErrorResponse("Skill name is required.")));
            }
            if (_skillsProvider.Exists(skill.Name))
            {
                return Json(BadRequest(new ErrorResponse(String.Format("Skill named {0} already exists.", skill.Name))));
            }
            int skillId;
            try
            {
                skillId = _skillsProvider.Create(skill);
            }
            catch (ArgumentException e)
            {
                return BadRequest(Json(new ErrorResponse(e.Message)));
            }

            return Json(new SkillIdResponse(_skillsProvider.Create(skill)));
        }

        /// <summary>
        /// Handles PUT calls to update the skill specified by the parameters
        /// </summary>
        /// <param name="id">The ID of the skill to update</param>
        /// <param name="skill">The new value to set the existing object to</param>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] KnownSkill skill)
        {
            if (skill == null)
            {
                return BadRequest(Json(new ErrorResponse("Bad request body")));
            }
            if (String.IsNullOrEmpty(skill.Name))
            {
                return Json(BadRequest(new ErrorResponse("Skill name is required.")));
            }
            if (_skillsProvider.Exists(skill.Name))
            {
                return Json(BadRequest(new ErrorResponse(String.Format("Skill named {0} already exists.", skill.Name))));
            }
            bool updateSuccessful = _skillsProvider.Update(id, skill);
            if (updateSuccessful)
            {
                KnownSkill knownSkill = _skillsProvider.Get(id);
                if (knownSkill != null)
                {
                    return Json(new SkillResponse(knownSkill));
                }
            }
            return Json(BadRequest(new ErrorResponse("Update failed")));
        }

        /// <summary>
        /// Handles DELETE calls to delete a specified skill
        /// </summary>
        /// <param name="id">The ID of skill to delete</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_skillsProvider.Delete(id))
            {
                return Json(new SkillIdResponse(id));
            }
            return Json(BadRequest(new ErrorResponse($"Can not delete the skill with id {id}")));
        }
    }
}
