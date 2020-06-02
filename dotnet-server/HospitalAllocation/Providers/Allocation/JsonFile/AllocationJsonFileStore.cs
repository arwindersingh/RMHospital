using System;
using System.IO;
using Newtonsoft.Json;
using HospitalAllocation.Data.Allocation;
using HospitalAllocation.Data.Allocation.StaffGroups;
using HospitalAllocation.Providers.Allocation.Interface;
using HospitalAllocation.Providers.Allocation.Memory;

namespace HospitalAllocation.Providers.Allocation.JsonFile
{
    /// <summary>
    /// Stores allocations by writing and reading a JSON file in the file system
    /// </summary>
    public class AllocationJsonFileStore : IAllocationStore
    {
        /// <summary>
        /// Create a JSON allocation store around a given file
        /// </summary>
        /// <returns>A new AllocationJsonFileStore using the given file as a persistence layer</returns>
        /// <param name="fileName">The path where the JSON file storing the allocation will be</param>
        public static AllocationJsonFileStore CreateFromFile(string fileName)
        {
            // If the file does not exist, we start with an empty allocation and don't read anything in
            if (!File.Exists(fileName))
            {
                return new AllocationJsonFileStore(new FileInfo(fileName), IcuAllocation.Empty);
            }

            // Read in the file, create an allocation from it, and use that to initialise the JSON store object
            using (StreamReader file = File.OpenText(fileName))
            {
                JsonSerializer serialiser = new JsonSerializer();
                IcuAllocation allocation = (IcuAllocation)serialiser.Deserialize(file, typeof(IcuAllocation));
                Console.Error.WriteLine("Got allocation: " + allocation);
                return new AllocationJsonFileStore(new FileInfo(fileName), allocation);
            }
        }

        // The file backing the JSON store
        private readonly FileInfo _jsonFile;

        // The memory store that stores the in-memory representation of the allocation data
        private readonly AllocationMemoryStore _allocation;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.JsonFile.AllocationJsonFileStore"/> class.
        /// </summary>
        /// <param name="file">The file backing the JSON store</param>
        /// <param name="allocation">The initial allocation data for the file store</param>
        private AllocationJsonFileStore(FileInfo file, IcuAllocation allocation)
        {
            _jsonFile = file;
            _allocation = new AllocationMemoryStore();
            AllocationProvider.SetPodAllocation(_allocation.PodA, allocation.PodA);
            AllocationProvider.SetPodAllocation(_allocation.PodB, allocation.PodB);
            AllocationProvider.SetPodAllocation(_allocation.PodC, allocation.PodC);
            AllocationProvider.SetPodAllocation(_allocation.PodD, allocation.PodD);
            AllocationProvider.SetSeniorTeamAllocation(_allocation.SeniorTeam, allocation.SeniorTeam);
        }

        /// <summary>
        /// Commit the allocation to the JSON file by writing it
        /// </summary>
        /// <param name="team">The team to commit.</param>
        /// <remarks>The team parameter is ignored and all teams are committed.</remarks>
        public void Commit(TeamType team)
        {
            using (StreamWriter file = new StreamWriter(_jsonFile.OpenWrite()))
            {
                JsonSerializer serialiser = new JsonSerializer()
                {
                    // Format the JSON so it can be read by a human
                    Formatting = Formatting.Indented
                };
                serialiser.Serialize(file, _allocation.Allocation);
                file.Flush();
            }
        }

        /// <summary>
        /// Interface to Pod A
        /// </summary>
        /// <value>The pod a.</value>
        public IPodStore PodA => _allocation.PodA;

        /// <summary>
        /// Interface to Pod B
        /// </summary>
        /// <value>The pod b.</value>
        public IPodStore PodB => _allocation.PodB;

        /// <summary>
        /// Interface to Pod C
        /// </summary>
        /// <value>The pod c.</value>
        public IPodStore PodC => _allocation.PodC;

        /// <summary>
        /// Interface to Pod D
        /// </summary>
        /// <value>The pod d.</value>
        public IPodStore PodD => _allocation.PodD;

        /// <summary>
        /// Interface to the Senior Team
        /// </summary>
        /// <value>The senior team.</value>
        public ISeniorTeamStore SeniorTeam => _allocation.SeniorTeam;
    }
}
