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
		/// <summary>
		/// Copies reusable layout from a predecessor workspace into the updated workspace, then renders its views as standalone SVG documents.
		/// </summary>
		/// <param name="workspace">The updated workspace to render and mutate with matching predecessor layout.</param>
		/// <param name="predecessor">The read-only workspace that supplies saved layout information.</param>
		/// <returns>A dictionary of SVG documents keyed by view key.</returns>
		public IReadOnlyDictionary<string, string> Render(Workspace workspace, Workspace predecessor)
		{
			if (workspace == null) throw new ArgumentNullException(nameof(workspace));
			if (predecessor == null) throw new ArgumentNullException(nameof(predecessor));

			workspace.Views.CopyLayoutInformationFrom(predecessor.Views);
			return Render(workspace);
		}

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
				List<(int X, int Y)> connectorPoints = new List<(int X, int Y)> { (sourceX, sourceY) };
				connectorPoints.AddRange(relationshipView.Vertices.Where(vertex => vertex.X.HasValue && vertex.Y.HasValue).Select(vertex => (vertex.X.Value, vertex.Y.Value)));
				connectorPoints.Add((destinationX, destinationY));
				RelationshipStyle relationshipStyle = ResolveRelationshipStyle(relationshipView.Relationship, styles);
				List<(int X, int Y)> visibleConnectorPoints = new List<(int X, int Y)>(connectorPoints);
				visibleConnectorPoints[0] = EdgeOf(sourceX, sourceY, connectorPoints[1], ResolveElementStyle(source.Element, styles));
				visibleConnectorPoints[visibleConnectorPoints.Count - 1] = EdgeOf(destinationX, destinationY, connectorPoints[connectorPoints.Count - 2], ResolveElementStyle(destination.Element, styles));
				string points = string.Join(" ", connectorPoints.Select(point => point.X + "," + point.Y));
				string visiblePoints = string.Join(" ", visibleConnectorPoints.Select(point => point.X + "," + point.Y));
				string relationshipColor = relationshipStyle == null || relationshipStyle.Color == null ? "#707070" : relationshipStyle.Color;
				string relationshipData = "data-c4-relationship-id=\"" + Escape(relationshipView.Id) + "\" data-c4-relationship-source-id=\"" + Escape(relationshipView.Relationship.Source.Id) + "\" data-c4-relationship-destination-id=\"" + Escape(relationshipView.Relationship.Destination.Id) + "\"";
				svg.Append("<polyline ").Append(relationshipData).Append(" data-c4-relationship-visible=\"true\" points=\"").Append(visiblePoints).Append("\" fill=\"none\" stroke=\"").Append(relationshipColor).Append("\"");
				svg.Append(" stroke-width=\"").Append(relationshipStyle != null && relationshipStyle.Thickness.HasValue ? relationshipStyle.Thickness.Value : 2).Append("\"");
				if (relationshipStyle != null && relationshipStyle.Dashed == true) svg.Append(" stroke-dasharray=\"5,5\"");
				svg.Append(" marker-end=\"url(#arrow)\" />");
				svg.Append("<polyline ").Append(relationshipData).Append(" data-c4-relationship-interaction=\"true\" points=\"").Append(points).Append("\" fill=\"none\" stroke=\"transparent\" stroke-width=\"12\" />");
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
					(int labelX, int labelY) = PointOnPolyline(connectorPoints, labelPosition);
					svg.Append("<text data-c4-relationship-label-id=\"").Append(Escape(relationshipView.Id)).Append("\" data-c4-relationship-label-position=\"").Append(labelPosition).Append("\" x=\"").Append(labelX).Append("\" y=\"").Append(labelY - 8).Append("\" text-anchor=\"middle\" font-family=\"Arial\" font-size=\"12\" fill=\"").Append(relationshipColor).Append("\">").Append(Escape(label)).Append("</text>");
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
				List<string> labelLines = WrapElementLabel(element.Element == null ? element.Id : element.Element.Name);
				int firstLabelY = y - (labelLines.Count - 1) * 8;
				svg.Append("<text text-anchor=\"middle\" font-family=\"Arial\" font-size=\"14\" fill=\"").Append(textColor).Append("\">");
				for (int lineIndex = 0; lineIndex < labelLines.Count; lineIndex++)
					svg.Append("<tspan x=\"").Append(x).Append("\" y=\"").Append(firstLabelY + lineIndex * 16).Append("\">").Append(Escape(labelLines[lineIndex])).Append("</tspan>");
				svg.Append("</text>");
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

		private static (int X, int Y) PointOnPolyline(List<(int X, int Y)> points, int position)
		{
			double totalLength = Enumerable.Range(0, points.Count - 1).Sum(index => Math.Sqrt(Math.Pow(points[index + 1].X - points[index].X, 2) + Math.Pow(points[index + 1].Y - points[index].Y, 2)));
			if (totalLength == 0) return points[0];
			double remaining = totalLength * position / 100d;
			for (int index = 0; index < points.Count - 1; index++)
			{
				double dx = points[index + 1].X - points[index].X;
				double dy = points[index + 1].Y - points[index].Y;
				double length = Math.Sqrt(dx * dx + dy * dy);
				if (length == 0) continue;
				if (remaining <= length) return ((int)(points[index].X + dx * remaining / length), (int)(points[index].Y + dy * remaining / length));
				remaining -= length;
			}
			return points[points.Count - 1];
		}

		private static (int X, int Y) EdgeOf(int centerX, int centerY, (int X, int Y) toward, ElementStyle style)
		{
			double dx = toward.X - centerX;
			double dy = toward.Y - centerY;
			if (dx == 0 && dy == 0) return (centerX, centerY);
			double scale = style != null && style.Shape == Shape.Circle
				? 35 / Math.Sqrt(dx * dx + dy * dy)
				: 1 / Math.Max(Math.Abs(dx) / 75, Math.Abs(dy) / 35);
			return ((int)Math.Round(centerX + dx * scale), (int)Math.Round(centerY + dy * scale));
		}

		private static List<string> WrapElementLabel(string label)
		{
			const int maximumLineLength = 20;
			const int maximumLines = 3;
			List<string> lines = new List<string>();
			string current = string.Empty;
			foreach (string originalWord in (label ?? string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
			{
				string word = originalWord;
				while (word.Length > maximumLineLength)
				{
					if (current.Length > 0) { lines.Add(current); current = string.Empty; }
					lines.Add(word.Substring(0, maximumLineLength));
					word = word.Substring(maximumLineLength);
				}
				if (current.Length == 0) current = word;
				else if (current.Length + 1 + word.Length <= maximumLineLength) current += " " + word;
				else { lines.Add(current); current = word; }
			}
			if (current.Length > 0) lines.Add(current);
			if (lines.Count == 0) lines.Add(string.Empty);
			if (lines.Count <= maximumLines) return lines;
			List<string> boundedLines = lines.Take(maximumLines).ToList();
			boundedLines[maximumLines - 1] = boundedLines[maximumLines - 1].Substring(0, maximumLineLength - 1) + "…";
			return boundedLines;
		}

		private static string Escape(string value)
		{
			return SecurityElement.Escape(value ?? string.Empty);
		}
	}
}
