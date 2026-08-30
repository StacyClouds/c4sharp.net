using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Represents a view on top of a view, which can be used to include or exclude specific elements. 
    /// </summary>
    [DataContract]
    public sealed class FilteredView
    {

        /// <summary>
        /// Identifies the filtered view.
        /// </summary>
        [DataMember(Name="key", EmitDefaultValue=false)]
        public string Key { get; internal set; }

        /// <summary>
        /// References the base view that is filtered.
        /// </summary>
        public View View { get; internal set; }

        /// <summary>
        /// Describes the purpose of the filtered view.
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; internal set; }

        /// <summary>
        /// Determines whether matching tags are included or excluded.
        /// </summary>
        [DataMember(Name="mode", EmitDefaultValue=true)]
        public FilterMode Mode { get; internal set; }

        /// <summary>
        /// Lists the tags used by the filter.
        /// </summary>
        [DataMember(Name="tags", EmitDefaultValue=false)]
        public ISet<string> Tags { get; internal set; } 
        
        private string _baseViewKey;

        /// <summary>
        /// Stores the key of the underlying base view for serialization.
        /// </summary>
        [DataMember(Name="baseViewKey", EmitDefaultValue=false)]
        public string BaseViewKey
        {
            get
            {
                if (View != null)
                {
                    return View.Key;
                }
                else
                {
                    return _baseViewKey;
                }
            }
            set { _baseViewKey = value; }
        }

        /// <summary>
        /// Initializes a filtered view during deserialization.
        /// </summary>
        [JsonConstructor]
        internal FilteredView()
        {
            Mode = FilterMode.Exclude;
            Tags = new HashSet<string>();
        }

        /// <summary>
        /// Creates a filtered view on top of the supplied static view.
        /// </summary>
        /// <param name="view">The base static view.</param>
        /// <param name="key">The unique key for the filtered view.</param>
        /// <param name="description">The filtered view description.</param>
        /// <param name="mode">Whether matching tags are included or excluded.</param>
        /// <param name="tags">The tags used for filtering.</param>
        internal FilteredView(StaticView view, string key, string description, FilterMode mode, params string[] tags) : this()
        {
            View = view;
            Key = key;
            Description = description;
            Mode = mode;

            if (tags != null)
            {
                foreach (string tag in tags)
                {
                    Tags.Add(tag);
                }
            }
        }

    }
    
}