namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Determines whether a filtered view keeps or hides matching tags.
    /// </summary>
    public enum FilterMode
    {
        
        /// <summary>
        /// Keeps only elements and relationships that carry one of the configured tags.
        /// </summary>
        Include,
        /// <summary>
        /// Hides elements and relationships that carry one of the configured tags.
        /// </summary>
        Exclude
        
    }
}