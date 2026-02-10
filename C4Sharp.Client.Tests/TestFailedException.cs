using System;

namespace StacyClouds.C4Sharp.Api.Tests
{
    public class TestFailedException : Exception
    {

        public TestFailedException()
        {
        }
        
        public TestFailedException(string message) : base(message)
        {
        }
        
    }
}