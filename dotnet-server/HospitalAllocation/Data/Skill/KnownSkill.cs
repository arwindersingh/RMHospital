using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HospitalAllocation.Data.Skill
{
    /// <summary>
    /// Describes a skill as a data passing object
    /// </summary>
    [DataContract]
    public class KnownSkill
    {
        /// <summary>
        /// Initialize a skill with given name
        /// </summary>
        /// <param name="name">Name of the skill</param>
        [JsonConstructor]
        public KnownSkill(String name)
        {
            Name = name;
        }

        /// <summary>
        /// Initialize a skill with name and skillId
        /// </summary>
        /// <param name="name">Name of the skill</param>
        /// <param name="skillId">The Id of the skill</param>
        public KnownSkill(String name, int skillId)
        {
            Name = name;
            SkillId = skillId;
        }

        /// <summary>
        /// The name of the skill
        /// </summary>
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; }

        /// <summary>
        /// The database id of the skill
        /// </summary>
        [DataMember(Name = "skill_id")]
        public int? SkillId { get; }
    }
}
