using System;
using System.Security.Cryptography;
using System.Text;

namespace StacyClouds.C4Sharp.Api
{
    /// <summary>
    /// Generates HMAC signatures for authenticated Structurizr API requests.
    /// </summary>
    internal class HashBasedMessageAuthenticationCode
    {

        private string apiSecret;

        /// <summary>
        /// Initializes the HMAC generator with the API secret.
        /// </summary>
        /// <param name="apiSecret">The API secret used as the HMAC key.</param>
        internal HashBasedMessageAuthenticationCode(string apiSecret)
        {
            this.apiSecret = apiSecret;
        }

        /// <summary>
        /// Produces a lowercase SHA-256 HMAC for the supplied request content.
        /// </summary>
        /// <param name="content">The canonical request content to sign.</param>
        /// <returns>The hexadecimal HMAC digest.</returns>
        public string Generate(string content)
        {
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            byte[] hash = hmac.ComputeHash(bytes);

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

    }
}
