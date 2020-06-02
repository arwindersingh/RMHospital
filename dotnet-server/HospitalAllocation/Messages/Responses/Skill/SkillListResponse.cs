using System.Runtime.Serialization;
using System.Collections.Immutable;
using HospitalAllocation.Data.Skill;

namespace HospitalAllocation.Messages.Responses.Skill
{
    /// <summary>
    /// A response message containing a list of skills
    /// </summary>
    [DataContract]
    public class SkillListResponse : ApiResponse
    {
        /// <summary>
        /// Construct an object with the provided list of skills
        /// </summary>
        /// <param name="skills">The list of skills to init the class with.</param>
        public SkillListResponse(ImmutableList<KnownSkill> skills) : base(ResponseStatus.Success) => Skills = skills;

        /// <summary>
        /// List containing the list of skills to construct the message
        /// </summary>
        [DataMember(Name = "skill_list")]
        public ImmutableList<KnownSkill> Skills { get; }
    }
}
