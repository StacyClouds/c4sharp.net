namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Coordinates sequence counters for dynamic view relationship numbering.
    /// </summary>
    internal class SequenceNumber
    {

        private SequenceCounter _counter = new SequenceCounter();

        /// <summary>
        /// Creates a new sequence number generator.
        /// </summary>
        internal SequenceNumber()
        {
        }

        /// <summary>
        /// Advances the current counter and returns the formatted sequence label.
        /// </summary>
        /// <returns>The next sequence label.</returns>
        internal string GetNext()
        {
            _counter.Increment();
            return _counter.AsString();
        }

        /// <summary>
        /// Starts a nested parallel sequence branch.
        /// </summary>
        internal void StartParallelSequence()
        {
            _counter = new ParallelSequenceCounter(_counter);
        }

        /// <summary>
        /// Ends the current parallel sequence branch.
        /// </summary>
        /// <param name="endAllParallelSequencesAndContinueNumbering">When <see langword="true"/>, carries the current sequence value back to the parent counter.</param>
        internal void EndParallelSequence(bool endAllParallelSequencesAndContinueNumbering)
        {
            if (endAllParallelSequencesAndContinueNumbering)
            {
                int sequence = _counter.Sequence;
                _counter = _counter.Parent;
                _counter.Sequence = sequence;
            }
            else
            {
                _counter = _counter.Parent;
            }
        }

    }
}