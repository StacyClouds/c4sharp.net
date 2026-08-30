namespace StacyClouds.C4Sharp
{
    
    /// <summary>
    /// The default strategy is to NOT create implied relationships.
    /// </summary>
    public class DefaultImpliedRelationshipsStrategy : AbstractImpliedRelationshipsStrategy
    {
        /// <summary>
        /// Leaves the model unchanged after a relationship is created.
        /// </summary>
        /// <param name="relationship">The newly created relationship.</param>
        public override void CreateImpliedRelationships(Relationship relationship)
        {
            // do nothing
        }
        
    }
    
}