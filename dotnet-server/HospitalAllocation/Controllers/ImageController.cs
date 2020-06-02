using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HospitalAllocation.Providers.Image.Interface;
using HospitalAllocation.Messages.Responses;

namespace HospitalAllocation.Controllers
{
    /// <summary>
    /// Provides an API for image storage and retrieval in the backend
    /// </summary>
    [Route("[controller]")]
    public class ImageController : Controller
    {
        // Provides the data storage for images
        private readonly IImageProvider _imageProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HospitalAllocation.Controllers.ImageController"/> class.
        /// </summary>
        /// <param name="imageProvider">Image provider.</param>
        public ImageController(IImageProvider imageProvider)
        {
            _imageProvider = imageProvider;
        }

        /// <summary>
        /// Lists the indices of all images stored
        /// </summary>
        /// <returns>The image indices.</returns>
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult ListImageIndices()
        {
            return Ok(_imageProvider.StoredImages);
        }

        /// <summary>
        /// Retrieve an image from the image data store given its numerical ID
        /// </summary>
        /// <returns>The image given by the ID</returns>
        /// <param name="id">The unique numerical ID of the image</param>
        [HttpGet("{id}")]
        public IActionResult RetrieveImage(int id)
        {
            IImageStream image = _imageProvider.GetImage(id);

            if (image == null)
            {
                return BadRequest(new ErrorResponse("No such image"));
            }

            // We need to copy the image to an internal memory stream in order
            // to transfer it out with a byte array. This is probably inefficient
            // since we copy once to the filesystem to memory and then from
            // memory to HTTP - worth refactoring later
            var dataStream = new MemoryStream();
            image.CopyTo(dataStream);
            return File(dataStream.ToArray(), image.Format.ContentTypeString());
        }

        /// <summary>
        /// Upload an image to be stored by the backend
        /// </summary>
        /// <returns>A message containing the ID assigned to the uploaded image</returns>
        /// <param name="file">The image file uploaded in the HTTP stream</param>
        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest(new ErrorResponse("Bad request format."));
            }

            // The HttpImageStream will scrutinise the Content-Type header
            // to make sure the file is a valid image format and return
            // null if not
            IImageStream image = HttpImageStream.CreateFromIFormFile(file);
            if (image == null)
            {
                return BadRequest(new ErrorResponse("Invalid image or unsupported format."));
            }

            int id;
            try
            {
                id = _imageProvider.CreateImage(image);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }

            return Ok(id);
        }

        /// <summary>
        /// Updates the image with the given ID with the transferred image data
        /// </summary>
        /// <returns>The image.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="file">File.</param>
        [HttpPut("{id}")]
        public IActionResult UpdateImage(int id, IFormFile file)
        {
            if (file == null)
            {
                return BadRequest(new ErrorResponse("Bad request format."));
            }

            IImageStream image = HttpImageStream.CreateFromIFormFile(file);
            try
            {
                _imageProvider.ReplaceImage(id, image);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }

            return Ok(id);
        }

        /// <summary>
        /// Deletes the image with the given index
        /// </summary>
        /// <returns>The image.</returns>
        /// <param name="id">Identifier.</param>
        [HttpDelete("{id}")]
        public IActionResult DeleteImage(int id)
        {
            try
            {
                _imageProvider.DeleteImage(id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }

            return ListImageIndices();
        }
    }
}
