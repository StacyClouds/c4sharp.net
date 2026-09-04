namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Describes whether an element belongs inside or outside the enterprise boundary.
    /// </summary>
    public enum Location
    {
        /// <summary>
        /// The location has not been specified.
        /// </summary>
        Unspecified,
        /// <summary>
        /// The element is inside the enterprise boundary.
        /// </summary>
        Internal,
        /// <summary>
        /// The element is outside the enterprise boundary.
        /// </summary>
        External

    }
}
