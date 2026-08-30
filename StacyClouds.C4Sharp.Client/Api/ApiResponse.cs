using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp.Api
{

    /// <summary>
    /// Represents the common JSON response envelope returned by the Structurizr API.
    /// </summary>
    [DataContract]
    internal sealed class ApiResponse
    {

        /// <summary>
        /// Indicates whether the API call succeeded.
        /// </summary>
        [DataMember(Name = "success", EmitDefaultValue = false)]
        internal bool Success;

        /// <summary>
        /// Carries the API message associated with the response.
        /// </summary>
        [DataMember(Name = "message", EmitDefaultValue = false)]
        internal string Message;

        /// <summary>
        /// Carries the workspace revision when the API includes one.
        /// </summary>
        [DataMember(Name = "revision", EmitDefaultValue = false)]
        internal long? Revision;

        /// <summary>
        /// Deserializes an API response payload.
        /// </summary>
        /// <param name="json">The JSON payload returned by the API.</param>
        /// <returns>The parsed API response.</returns>
        static internal ApiResponse Parse(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Converters = new List<JsonConverter> {
                    new IsoDateTimeConverter()
                }
            };

            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(json, settings);
            return apiResponse;
        }

    }
}