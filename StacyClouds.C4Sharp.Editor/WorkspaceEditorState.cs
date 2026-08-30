using System;
using System.Collections.Generic;
using System.Linq;
using StacyClouds.C4Sharp;

namespace StacyClouds.C4Sharp.Editor
{
	/// <summary>Mutates persisted layout information for the currently selected workspace view.</summary>
	public sealed class WorkspaceEditorState
	{
		private readonly Workspace workspace;

		/// <summary>
		/// Creates editing state for a workspace and selects an initial view.
		/// </summary>
		/// <param name="workspace">The workspace whose layout should be edited.</param>
		/// <param name="selectedViewKey">The initially selected view key, or <c>null</c> to select the first available view.</param>
		public WorkspaceEditorState(Workspace workspace, string? selectedViewKey = null)
		{
			this.workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));
			SelectedViewKey = selectedViewKey ?? GetViewKeys().FirstOrDefault() ?? string.Empty;
		}

		/// <summary>
		/// Identifies the view currently being edited.
		/// </summary>
		public string SelectedViewKey { get; private set; }

		/// <summary>Gets the updated workspace supplied to a host save callback.</summary>
		public Workspace Workspace => workspace;

		/// <summary>
		/// Lists the keys of all editable views in the workspace.
		/// </summary>
		/// <returns>The ordered set of editable view keys.</returns>
		public IEnumerable<string> GetViewKeys()
		{
			return workspace.Views.SystemLandscapeViews.Cast<View>().Concat(workspace.Views.SystemContextViews).Concat(workspace.Views.ContainerViews).Concat(workspace.Views.ComponentViews).Concat(workspace.Views.DynamicViews).Concat(workspace.Views.DeploymentViews).Select(view => view.Key).Concat(workspace.Views.FilteredViews.Select(view => view.Key)).OrderBy(key => key, StringComparer.Ordinal);
		}

		/// <summary>
		/// Changes the selected view.
		/// </summary>
		/// <param name="viewKey">The key of the view to edit.</param>
		/// <exception cref="ArgumentException">Thrown when <paramref name="viewKey"/> is missing or does not exist in the workspace.</exception>
		public void SelectView(string viewKey)
		{
			if (string.IsNullOrWhiteSpace(viewKey)) throw new ArgumentException("View key must be provided.", nameof(viewKey));
			if (!GetViewKeys().Contains(viewKey, StringComparer.Ordinal)) throw new ArgumentException("The view does not exist in the workspace.", nameof(viewKey));
			SelectedViewKey = viewKey;
		}

		/// <summary>
		/// Moves an element within the selected view.
		/// </summary>
		/// <param name="elementId">The identifier of the element to move.</param>
		/// <param name="x">The new X coordinate.</param>
		/// <param name="y">The new Y coordinate.</param>
		/// <exception cref="ArgumentException">Thrown when the element does not exist in the selected view.</exception>
		public void MoveElement(string elementId, int x, int y)
		{
			ElementView element = GetLayoutView().Elements.FirstOrDefault(candidate => candidate.Id == elementId);
			if (element == null) throw new ArgumentException("The element does not exist in the selected view.", nameof(elementId));
			element.X = x;
			element.Y = y;
		}

		/// <summary>
		/// Inserts a relationship vertex at the nearest segment within the selected view.
		/// </summary>
		/// <param name="relationshipId">The identifier of the relationship to edit.</param>
		/// <param name="x">The X coordinate of the new vertex.</param>
		/// <param name="y">The Y coordinate of the new vertex.</param>
		/// <exception cref="ArgumentException">Thrown when the relationship does not exist in the selected view.</exception>
		public void AddRelationshipVertex(string relationshipId, int x, int y)
		{
			View view = GetLayoutView();
			RelationshipView relationship = view.Relationships.FirstOrDefault(candidate => candidate.Id == relationshipId);
			if (relationship == null) throw new ArgumentException("The relationship does not exist in the selected view.", nameof(relationshipId));
			List<Vertex> vertices = relationship.Vertices;
			vertices.Insert(FindNearestSegment(view, relationship, vertices, x, y), new Vertex(x, y));
			relationship.SetVertices(vertices);
		}

		/// <summary>
		/// Moves an existing relationship vertex within the selected view.
		/// </summary>
		/// <param name="relationshipId">The identifier of the relationship to edit.</param>
		/// <param name="index">The zero-based vertex index.</param>
		/// <param name="x">The new X coordinate.</param>
		/// <param name="y">The new Y coordinate.</param>
		/// <exception cref="ArgumentException">Thrown when the relationship does not exist in the selected view.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> does not identify an existing vertex.</exception>
		public void MoveRelationshipVertex(string relationshipId, int index, int x, int y)
		{
			RelationshipView relationship = GetRelationship(relationshipId);
			List<Vertex> vertices = relationship.Vertices;
			if (index < 0 || index >= vertices.Count) throw new ArgumentOutOfRangeException(nameof(index));
			vertices[index] = new Vertex(x, y);
			relationship.SetVertices(vertices);
		}

		/// <summary>
		/// Removes an existing relationship vertex from the selected view.
		/// </summary>
		/// <param name="relationshipId">The identifier of the relationship to edit.</param>
		/// <param name="index">The zero-based vertex index.</param>
		/// <exception cref="ArgumentException">Thrown when the relationship does not exist in the selected view.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> does not identify an existing vertex.</exception>
		public void RemoveRelationshipVertex(string relationshipId, int index)
		{
			RelationshipView relationship = GetRelationship(relationshipId);
			List<Vertex> vertices = relationship.Vertices;
			if (index < 0 || index >= vertices.Count) throw new ArgumentOutOfRangeException(nameof(index));
			vertices.RemoveAt(index);
			relationship.SetVertices(vertices);
		}

		/// <summary>
		/// Repositions a relationship label along the selected view's connector path.
		/// </summary>
		/// <param name="relationshipId">The identifier of the relationship to edit.</param>
		/// <param name="x">The target X coordinate used to project the label onto the connector.</param>
		/// <param name="y">The target Y coordinate used to project the label onto the connector.</param>
		/// <exception cref="ArgumentException">Thrown when the relationship does not exist in the selected view.</exception>
		public void MoveRelationshipLabel(string relationshipId, int x, int y)
		{
			View view = GetLayoutView();
			RelationshipView relationship = GetRelationship(relationshipId);
			List<ElementView> elements = view.Elements.OrderBy(element => element.Id, StringComparer.Ordinal).ToList();
			ElementView source = elements.First(element => element.Id == relationship.Relationship.Source.Id);
			ElementView destination = elements.First(element => element.Id == relationship.Relationship.Destination.Id);
			List<(int X, int Y)> points = new List<(int X, int Y)> { PositionOf(source, elements.IndexOf(source)) };
			points.AddRange(relationship.Vertices.Where(vertex => vertex.X.HasValue && vertex.Y.HasValue).Select(vertex => (vertex.X.Value, vertex.Y.Value)));
			points.Add(PositionOf(destination, elements.IndexOf(destination)));
			double totalLength = Enumerable.Range(0, points.Count - 1).Sum(index => Length(points[index], points[index + 1]));
			double distanceBefore = 0;
			double bestDistance = double.MaxValue;
			double bestPosition = 0;
			for (int index = 0; index < points.Count - 1; index++)
			{
				double length = Length(points[index], points[index + 1]);
				double t = Projection(points[index], points[index + 1], x, y);
				double projectedX = points[index].X + (points[index + 1].X - points[index].X) * t;
				double projectedY = points[index].Y + (points[index + 1].Y - points[index].Y) * t;
				double distance = Math.Pow(x - projectedX, 2) + Math.Pow(y - projectedY, 2);
				if (distance < bestDistance) { bestDistance = distance; bestPosition = distanceBefore + length * t; }
				distanceBefore += length;
			}
			relationship.Position = totalLength == 0 ? 0 : (int)Math.Round(bestPosition * 100 / totalLength);
		}

		private RelationshipView GetRelationship(string relationshipId)
		{
			RelationshipView relationship = GetLayoutView().Relationships.FirstOrDefault(candidate => candidate.Id == relationshipId);
			return relationship ?? throw new ArgumentException("The relationship does not exist in the selected view.", nameof(relationshipId));
		}

		private static (int X, int Y) PositionOf(ElementView element, int index)
		{
			return element.X == 0 && element.Y == 0 ? (120 + (index % 3) * 240, 100 + (index / 3) * 180) : (element.X, element.Y);
		}

		private static double Length((int X, int Y) start, (int X, int Y) end)
		{
			return Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
		}

		private static double Projection((int X, int Y) start, (int X, int Y) end, int x, int y)
		{
			double dx = end.X - start.X; double dy = end.Y - start.Y;
			return dx == 0 && dy == 0 ? 0 : Math.Max(0, Math.Min(1, ((x - start.X) * dx + (y - start.Y) * dy) / (dx * dx + dy * dy)));
		}

		private View GetLayoutView()
		{
			return workspace.Views.SystemLandscapeViews.Cast<View>().Concat(workspace.Views.SystemContextViews).Concat(workspace.Views.ContainerViews).Concat(workspace.Views.ComponentViews).Concat(workspace.Views.DynamicViews).Concat(workspace.Views.DeploymentViews).FirstOrDefault(view => view.Key == SelectedViewKey) ?? workspace.Views.FilteredViews.FirstOrDefault(view => view.Key == SelectedViewKey)?.View ?? throw new InvalidOperationException("The selected view does not exist.");
		}

		private static int FindNearestSegment(View view, RelationshipView relationship, List<Vertex> vertices, int x, int y)
		{
			List<ElementView> elements = view.Elements.OrderBy(element => element.Id, StringComparer.Ordinal).ToList();
			ElementView source = elements.First(element => element.Id == relationship.Relationship.Source.Id);
			ElementView destination = elements.First(element => element.Id == relationship.Relationship.Destination.Id);

			List<(int X, int Y)> points = new List<(int X, int Y)> { PositionOf(source, elements.IndexOf(source)) };
			points.AddRange(vertices.Where(vertex => vertex.X.HasValue && vertex.Y.HasValue).Select(vertex => (vertex.X.Value, vertex.Y.Value)));
			points.Add(PositionOf(destination, elements.IndexOf(destination)));

			return Enumerable.Range(0, points.Count - 1)
				.OrderBy(index => DistanceToSegmentSquared(points[index], points[index + 1], x, y))
				.First();
		}

		private static double DistanceToSegmentSquared((int X, int Y) start, (int X, int Y) end, int x, int y)
		{
			double dx = end.X - start.X; double dy = end.Y - start.Y;
			if (dx == 0 && dy == 0) return Math.Pow(x - start.X, 2) + Math.Pow(y - start.Y, 2);
			double t = Math.Max(0, Math.Min(1, ((x - start.X) * dx + (y - start.Y) * dy) / (dx * dx + dy * dy)));
			return Math.Pow(x - (start.X + t * dx), 2) + Math.Pow(y - (start.Y + t * dy), 2);
		}
	}
}
