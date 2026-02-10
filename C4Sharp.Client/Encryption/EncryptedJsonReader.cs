using Newtonsoft.Json;
using StacyClouds.C4Sharp.IO.Json;
using System.IO;

namespace StacyClouds.C4Sharp.Encryption
{
    public class EncryptedJsonReader
    {

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
