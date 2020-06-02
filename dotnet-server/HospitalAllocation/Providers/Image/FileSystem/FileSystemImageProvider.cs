using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using HospitalAllocation.Providers.Image.Interface;

namespace HospitalAllocation.Providers.Image.FileSystem
{
    /// <summary>
    /// Stores images on the file system by creating and tracking files
    /// in a given directory
    /// </summary>
    public class FileSystemImageProvider : IImageProvider
    {
        /// <summary>
        /// Create a file system image provider rooted in the given directory
        /// </summary>
        /// <returns>The from directory.</returns>
        /// <param name="directory">Directory.</param>
        public static FileSystemImageProvider CreateFromDirectory(DirectoryInfo directory)
        {
            // If the directory doesn't exist, create it and start fresh
            if (!directory.Exists)
            {
                directory.Create();
                return new FileSystemImageProvider(directory, new HashSet<int>(), 1);
            }

            // Look at all the files in the given directory
            // and determine which might be image files from another run
            int maxIndex = 0;
            var seenIndices = new HashSet<int>();
            foreach (FileInfo file in directory.GetFiles())
            {
                string filePrefix = file.Name.Substring(0, file.Name.Length - file.Extension.Length);
                if (!Int32.TryParse(filePrefix, out int imageIndex))
                {
                    continue;
                }

                seenIndices.Add(imageIndex);
                if (imageIndex > maxIndex)
                {
                    maxIndex = imageIndex;
                }
            }

            // Go through the files seen and work out what
            // numbers are missing from the image indices
            var missingIndices = new HashSet<int>();
            for (int i = 1; i < maxIndex; i++)
            {
                if (!seenIndices.Contains(i))
                {
                    missingIndices.Add(i);
                }
            }

            return new FileSystemImageProvider(directory, missingIndices, maxIndex + 1);
        }

        // The directory that stores the images for this provider
        private readonly DirectoryInfo _imageRootDir;
        // The image indices which have been deleted
        private readonly ISet<int> _deletedIndices;

        // The index to be assigned to the next image
        private int _nextImageIndex;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Image.FileSystem.FileSystemImageProvider"/> class.
        /// </summary>
        /// <param name="imageRootDir">Image root dir.</param>
        /// <param name="missingFiles">Missing files.</param>
        /// <param name="startIndex">Max file index.</param>
        private FileSystemImageProvider(DirectoryInfo imageRootDir, ISet<int> missingFiles, int startIndex)
        {
            _imageRootDir = imageRootDir;
            _deletedIndices = new HashSet<int>(missingFiles);
            _nextImageIndex = startIndex;
        }

        /// <summary>
        /// Takes an image from an image stream and saves it to the directory
        /// in the conventional format of this image provider
        /// </summary>
        /// <returns>The image.</returns>
        /// <param name="imageData">Image data.</param>
        public int CreateImage(IImageStream imageData)
        {
            // Always assign to a deleted index first if possible
            int index = -1;
            if (_deletedIndices.Any())
            {
                index = _deletedIndices.FirstOrDefault();
                _deletedIndices.Remove(index);
            }
            else
            {
                index = _nextImageIndex;
                _nextImageIndex++;
            }

            using (var stream = FileForNewImage(index, imageData.Format).OpenWrite())
            {
                imageData.CopyTo(stream);
            }

            return index;
        }

        /// <summary>
        /// Delete the image file with the given index from the storage directory
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void DeleteImage(int id)
        {
            FileInfo file = GetImageFileFromIndex(id);
            if (!file.Exists)
            {
                throw new ArgumentException(String.Format("Image with ID {0} does not exist.", id));
            }

            file.Delete();
            _deletedIndices.Add(id);
        }

        /// <summary>
        /// Get the image corresponding to the given index.
        /// Return null if no such image exists
        /// </summary>
        /// <returns>The image.</returns>
        /// <param name="id">Identifier.</param>
        public IImageStream GetImage(int id)
        {
            // Try to determine without querying the file system
            // whether we have the image or not
            if (!ContainsImage(id))
            {
                return null;
            }

            FileInfo file = GetImageFileFromIndex(id);
            // We shouldn't need to do this, but it will help for now
            if (!file.Exists)
            {
                // Should throw something here
                return null;
            }

            return FileSystemImageStream.FromFile(file);
        }

        /// <summary>
        /// Replace the image at the given index with the given one
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="newImage">New image.</param>
        public void ReplaceImage(int id, IImageStream newImage)
        {
            FileInfo file = GetImageFileFromIndex(id);
            if (file == null || !file.Exists)
            {
                throw new ArgumentException(String.Format("Image with ID {0} does not exist.", id));
            }

            using (var stream = file.OpenWrite())
            {
                newImage.CopyTo(stream);
            }
        }

        /// <summary>
        /// List the indices of all store images in the directory
        /// </summary>
        /// <value>The stored images.</value>
        public IReadOnlyList<int> StoredImages
        {
            get
            {
                var storedImages = new List<int>();
                for (int i = 1; i < _nextImageIndex; i++)
                {
                    if (!_deletedIndices.Contains(i))
                    {
                        storedImages.Add(i);
                    }
                }

                return storedImages;
            }
        }

        /// <summary>
        /// Check if the provider contains an image with the given index
        /// </summary>
        /// <returns><c>true</c>, if image was containsed, <c>false</c> otherwise.</returns>
        /// <param name="imageIndex">Image index.</param>
        private bool ContainsImage(int imageIndex)
        {
            return imageIndex < _nextImageIndex && !_deletedIndices.Contains(imageIndex);
        }

        /// <summary>
        /// Retrieve the file storing the image with the given index
        /// </summary>
        /// <returns>The file in directory.</returns>
        /// <param name="imageIndex">Image index.</param>
        private FileInfo GetImageFileFromIndex(int imageIndex)
        {
            return _imageRootDir.GetFiles(imageIndex.ToString() + "*").FirstOrDefault();
        }

        /// <summary>
        /// Provides a file info object describing a file to be created for an
        /// image with a given index and format
        /// </summary>
        /// <returns>The for new image.</returns>
        /// <param name="imageIndex">Image index.</param>
        /// <param name="format">Format.</param>
        private FileInfo FileForNewImage(int imageIndex, ImageFormat format)
        {
            return new FileInfo(Path.Combine(_imageRootDir.FullName, imageIndex + format.FileExtension()));
        }
    }
}
