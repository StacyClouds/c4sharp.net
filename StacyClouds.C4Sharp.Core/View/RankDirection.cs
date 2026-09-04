namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Specifies the direction used by automatic layout to arrange ranks.
    /// </summary>
    public enum RankDirection
    {
        /// <summary>
        /// Places higher-level ranks above lower-level ranks.
        /// </summary>
        TopBottom,
        /// <summary>
        /// Places higher-level ranks below lower-level ranks.
        /// </summary>
        BottomTop,
        /// <summary>
        /// Places higher-level ranks to the left of lower-level ranks.
        /// </summary>
        LeftRight,
        /// <summary>
        /// Places higher-level ranks to the right of lower-level ranks.
        /// </summary>
        RightLeft
    }
    
}