using System;
using System.Collections.Generic;
using System.IO;

namespace StacyClouds.C4Sharp.Documentation
{
    
    /// <summary>
    /// Resolves documentation markup formats from file extensions.
    /// </summary>
    internal class FormatFinder
    {
        
        private static ISet<string> MARKDOWN_EXTENSIONS = new HashSet<string>
        {
            ".md", ".markdown", ".text"
        };

        private static ISet<string> ASCIIDOC_EXTENSIONS = new HashSet<string>
        {
            ".asciidoc", ".adoc", ".asc"
        };

        /// <summary>
        /// Infers the documentation format from a file extension.
        /// </summary>
        /// <param name="file">The file whose extension should be examined.</param>
        /// <returns>The detected format, defaulting to <see cref="Format.Markdown"/> when the extension is unknown.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="file"/> is <c>null</c>.</exception>
        internal static Format FindFormat(FileSystemInfo file) {
            if (file == null) {
                throw new ArgumentException("A file must be specified.");
            }

            if (MARKDOWN_EXTENSIONS.Contains(file.Extension)) {
                return Format.Markdown;
            } else if (ASCIIDOC_EXTENSIONS.Contains(file.Extension)) {
                return Format.AsciiDoc;
            } else {
                // just assume Markdown
                return Format.Markdown;
            }

        }
        
    }
    
}