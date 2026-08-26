using System;
using System.Collections.Generic;
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
			List<ElementView> elements = view.Elements.Where(element => IsIncluded(element, filtered)).OrderBy(element => element.Id, StringComparer.Ordinal).ToList();
			Dictionary<string, ElementView> positioned = elements.ToDictionary(element => element.Id);
			(int minX, int minY, int width, int height) = GetBounds(view, elements);
			StringBuilder svg = new StringBuilder();
			svg.Append("<svg xmlns=\"http://www.w3.org/2000/svg\" data-c4-view-key=\"").Append(Escape(key)).Append("\" width=\"").Append(width).Append("\" height=\"").Append(height).Append("\" viewBox=\"").Append(minX).Append(' ').Append(minY).Append(' ').Append(width).Append(' ').Append(height).Append("\">");
			svg.Append("<defs><marker id=\"arrow\" markerWidth=\"10\" markerHeight=\"10\" refX=\"9\" refY=\"3\" orient=\"auto\"><path d=\"M0,0 L0,6 L9,3 z\" fill=\"#707070\" /></marker></defs>");
			svg.Append("<text x=\"20\" y=\"30\" font-family=\"Arial\" font-size=\"20\">").Append(Escape(string.IsNullOrEmpty(view.Title) ? key : view.Title)).Append("</text>");

			foreach (RelationshipView relationshipView in view.Relationships.Where(relationship => relationship.Relationship != null && positioned.ContainsKey(relationship.Relationship.Source.Id) && positioned.ContainsKey(relationship.Relationship.Destination.Id)))
			{
				ElementView source = positioned[relationshipView.Relationship.Source.Id];
				ElementView destination = positioned[relationshipView.Relationship.Destination.Id];
				int sourceX = X(source, elements.IndexOf(source));
				int sourceY = Y(source, elements.IndexOf(source));
				int destinationX = X(destination, elements.IndexOf(destination));
				int destinationY = Y(destination, elements.IndexOf(destination));
				string points = sourceX + "," + sourceY + " " + string.Join(" ", relationshipView.Vertices.Where(vertex => vertex.X.HasValue && vertex.Y.HasValue).Select(vertex => vertex.X.Value + "," + vertex.Y.Value)) + " " + destinationX + "," + destinationY;
				RelationshipStyle relationshipStyle = ResolveRelationshipStyle(relationshipView.Relationship, styles);
				string relationshipColor = relationshipStyle == null || relationshipStyle.Color == null ? "#707070" : relationshipStyle.Color;
				svg.Append("<polyline data-c4-relationship-id=\"").Append(Escape(relationshipView.Id)).Append("\" data-c4-relationship-source-id=\"").Append(Escape(relationshipView.Relationship.Source.Id)).Append("\" data-c4-relationship-destination-id=\"").Append(Escape(relationshipView.Relationship.Destination.Id)).Append("\" points=\"").Append(points).Append("\" fill=\"none\" stroke=\"").Append(relationshipColor).Append("\"");
				svg.Append(" stroke-width=\"").Append(relationshipStyle != null && relationshipStyle.Thickness.HasValue ? relationshipStyle.Thickness.Value : 2).Append("\"");
				if (relationshipStyle != null && relationshipStyle.Dashed == true) svg.Append(" stroke-dasharray=\"5,5\"");
				svg.Append(" marker-end=\"url(#arrow)\" />");
				int vertexIndex = 0;
				foreach (Vertex vertex in relationshipView.Vertices.Where(vertex => vertex.X.HasValue && vertex.Y.HasValue))
				{
					svg.Append("<circle data-c4-relationship-id=\"").Append(Escape(relationshipView.Id)).Append("\" data-c4-relationship-vertex-index=\"").Append(vertexIndex++).Append("\" cx=\"").Append(vertex.X.Value).Append("\" cy=\"").Append(vertex.Y.Value).Append("\" r=\"6\" fill=\"#ffffff\" stroke=\"").Append(relationshipColor).Append("\" stroke-width=\"2\" />");
				}

				string label = string.IsNullOrEmpty(relationshipView.Description) ? relationshipView.Relationship.Description : relationshipView.Description;
				if (!string.IsNullOrEmpty(label))
				{
					if (view is DynamicView && !string.IsNullOrEmpty(relationshipView.Order)) label = relationshipView.Order + ": " + label;
					int labelPosition = relationshipView.Position ?? 50;
					int labelX = sourceX + (destinationX - sourceX) * labelPosition / 100;
					int labelY = sourceY + (destinationY - sourceY) * labelPosition / 100 - 8;
					svg.Append("<text x=\"").Append(labelX).Append("\" y=\"").Append(labelY).Append("\" text-anchor=\"middle\" font-family=\"Arial\" font-size=\"12\" fill=\"").Append(relationshipColor).Append("\">").Append(Escape(label)).Append("</text>");
				}
			}

			foreach (ElementView element in elements)
			{
				int index = elements.IndexOf(element);
				int x = X(element, index);
				int y = Y(element, index);
				ElementStyle elementStyle = ResolveElementStyle(element.Element, styles);
				string background = elementStyle == null || elementStyle.Background == null ? "#dddddd" : elementStyle.Background;
				string stroke = elementStyle == null || elementStyle.Stroke == null ? "#707070" : elementStyle.Stroke;
				string textColor = elementStyle == null || elementStyle.Color == null ? "#000000" : elementStyle.Color;
				svg.Append("<g data-c4-element-id=\"").Append(Escape(element.Id)).Append("\">");
				if (elementStyle != null && elementStyle.Shape == Shape.Circle)
					svg.Append("<circle cx=\"").Append(x).Append("\" cy=\"").Append(y).Append("\" r=\"35\" fill=\"").Append(background).Append("\" stroke=\"").Append(stroke).Append("\" />");
				else
					svg.Append("<rect x=\"").Append(x - 75).Append("\" y=\"").Append(y - 35).Append("\" width=\"150\" height=\"70\" rx=\"8\" fill=\"").Append(background).Append("\" stroke=\"").Append(stroke).Append("\" />");
				svg.Append("<text x=\"").Append(x).Append("\" y=\"").Append(y).Append("\" text-anchor=\"middle\" font-family=\"Arial\" font-size=\"14\" fill=\"").Append(textColor).Append("\">").Append(Escape(element.Element == null ? element.Id : element.Element.Name)).Append("</text>");
				svg.Append("</g>");
			}

			return svg.Append("</svg>").ToString();
		}

		private static bool IsIncluded(ElementView element, FilteredView filtered)
		{
			if (filtered == null || element.Element == null) return true;
			bool tagged = element.Element.GetTagsAsSet().Any(tag => filtered.Tags.Contains(tag));
			return filtered.Mode == FilterMode.Include ? tagged : !tagged;
		}

		private static ElementStyle ResolveElementStyle(Element element, Styles styles)
		{
			return element == null ? null : styles.Elements.LastOrDefault(style => element.GetTagsAsSet().Contains(style.Tag));
		}

		private static RelationshipStyle ResolveRelationshipStyle(Relationship relationship, Styles styles)
		{
			return relationship == null ? null : styles.Relationships.LastOrDefault(style => relationship.GetTagsAsSet().Contains(style.Tag));
		}

		private static int X(ElementView element, int index)
		{
			return element.X == 0 && element.Y == 0 ? 120 + (index % 3) * 240 : element.X;
		}

		private static int Y(ElementView element, int index)
		{
			return element.X == 0 && element.Y == 0 ? 100 + (index / 3) * 180 : element.Y;
		}

		private static (int MinX, int MinY, int Width, int Height) GetBounds(View view, List<ElementView> elements)
		{
			int minX = 0;
			int minY = 0;
			int maxX = view.Dimensions == null ? 800 : view.Dimensions.Width;
			int maxY = view.Dimensions == null ? 600 : view.Dimensions.Height;
			for (int index = 0; index < elements.Count; index++)
			{
				int x = X(elements[index], index);
				int y = Y(elements[index], index);
				minX = Math.Min(minX, x - 75);
				minY = Math.Min(minY, y - 35);
				maxX = Math.Max(maxX, x + 175);
				maxY = Math.Max(maxY, y + 135);
			}
			foreach (Vertex vertex in view.Relationships.SelectMany(relationship => relationship.Vertices).Where(vertex => vertex.X.HasValue && vertex.Y.HasValue))
			{
				minX = Math.Min(minX, vertex.X.Value);
				minY = Math.Min(minY, vertex.Y.Value);
				maxX = Math.Max(maxX, vertex.X.Value + 100);
				maxY = Math.Max(maxY, vertex.Y.Value + 100);
			}
			return (minX, minY, maxX - minX, maxY - minY);
		}

		private static string Escape(string value)
		{
			return SecurityElement.Escape(value ?? string.Empty);
		}
	}
}
