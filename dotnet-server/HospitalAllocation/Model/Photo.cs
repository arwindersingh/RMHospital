using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HospitalAllocation.Providers.Image.Interface;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// Describes an image to be stored in the database in raw byte form
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// The numerical ID of this photo
        /// </summary>
        [Key]
        public int PhotoId { get; set; }

        /// <summary>
        /// The format that the byte blob of the image is encoded in
        /// </summary>
        [Required]
        public ImageFormat Format { get; set; }

        /// <summary>
        /// The raw bytes of the photo
        /// </summary>
        [Required]
        public byte[] Image { get; set; }
    }
}
