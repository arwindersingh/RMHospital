using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using HospitalAllocation.Model;
using HospitalAllocation.Providers.Image.Interface;

namespace HospitalAllocation.Providers.Image.Database
{
    /// <summary>
    /// Provides an image store interface over the hospital allocation relational database
    /// </summary>
    public class DbImageProvider : IImageProvider
    {
        /// <summary>
        /// The maximum length for a single side of an image.
        /// </summary>
        private const int MaxSideLength = 250;

        /// <summary>
        /// The quality level of the encoded JPEGs.
        /// </summary>
        private const int JpegQualityLevel = 90;

        // Options to build a database connection
        private readonly DbContextOptions<AllocationContext> _dbOptions;

        /// <summary>
        /// Make a new database image provider from a DbContextOptions object
        /// </summary>
        /// <param name="dbOptions"></param>
        public DbImageProvider(DbContextOptions<AllocationContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        /// <summary>
        /// List the IDs of all stored images in the database
        /// </summary>
        public IReadOnlyList<int> StoredImages
        {
            get
            {
                using (var dbContext = new AllocationContext(_dbOptions))
                {
                    return dbContext.Photos.Select(p => p.PhotoId).ToList();
                }
            }
        }

        /// <summary>
        /// Store a given image in the database
        /// </summary>
        /// <param name="imageData">the image data stream to store image data from</param>
        /// <returns>the database ID of the newly stored image</returns>
        public int CreateImage(IImageStream imageData)
        {
            SKData image = EncodeImage(imageData);

            if (image == null)
            {
                throw new ArgumentException("Invalid image or unsupported format.");
            }

            // Build a new database photo object entry
            var photo = new Photo() { Format = ImageFormat.Jpeg, Image = image.ToArray() };

            // Now put it in the database
            using (var dbContext = new AllocationContext(_dbOptions))
            {
                dbContext.Photos.Add(photo);
                dbContext.SaveChanges();

                return photo.PhotoId;
            }
        }

        /// <summary>
        /// Delete an image with the given ID from the database
        /// </summary>
        /// <param name="id">the database identifier of the image to delete</param>
        public void DeleteImage(int id)
        {
            using (var dbContext = new AllocationContext(_dbOptions))
            {
                Photo photo = dbContext.Photos.SingleOrDefault(p => p.PhotoId == id);
                if (photo == null)
                {
                    throw new ArgumentException(String.Format("Image with ID {0} does not exist.", id));
                }

                if (dbContext.StaffMembers.Any(s => s.PhotoId == id))
                {
                    throw new ArgumentException("Image is still associated with a staff member.");
                }

                dbContext.Remove(photo);
                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieve an image with the given identifier from the database
        /// </summary>
        /// <param name="id">the database identifier of the image to retrieve</param>
        /// <returns>a data stream of the image</returns>
        public IImageStream GetImage(int id)
        {
            using (var dbContext = new AllocationContext(_dbOptions))
            {
                Photo photo = dbContext.Photos.SingleOrDefault(p => p.PhotoId == id);

                if (photo == null)
                {
                    return null;
                }

                return new MemoryImageStream(photo.Image, photo.Format);
            }
        }

        /// <summary>
        /// Replace an existing image in the database with a new one, keeping the same ID
        /// </summary>
        /// <param name="id">the database identifier of the image to replace</param>
        /// <param name="newImage">the new image to associate with the given identifier</param>
        public void ReplaceImage(int id, IImageStream newImage)
        {
            using (var dbContext = new AllocationContext(_dbOptions))
            {
                Photo photo = dbContext.Photos.SingleOrDefault(p => p.PhotoId == id);
                if (photo == null)
                {
                    throw new ArgumentException(String.Format("Image with ID {0} does not exist.", id));
                }

                SKData image = EncodeImage(newImage);

                if (image == null)
                {
                    throw new ArgumentException("Invalid image or unsupported format.");
                }

                photo.Format = ImageFormat.Jpeg;
                photo.Image = image.ToArray();

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Encodes an image for storage.
        /// </summary>
        /// <param name="imageStream">The image to encode.</param>
        /// <returns>The encoded image, or null if decoding fails.</returns>
        /// <remarks>
        /// The image returned is guaranteed to have no side longer than
        /// <see cref="F:HospitalAllocation.Providers.Image.Database.DbImageProvider.MaxSideLength"/> pixels.
        /// </remarks>
        private static SKData EncodeImage(IImageStream imageStream)
        {
            // Get the image
            var imageArray = new byte[imageStream.Length];
            var stream = new MemoryStream(imageArray);
            imageStream.CopyTo(stream);

            var data = SKData.CreateCopy(imageArray);
            var image = SKImage.FromEncodedData(data);

            // Decode failed
            if (image == null)
            {
                return null;
            }

            // Resize the image if required
            if (Math.Max(image.Height, image.Width) > MaxSideLength)
            {
                int resizedHeight;
                int resizedWidth;
                float scale;
                if (image.Height > image.Width)
                {
                    resizedHeight = MaxSideLength;
                    scale = ((float)resizedHeight / image.Height);
                    resizedWidth = (int)Math.Round(image.Width * scale);
                }
                else
                {
                    resizedWidth = MaxSideLength;
                    scale = ((float)resizedWidth / image.Width);
                    resizedHeight = (int)Math.Round(image.Height * scale);
                }

                var surface = SKSurface.Create(new SKImageInfo(resizedWidth, resizedHeight));
                var paint = new SKPaint
                {
                    FilterQuality = SKFilterQuality.High
                };
                surface.Canvas.Scale(scale);
                surface.Canvas.DrawImage(image, 0, 0, paint);
                surface.Canvas.Flush();

                image = surface.Snapshot();
            }

            // Encode the image
            return image.Encode(SKEncodedImageFormat.Jpeg, JpegQualityLevel);
        }
    }
}
