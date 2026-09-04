using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StacyClouds.C4Sharp.Encryption;
using System;
using System.Reflection;

namespace StacyClouds.C4Sharp.IO.Json
{
    /// <summary>
    /// Deserializes polymorphic encryption strategy JSON payloads.
    /// </summary>
    internal class EncryptionStrategyJsonConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether the converter can handle the supplied type.
        /// </summary>
        /// <param name="objectType">The candidate type.</param>
        /// <returns><c>true</c> when the type derives from <see cref="EncryptionStrategy"/>; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(EncryptionStrategy).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        /// <summary>
        /// Reads an encryption strategy instance from JSON.
        /// </summary>
        /// <param name="reader">The JSON reader positioned at the strategy payload.</param>
        /// <param name="objectType">The requested CLR type.</param>
        /// <param name="existingValue">An existing value supplied by the serializer.</param>
        /// <param name="serializer">The active serializer.</param>
        /// <returns>The deserialized <see cref="EncryptionStrategy"/>.</returns>
        /// <exception cref="NotSupportedException">Thrown when the serialized strategy type is unknown.</exception>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);
            string type = item["type"].Value<string>();
            if (type == "aes")
            {
                return item.ToObject<AesEncryptionStrategy>();
            }
            else
            {
                throw new NotSupportedException("The encryption strategy type of " + type + " is not supported");
            }
        }

        /// <summary>
        /// Writes an encryption strategy instance to JSON.
        /// </summary>
        /// <param name="writer">The JSON writer to output to.</param>
        /// <param name="value">The value to serialize.</param>
        /// <param name="serializer">The active serializer.</param>
        /// <exception cref="NotImplementedException">Always thrown because this converter only supports reading.</exception>
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("This operation is not implemented");
        }
    }
}
