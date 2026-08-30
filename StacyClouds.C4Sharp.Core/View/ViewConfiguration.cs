using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using StacyClouds.C4Sharp.Core.View;
using StacyClouds.C4Sharp.Util;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// The configuration associated with a set of views.
    /// </summary>
    [DataContract]
    public sealed class ViewConfiguration
    {

        /// <summary>
        /// Initializes a view configuration with empty styles, branding, and terminology.
        /// </summary>
        internal ViewConfiguration()
        {
            this.Styles = new Styles();
            this.Branding = new Branding();
            this.Terminology = new Terminology();
        }

        /// <summary>
        /// Stores the style definitions shared by all views in the workspace.
        /// </summary>
        [DataMember(Name = "styles", EmitDefaultValue = false)]
        public Styles Styles { get; internal set; }

        private string[] _themes;
        
        /// <summary>
        /// Provides compatibility access to the first configured theme URL.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the supplied value is not a valid URL.</exception>
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
        
        /// <summary>
        /// Lists the theme URLs applied to the view set.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when any supplied value is not a valid URL.</exception>
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
        
        /// <summary>
        /// Stores workspace-wide branding such as fonts and logos.
        /// </summary>
        [DataMember(Name = "branding", EmitDefaultValue = false)]
        public Branding Branding { get; internal set; }

        /// <summary>
        /// Stores workspace-wide terminology overrides.
        /// </summary>
        [DataMember(Name = "terminology", EmitDefaultValue = false)]
        public Terminology Terminology { get; internal set; }

        /// <summary>
        /// The type of symbols to use when rendering metadata.
        /// </summary>
        [DataMember(Name = "metadataSymbols", EmitDefaultValue = false)]
        public MetadataSymbols? MetadataSymbols { get; set; }

        /// <summary>
        /// Stores the key of the view that should be shown by default.
        /// </summary>
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

        /// <summary>
        /// Tracks the last view key persisted by the backing workspace service.
        /// </summary>
        [DataMember(Name = "lastSavedView", EmitDefaultValue = false)]
        internal string LastSavedView { get; set; }

        /// <summary>
        /// Copies persisted configuration metadata from another view configuration.
        /// </summary>
        /// <param name="configuration">The configuration to copy from.</param>
        public void CopyConfigurationFrom(ViewConfiguration configuration)
        {
            LastSavedView = configuration.LastSavedView;
        }

        /// <summary>
        /// Controls how views are ordered when exported or displayed.
        /// </summary>
        [DataMember(Name = "viewSortOrder", EmitDefaultValue = true)]
        public ViewSortOrder ViewSortOrder { get; set; }

    }
}
