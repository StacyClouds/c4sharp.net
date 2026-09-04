using System;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Captures the automatic layout settings applied to a view.
    /// </summary>
    [DataContract]
    public class AutomaticLayout
    {

        /// <summary>
        /// Controls the direction in which ranks are arranged.
        /// </summary>
        [DataMember(Name = "rankDirection", EmitDefaultValue = true)]
        public RankDirection RankDirection;

        private int _rankSeparation;

        /// <summary>
        /// Specifies the vertical or horizontal distance between ranks, in pixels.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the value is negative.</exception>
        [DataMember(Name = "rankSeparation", EmitDefaultValue = false)]
        public int RankSeparation
        {
            get { return _rankSeparation; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The rank separation must be a positive integer.");
                }

                _rankSeparation = value;
            }
        }

        private int _nodeSeparation;
        
        /// <summary>
        /// Specifies the distance between nodes within the same rank, in pixels.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the value is negative.</exception>
        [DataMember(Name = "nodeSeparation", EmitDefaultValue = false)]
        public int NodeSeparation
        {
            get { return _nodeSeparation; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The node separation must be a positive integer.");
                }

                _nodeSeparation = value;
            }
        }

        private int _edgeSeparation;

        /// <summary>
        /// Specifies the distance between parallel edges, in pixels.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the value is negative.</exception>
        [DataMember(Name = "edgeSeparation", EmitDefaultValue = false)]
        public int EdgeSeparation
        {
            get { return _edgeSeparation; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The edge separation must be a positive integer.");
                }

                _edgeSeparation = value;
            }
        }

        /// <summary>
        /// Indicates whether the layout engine should persist intermediate connector vertices.
        /// </summary>
        [DataMember(Name = "vertices", EmitDefaultValue = true)]
        public bool Vertices;

        /// <summary>
        /// Initializes a layout settings object during deserialization.
        /// </summary>
        internal AutomaticLayout()
        {
        }

        /// <summary>
        /// Creates a set of automatic layout settings.
        /// </summary>
        /// <param name="rankDirection">The direction used to arrange ranks.</param>
        /// <param name="rankSeparation">The distance between ranks, in pixels.</param>
        /// <param name="nodeSeparation">The distance between nodes in the same rank, in pixels.</param>
        /// <param name="edgeSeparation">The distance between edges, in pixels.</param>
        /// <param name="vertices">Whether connector vertices should be generated.</param>
        internal AutomaticLayout(RankDirection rankDirection, int rankSeparation, int nodeSeparation,
            int edgeSeparation, bool vertices)
        {
            RankDirection = rankDirection;
            RankSeparation = rankSeparation;
            NodeSeparation = nodeSeparation;
            EdgeSeparation = edgeSeparation;
            Vertices = vertices;
        }
        
    }

}