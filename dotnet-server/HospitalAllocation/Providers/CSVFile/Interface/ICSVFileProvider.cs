using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using HospitalAllocation.Model;

namespace HospitalAllocation.Providers.CSVFile.Interface
{
    /// <summary>
    /// Records the different supported file formats
    /// </summary>
    public enum CSVFileFormat
    {
        /// <summary>
        /// A CSV file
        /// </summary>
        csv,
    }

    /// <summary>
    /// Provides extension methods to the CSVFileFormat enum
    /// </summary>
    public static class CSVFileFormatExtension
    {
        // Provides a lookup table from CSVFile format enum to HTTP content types
        private static readonly IDictionary<CSVFileFormat, string> s_formatContentTypes;

        /// <summary>
        /// Populates static data structures for the CSVFileFormatExtension class
        /// </summary>
        static CSVFileFormatExtension()
        {
            s_formatContentTypes = new Dictionary<CSVFileFormat, string>
            {
                { CSVFileFormat.csv, "csvfile/csv" },
            };
        }
        
        /// <summary>
        /// Gets the file extension associated with the given CSVFile format
        /// </summary>
        /// <returns>The extension.</returns>
        /// <param name="csvfileformat">CSVFile format.</param>
        public static string FileExtension(this CSVFileFormat csvfileFormat)
        {
            return "." + csvfileFormat.ToString().ToLower();
        }

        /// <summary>
        /// Gets the HTTP content type header associated with the given CSVFile format
        /// </summary>
        /// <returns>The type string</returns>
        /// <param name="csvfileFormat">CSVFile Format</param>
        public static string ContentTypeString(this CSVFileFormat csvfileFormat)
        {
            if(!s_formatContentTypes.TryGetValue(csvfileFormat, out string contentType))
            {
                return null;
            }

            return contentType;
        }
    }

    /// <summary>
    /// Describes a resource that provides csvfile data storage
    /// </summary>
    public interface ICSVFileProvider
    {
        /// <summary>
        /// Get the csvfile with the given timestamp
        /// </summary>
        /// <param name="timestamp">Identifier.</param>
        /// <returns>The csvfile</returns>
        ICSVFileStream GetCSVFile(DateTime timestamp);

        /// <summary>
        /// Get the csvfile with the given id
        /// </summary>
        /// <param name="id">CSV File id.</param>
        /// <returns>The csvfile</returns>
        ICSVFileStream GetCSVFile(int id);

        /// <summary>
        /// Get the most recent csv file
        /// </summary>
        /// <returns>The csvfile</returns>
        ICSVFileStream GetMostRecentCSVFile();

        /// <summary>
        /// Store a given csv file in the provider and return its id
        /// </summary>
        /// <param name="csvfileData">CSVFile data.</param>
        /// <returns>The csvfile</returns>
        CSVFileStorage CreateCSVFile(ICSVFileStream csvfileData);

        //void DeleteCSVFile(long timestamp);
    }
}