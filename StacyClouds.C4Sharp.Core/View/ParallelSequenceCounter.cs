namespace StacyClouds.C4Sharp
{

    internal class ParallelSequenceCounter : SequenceCounter
    {

        internal ParallelSequenceCounter(SequenceCounter parent) : base(parent)
        {
            Sequence = Parent.Sequence;
        }

    }

}