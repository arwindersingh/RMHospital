using System;
using System.Collections.Generic;
using System.Linq;
using HospitalAllocation.Data.Allocation.Positions;
using HospitalAllocation.Providers.Allocation.Interface.Positions;

namespace HospitalAllocation.Providers.Allocation.Memory.Positions
{
    /// <summary>
    /// Stores a position allocation in memory
    /// </summary>
    public class PositionMemoryStore : IPositionStore
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Memory.Positions.PositionMemoryStore"/> class.
        /// </summary>
        /// <param name="positionName">The permanent name of this position</param>
        public PositionMemoryStore(string positionName)
        {
            PositionName = positionName;
            _position = UnidentifiedPosition.Empty;
            ShiftType = ShiftType.EightHour;
        }

        /// <summary>
        /// Store a position associated with this PositionMemoryStore
        /// </summary>
        private Position _position;

        /// <summary>
        /// Gets the name of the position.
        /// </summary>
        /// <value>The name of the position.</value>
        public string PositionName { get; }

        /// <summary>
        /// Gets or sets the type of the shift allocated to the staff member at this position
        /// </summary>
        /// <value>The type of the shift.</value>
        public ShiftType ShiftType { get; set; }

        /// <summary>
        /// Gets a snapshot representation of this position as a POCO
        /// </summary>
        /// <value>As position.</value>
        public Position Position
        {
            get => _position;
            // Only update if the new position is valid
            set => _position = value ?? _position;
        }
    }

    /// <summary>
    /// Stores a set of bed allocations by integer bed ID
    /// </summary>
    public class BedSetMemoryStore : IBedSetStore
    {
        // The backing dictionary storing the bed allocations
        private readonly Dictionary<int, IPositionStore> _bedDict;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Memory.Positions.BedSetMemoryStore"/> class.
        /// </summary>
        /// <param name="bedCapacity">The number of beds in this set</param>
        public BedSetMemoryStore(int bedCapacity)
        {
            _bedDict = new Dictionary<int, IPositionStore>();

            for (int i = 1; i <= bedCapacity; i++)
            {
                _bedDict.Add(i, new PositionMemoryStore(i.ToString()));
            }
        }

        /// <summary>
        /// Get an allocation interface to the bed with the indexed ID
        /// </summary>
        /// <param name="bedId">Bed identifier.</param>
        public IPositionStore this[int bedId] => _bedDict[bedId];

        /// <summary>
        /// Get a dictionary value object representing all beds
        /// allocated in this store
        /// </summary>
        /// <value>The beds.</value>
        public IDictionary<int, Position> Beds
        {
            get => _bedDict.ToDictionary(kv => kv.Key, kv => kv.Value.Position);
        }
    }

    /// <summary>
    /// Stores a list of senior staff allocations for a position
    /// </summary>
    public class SeniorListMemoryStore : ISeniorListStore
    {
        // The senior positions allocated here
        private Position[] _positions;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Memory.Positions.SeniorListMemoryStore"/> class.
        /// </summary>
        public SeniorListMemoryStore()
        {
            _positions = new Position[0];
        }

        /// <summary>
        /// Get and set the senior positions for this role with an array
        /// </summary>
        /// <value>The array.</value>
        public Position[] Array
        {
            get => (Position[])_positions.Clone();
            set => _positions = (Position[])value.Clone();
        }
    }
}
