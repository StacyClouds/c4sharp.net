using Bunit;
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
	}
}
