using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;

namespace StacyClouds.C4Sharp.IO.Json
{
    /// <summary>
    /// Serializes workspaces to JSON.
    /// </summary>
    public class JsonWriter
    {

        /// <summary>
        /// Controls whether generated JSON should be indented for readability.
        /// </summary>
        public bool IndentOutput { get; set; }

        /// <summary>
        /// Creates a workspace JSON writer.
        /// </summary>
        /// <param name="indentOutput">Whether generated JSON should be indented.</param>
        public JsonWriter(bool indentOutput)
        {
            this.IndentOutput = indentOutput;
        }

        /// <summary>
        /// Writes a workspace as JSON.
        /// </summary>
        /// <param name="workspace">The workspace to serialize.</param>
        /// <param name="writer">The writer that receives the JSON payload.</param>
        public void Write(Workspace workspace, TextWriter writer)
        {
            string json = JsonConvert.SerializeObject(workspace,
                IndentOutput ? Formatting.Indented : Formatting.None,
                new StringEnumConverter(),
                new IsoDateTimeConverter(),
                new PaperSizeJsonConverter());

            writer.Write(json);
        }

    }
}
