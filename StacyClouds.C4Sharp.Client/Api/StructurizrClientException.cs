using System;

namespace StacyClouds.C4Sharp.Api
{
    public class StructurizrClientException : Exception
    {

        public StructurizrClientException(String message) : base(message) { }

        public StructurizrClientException(String message, Exception innerException) : base(message, innerException) { }

    }
}
