using Bunit;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Shouldly;
using StacyClouds.C4Sharp;
using Xunit;

namespace StacyClouds.C4Sharp.Editor.Tests
{
	public class WorkspaceEditorComponentTests : TestContext
	{
		[Fact]
		public void ViewThumbnail_ReportsItsViewKeyWhenClicked()
		{
			string? selected = null;
			IRenderedComponent<ViewThumbnail> cut = Render<ViewThumbnail>(parameters => parameters
				.Add(component => component.ViewKey, "view")
				.Add(component => component.SelectedChanged, EventCallback.Factory.Create<string>(this, value => selected = value)));

			cut.Find("button").Click();

			selected.ShouldBe("view");
		}

		[Fact]
		public async Task WorkspaceEditor_LoadsTheInitiallySelectedViewAndReportsThumbnailSelection()
		{
			Workspace workspace = new Workspace("Test", "Description");
			workspace.Views.CreateSystemLandscapeView("first", "First");
			workspace.Views.CreateDynamicView("second", "Second");
			string? selected = null;
			JSInterop.SetupVoid("c4sharpEditor.initialize", _ => true);
			IRenderedComponent<WorkspaceEditor> cut = Render<WorkspaceEditor>(parameters => parameters
				.Add(component => component.Workspace, workspace)
				.Add(component => component.SelectedViewKey, "first")
				.Add(component => component.SelectedViewKeyChanged, EventCallback.Factory.Create<string>(this, value => selected = value)));

			cut.Find(".c4sharp-workspace-editor__surface strong").TextContent.ShouldBe("first");
			cut.Find(".c4sharp-workspace-editor__surface svg").GetAttribute("data-c4-view-key").ShouldBe("first");

			await cut.InvokeAsync(() => cut.FindComponents<ViewThumbnail>()[1].Instance.SelectedChanged.InvokeAsync("second"));

			selected.ShouldBe("second");
			cut.Find(".c4sharp-workspace-editor__surface strong").TextContent.ShouldBe("second");
			cut.Find(".c4sharp-workspace-editor__surface svg").GetAttribute("data-c4-view-key").ShouldBe("second");
		}

		[Fact]
		public async Task ViewEditor_PersistsDragAndConnectorEditsFromInteractiveCallbacks()
		{
			Workspace workspace = new Workspace("Test", "Description");
			SoftwareSystem source = workspace.Model.AddSoftwareSystem("Source", "Description");
			SoftwareSystem destination = workspace.Model.AddSoftwareSystem("Destination", "Description");
			Relationship relationship = source.Uses(destination, "Calls");
			SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("view", "View");
			view.AddAllSoftwareSystems();
			view.GetElementView(source).X = 100;
			view.GetElementView(source).Y = 100;
			view.GetElementView(destination).X = 500;
			view.GetElementView(destination).Y = 300;
			JSInterop.SetupVoid("c4sharpEditor.initialize", _ => true);
			IRenderedComponent<ViewEditor> cut = Render<ViewEditor>(parameters => parameters
				.Add(component => component.Workspace, workspace)
				.Add(component => component.ViewKey, view.Key));

			await cut.InvokeAsync(() => cut.Instance.MoveElement(source.Id, 150, 175));
			await cut.InvokeAsync(() => cut.Instance.AddRelationshipVertex(relationship.Id, 300, 150));

			view.GetElementView(source).X.ShouldBe(150);
			view.GetElementView(source).Y.ShouldBe(175);
			view.GetRelationshipView(relationship).Vertices.Count.ShouldBe(1);
			cut.Find("[data-c4-element-id='" + source.Id + "'] rect").GetAttribute("x").ShouldBe("75");
			cut.Find("[data-c4-element-id='" + source.Id + "'] rect").GetAttribute("y").ShouldBe("140");
			cut.Find("[data-c4-relationship-id='" + relationship.Id + "']").GetAttribute("points").ShouldContain("300,150");
		}

		[Fact]
		public void EditorScript_UpdatesTheDraggedElementBeforePointerUp()
		{
			string scriptPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "StacyClouds.C4Sharp.Editor", "wwwroot", "c4sharp-editor.js"));
			string script = File.ReadAllText(scriptPath);

			script.ShouldContain("pointermove");
			script.ShouldContain("setAttribute('transform'");
			script.ShouldContain("c4RelationshipSourceId");
			script.ShouldContain("c4RelationshipDestinationId");
			script.ShouldContain("updateConnectorVertex");
			script.ShouldContain("updateRelationshipLabel");
			script.ShouldContain("MoveRelationshipLabel");
			script.ShouldContain("closestPointOnConnector");
		}
	}
}
