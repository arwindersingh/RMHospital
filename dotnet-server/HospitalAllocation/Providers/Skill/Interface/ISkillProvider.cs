using System;
using System.Collections.Immutable;
using System.Linq;
using HospitalAllocation.Data.Skill;

namespace HospitalAllocation.Providers.Skill.Interface
{
    /// <summary>
    /// Interface which defines the set of functions that any skill provider class must implement
    /// </summary>
    public interface ISkillProvider
    {
        /// <summary>
        /// Creates a new skill
        /// </summary>
        /// <param name="skill">New skill to store in the database</param>
        /// <returns>The database identifier key of newly created skill</returns>
        int Create(KnownSkill skill);

        /// <summary>
        /// Lists the entire collection of skills
        /// </summary>
        /// <returns>Immutable list of all the skills in the database</returns>
        ImmutableList<KnownSkill> List();

        /// <summary>
        /// Get a skill matching the Id
        /// </summary>
        /// <param name="skillId">The id with which to query the databse</param>
        /// <returns>The matching skill</returns>
        KnownSkill Get(int skillId);

        /// <summary>
        /// Update an existing skill
        /// </summary>
        /// <param name="skillId">The id of existing skill</param>
        /// <param name="skill">The new skill to update to</param>
        /// <returns>Success status</returns>
        bool Update(int skillId, KnownSkill skill);

        /// <summary>
        /// Deletes a skill if not associated with any staff
        /// </summary>
        /// <param name="skillId">The id of skill to delete</param>
        /// <returns>Success status</returns>
        bool Delete(int skillId);

        /// <summary>
        /// Check if a skill exists
        /// </summary>
        /// <param name="name">The given name</param>
        /// <returns>If it exists</returns>
        bool Exists(string name);
    }
}
