using System;

namespace StacyClouds.C4Sharp
{
    
    public sealed class ElementNotPermittedInViewException : Exception
    {

        internal ElementNotPermittedInViewException(string message) : base(message)
        {
        }
        
    }
    
}