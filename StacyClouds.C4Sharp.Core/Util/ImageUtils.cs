using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StacyClouds.C4Sharp.Util
{
    /// <summary>
    /// Provides helpers for reading supported documentation image files.
    /// </summary>
    public class ImageUtils
    {

        /// <summary>
        /// Resolves the MIME type for a supported image file.
        /// </summary>
        /// <param name="file">The image file to inspect.</param>
        /// <returns>The MIME type for the image.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="file"/> is missing, is a directory, does not exist, or is not a supported image type.</exception>
        public static string GetContentType(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentException("A file must be specified.");
            }
            else if (Directory.Exists(file.FullName))
            {
                throw new ArgumentException(file.FullName + " is not a file.");
            }
            else if (!file.Exists)
            {
                throw new ArgumentException(file.FullName + " does not exist.");
            }

            string fileExtension = file.FullName.Substring(file.FullName.LastIndexOf(".") + 1).ToLower();
            if (fileExtension.Equals("jpg"))
            {
                fileExtension = "jpeg";
            }

            if (fileExtension == "png" || fileExtension == "jpeg" || fileExtension == "gif")
            {
                return "image/" + fileExtension;
            }
            else
            {
                throw new ArgumentException(file.FullName + " is not a supported image file.");
            }
        }

        /// <summary>
        /// Reads a supported image file and returns its Base64-encoded bytes.
        /// </summary>
        /// <param name="file">The image file to read.</param>
        /// <returns>The Base64-encoded image content.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="file"/> is missing, invalid, or unsupported.</exception>
        public static string GetImageAsBase64(FileInfo file)
        {
            String contentType = GetContentType(file); // this does the file checks
            byte[] imageArray = File.ReadAllBytes(file.FullName);
            return Convert.ToBase64String(imageArray);
        }

        /// <summary>
        /// Reads a supported image file and returns it as a data URI.
        /// </summary>
        /// <param name="file">The image file to read.</param>
        /// <returns>A data URI that embeds the image content.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="file"/> is missing, invalid, or unsupported.</exception>
        public static string GetImageAsDataUri(FileInfo file)
        {
            String contentType = GetContentType(file);
            String base64Content = GetImageAsBase64(file);

            return "data:" + contentType + ";base64," + base64Content;
        }

    }

}
