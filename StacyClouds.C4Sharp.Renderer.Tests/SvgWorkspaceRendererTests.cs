using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace StacyClouds.C4Sharp.Renderer.Tests
{
    public class SvgWorkspaceRendererTests
    {
        [Fact]
        public void Render_ReturnsAnSvgForEachWorkspaceView()
        {
            Workspace workspace = new Workspace("Test", "Description");
            workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
            workspace.Views.CreateDynamicView("dynamic", "Dynamic");

            IReadOnlyDictionary<string, string> diagrams = new SvgWorkspaceRenderer().Render(workspace);

            diagrams.Keys.ShouldBe(new[] { "dynamic", "landscape" }, ignoreOrder: true);
            diagrams["landscape"].ShouldStartWith("<svg");
        }

        [Fact]
        public void Render_UsesPersistedCoordinatesAndDerivesViewportBounds()
        {
            Workspace workspace = new Workspace("Test", "Description");
            SoftwareSystem system = workspace.Model.AddSoftwareSystem("System", "Description");
            SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
            view.Add(system);
            view.GetElementView(system).X = 1200;
            view.GetElementView(system).Y = 900;

            string svg = new SvgWorkspaceRenderer().Render(workspace)["landscape"];

            svg.ShouldContain("x=\"1125\"");
            svg.ShouldContain("width=\"1375\"");
            view.GetElementView(system).X.ShouldBe(1200);
        }

        [Fact]
        public void Render_AppliesFilteredViewTagsAndOmitsRemovedRelationships()
        {
            Workspace workspace = new Workspace("Test", "Description");
            SoftwareSystem visible = workspace.Model.AddSoftwareSystem("Visible", "Description");
            SoftwareSystem hidden = workspace.Model.AddSoftwareSystem("Hidden", "Description");
            hidden.AddTags("Hidden");
            visible.Uses(hidden, "Uses");
            SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
            view.AddAllSoftwareSystems();
            workspace.Views.CreateFilteredView(view, "filtered", "Filtered", FilterMode.Exclude, "Hidden");

            string svg = new SvgWorkspaceRenderer().Render(workspace)["filtered"];

            svg.ShouldContain("Visible");
            svg.ShouldNotContain("Hidden");
            svg.ShouldNotContain("polyline");
        }

        [Fact]
        public void Render_EnumeratesEverySupportedViewTypeAndHonoursExplicitDimensions()
        {
            Workspace workspace = new Workspace("Test", "Description");
            SoftwareSystem system = workspace.Model.AddSoftwareSystem("System", "Description");
            Container container = system.AddContainer("Container", "Description", "Technology");
            SystemLandscapeView landscape = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
            workspace.Views.CreateSystemContextView(system, "context", "Context");
            workspace.Views.CreateContainerView(system, "container", "Container");
            workspace.Views.CreateComponentView(container, "component", "Component");
            workspace.Views.CreateDynamicView("dynamic", "Dynamic");
            workspace.Views.CreateDeploymentView("deployment", "Deployment");
            workspace.Views.CreateFilteredView(landscape, "filtered", "Filtered", FilterMode.Exclude, "None");
            landscape.Dimensions = new Dimensions(321, 654);

            IReadOnlyDictionary<string, string> diagrams = new SvgWorkspaceRenderer().Render(workspace);

            diagrams.Keys.Count().ShouldBe(7);
            diagrams["landscape"].ShouldContain("width=\"321\" height=\"654\"");
        }

        [Fact]
        public void Render_UsesADeterministicGridForUnlaidOutElements()
        {
            Workspace workspace = new Workspace("Test", "Description");
            workspace.Model.AddSoftwareSystem("One", "Description");
            workspace.Model.AddSoftwareSystem("Two", "Description");
            SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
            view.AddAllSoftwareSystems();

            string first = new SvgWorkspaceRenderer().Render(workspace)["landscape"];
            string second = new SvgWorkspaceRenderer().Render(workspace)["landscape"];

            first.ShouldBe(second);
            first.ShouldContain("x=\"45\"");
            first.ShouldContain("x=\"285\"");
        }

        [Fact]
        public void Render_EscapesLabelsAndRoutesRelationshipsThroughConnectorVertices()
        {
            Workspace workspace = new Workspace("Test", "Description");
            SoftwareSystem source = workspace.Model.AddSoftwareSystem("Source & API", "Description");
            SoftwareSystem destination = workspace.Model.AddSoftwareSystem("Destination", "Description");
            Relationship relationship = source.Uses(destination, "Calls <service>");
            SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
            view.AddAllSoftwareSystems();
            view.GetElementView(source).X = 100;
            view.GetElementView(source).Y = 100;
            view.GetElementView(destination).X = 500;
            view.GetElementView(destination).Y = 300;
            view.GetRelationshipView(relationship).SetVertices(new[] { new Vertex(300, 100), new Vertex(300, 300) });

            string svg = new SvgWorkspaceRenderer().Render(workspace)["landscape"];

            svg.ShouldContain("Source &amp; API");
            svg.ShouldContain("Calls &lt;service&gt;");
            svg.ShouldContain("100,100 300,100 300,300 500,300");
            svg.ShouldContain("marker-end=\"url(#arrow)\"");
        }

        [Fact]
        public void Render_IdentifiesWorkspaceObjectsForOptionalInteractiveConsumers()
        {
            Workspace workspace = new Workspace("Test", "Description");
            SoftwareSystem source = workspace.Model.AddSoftwareSystem("Source", "Description");
            SoftwareSystem destination = workspace.Model.AddSoftwareSystem("Destination", "Description");
            Relationship relationship = source.Uses(destination, "Calls");
            SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape & view", "Landscape");
            view.AddAllSoftwareSystems();

            string svg = new SvgWorkspaceRenderer().Render(workspace)["landscape & view"];

            svg.ShouldContain("data-c4-view-key=\"landscape &amp; view\"");
            svg.ShouldContain("data-c4-element-id=\"" + source.Id + "\"");
            svg.ShouldContain("data-c4-element-id=\"" + destination.Id + "\"");
            svg.ShouldContain("data-c4-relationship-id=\"" + relationship.Id + "\"");
        }

        [Fact]
        public void Render_AppliesConfiguredElementAndRelationshipStyles()
        {
            Workspace workspace = new Workspace("Test", "Description");
            SoftwareSystem source = workspace.Model.AddSoftwareSystem("Source", "Description");
            SoftwareSystem destination = workspace.Model.AddSoftwareSystem("Destination", "Description");
            source.AddTags("Critical");
            Relationship relationship = source.Uses(destination, "Calls");
            relationship.AddTags("Async");
            workspace.Views.Configuration.Styles.Add(new ElementStyle("Critical") { Background = "#123456", Color = "#ffffff", Shape = Shape.Circle });
            workspace.Views.Configuration.Styles.Add(new RelationshipStyle("Async") { Color = "#ff0000", Dashed = true, Thickness = 3 });
            SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
            view.AddAllSoftwareSystems();

            string svg = new SvgWorkspaceRenderer().Render(workspace)["landscape"];

            svg.ShouldContain("<circle");
            svg.ShouldContain("fill=\"#123456\"");
            svg.ShouldContain("stroke=\"#ff0000\"");
            svg.ShouldContain("stroke-dasharray");
            svg.ShouldContain("stroke-width=\"3\"");
        }

        [Fact]
        public void Render_UsesDynamicOrderAndRelationshipLabelPosition()
        {
            Workspace workspace = new Workspace("Test", "Description");
            SoftwareSystem source = workspace.Model.AddSoftwareSystem("Source", "Description");
            SoftwareSystem middle = workspace.Model.AddSoftwareSystem("Middle", "Description");
            SoftwareSystem destination = workspace.Model.AddSoftwareSystem("Destination", "Description");
            Relationship first = source.Uses(middle, "First");
            Relationship second = middle.Uses(destination, "Second");
            DynamicView view = workspace.Views.CreateDynamicView("dynamic", "Dynamic");
            RelationshipView firstView = view.Add(first, "First");
            RelationshipView secondView = view.Add(second, "Second");
            firstView.Position = 0;

            string svg = new SvgWorkspaceRenderer().Render(workspace)["dynamic"];

            svg.IndexOf("1: First").ShouldBeLessThan(svg.IndexOf("2: Second"));
            svg.ShouldContain("x=\"120\" y=\"92\"");
        }

        [Fact]
        public void SvgRenderingExample_ProducesTheDocumentedWorkspaceSvg()
        {
            IReadOnlyDictionary<string, string> diagrams = StacyClouds.C4Sharp.Examples.SvgRenderingExample.CreateSvgDocuments();

			diagrams.ContainsKey("system-context").ShouldBeTrue();
            diagrams["system-context"].ShouldStartWith("<svg");
        }
    }
}
