using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Structurizr.Core.View;
using Structurizr.Util;

namespace Structurizr
{

    /// <summary>
    /// The configuration associated with a set of views.
    /// </summary>
    [DataContract]
    public sealed class ViewConfiguration
    {

        internal ViewConfiguration()
        {
            this.Styles = new Styles();
            this.Branding = new Branding();
            this.Terminology = new Terminology();
        }

        [DataMember(Name = "styles", EmitDefaultValue = false)]
        public Styles Styles { get; internal set; }

        private string[] _themes;
        
        public string Theme
        {
            get
            {
                if (_themes != null && _themes.Length > 0)
                {
                    return _themes[0];
                }
                else
                {
                    return null;
                }
            }
            
            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    if (Url.IsUrl(value))
                    {
                        _themes = new string[]{ value.Trim() };
                    }
                    else {
                        throw new ArgumentException(value + " is not a valid URL.");
                    }
                }
            }
        }
        
        [DataMember(Name = "themes", EmitDefaultValue = false)]
        public string[] Themes
        {
            get { return _themes; }
            set
            {
                List<string> list = new List<string>(); 
                if (value != null)
                {
                    foreach (string theme in value)
                    {
                        if (value != null && theme.Trim().Length > 0)
                        {
                            if (Url.IsUrl(theme))
                            {
                                list.Add(theme.Trim());
                            }
                            else {
                                throw new ArgumentException(value + " is not a valid URL.");
                            }
                        }
                    }
                }

                _themes = list.ToArray();
            }
        }
        
        [DataMember(Name = "branding", EmitDefaultValue = false)]
        public Branding Branding { get; internal set; }

        [DataMember(Name = "terminology", EmitDefaultValue = false)]
        public Terminology Terminology { get; internal set; }

        /// <summary>
        /// The type of symbols to use when rendering metadata.
        /// </summary>
        [DataMember(Name = "metadataSymbols", EmitDefaultValue = false)]
        public MetadataSymbols? MetadataSymbols { get; set; }

        [DataMember(Name = "defaultView", EmitDefaultValue = false)]
        public string DefaultView { get; private set; }

        /// <summary>
        /// Sets the view that should be shown by default.
        /// </summary>
        /// <param name="view">A View object</param>
        public void SetDefaultView(View view)
        {
            if (view != null)
            {
                this.DefaultView = view.Key;
            }
        }

        [DataMember(Name = "lastSavedView", EmitDefaultValue = false)]
        internal string LastSavedView { get; set; }

        public void CopyConfigurationFrom(ViewConfiguration configuration)
        {
            LastSavedView = configuration.LastSavedView;
        }

        [DataMember(Name = "viewSortOrder", EmitDefaultValue = true)]
        public ViewSortOrder ViewSortOrder { get; set; }

    }
}
