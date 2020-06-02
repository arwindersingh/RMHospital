using System;
using System.IO;
using System.Collections.Generic;
using HospitalAllocation.Providers.Image.Interface;

namespace HospitalAllocation.Providers.Image.FileSystem
{
    /// <summary>
    /// Describes an image data stream based on file system storage
    /// </summary>
    public class FileSystemImageStream : IImageStream
    {
        // Lookup table from file extensions to data formats
        private static readonly IDictionary<string, ImageFormat> s_imageFileExtensions;

        /// <summary>
        /// Initializes the static data structures used by the
        /// <see cref="T:HospitalAllocation.Providers.Image.Interface.FileSystemImageStream"/> class.
        /// </summary>
        static FileSystemImageStream()
        {
            s_imageFileExtensions = new Dictionary<string, ImageFormat>()
            {
                {".bmp", ImageFormat.Bmp},
                {".jpeg", ImageFormat.Jpeg},
                {".pic", ImageFormat.Pic},
                {".png", ImageFormat.Png},
                {".tiff", ImageFormat.Tiff},
            };
        }

        /// <summary>
        /// Create a file system image stream from the given file. If the file does
        /// not contain an image, this will return null
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="fileInfo">File info.</param>
        public static FileSystemImageStream FromFile(FileInfo fileInfo)
        {
            // This trusts that a file extension is accurate. We could use more
            // magic here, but it would be a lot of work for a temporary solution
            if (!s_imageFileExtensions.TryGetValue(fileInfo.Extension, out ImageFormat format))
            {
                return null;
            }

            return new FileSystemImageStream(fileInfo, format);
        }

        // The underlying file containing the image
        private readonly FileInfo _fileInfo;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Image.Interface.FileSystemImageStream"/> class.
        /// </summary>
        /// <param name="fileInfo">File info.</param>
        /// <param name="format">Format.</param>
        private FileSystemImageStream(FileInfo fileInfo, ImageFormat format)
        {
            _fileInfo = fileInfo;
            Format = format;
        }

        /// <summary>
        /// The format of the image contained by the file
        /// </summary>
        /// <value>The format.</value>
        public ImageFormat Format { get; }

        /// <summary>
        /// The size in bytes of the image
        /// </summary>
        /// <value>The length.</value>
        public long Length { get => _fileInfo.Length; }

        /// <summary>
        /// Copy the image data in this file to the given stream
        /// </summary>
        /// <param name="stream">Stream.</param>
        public void CopyTo(Stream stream)
        {
            using (var fileStream = _fileInfo.OpenRead())
            {
                fileStream.CopyTo(stream);
            }
        }
    }
}
