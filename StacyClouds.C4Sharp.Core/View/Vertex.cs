using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// Represents the X and Y coordinate of a bend in a relationship line.
    /// </summary>
    [DataContract]
    public sealed class Vertex
    {

        /// <summary>
        /// Creates an empty vertex.
        /// </summary>
        public Vertex()
        {
        }

        /// <summary>
        /// Creates a vertex at the supplied coordinates.
        /// </summary>
        /// <param name="x">The horizontal position.</param>
        /// <param name="y">The vertical position.</param>
        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// The horizontal position of the vertex when rendered.
        /// </summary>
        [DataMember(Name="x", EmitDefaultValue=false)]
        public int? X { get; set; }
  
        
        /// <summary>
        /// The vertical position of the vertex when rendered.
        /// </summary>
        [DataMember(Name="y", EmitDefaultValue=false)]
        public int? Y { get; set; }
  
    }
    
}
