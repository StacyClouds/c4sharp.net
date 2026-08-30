using System.Linq;

namespace StacyClouds.C4Sharp
{
    
    /// <summary>
    /// This strategy creates implied relationships between all valid combinations of the parent elements,
    /// unless the same relationship already exists between them.
    /// </summary>
    public class CreateImpliedRelationshipsUnlessSameRelationshipExistsStrategy : AbstractImpliedRelationshipsStrategy
    {
        /// <summary>
        /// Creates implied relationships while avoiding duplicates with the same destination and description.
        /// </summary>
        /// <param name="relationship">The explicit relationship that triggered implied relationship generation.</param>
        /// <remarks>
        /// This strategy walks up both parent hierarchies and only suppresses an implied
        /// relationship when an equivalent description already exists between the candidate elements.
        /// </remarks>
        public override void CreateImpliedRelationships(Relationship relationship)
        {
            Element source = relationship.Source;
            Element destination = relationship.Destination;

            Model model = source.Model;

            while (source != null) {
                while (destination != null) {
                    if (ImpliedRelationshipIsAllowed(source, destination)) {
                        bool createRelationship = !source.HasEfferentRelationshipWith(destination, relationship.Description);

                        if (createRelationship) {
                            model.AddRelationship(source, destination, relationship.Description, relationship.Technology, relationship.InteractionStyle, relationship.GetTagsAsSet().ToArray(), false);
                        }
                    }

                    destination = destination.Parent;
                }

                destination = relationship.Destination;
                source = source.Parent;
            }
        }
    }
    
}