using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HospitalAllocation.Providers.Image.Interface;

namespace HospitalAllocation.Providers.Image.Database
{
    /// <summary>
    /// Streams an image from a byte array to another stream
    /// </summary>
    public class MemoryImageStream : IImageStream
    {
        // The underlying image data
        private readonly byte[] _imageData;

        /// <summary>
        /// Construct a new memory image stream from a byte array and an image format tag.
        /// Trusts that the byte array will not be invalidated while this object has it
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="format"></param>
        public MemoryImageStream(byte[] imageData, ImageFormat format)
        {
            _imageData = imageData;
            Format = format;
        }

        /// <summary>
        /// The format of the underlying image byte data
        /// </summary>
        public ImageFormat Format { get; }

        /// <summary>
        /// The number of bytes available to stream
        /// </summary>
        public long Length => _imageData.Length;

        /// <summary>
        /// Copy the data in this stream to another stream
        /// </summary>
        /// <param name="stream">the copy destination stream</param>
        public void CopyTo(Stream stream)
        {
            var byteStream = new MemoryStream(_imageData);
            byteStream.CopyTo(stream);
        }
    }
}
