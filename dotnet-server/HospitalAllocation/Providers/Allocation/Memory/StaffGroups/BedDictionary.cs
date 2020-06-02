using System;
using System.Collections;
using System.Collections.Generic;
using HospitalAllocation.Data.Allocation.Positions;

namespace HospitalAllocation.Providers.Allocation.Memory.StaffGroups
{
    /// <summary>
    /// Keeps a list of beds indexed by bed number. Does not allow null entries.
    /// </summary>
    public class BedDictionary : IDictionary<int, Position>
    {
        // The internal underlying store of beds
        private readonly Dictionary<int, Position> _bedDict;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Memory.StaffGroups.BedDictionary"/> class.
        /// </summary>
        /// <param name="capacity">Capacity.</param>
        public BedDictionary(int capacity)
        {
            if (capacity < 1)
            {
                throw new InvalidOperationException("Cannot create a bed dict with non-positive capacity");
            }

            // Load the store with empty beds up to its capacity
            _bedDict = new Dictionary<int, Position>(capacity);
            for (int i = 1; i <= capacity; i++)
            {
                _bedDict.Add(i, UnidentifiedPosition.Empty);
            }

            Capacity = capacity;
        }

        /// <summary>
        /// The number of beds in this store
        /// </summary>
        /// <value>The capacity.</value>
        public int Capacity { get; }

        /// <summary>
        /// Indexes the beds in this store by bed number
        /// </summary>
        /// <param name="key">Key.</param>
        public Position this[int key]
        {
            get => _bedDict[key];

            set
            {
                if (!_bedDict.ContainsKey(key))
                {
                    return;
                }

                _bedDict[key] = value ?? throw new InvalidOperationException("Cannot add a null bed to this store");
            }
        }

        /// <summary>
        /// Return a collection of all the bed numbers in this bed store
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<int> Keys => _bedDict.Keys;

        /// <summary>
        /// Return a collection of all the beds in this store
        /// </summary>
        /// <value>The values.</value>
        public ICollection<Position> Values => _bedDict.Values;

        /// <summary>
        /// Return the number of beds in the store
        /// </summary>
        /// <value>The count.</value>
        public int Count => _bedDict.Count;

        /// <summary>
        /// Gets a value indicating whether this
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Memory.StaffGroups.BedDictionary"/> is read only.
        /// </summary>
        /// <value><c>true</c> if is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly => ((IDictionary<int, Position>)_bedDict).IsReadOnly;

        /// <summary>
        /// Add a bed into the store with the given bed number
        /// </summary>
        /// <returns>The add.</returns>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Add(int key, Position value)
        {
            this[key] = value;
        }

        /// <summary>
        /// Add a bed number/bed key/value pair into this bed store
        /// </summary>
        /// <returns>The add.</returns>
        /// <param name="item">Item.</param>
        public void Add(KeyValuePair<int, Position> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Reset all the beds in this instance to empty
        /// </summary>
        public void Clear()
        {
            _bedDict.Clear();

            for (int i = 1; i <= Capacity; i++)
            {
                _bedDict.Add(i, UnidentifiedPosition.Empty);
            }
        }

        /// <summary>
        /// Checks if the given bed/bed-number pair are in the bed store
        /// </summary>
        /// <returns>The contains.</returns>
        /// <param name="item">Item.</param>
        public bool Contains(KeyValuePair<int, Position> item)
        {
            return ((IDictionary<int, Position>)_bedDict).Contains(item);
        }

        /// <summary>
        /// Checks if the given bed number is in this store
        /// </summary>
        /// <returns><c>true</c>, if key was containsed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public bool ContainsKey(int key)
        {
            return _bedDict.ContainsKey(key);
        }

        /// <summary>
        /// Copies the bed-number/bed pairs into the given array
        /// </summary>
        /// <param name="array">Array.</param>
        /// <param name="arrayIndex">Array index.</param>
        public void CopyTo(KeyValuePair<int, Position>[] array, int arrayIndex)
        {
            ((IDictionary<int, Position>)_bedDict).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator over the key/value pairs in this store
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<KeyValuePair<int, Position>> GetEnumerator()
        {
            return _bedDict.GetEnumerator();
        }

        /// <summary>
        /// Resets the bed with the given bed number to empty, if it exists
        /// in this store
        /// </summary>
        /// <returns>The remove.</returns>
        /// <param name="key">Key.</param>
        public bool Remove(int key)
        {
            if (ContainsKey(key) && !_bedDict[key].IsEmpty())
            {
                _bedDict[key] = UnidentifiedPosition.Empty;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets the given bed-number/bed pair to empty if it exists in this store
        /// </summary>
        /// <returns>The remove.</returns>
        /// <param name="item">Item.</param>
        public bool Remove(KeyValuePair<int, Position> item)
        {
            if (Contains(item) && !item.Value.IsEmpty())
            {
                _bedDict[item.Key] = UnidentifiedPosition.Empty;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the out bed value to the requested value if that bed is in
        /// this store
        /// </summary>
        /// <returns><c>true</c>, if get value was present, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public bool TryGetValue(int key, out Position value)
        {
            return _bedDict.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns an enumerator on this bed store
        /// </summary>
        /// <returns>The collections. IE numerable. get enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
