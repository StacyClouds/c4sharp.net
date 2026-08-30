using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// Provides common metadata shared by elements and relationships in the model.
    /// </summary>
    [DataContract]
    public abstract class ModelItem
    {

        /// <summary>
        /// The ID of this item in the model.
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; internal set; }

        private List<string> _tags = new List<string>();

        /// <summary>
        /// Returns the full tag list, including required built-in tags and user-specified tags.
        /// </summary>
        /// <returns>The complete set of tags as individual values.</returns>
        public IEnumerable<string> GetAllTags()
        {
            if (String.IsNullOrWhiteSpace(Tags))
                return Enumerable.Empty<string>();
            return Tags.Split(new [] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }
        
        /// <summary>
        /// Returns the full tag list as a set without duplicates.
        /// </summary>
        /// <returns>A set containing required and user-specified tags.</returns>
        public ISet<string> GetTagsAsSet()
        {
            ISet<string> setOfTags = new HashSet<string>(GetRequiredTags());
            foreach (string tag in _tags)
            {
                setOfTags.Add(tag);
            }

            return setOfTags;
        }

        /// <summary>
        /// The comma-separated tag list stored with this model item.
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public string Tags
        {
            get
            {
                List<string> listOfTags = new List<string>(GetRequiredTags());
                listOfTags.AddRange(_tags);

                if (listOfTags.Count == 0)
                {
                    return "";
                }

                StringBuilder buf = new StringBuilder();
                foreach (string tag in listOfTags)
                {
                    buf.Append(tag);
                    buf.Append(",");
                }

                string tagsAsString = buf.ToString();
                return tagsAsString.Substring(0, tagsAsString.Length - 1);
            }

            set
            {
                this._tags.Clear();

                if (value == null)
                {
                    return;
                }

                this._tags.AddRange(value.Split(','));
            }
        }

        /// <summary>
        /// Initializes a model item for deserialization.
        /// </summary>
        internal ModelItem()
        {
        }

        /// <summary>
        /// Adds one or more user-defined tags to this model item.
        /// </summary>
        /// <param name="tags">The tags to add.</param>
        public void AddTags(params string[] tags)
        {
            if (tags == null)
            {
                return;
            }

            foreach (string tag in tags)
            {
                if (tag != null)
                {
                    string t = tag.Trim();
                    if (!_tags.Contains(t))
                    {
                        _tags.Add(t);
                    }
                }
            }
        }

        /// <summary>
        /// Removes a user-defined tag from this model item.
        /// </summary>
        /// <param name="tag">The tag to remove.</param>
        public virtual void RemoveTag(string tag)
        {
            if (tag != null)
            {
                this._tags.Remove(tag.Trim());
            }
        }

        /// <summary>
        /// Returns the built-in tags that must always be present for this model item.
        /// </summary>
        /// <returns>The required tags.</returns>
        public abstract List<string> GetRequiredTags();

        private Dictionary<string, string> _properties = new Dictionary<string, string>();

        /// <summary>
        /// The collection of name-value property pairs associated with this element, as a Dictionary.
        /// </summary>
        [DataMember(Name = "properties", EmitDefaultValue = false)]
        public Dictionary<string, string> Properties
        {
            get
            {
                return _properties;
            }

            internal set
            {
                if (value != null)
                {
                    _properties = value;
                }
                else
                {
                    _properties.Clear();
                }
            }
        }

        /// <summary>
        /// Adds a name-value pair property to this element. 
        /// </summary>
        /// <param name="name">the name of the property</param>
        /// <param name="value">the value of the property</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="name"/> or <paramref name="value"/> is null, empty, or whitespace.
        /// </exception>
        public void AddProperty(string name, string value)
        {
            if (String.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException("A property name must be specified.");
            }

            if (String.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException("A property value must be specified.");
            }

            Properties[name] = value;
        }

        private ISet<Perspective> _perspectives = new HashSet<Perspective>();

        /// <summary>
        /// The set of perspectives associated with this element.
        /// </summary>
        [DataMember(Name = "perspectives", EmitDefaultValue = false)]
        public ISet<Perspective> Perspectives
        {
            get
            {
                return new HashSet<Perspective>(_perspectives);
            }

            internal set
            {
                _perspectives = new HashSet<Perspective>(value);
            }
        }

        /// <summary>
        /// Adds a perspective to this model item.
        /// </summary>
        /// <param name="name">the name of the perspective (e.g. "Security", must be unique)</param>
        /// <param name="description">The narrative associated with the perspective.</param>
        /// <returns>The created <see cref="Perspective"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the name or description is blank, or when a perspective with the same name already exists.
        /// </exception>
        public Perspective AddPerspective(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("A name must be specified.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("A description must be specified.");
            }

            if (Perspectives.Contains(new Perspective(name, "")))
            {
                throw new ArgumentException("A perspective named \"" + name + "\" already exists.");
            }

            Perspective perspective = new Perspective(name, description);
            _perspectives.Add(perspective);

            return perspective;
        }

    }
}