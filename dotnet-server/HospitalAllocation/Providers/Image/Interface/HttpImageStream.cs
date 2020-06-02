using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace HospitalAllocation.Providers.Image.Interface
{
    /// <summary>
    /// Decsribes an image streamed over HTTP
    /// </summary>
    public class HttpImageStream : IImageStream
    {
        // Lookup table to translate HTTP content type headers to image formats
        private static readonly IDictionary<string, ImageFormat> s_contentTypeFormats;

        /// <summary>
        /// Initializes static variables used by the
        /// <see cref="T:HospitalAllocation.Providers.Image.Interface.HttpImageStream"/> class.
        /// </summary>
        static HttpImageStream()
        {
            s_contentTypeFormats = new Dictionary<string, ImageFormat>()
            {
                {"image/bmp", ImageFormat.Bmp},
                {"image/jpeg", ImageFormat.Jpeg},
                {"image/pic", ImageFormat.Pic},
                {"image/png", ImageFormat.Png},
                {"image/tiff", ImageFormat.Tiff},
            };
        }

        /// <summary>
        /// Create an HTTP image stream from an HTTP IFormFile object. If the content
        /// type is not recognised as compatible, returns null
        /// </summary>
        /// <returns>The from IF orm file.</returns>
        /// <param name="formFile">Form file.</param>
        public static HttpImageStream CreateFromIFormFile(IFormFile formFile)
        {
            // If the IFormFile does not have the right content type, return null
            if (!s_contentTypeFormats.TryGetValue(formFile.ContentType, out ImageFormat format))
            {
                return null;
            }

            return new HttpImageStream(formFile, format);
        }

        // The underlying HTTP image stream
        private readonly IFormFile _httpFileStream;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Image.Interface.HttpImageStream"/> class.
        /// </summary>
        /// <param name="formFile">Form file.</param>
        /// <param name="format">Format.</param>
        private HttpImageStream(IFormFile formFile, ImageFormat format)
        {
            _httpFileStream = formFile;
            Format = format;
        }

        /// <summary>
        /// Get the format of the image in this stream
        /// </summary>
        /// <value>The format.</value>
        public ImageFormat Format { get; }

        /// <summary>
        /// Get the size of this image in bytes
        /// </summary>
        /// <value>The length.</value>
        public long Length { get => _httpFileStream.Length; }

        /// <summary>
        /// Copy the image in this stream to the given stream
        /// </summary>
        /// <param name="stream">Stream.</param>
        public void CopyTo(Stream stream)
        {
            _httpFileStream.CopyTo(stream);
        }
    }
}
