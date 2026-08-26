using Shouldly;
using StacyClouds.C4Sharp;
using Xunit;

namespace StacyClouds.C4Sharp.Editor.Tests
{
	public class WorkspaceEditorStateTests
	{
		[Fact]
		public void SelectView_UsesTheRequestedWorkspaceView()
		{
			Workspace workspace = new Workspace("Test", "Description");
			workspace.Views.CreateSystemLandscapeView("first", "First");
			workspace.Views.CreateDynamicView("second", "Second");
			WorkspaceEditorState state = new WorkspaceEditorState(workspace, "second");

			state.SelectedViewKey.ShouldBe("second");
			state.SelectView("first");
			state.SelectedViewKey.ShouldBe("first");
		}

		[Fact]
		public void MoveElement_PersistsCoordinatesAndInsertsVerticesByNearestSegment()
		{
			Workspace workspace = new Workspace("Test", "Description");
			SoftwareSystem source = workspace.Model.AddSoftwareSystem("Source", "Description");
			SoftwareSystem destination = workspace.Model.AddSoftwareSystem("Destination", "Description");
			Relationship relationship = source.Uses(destination, "Calls");
			SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
			view.AddAllSoftwareSystems();
			view.GetElementView(source).X = 100;
			view.GetElementView(source).Y = 100;
			view.GetElementView(destination).X = 500;
			view.GetElementView(destination).Y = 300;
			view.GetRelationshipView(relationship).AddVertex(new Vertex(300, 100));
			WorkspaceEditorState state = new WorkspaceEditorState(workspace, view.Key);

			state.MoveElement(source.Id, 140, 160);
			state.AddRelationshipVertex(relationship.Id, 320, 250);

			view.GetElementView(source).X.ShouldBe(140);
			view.GetElementView(source).Y.ShouldBe(160);
			view.GetRelationshipView(relationship).Vertices.Count.ShouldBe(2);
			view.GetRelationshipView(relationship).Vertices[1].X.ShouldBe(320);
			view.GetRelationshipView(relationship).Vertices[1].Y.ShouldBe(250);
		}

		[Fact]
		public void Workspace_ReturnsTheEditedWorkspaceForTheHostSaveHandler()
		{
			Workspace workspace = new Workspace("Test", "Description");
			WorkspaceEditorState state = new WorkspaceEditorState(workspace);

			state.Workspace.ShouldBeSameAs(workspace);
		}

		[Fact]
		public void MoveAndRemoveRelationshipVertex_PersistTheRequestedVertexMutation()
		{
			Workspace workspace = new Workspace("Test", "Description");
			SoftwareSystem source = workspace.Model.AddSoftwareSystem("Source", "Description");
			SoftwareSystem destination = workspace.Model.AddSoftwareSystem("Destination", "Description");
			Relationship relationship = source.Uses(destination, "Calls");
			SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
			view.AddAllSoftwareSystems();
			view.GetRelationshipView(relationship).SetVertices(new[] { new Vertex(200, 150) });
			WorkspaceEditorState state = new WorkspaceEditorState(workspace, view.Key);

			state.MoveRelationshipVertex(relationship.Id, 0, 250, 175);
			view.GetRelationshipView(relationship).Vertices[0].X.ShouldBe(250);
			view.GetRelationshipView(relationship).Vertices[0].Y.ShouldBe(175);
			state.RemoveRelationshipVertex(relationship.Id, 0);

			view.GetRelationshipView(relationship).Vertices.Count.ShouldBe(0);
		}

		[Fact]
		public void MoveRelationshipLabel_PersistsItsPositionAlongTheConnectorPath()
		{
			Workspace workspace = new Workspace("Test", "Description");
			SoftwareSystem source = workspace.Model.AddSoftwareSystem("Source", "Description");
			SoftwareSystem destination = workspace.Model.AddSoftwareSystem("Destination", "Description");
			Relationship relationship = source.Uses(destination, "Calls");
			SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
			view.AddAllSoftwareSystems();
			view.GetElementView(source).X = 100;
			view.GetElementView(source).Y = 100;
			view.GetElementView(destination).X = 500;
			view.GetElementView(destination).Y = 100;
			WorkspaceEditorState state = new WorkspaceEditorState(workspace, view.Key);

			state.MoveRelationshipLabel(relationship.Id, 400, 100);

			view.GetRelationshipView(relationship).Position.ShouldBe(75);
		}
	}
}
