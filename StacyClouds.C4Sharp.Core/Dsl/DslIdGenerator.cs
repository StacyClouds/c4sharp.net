using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StacyClouds.C4Sharp.Dsl
{
    /// <summary>
    /// Generates stable, slug-based identifiers for elements and relationships imported from DSL-shaped data.
    /// </summary>
    public sealed class DslIdGenerator : IdGenerator
    {
        private readonly HashSet<string> _seenIds = new HashSet<string>();
        private readonly Dictionary<string, int> _generatedCounts = new Dictionary<string, int>();

        /// <summary>
        /// Generates a unique identifier for an element.
        /// </summary>
        /// <param name="element">The element that needs an identifier.</param>
        /// <returns>A unique, slug-based identifier.</returns>
        public string GenerateId(Element element)
        {
            return GenerateUniqueId(Slugify(element?.CanonicalName ?? element?.Name ?? "element"));
        }

        /// <summary>
        /// Generates a unique identifier for a relationship.
        /// </summary>
        /// <param name="relationship">The relationship that needs an identifier.</param>
        /// <returns>A unique, slug-based identifier.</returns>
        public string GenerateId(Relationship relationship)
        {
            string baseId = string.Join("-",
                new[]
                {
                    relationship?.SourceId,
                    relationship?.DestinationId,
                    relationship?.Description
                }.Where(value => !string.IsNullOrWhiteSpace(value)));

            if (string.IsNullOrWhiteSpace(baseId))
            {
                baseId = "relationship";
            }

            return GenerateUniqueId(Slugify(baseId));
        }

        /// <summary>
        /// Registers an explicit identifier so future generated identifiers do not collide with it.
        /// </summary>
        /// <param name="id">The identifier that should be treated as already in use.</param>
        public void Found(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                _seenIds.Add(id);
            }
        }

        private string GenerateUniqueId(string baseId)
        {
            if (string.IsNullOrWhiteSpace(baseId))
            {
                baseId = "id";
            }

            if (!_seenIds.Contains(baseId))
            {
                _seenIds.Add(baseId);
                return baseId;
            }

            if (!_generatedCounts.ContainsKey(baseId))
            {
                _generatedCounts[baseId] = 1;
            }

            while (true)
            {
                _generatedCounts[baseId]++;
                string candidate = baseId + "-" + _generatedCounts[baseId];
                if (_seenIds.Add(candidate))
                {
                    return candidate;
                }
            }
        }

        private static string Slugify(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            bool previousWasSeparator = false;

            foreach (char character in value.Trim().ToLowerInvariant())
            {
                if (char.IsLetterOrDigit(character))
                {
                    builder.Append(character);
                    previousWasSeparator = false;
                }
                else if (!previousWasSeparator)
                {
                    builder.Append('-');
                    previousWasSeparator = true;
                }
            }

            string slug = builder.ToString().Trim('-');
            return slug;
        }
    }
}
