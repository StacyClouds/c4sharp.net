namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// Tracks numbering for a nested parallel branch in a dynamic view sequence.
    /// </summary>
    internal class ParallelSequenceCounter : SequenceCounter
    {

        /// <summary>
        /// Creates a parallel counter that starts from the parent sequence number.
        /// </summary>
        /// <param name="parent">The counter that owns the parallel branch.</param>
        internal ParallelSequenceCounter(SequenceCounter parent) : base(parent)
        {
            Sequence = Parent.Sequence;
        }

    }

}