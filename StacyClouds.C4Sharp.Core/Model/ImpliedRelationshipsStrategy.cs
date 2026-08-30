namespace StacyClouds.C4Sharp
{
    
    /// <summary>
    /// Defines the contract for strategies that create implied relationships after an explicit relationship is added.
    /// </summary>
    public interface IImpliedRelationshipsStrategy
    {
        
        /// <summary>
        /// Called after a relationship has been created in the model,
        /// providing an opportunity to create any resulting implied relationships.
        /// </summary>
        /// <param name="relationship">the newly created Relationship</param>
        void CreateImpliedRelationships(Relationship relationship);
    
    }
    
}