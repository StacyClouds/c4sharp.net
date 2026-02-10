using System;

namespace StacyClouds.C4Sharp.Api
{
    public class C4SharpClientException : Exception
    {

        public C4SharpClientException(String message) : base(message) { }

        public C4SharpClientException(String message, Exception innerException) : base(message, innerException) { }

    }
}
