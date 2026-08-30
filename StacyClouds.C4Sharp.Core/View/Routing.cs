namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Defines the supported routing styles for relationship lines.
    /// </summary>
    public enum Routing
    {
        /// <summary>
        /// Draws the shortest straight connector between elements.
        /// </summary>
        Direct,
        /// <summary>
        /// Draws the connector as a smooth curve.
        /// </summary>
        Curved,
        /// <summary>
        /// Draws the connector using right-angled segments.
        /// </summary>
        Orthogonal
    }
}