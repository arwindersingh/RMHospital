using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using HospitalAllocation.Model;
//using SkiaSharp;
using HospitalAllocation.Providers.CSVFile.Interface;

namespace HospitalAllocation.Providers.CSVFile.Database
{
    /// <summary>
    /// Provides a csv file store interface over the hospital allocation relational database
    /// </summary>
    public class DbCSVFileProvider : ICSVFileProvider
    {
        /// <summary>
        /// Option to build a database connection
        /// </summary>
        private readonly DbContextOptions<AllocationContext> _dbOptions;

        /// <summary>
        /// Make a new database csvfile provider from a DbContextOptions object
        /// </summary>
        /// <param name="dbOptions"></param>
        public DbCSVFileProvider(DbContextOptions<AllocationContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        /// <summary>
        /// List the TimeStamps of all the stored csv files in the database
        /// </summary>
       /* public IReadOnlyList<long> StoredCSVFiles
        {
            get
            {
                using (var dbContext = new AllocationContext(_dbOptions))
                {
                    return dbContext.CSVFileStorage.Select(c => c.TimeStamp).ToList();
                }
            }
        }*/

        /// <summary>
        /// Store a csv file in the database
        /// </summary>
        /// <param name="csvfileData"> the csvfile data stream to store csvfile data from</param>
        /// <returns>the database TimeStamp of the newly stored file</returns>
        public CSVFileStorage CreateCSVFile(ICSVFileStream csvfileData)
        {
            //SKData cv = EncodeFile(csvfileData);
            if (csvfileData == null)
            {
                throw new ArgumentException("Invalid file or unsupported format");
            }

            //byte[] fileBytes = new byte[csvfileData.Length];
            //FileStream filestream = csvfileData as FileStream;
            //filestream.Read(fileBytes, 0, fileBytes.Length);
            //csvfileData.

            byte[] fileBytes = new byte[csvfileData.Length];

            if (csvfileData.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    csvfileData.File.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }

            var csvfilestorage = new CSVFileStorage() { CSVFileData = fileBytes, CSVTimeStamp = csvfileData.TimeStamp, CSVFileFormat = csvfileData.Format };

            using (var dbContext = new AllocationContext(_dbOptions))
            {
                dbContext.CSVFileStorages.Add(csvfilestorage);
                dbContext.SaveChanges();

                return csvfilestorage;
                
                //return csvfileData.TimeStamp;
            }
        }

        public ICSVFileStream GetCSVFile(DateTime timestamp)
        {
            using (var dbContext = new AllocationContext(_dbOptions))
            {
                CSVFileStorage csvfilestorage = dbContext.CSVFileStorages.Single(c => c.CSVTimeStamp == timestamp);
                if (csvfilestorage == null)
                {
                    return null;
                }
                
                return new MemoryCSVFileStream(csvfilestorage.CSVFileData, csvfilestorage.CSVTimeStamp, csvfilestorage.CSVFileFormat);
            }
        }

        public ICSVFileStream GetMostRecentCSVFile()
        {
            using (var dbContext = new AllocationContext(_dbOptions))
            {
                CSVFileStorage csvfilestorage = dbContext.CSVFileStorages.OrderBy(c => c.CSVTimeStamp).Last();
                if (csvfilestorage == null)
                {
                    return null;
                }

                return new MemoryCSVFileStream(csvfilestorage.CSVFileData, csvfilestorage.CSVTimeStamp, csvfilestorage.CSVFileFormat);
            }
        }

        public ICSVFileStream GetCSVFile(int id)
        {
            using (var dbContext = new AllocationContext(_dbOptions))
            {
                try
                {
                    CSVFileStorage csvfilestorage = dbContext.CSVFileStorages.Single(c => c.CSVFileID == id);
                    return new MemoryCSVFileStream(csvfilestorage.CSVFileData, csvfilestorage.CSVTimeStamp, csvfilestorage.CSVFileFormat);
                }
                catch
                {
                    return null;
                }

            }
        }

      
    }
}

