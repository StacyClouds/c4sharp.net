using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;

namespace StacyClouds.C4Sharp.Renderer
{
    /// <summary>Renders the views in a workspace as standalone SVG documents.</summary>
    public sealed class SvgWorkspaceRenderer
    {
        public IReadOnlyDictionary<string, string> Render(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException(nameof(workspace));

            Dictionary<string, string> diagrams = new Dictionary<string, string>();
            foreach (View view in GetViews(workspace))
            {
                diagrams.Add(view.Key, RenderView(view, view.Key, view.Description, workspace.Views.Configuration.Styles));
            }
            foreach (FilteredView filtered in workspace.Views.FilteredViews)
            {
                if (filtered.View == null) throw new InvalidOperationException("Filtered view '" + filtered.Key + "' has no base view.");
                diagrams.Add(filtered.Key, RenderView(filtered.View, filtered.Key, filtered.Description, workspace.Views.Configuration.Styles, filtered));
            }
            return diagrams;
        }

        private static IEnumerable<View> GetViews(Workspace workspace)
        {
            return workspace.Views.SystemLandscapeViews.Cast<View>()
                .Concat(workspace.Views.SystemContextViews)
                .Concat(workspace.Views.ContainerViews)
                .Concat(workspace.Views.ComponentViews)
                .Concat(workspace.Views.DynamicViews)
                .Concat(workspace.Views.DeploymentViews)
                .OrderBy(view => view.Key, StringComparer.Ordinal);
        }

        private static string RenderView(View view, string key, string description, Styles styles, FilteredView filtered = null)
        {
            bool needsLayout = elements.Count > 0 && elements.All(element => element.X == 0 && element.Y == 0);
                ? Math.Max(800, elements.Select((element, index) => X(element, index, needsLayout) + 175).DefaultIfEmpty(800).Max())
                : view.Dimensions.Width;
            int height = view.Dimensions == null
                ? Math.Max(600, elements.Select((element, index) => Y(element, index, needsLayout) + 135).DefaultIfEmpty(600).Max())
                : view.Dimensions.Height;
            StringBuilder svg = new StringBuilder();
            svg.Append("<defs><marker id=\"arrow\" markerWidth=\"10\" markerHeight=\"10\" refX=\"9\" refY=\"3\" orient=\"auto\"><path d=\"M0,0 L0,6 L9,3 z\" fill=\"context-stroke\" /></marker></defs>");
            foreach (RelationshipView relationshipView in view.Relationships.Where(relationship => relationship.Relationship != null && positioned.ContainsKey(relationship.Relationship.Source.Id) && positioned.ContainsKey(relationship.Relationship.Destination.Id)))
            {
                ElementView source = positioned[relationshipView.Relationship.Source.Id];
                ElementView destination = positioned[relationshipView.Relationship.Destination.Id];
                int sourceX = X(source, elements.IndexOf(source), needsLayout); int sourceY = Y(source, elements.IndexOf(source), needsLayout);
                int destinationX = X(destination, elements.IndexOf(destination), needsLayout); int destinationY = Y(destination, elements.IndexOf(destination), needsLayout);
                string points = sourceX + "," + sourceY + " " + string.Join(" ", relationshipView.Vertices.Where(vertex => vertex.X.HasValue && vertex.Y.HasValue).Select(vertex => vertex.X.Value + "," + vertex.Y.Value)) + " " + destinationX + "," + destinationY;
                RelationshipStyle relationshipStyle = ResolveRelationshipStyle(relationshipView.Relationship, styles);
                string relationshipColor = relationshipStyle == null || relationshipStyle.Color == null ? "#707070" : relationshipStyle.Color;
                svg.Append("<polyline points=\"").Append(points).Append("\" fill=\"none\" stroke=\"").Append(relationshipColor).Append("\"");
                if (relationshipStyle != null && relationshipStyle.Thickness.HasValue) svg.Append(" stroke-width=\"").Append(relationshipStyle.Thickness.Value).Append("\"");
                if (relationshipStyle != null && relationshipStyle.Dashed == true) svg.Append(" stroke-dasharray=\"5,5\"");
                svg.Append(" marker-end=\"url(#arrow)\" />");
                string label = string.IsNullOrEmpty(relationshipView.Description) ? relationshipView.Relationship.Description : relationshipView.Description;
                if (!string.IsNullOrEmpty(label))
                {
                    if (view is DynamicView && !string.IsNullOrEmpty(relationshipView.Order)) label = relationshipView.Order + ": " + label;
                    int labelPosition = relationshipView.Position ?? 50;
                    svg.Append("<text x=\"").Append(labelX).Append("\" y=\"").Append(labelY).Append("\" text-anchor=\"middle\" font-family=\"Arial\" font-size=\"12\" fill=\"").Append(relationshipColor).Append("\">").Append(Escape(label)).Append("</text>");
            }
            foreach (ElementView element in elements)
            {
                int index = elements.IndexOf(element); int x = X(element, index, needsLayout); int y = Y(element, index, needsLayout);
                ElementStyle elementStyle = ResolveElementStyle(element.Element, styles);
                string background = elementStyle == null || elementStyle.Background == null ? "#dddddd" : elementStyle.Background;
                string stroke = elementStyle == null || elementStyle.Stroke == null ? "#707070" : elementStyle.Stroke;
                string textColor = elementStyle == null || elementStyle.Color == null ? "#000000" : elementStyle.Color;
                if (elementStyle != null && elementStyle.Shape == Shape.Circle)
                    svg.Append("<circle cx=\"").Append(x).Append("\" cy=\"").Append(y).Append("\" r=\"35\" fill=\"").Append(background).Append("\" stroke=\"").Append(stroke).Append("\" />");
                else
                    svg.Append("<rect x=\"").Append(x - 75).Append("\" y=\"").Append(y - 35).Append("\" width=\"150\" height=\"70\" rx=\"8\" fill=\"").Append(background).Append("\" stroke=\"").Append(stroke).Append("\" />");
                svg.Append("<text x=\"").Append(x).Append("\" y=\"").Append(y).Append("\" text-anchor=\"middle\" font-family=\"Arial\" font-size=\"14\" fill=\"").Append(textColor).Append("\">").Append(Escape(element.Element == null ? element.Id : element.Element.Name)).Append("</text>");
            return svg.Append("</svg>").ToString();
        }

        private static bool IsIncluded(ElementView element, FilteredView filtered)
        {
            if (filtered == null || element.Element == null) return true;
            bool tagged = element.Element.GetTagsAsSet().Any(tag => filtered.Tags.Contains(tag));
            return filtered.Mode == FilterMode.Include ? tagged : !tagged;
        }
        private static ElementStyle ResolveElementStyle(Element element, Styles styles) => element == null ? null : styles.Elements.LastOrDefault(style => element.GetTagsAsSet().Contains(style.Tag));
        private static RelationshipStyle ResolveRelationshipStyle(Relationship relationship, Styles styles) => relationship == null ? null : styles.Relationships.LastOrDefault(style => relationship.GetTagsAsSet().Contains(style.Tag));
        private static int X(ElementView element, int index, bool fallback) => fallback ? 120 + (index % 3) * 240 : element.X;
        private static int Y(ElementView element, int index, bool fallback) => fallback ? 100 + (index / 3) * 180 : element.Y;
        private static string Escape(string value) => SecurityElement.Escape(value ?? string.Empty);
    }
}
