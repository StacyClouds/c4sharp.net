namespace StacyClouds.C4Sharp
{

   /// <summary>
    /// Maintains the numeric state used to build dynamic view sequence labels.
    /// </summary>
    internal class SequenceCounter
    {

        /// <summary>
        /// Points to the parent counter when numbering a nested parallel sequence.
        /// </summary>
        internal readonly SequenceCounter Parent;
        /// <summary>
        /// Stores the current numeric value for this counter.
        /// </summary>
        internal int Sequence { get; set; }

        /// <summary>
        /// Creates the root sequence counter.
        /// </summary>
        internal SequenceCounter()
        {
        }

        /// <summary>
        /// Creates a nested counter beneath an existing counter.
        /// </summary>
        /// <param name="parent">The parent counter that owns this nested counter.</param>
        internal SequenceCounter(SequenceCounter parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Advances the current sequence value by one.
        /// </summary>
        internal virtual void Increment()
        {
            Sequence++;
        }

        /// <summary>
        /// Formats the current sequence value for use in relationship order labels.
        /// </summary>
        /// <returns>The current sequence number as text.</returns>
        public virtual string AsString()
        {
            return "" + Sequence;
        }

    }

}