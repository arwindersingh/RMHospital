using System.Runtime.Serialization;
using System.Collections.Generic;
using HospitalAllocation.Data.Skill;

namespace HospitalAllocation.Messages.Responses.Skill
{
    /// <summary>
    /// Message containing the details of a single skill
    /// </summary>
    [DataContract]
    public class SkillResponse : ApiResponse
    {
        /// <summary>
        /// Constructs a Message with the supplied skill
        /// </summary>
        /// <param name="skill">Skill to initialize the class with</param>
        public SkillResponse(KnownSkill skill) : base(ResponseStatus.Success) => Skill = skill;

        /// <summary>
        /// The value of a skill used to construct the message
        /// </summary>
        [DataMember(Name = "skill")]
        public KnownSkill Skill { get; }
    }
}
