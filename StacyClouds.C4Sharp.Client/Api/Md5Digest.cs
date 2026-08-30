using System;
using System.Security.Cryptography;
using System.Text;

namespace StacyClouds.C4Sharp.Api
{
    /// <summary>
    /// Produces MD5 digests for Structurizr API payload signing.
    /// </summary>
    internal class Md5Digest
    {

        /// <summary>
        /// Generates a lowercase hexadecimal MD5 digest for the supplied content.
        /// </summary>
        /// <param name="content">The content to hash.</param>
        /// <returns>The hexadecimal MD5 digest.</returns>
        internal string Generate(string content)
        {
            if (content == null)
            {
                content = "";
            }

            MD5 md5 = MD5.Create();
            byte[] textToHash = Encoding.UTF8.GetBytes(content);
            byte[] result = md5.ComputeHash(textToHash);

            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

    }
}
