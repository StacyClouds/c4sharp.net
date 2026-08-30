namespace StacyClouds.C4Sharp
{
	/// <summary>
	/// Provides shared guard logic for implied-relationship strategies.
	/// </summary>
	/// <remarks>
	/// Implied relationships are never created between the same element or between
	/// parent and child elements in the static structure hierarchy.
	/// </remarks>
    public abstract class AbstractImpliedRelationshipsStrategy : IImpliedRelationshipsStrategy
    {
		/// <summary>
		/// Determines whether an implied relationship can be created between the supplied elements.
		/// </summary>
		/// <param name="source">The candidate source element.</param>
		/// <param name="destination">The candidate destination element.</param>
		/// <returns>
		/// <see langword="true"/> when the elements are distinct and are not in a parent-child relationship;
		/// otherwise, <see langword="false"/>.
		/// </returns>
        protected bool ImpliedRelationshipIsAllowed(Element source, Element destination)
        {
            if (source.Equals(destination))
            {
                return false;
            }

            return !(IsChildOf(source, destination) || IsChildOf(destination, source));
        }

        private bool IsChildOf(Element e1, Element e2)
        {
            if (e1 is Person || e2 is Person) {
                return false;
            }

            Element parent = e2.Parent;
            while (parent != null) {
                if (parent.Id.Equals(e1.Id)) {
                    return true;
                }

                parent = parent.Parent;
            }

            return false;
        }

        /// <summary>
        /// Called after a relationship has been created in the model,
        /// providing an opportunity to create any resulting implied relationships.
        /// </summary>
        /// <param name="relationship">the newly created Relationship</param>
        public abstract void CreateImpliedRelationships(Relationship relationship);
        
    }
    
}