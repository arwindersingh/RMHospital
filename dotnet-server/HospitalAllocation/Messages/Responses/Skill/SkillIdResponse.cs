using System.Runtime.Serialization;

namespace HospitalAllocation.Messages.Responses.Skill
{
    /// <summary>
    /// A response message with an id of the chosen skill
    /// </summary>
    [DataContract]
    public class SkillIdResponse : ApiResponse
    {
        /// <summary>
        /// Constructs an object with the supplied skill id
        /// </summary>
        /// <param name="skillId">Skill Id to initialize the class with</param>
        public SkillIdResponse(int skillId) : base(ResponseStatus.Success) => SkillId = skillId;

        /// <summary>
        /// The integer id which represents the skill
        /// </summary>
        [DataMember(Name = "skill_id")]
        public int SkillId { get; }
    }
}
