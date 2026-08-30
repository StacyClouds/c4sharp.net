namespace StacyClouds.C4Sharp.Documentation
{
    
    /// <summary>
    /// Couples documentation content with the format detected for that content.
    /// </summary>
    internal class FormattedContent
    {

        /// <summary>
        /// The raw documentation content.
        /// </summary>
        internal string Content { get; }
        /// <summary>
        /// The markup format used by <see cref="Content"/>.
        /// </summary>
        internal Format Format { get; }

        /// <summary>
        /// Initializes a formatted content value.
        /// </summary>
        /// <param name="content">The raw documentation content.</param>
        /// <param name="format">The markup format.</param>
        internal FormattedContent(string content, Format format)
        {
            Content = content;
            Format = format;
        }
 
    }   
    
}