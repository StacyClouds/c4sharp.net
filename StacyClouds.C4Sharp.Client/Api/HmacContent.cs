using System.Text;

namespace StacyClouds.C4Sharp.Api
{
    /// <summary>
    /// Builds the canonical newline-delimited content string used for HMAC signing.
    /// </summary>
    internal class HmacContent
    {

        private string[] strings;

        /// <summary>
        /// Initializes the canonical content builder.
        /// </summary>
        /// <param name="strings">The content fragments to join in signing order.</param>
        internal HmacContent(params string[] strings)
        {
            this.strings = strings;
        }

        /// <summary>
        /// Joins the configured fragments with trailing newline separators.
        /// </summary>
        /// <returns>The canonical content string to sign.</returns>
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            foreach (string s in strings)
            {
                buf.Append(s);
                buf.Append("\n");
            }

            return buf.ToString();
        }

    }
}
