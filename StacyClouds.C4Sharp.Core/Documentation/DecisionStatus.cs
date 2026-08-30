namespace StacyClouds.C4Sharp.Documentation
{

    /// <summary>
    /// Represents the status of a decision.
    /// </summary>
    public enum DecisionStatus
    {

        /// <summary>
        /// The decision is being proposed and has not been ratified yet.
        /// </summary>
        Proposed,
        /// <summary>
        /// The decision has been accepted and is in force.
        /// </summary>
        Accepted,
        /// <summary>
        /// The decision was accepted previously but has been replaced by another one.
        /// </summary>
        Superseded,
        /// <summary>
        /// The decision remains recorded but should no longer guide new work.
        /// </summary>
        Deprecated,
        /// <summary>
        /// The decision was considered and explicitly declined.
        /// </summary>
        Rejected

    }

}