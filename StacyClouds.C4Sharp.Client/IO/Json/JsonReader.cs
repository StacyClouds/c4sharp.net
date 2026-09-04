using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Converters;

namespace StacyClouds.C4Sharp.IO.Json
{
    /// <summary>
    /// Deserializes workspace JSON payloads into hydrated workspace objects.
    /// </summary>
    public class JsonReader
    {

        /// <summary>
        /// Overrides the identifier generator assigned to the deserialized model.
        /// </summary>
        public IdGenerator IdGenerator;

        /// <summary>
        /// Reads a workspace definition from a string reader.
        /// </summary>
        /// <param name="reader">The reader that supplies the JSON payload.</param>
        /// <returns>The hydrated workspace.</returns>
        public Workspace Read(StringReader reader)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Converters = new List<JsonConverter> {
                    new StringEnumConverter(),
                    new IsoDateTimeConverter(),
                    new PaperSizeJsonConverter()
                },
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };

            Workspace workspace = JsonConvert.DeserializeObject<Workspace>(reader.ReadToEnd(), settings);
            
            if (IdGenerator != null) {
                workspace.Model.IdGenerator = IdGenerator;
            }

            workspace.Hydrate();

            return workspace;
        }

    }
}
