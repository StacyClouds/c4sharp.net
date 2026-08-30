using Newtonsoft.Json;
using StacyClouds.C4Sharp.IO.Json;
using System.IO;

namespace StacyClouds.C4Sharp.Encryption
{
    /// <summary>
    /// Deserializes encrypted workspace JSON payloads.
    /// </summary>
    public class EncryptedJsonReader
    {

        /// <summary>
        /// Reads an encrypted workspace definition from a string reader.
        /// </summary>
        /// <param name="reader">The reader that supplies the JSON payload.</param>
        /// <returns>The deserialized encrypted workspace.</returns>
        public EncryptedWorkspace Read(StringReader reader)
        {
            EncryptedWorkspace workspace = JsonConvert.DeserializeObject<EncryptedWorkspace>(
                reader.ReadToEnd(),
                new Newtonsoft.Json.Converters.StringEnumConverter(),
                new PaperSizeJsonConverter(),
                new EncryptionStrategyJsonConverter());

            return workspace;
        }

    }
}
