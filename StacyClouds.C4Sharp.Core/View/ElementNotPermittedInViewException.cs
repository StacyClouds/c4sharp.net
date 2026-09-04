using System;

namespace StacyClouds.C4Sharp
{
    
    /// <summary>
    /// The exception thrown when a view rejects an element type or scope.
    /// </summary>
    public sealed class ElementNotPermittedInViewException : Exception
    {

        /// <summary>
        /// Creates an exception describing why the element cannot be added.
        /// </summary>
        /// <param name="message">The validation message for the rejected element.</param>
        internal ElementNotPermittedInViewException(string message) : base(message)
        {
        }
        
    }
    
}