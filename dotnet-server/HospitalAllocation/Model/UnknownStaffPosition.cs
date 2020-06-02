namespace HospitalAllocation.Model
{
    /// <summary>
    /// A position filled by a staff member unknown to the database.
    /// </summary>
    public class UnknownStaffPosition : StaffPosition
    {
        /// <summary>
        /// The name of the staff member.
        /// </summary>
        public string StaffName { get; set; }
    }
}
