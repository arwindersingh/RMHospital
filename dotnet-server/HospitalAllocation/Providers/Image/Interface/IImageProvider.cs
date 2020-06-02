using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace HospitalAllocation.Providers.Image.Interface
{
    /// <summary>
    /// Records the different supported image formats
    /// </summary>
    public enum ImageFormat
    {
        /// <summary>
        /// A JPEG image
        /// </summary>
        Jpeg,

        /// <summary>
        /// A PNG image
        /// </summary>
        Png,

        /// <summary>
        /// A TIFF image
        /// </summary>
        Tiff,

        /// <summary>
        /// A Pic image
        /// </summary>
        Pic,

        /// <summary>
        /// A bitmap image
        /// </summary>
        Bmp,
    }

    /// <summary>
    /// Provides extension methods to the ImageFormat enum
    /// </summary>
    public static class ImageFormatExtension
    {
        // Provides a lookup table from image format enum to HTTP content types
        private static readonly IDictionary<ImageFormat, string> s_formatContentTypes;

        /// <summary>
        /// Populates static data structures for the ImageFormatExtension class
        /// </summary>
        static ImageFormatExtension()
        {
            s_formatContentTypes = new Dictionary<ImageFormat, string>
            {
                {ImageFormat.Bmp, "image/bmp"},
                {ImageFormat.Jpeg, "image/jpeg"},
                {ImageFormat.Pic, "image/pic"},
                {ImageFormat.Png, "image/png"},
                {ImageFormat.Tiff, "image/tiff"},
            };
        }

        /// <summary>
        /// Gets the file extension associated with the given image format
        /// </summary>
        /// <returns>The extension.</returns>
        /// <param name="imageFormat">Image format.</param>
        public static string FileExtension(this ImageFormat imageFormat)
        {
            return "." + imageFormat.ToString().ToLower();
        }

        /// <summary>
        /// Gets the HTTP content type header associated with the given image format
        /// </summary>
        /// <returns>The type string.</returns>
        /// <param name="imageFormat">Image format.</param>
        public static string ContentTypeString(this ImageFormat imageFormat)
        {
            if (!s_formatContentTypes.TryGetValue(imageFormat, out string contentType))
            {
                return null;
            }

            return contentType;
        }
    }

    /// <summary>
    /// Describes a resource that provides image data storage
    /// </summary>
    public interface IImageProvider
    {
        /// <summary>
        /// Get the image with the given ID
        /// </summary>
        /// <returns>The image.</returns>
        /// <param name="id">Identifier.</param>
        IImageStream GetImage(int id);

        /// <summary>
        /// Store a given image in the provider and return its newly given ID
        /// that can be used to retrieve it later
        /// </summary>
        /// <returns>The image.</returns>
        /// <param name="imageData">Image data.</param>
        int CreateImage(IImageStream imageData);

        /// <summary>
        /// Replace the image with the given ID with the new given image
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="newImage">New image.</param>
        void ReplaceImage(int id, IImageStream newImage);

        /// <summary>
        /// Delete the image with the given ID
        /// </summary>
        /// <param name="id">Identifier.</param>
        void DeleteImage(int id);

        /// <summary>
        /// Get a list of the indices of all the images stored by this provider
        /// </summary>
        /// <value>The stored images.</value>
        IReadOnlyList<int> StoredImages { get; }
    }
}
