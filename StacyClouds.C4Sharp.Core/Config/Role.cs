namespace StacyClouds.C4Sharp.Config
{
    /// <summary>
    /// Defines the level of access granted to a workspace user.
    /// </summary>
    public enum Role
    {
     
        /// <summary>
        /// Allows reading and modifying the workspace.
        /// </summary>
        ReadWrite,
        /// <summary>
        /// Allows reading the workspace without making changes.
        /// </summary>
        ReadOnly
        
    }
    
}