using System;

namespace StacyClouds.C4Sharp.Api
{
    /// <summary>
    /// Represents failures that occur while calling the Structurizr API.
    /// </summary>
    public class StructurizrClientException : Exception
    {

        /// <summary>
        /// Initializes an exception with a client-specific error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public StructurizrClientException(String message) : base(message) { }

        /// <summary>
        /// Initializes an exception with a client-specific error message and the originating exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The exception that caused this failure.</param>
        public StructurizrClientException(String message, Exception innerException) : base(message, innerException) { }

    }
}
