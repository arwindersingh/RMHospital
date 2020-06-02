using System;
using System.IO;

namespace HospitalAllocation.Providers.Image.Interface
{
    /// <summary>
    /// Describes a data source for an image
    /// </summary>
    public interface IImageStream
    {
        /// <summary>
        /// The data format of the image
        /// </summary>
        /// <value>The format.</value>
        ImageFormat Format { get; }

        /// <summary>
        /// The size of image in bytes
        /// </summary>
        /// <value>The length.</value>
        long Length { get; }

        /// <summary>
        /// Copy the image data to the given data stream
        /// </summary>
        /// <param name="stream">Stream.</param>
        void CopyTo(Stream stream);
    }
}
