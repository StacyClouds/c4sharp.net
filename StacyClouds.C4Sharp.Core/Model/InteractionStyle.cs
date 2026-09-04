namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Describes whether a relationship happens synchronously or asynchronously.
    /// </summary>
    public enum InteractionStyle
    {
        /// <summary>
        /// The source waits for the destination to complete the interaction.
        /// </summary>
        Synchronous,
        /// <summary>
        /// The source and destination are decoupled in time.
        /// </summary>
        Asynchronous

    }
}
