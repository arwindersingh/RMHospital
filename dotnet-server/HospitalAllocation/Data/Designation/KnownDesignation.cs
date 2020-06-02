using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using HospitalAllocation.Data.StaffMember;
using Newtonsoft.Json;

namespace HospitalAllocation.Data.Designation
{
    [DataContract]
    public class KnownDesignation
    {
        /// <summary>
        /// The name of the designation
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; }

        /// <summary>
        /// The database id of the designation
        /// </summary>
        [DataMember(Name = "designation_id")]
        public int DesignationId { get; }

        /// <summary>
        /// Initialize a designation with given name
        /// </summary>
        /// <param name="name">The given designation name</param>
        [JsonConstructor]
        public KnownDesignation(String name)
        {
            Name = name;
        }

        /// <summary>
        /// Initialize a designation with given name and designation id
        /// </summary>
        /// <param name="id">The given designation id</param>
        /// <param name="name">The given designation name</param>
        public KnownDesignation(int id, String name)
        {
            DesignationId = id;
            Name = name;
        }
    }
}