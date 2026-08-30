using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Identifies how a code element contributes to a component implementation.
    /// </summary>
    public enum CodeElementRole
    {
        /// <summary>
        /// Marks the primary type that represents the component itself.
        /// </summary>
        Primary,
        /// <summary>
        /// Marks an additional supporting type used by the component.
        /// </summary>
        Supporting

    }

}
