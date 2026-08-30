using System;
using System.Text;

namespace StacyClouds.C4Sharp.Api
{
    /// <summary>
    /// Formats the Structurizr HMAC authorization header value.
    /// </summary>
    internal class HmacAuthorizationHeader
    {

        private string apiKey;
        private string hmac;

        /// <summary>
        /// Initializes an authorization header value.
        /// </summary>
        /// <param name="apiKey">The workspace API key.</param>
        /// <param name="hmac">The hexadecimal HMAC signature.</param>
        public HmacAuthorizationHeader(string apiKey, string hmac)
        {
            this.apiKey = apiKey;
            this.hmac = hmac;
        }

        /// <summary>
        /// Formats the authorization header as <c>apiKey:base64(hmac)</c>.
        /// </summary>
        /// <returns>The header value to send with the request.</returns>
        public override string ToString()
        {
            return apiKey + ":" + Convert.ToBase64String(Encoding.UTF8.GetBytes(hmac));
        }

    }
}
