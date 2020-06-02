using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HospitalAllocation.Providers.CSVFile.Interface;


namespace HospitalAllocation.Model
{
    /// <summary>
    /// This file will store the CSV File into the database
    /// </summary>
    public class CSVFileStorage
    {
        [Key]
        public int CSVFileID {get; set;}
        /// <summary>
        /// This will store the timestamp of file when it is uploaded
        /// </summary>
        [Required]
        public DateTime CSVTimeStamp { get; set; }

        /// <summary>
        /// This will store the name of the file.
        /// </summary>
      //  [Required]
     //   public string CSVFileName { get; set; }

        /// <summary>
        /// This will store the data of the file
        /// </summary>
        [Required]
        public byte[] CSVFileData { get; set; }

        [Required]
        public CSVFileFormat CSVFileFormat {get; set;}
        

    }

}

