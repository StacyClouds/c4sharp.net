namespace StacyClouds.C4Sharp.Core.View
{

    /// <summary>
    /// The type of symbols to use when rendering metadata.
    /// </summary>
    public enum MetadataSymbols
    {
        
        /// <summary>
        /// Renders metadata inside square brackets.
        /// </summary>
        SquareBrackets,
        /// <summary>
        /// Renders metadata inside round brackets.
        /// </summary>
        RoundBrackets,
        /// <summary>
        /// Renders metadata inside curly brackets.
        /// </summary>
        CurlyBrackets,
        /// <summary>
        /// Renders metadata inside angle brackets.
        /// </summary>
        AngleBrackets,
        /// <summary>
        /// Renders metadata inside double angle brackets.
        /// </summary>
        DoubleAngleBrackets,
        /// <summary>
        /// Hides metadata symbols entirely.
        /// </summary>
        None

    }
}