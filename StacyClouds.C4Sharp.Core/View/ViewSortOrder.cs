namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// Controls how a view set orders views when presented to consumers.
    /// </summary>
    public enum ViewSortOrder
    {
        
        /// <summary>
        /// Groups views by software system and then by view type.
        /// </summary>
        Default,
        /// <summary>
        /// Sorts views primarily by view type.
        /// </summary>
        Type,
        /// <summary>
        /// Sorts views alphabetically by key.
        /// </summary>
        Key
        
    }
    
}