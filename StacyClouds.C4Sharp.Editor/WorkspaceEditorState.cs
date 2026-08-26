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

		public WorkspaceEditorState(Workspace workspace, string selectedViewKey = null)
		{
			this.workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));
			SelectedViewKey = selectedViewKey ?? GetViewKeys().FirstOrDefault();
		}

		public string SelectedViewKey { get; private set; }

		/// <summary>Gets the updated workspace supplied to a host save callback.</summary>
		public Workspace Workspace => workspace;

		public IEnumerable<string> GetViewKeys()
		{
			return workspace.Views.SystemLandscapeViews.Cast<View>().Concat(workspace.Views.SystemContextViews).Concat(workspace.Views.ContainerViews).Concat(workspace.Views.ComponentViews).Concat(workspace.Views.DynamicViews).Concat(workspace.Views.DeploymentViews).Select(view => view.Key).Concat(workspace.Views.FilteredViews.Select(view => view.Key)).OrderBy(key => key, StringComparer.Ordinal);
		}

		public void SelectView(string viewKey)
		{
			if (!GetViewKeys().Contains(viewKey)) throw new ArgumentException("The view does not exist in the workspace.", nameof(viewKey));
			SelectedViewKey = viewKey;
		}

		public void MoveElement(string elementId, int x, int y)
		{
			ElementView element = GetLayoutView().Elements.FirstOrDefault(candidate => candidate.Id == elementId);
			if (element == null) throw new ArgumentException("The element does not exist in the selected view.", nameof(elementId));
			element.X = x;
			element.Y = y;
		}

		public void AddRelationshipVertex(string relationshipId, int x, int y)
		{
			View view = GetLayoutView();
			RelationshipView relationship = view.Relationships.FirstOrDefault(candidate => candidate.Id == relationshipId);
			if (relationship == null) throw new ArgumentException("The relationship does not exist in the selected view.", nameof(relationshipId));
			List<Vertex> vertices = relationship.Vertices;
			vertices.Insert(FindNearestSegment(view, relationship, vertices, x, y), new Vertex(x, y));
			relationship.SetVertices(vertices);
		}

		private View GetLayoutView()
		{
			return workspace.Views.SystemLandscapeViews.Cast<View>().Concat(workspace.Views.SystemContextViews).Concat(workspace.Views.ContainerViews).Concat(workspace.Views.ComponentViews).Concat(workspace.Views.DynamicViews).Concat(workspace.Views.DeploymentViews).FirstOrDefault(view => view.Key == SelectedViewKey) ?? workspace.Views.FilteredViews.FirstOrDefault(view => view.Key == SelectedViewKey)?.View ?? throw new InvalidOperationException("The selected view does not exist.");
		}

		private static int FindNearestSegment(View view, RelationshipView relationship, List<Vertex> vertices, int x, int y)
		{
			ElementView source = view.Elements.First(element => element.Id == relationship.Relationship.Source.Id);
			ElementView destination = view.Elements.First(element => element.Id == relationship.Relationship.Destination.Id);
			List<(int X, int Y)> points = new List<(int X, int Y)> { (source.X, source.Y) };
			points.AddRange(vertices.Where(vertex => vertex.X.HasValue && vertex.Y.HasValue).Select(vertex => (vertex.X.Value, vertex.Y.Value)));
			points.Add((destination.X, destination.Y));
			return Enumerable.Range(0, points.Count - 1).OrderBy(index => DistanceToSegmentSquared(points[index], points[index + 1], x, y)).First();
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
