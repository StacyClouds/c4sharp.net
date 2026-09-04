using System;

namespace StacyClouds.C4Sharp
{
    
    /// <summary>
    /// An ID generator that simply uses a sequential number when generating IDs for model elements and relationships.
    /// This is the default ID generator.
    /// </summary>
    public class SequentialIntegerIdGeneratorStrategy : IdGenerator
    {

        private int Id = 0;

        /// <summary>
        /// Generates the next sequential identifier for an element.
        /// </summary>
        /// <param name="element">The element receiving an identifier.</param>
        /// <returns>The next identifier as a string.</returns>
        public string GenerateId(Element element)
        {
            lock(this)
            {
                return "" + ++Id;
            }
        }

        /// <summary>
        /// Generates the next sequential identifier for a relationship.
        /// </summary>
        /// <param name="relationship">The relationship receiving an identifier.</param>
        /// <returns>The next identifier as a string.</returns>
        public string GenerateId(Relationship relationship)
        {
            lock(this)
            {
                return "" + ++Id;
            }
        }

        /// <summary>
        /// Advances the generator when an existing identifier is discovered during hydration or deserialization.
        /// </summary>
        /// <param name="id">The identifier that is already in use.</param>
        /// <exception cref="FormatException">Thrown when <paramref name="id"/> is not a valid integer value.</exception>
        public void Found(string id)
        {
            int idAsInt = int.Parse(id);
            if (idAsInt > Id)
            {
                Id = idAsInt;
            }
        }
        
    }
}
