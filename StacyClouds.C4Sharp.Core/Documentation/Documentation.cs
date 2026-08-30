using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace StacyClouds.C4Sharp.Documentation
{

    /// <summary>
    /// Represents the documentation within a workspace - a collection of
    /// content in Markdown or AsciiDoc format, optionally with attached images.
    ///
    /// See https://structurizr.com/help/documentation on the Structurizr website for more details.
    /// </summary>
    [DataContract]
    public sealed class Documentation
    {

        /// <summary>
        /// Points to the model that documentation sections and decisions reference.
        /// </summary>
        public Model Model { get; set; }

        /// <summary>
        /// Stores the ordered documentation sections attached to the workspace and its elements.
        /// </summary>
        [DataMember(Name = "sections", EmitDefaultValue = false)]
        public ISet<Section> Sections { get; internal set; }

        /// <summary>
        /// Stores the architecture decisions attached to the workspace and its elements.
        /// </summary>
        [DataMember(Name = "decisions", EmitDefaultValue = false)]
        public ISet<Decision> Decisions { get; internal set; }

        /// <summary>
        /// Stores images that can be referenced from documentation content.
        /// </summary>
        [DataMember(Name = "images", EmitDefaultValue = false)]
        public ISet<Image> Images { get; internal set; }

        /// <summary>
        /// Initializes empty documentation collections for serializers and new instances.
        /// </summary>
        [JsonConstructor]
        internal Documentation()
        {
            Sections = new HashSet<Section>();
            Decisions = new HashSet<Decision>();
            Images = new HashSet<Image>();
        }

        /// <summary>
        /// Creates documentation bound to a specific model.
        /// </summary>
        /// <param name="model">The model that owns any documented elements.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="model"/> is <c>null</c>.</exception>
        public Documentation(Model model) : this()
        {
            if (model == null)
            {
                throw new ArgumentException("A model must be specified.");
            }
            
            Model = model;
        }

        /// <summary>
        /// Resolves stored element identifiers back to model element references.
        /// </summary>
        public void Hydrate()
        {
            foreach (Section section in Sections)
            {
                if (!string.IsNullOrWhiteSpace(section.ElementId))
                {
                    section.Element = Model.GetElement(section.ElementId);
                }
            }

            foreach (Decision decision in Decisions)
            {
                if (!string.IsNullOrWhiteSpace(decision.ElementId))
                {
                    decision.Element = Model.GetElement(decision.ElementId);
                }
            }
        }

        /// <summary>
        /// Creates a new documentation section for the workspace or a specific element.
        /// </summary>
        /// <param name="element">The owning element, or <c>null</c> for a workspace-level section.</param>
        /// <param name="title">The section title.</param>
        /// <param name="format">The format of <paramref name="content"/>.</param>
        /// <param name="content">The section body.</param>
        /// <returns>The section that was added.</returns>
        /// <exception cref="ArgumentException">Thrown when the element is not in the model, when required values are missing, or when the scoped title already exists.</exception>
        internal Section AddSection(Element element, string title, Format format, string content)
        {
            if (element != null && !Model.Contains(element))
            {
                throw new ArgumentException("The element named " + element.Name + " does not exist in the model associated with this documentation.");
            }

            CheckTitleIsSpecified(title);
            CheckContentIsSpecified(content);
            CheckSectionIsUnique(element, title);
            CheckFormatIsSpecified(format);

            Section section = new Section(element, title, CalculateOrder(), format, content);
            Sections.Add(section);
            return section;
        }

        private void CheckTitleIsSpecified(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("A title must be specified.");
            }
        }

        private void CheckContentIsSpecified(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Content must be specified.");
            }
        }

        private void CheckFormatIsSpecified(Format format)
        {
            if (format == null)
            {
                throw new ArgumentException("A format must be specified.");
            }
        }

        private void CheckSectionIsUnique(Element element, String title)
        {
            if (element == null)
            {
                foreach (Section section in Sections)
                {
                    if (section.Element == null && title.Equals(section.Title))
                    {
                        throw new ArgumentException("A section with a title of " + title + " already exists for this workspace.");
                    }
                }
            }
            else
            {
                foreach (Section section in Sections)
                {
                    if (element.Id.Equals(section.ElementId) && title.Equals(section.Title))
                    {
                        throw new ArgumentException("A section with a title of " + title + " already exists for the element named " + element.Name + ".");
                    }
                }
            }
        }

        /// <summary>
        /// Adds a workspace-level architecture decision.
        /// </summary>
        /// <param name="id">The unique decision identifier.</param>
        /// <param name="date">The date associated with the decision.</param>
        /// <param name="title">The decision title.</param>
        /// <param name="status">The decision status.</param>
        /// <param name="format">The format of <paramref name="content"/>.</param>
        /// <param name="content">The decision body.</param>
        /// <returns>The decision that was added.</returns>
        /// <exception cref="ArgumentException">Thrown when required values are missing or when the identifier already exists at workspace scope.</exception>
        public Decision AddDecision(string id, DateTime date, string title, DecisionStatus status, Format format, string content)
        {
            return AddDecision(null, id, date, title, status, format, content);
        }

        /// <summary>
        /// Adds an architecture decision scoped to a software system.
        /// </summary>
        /// <param name="softwareSystem">The software system that owns the decision.</param>
        /// <param name="id">The unique decision identifier within the software system.</param>
        /// <param name="date">The date associated with the decision.</param>
        /// <param name="title">The decision title.</param>
        /// <param name="status">The decision status.</param>
        /// <param name="format">The format of <paramref name="content"/>.</param>
        /// <param name="content">The decision body.</param>
        /// <returns>The decision that was added.</returns>
        /// <exception cref="ArgumentException">Thrown when required values are missing or when the identifier already exists for the software system.</exception>
        public Decision AddDecision(SoftwareSystem softwareSystem, string id, DateTime date, string title, DecisionStatus status, Format format, string content)
        {
            CheckIdIsSpecified(id);
            CheckTitleIsSpecified(title);
            CheckContentIsSpecified(content);
            CheckDecisionStatusIsSpecified(status);
            CheckFormatIsSpecified(format);
            CheckDecisionIsUnique(softwareSystem, id);

            Decision decision = new Decision(softwareSystem, id, date, title, status, format, content);
            Decisions.Add(decision);

            return decision;
        }

        private void CheckIdIsSpecified(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("An ID must be specified.");
            }
        }

        private void CheckDecisionStatusIsSpecified(DecisionStatus status)
        {
            if (status == null)
            {
                throw new ArgumentException("A status must be specified.");
            }
        }

        private void CheckDecisionIsUnique(Element element, string id)
        {
            if (element == null)
            {
                foreach (Decision decision in Decisions)
                {
                    if (decision.Element == null && id.Equals(decision.Id))
                    {
                        throw new ArgumentException("A decision with an ID of " + id + " already exists for this workspace.");
                    }
                }
            }
            else
            {
                foreach (Decision decision in Decisions)
                {
                    if (element.Id.Equals(decision.ElementId) && id.Equals(decision.Id))
                    {
                        throw new ArgumentException("A decision with an ID of " + id + " already exists for the element named " + element.Name + ".");
                    }
                }
            }
        }


        private int CalculateOrder()
        {
            return Sections.Count+1;
        }

        /// <summary>
        /// Adds an image asset to the documentation collection.
        /// </summary>
        /// <param name="image">The image to store.</param>
        internal void Add(Image image)
        {
            Images.Add(image);
        }
        
        /// <summary>
        /// Indicates whether the documentation has any sections or images.
        /// </summary>
        /// <returns><c>true</c> when no sections and no images are present; otherwise, <c>false</c>.</returns>
        public bool IsEmpty()
        {
            return Sections.Count == 0 && Images.Count == 0;
        }

    }

}