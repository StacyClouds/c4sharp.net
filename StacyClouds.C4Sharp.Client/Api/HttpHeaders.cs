namespace StacyClouds.C4Sharp.Api
{
    /// <summary>
    /// Defines HTTP header names used by the Structurizr API client.
    /// </summary>
    internal class HttpHeaders
    {

        /// <summary>
        /// The standard user agent header.
        /// </summary>
        internal const string UserAgent = "User-Agent";
        /// <summary>
        /// The standard authorization header.
        /// </summary>
        internal const string Authorization = "Authorization";
        /// <summary>
        /// The Structurizr HMAC authorization header.
        /// </summary>
        internal const string XAuthorization = "X-Authorization";
        /// <summary>
        /// The content type header.
        /// </summary>
        internal const string ContentType = "Content-Type";
        /// <summary>
        /// The content MD5 header.
        /// </summary>
        internal const string ContentMd5 = "Content-MD5";
        /// <summary>
        /// The nonce header used for request signing.
        /// </summary>
        internal const string Nonce = "Nonce";

    }
}
