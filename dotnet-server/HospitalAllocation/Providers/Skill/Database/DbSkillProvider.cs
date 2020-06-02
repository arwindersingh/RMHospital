using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using HospitalAllocation.Model;
using HospitalAllocation.Providers.Skill.Interface;
using HospitalAllocation.Data.Skill;

namespace HospitalAllocation.Providers.Skill.Database
{
    /// <summary>
    /// Skill Provider which is backed up by a database
    /// </summary>
    public class DbSkillProvider : ISkillProvider
    {
        /// <summary>
        /// The database context option used to construct a database connection
        /// </summary>
        private readonly DbContextOptions<AllocationContext> _dbContextOptions;

        /// <summary>
        /// Construct the provider object with the supplied context options
        /// </summary>
        /// <param name="dbContextOptions">The context to init database connection with</param>
        public DbSkillProvider(DbContextOptions<AllocationContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        /// <summary>
        /// Creates a new skill
        /// </summary>
        /// <param name="skill">New skill to store in the database</param>
        /// <returns>The database identifier key of newly created skill</returns>
        public int Create(KnownSkill newSkill)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Skill skill = dbContext.Skills.FirstOrDefault(
                    s => String.Equals(s.Name, newSkill.Name, StringComparison.OrdinalIgnoreCase));

                if (skill == null) // No existing skill was found
                {
                    skill = new Model.Skill() { Name = newSkill.Name };
                    dbContext.Skills.Add(skill);
                    dbContext.SaveChanges();
                }
                return skill.SkillId;
            }
        }

        /// <summary>
        /// Lists the entire collection of skills
        /// </summary>
        /// <returns>Immutable list of all the skills in the database</returns>
        public ImmutableList<KnownSkill> List()
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                List<Model.Skill> skills = dbContext.Skills.Select(x => x).ToList();
                // Map the Model list to immutable data list
                return skills.Select(x => new KnownSkill(x.Name, x.SkillId)).ToImmutableList();
            }

        }

        /// <summary>
        /// Get a skill matching the Id
        /// </summary>
        /// <param name="skillId">The id with which to query the database</param>
        /// <returns>The matching skill</returns>
        public KnownSkill Get(int id)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Skill skill = dbContext.Skills.Find(id);
                if (skill == null)
                {
                    return null;
                }
                return new KnownSkill(skill.Name, skill.SkillId);
            }

        }

        /// <summary>
        /// Update an existing skill
        /// </summary>
        /// <param name="skillId">The id of the existing skill</param>
        /// <param name="skill">The new skill to update to</param>
        /// <returns>Success status</returns>
        public bool Update(int skillId, KnownSkill skill)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Skill dbSkill = dbContext.Skills.Find(skillId);
                if (dbSkill == null)
                {
                    return false;
                }
                dbSkill.Name = skill.Name;
                dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Deletes a skill if not associated with any staff
        /// </summary>
        /// <param name="skillId">The id of the skill to delete</param>
        /// <returns>Success status</returns>
        public bool Delete(int skillId)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                Model.Skill skill = dbContext.Skills
                    .Include(s => s.StaffSkills)
                    .SingleOrDefault(s => s.SkillId == skillId);

                // Only delete a skill that exists and is not associated with other staff
                // If skill == null , cannot delete a non-existant skill
                // if skill.staffSkills != null, delete the skill only if StaffSkills does not have any elements
                // If staffskills is null, then delete the skill
                if (skill == null || (skill.StaffSkills != null && skill.StaffSkills.Any()))
                {
                    return false;
                }
                dbContext.Skills.Remove(skill);
                dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Check if a skill exists
        /// </summary>
        /// <param name="name">The given name</param>
        /// <returns>If it exists</returns>
        public bool Exists(string name)
        {
            using (var dbContext = new AllocationContext(_dbContextOptions))
            {
                // If there is any existing desigation in the database that has the same name.
                return dbContext.Skills.Any(s => String.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
