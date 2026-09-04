using Newtonsoft.Json;
using System;
using System.Reflection;

namespace StacyClouds.C4Sharp.IO.Json
{
    /// <summary>
    /// Converts paper size objects to and from their serialized string keys.
    /// </summary>
    internal class PaperSizeJsonConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether the converter can handle the supplied type.
        /// </summary>
        /// <param name="objectType">The candidate type.</param>
        /// <returns><c>true</c> when the type derives from <see cref="PaperSize"/>; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(PaperSize).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        /// <summary>
        /// Reads a paper size from its serialized key.
        /// </summary>
        /// <param name="reader">The JSON reader positioned at the paper size value.</param>
        /// <param name="objectType">The requested CLR type.</param>
        /// <param name="existingValue">An existing value supplied by the serializer.</param>
        /// <param name="serializer">The active serializer.</param>
        /// <returns>The resolved <see cref="PaperSize"/>.</returns>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return PaperSize.GetPaperSize(reader.Value as string);
        }

        /// <summary>
        /// Writes a paper size as its serialized key.
        /// </summary>
        /// <param name="writer">The JSON writer to output to.</param>
        /// <param name="value">The paper size value to serialize.</param>
        /// <param name="serializer">The active serializer.</param>
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, JsonSerializer serializer)
        {
            PaperSize paperSize = value as PaperSize;
            if (paperSize != null)
            {
                writer.WriteValue(paperSize.Key);
            }
        }
    }
}
