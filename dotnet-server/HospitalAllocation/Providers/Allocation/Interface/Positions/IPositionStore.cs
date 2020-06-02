using System;
using System.Collections.Generic;
using HospitalAllocation.Data.Allocation.Positions;

namespace HospitalAllocation.Providers.Allocation.Interface.Positions
{
    /// <summary>
    /// Stores a staff position
    /// </summary>
    public interface IPositionStore
    {
        /// <summary>
        /// The type of shift allocated to this position
        /// </summary>
        /// <value>The type of the shift.</value>
        ShiftType ShiftType { get; set; }

        /// <summary>
        /// Gets a snapshot POCO of this position
        /// </summary>
        /// <value>A snapshot of this position's value</value>
        Position Position { get; set; }
    }

    /// <summary>
    /// Stores an allocated list of senior staff to a senior position
    /// </summary>
    public interface ISeniorListStore
    {
        /// <summary>
        /// The interface to set the list of senior staff at this position
        /// </summary>
        /// <value>The array.</value>
        Position[] Array { get; set; }
    }

    /// <summary>
    /// Stores a set of beds indexed by integral bed ID
    /// </summary>
    public interface IBedSetStore
    {
        /// <summary>
        /// Gets the interface to the indexed bed
        /// </summary>
        /// <param name="bedId">Bed identifier.</param>
        IPositionStore this[int bedId] { get; }

        /// <summary>
        /// Gets a dictionary representation of the current state
        /// of the bed store allocation
        /// </summary>
        /// <value>The beds.</value>
        IDictionary<int, Position> Beds { get; }
    }
}
