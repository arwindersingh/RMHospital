namespace HospitalAllocation.Model
{
    /// <summary>
    /// A bed position within an allocation.
    /// </summary>
    public class BedPosition : Position
    {
        /// <summary>
        /// The position of the bed.
        /// </summary>
        public int Position { get; set; }
    }
}
