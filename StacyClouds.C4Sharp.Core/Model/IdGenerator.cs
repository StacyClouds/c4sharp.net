namespace StacyClouds.C4Sharp
{
    
    /// <summary>
    /// Defines how identifiers are generated and tracked for model elements and relationships.
    /// </summary>
    public interface IdGenerator
    {

        /// <summary>
        /// Generates an ID for the specified model element.
        /// </summary>
        /// <param name="element">an Element instance</param>
        /// <returns>the ID</returns>
        string GenerateId(Element element);

        /// <summary>
        /// Generates an ID for the specified relationship.
        /// </summary>
        /// <param name="relationship">A relationship instance.</param>
        /// <returns>The generated identifier.</returns>
        string GenerateId(Relationship relationship);

        /// <summary>
        /// Called when loading/deserializing a model, to indicate that the specified ID has been found
        /// (and shouldn't be reused when generating new IDs).
        /// </summary>
        /// <param name="id">The ID that has been found.</param>
        void Found(string id);
        
    }
}
