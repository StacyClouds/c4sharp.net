using System;
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
        public void Render_WithPredecessor_CopiesMatchingLayoutAndRendersIt()
        {
            Workspace predecessor = new Workspace("Predecessor", "Description");
            SoftwareSystem predecessorSource = predecessor.Model.AddSoftwareSystem("Source", "Description");
            SoftwareSystem predecessorDestination = predecessor.Model.AddSoftwareSystem("Destination", "Description");
            Relationship predecessorRelationship = predecessorSource.Uses(predecessorDestination, "Calls");
            SystemLandscapeView predecessorView = predecessor.Views.CreateSystemLandscapeView("landscape", "Landscape");
            predecessorView.AddAllSoftwareSystems();
            predecessorView.Dimensions = new Dimensions(900, 700);
            predecessorView.GetElementView(predecessorSource).X = 210;
            predecessorView.GetElementView(predecessorSource).Y = 150;
            predecessorView.GetElementView(predecessorDestination).X = 650;
            predecessorView.GetElementView(predecessorDestination).Y = 450;
            RelationshipView predecessorRelationshipView = predecessorView.GetRelationshipView(predecessorRelationship);
            predecessorRelationshipView.SetVertices(new[] { new Vertex(400, 150), new Vertex(400, 450) });
            predecessorRelationshipView.Routing = Routing.Orthogonal;
            predecessorRelationshipView.Position = 75;

            Workspace successor = new Workspace("Successor", "Description");
            SoftwareSystem successorSource = successor.Model.AddSoftwareSystem("Source", "Description");
            SoftwareSystem successorDestination = successor.Model.AddSoftwareSystem("Destination", "Description");
            Relationship successorRelationship = successorSource.Uses(successorDestination, "Calls");
            SystemLandscapeView successorView = successor.Views.CreateSystemLandscapeView("landscape", "Landscape");
            successorView.AddAllSoftwareSystems();

            string svg = new SvgWorkspaceRenderer().Render(successor, predecessor)["landscape"];

            successorView.GetElementView(successorSource).X.ShouldBe(210);
            successorView.GetElementView(successorSource).Y.ShouldBe(150);
            successorView.GetElementView(successorDestination).X.ShouldBe(650);
            successorView.GetElementView(successorDestination).Y.ShouldBe(450);
            successorView.Dimensions.Width.ShouldBe(900);
            successorView.Dimensions.Height.ShouldBe(700);
            RelationshipView successorRelationshipView = successorView.GetRelationshipView(successorRelationship);
            successorRelationshipView.Routing.ShouldBe(Routing.Orthogonal);
            successorRelationshipView.Position.ShouldBe(75);
            successorRelationshipView.Vertices.Count.ShouldBe(2);
            successorRelationshipView.Vertices[0].X.ShouldBe(400);
            successorRelationshipView.Vertices[0].Y.ShouldBe(150);
            successorRelationshipView.Vertices[1].X.ShouldBe(400);
            successorRelationshipView.Vertices[1].Y.ShouldBe(450);
            svg.ShouldContain("x=\"135\" y=\"115\"");
            svg.ShouldContain("210,150 400,150 400,450 650,450");
            svg.ShouldContain("data-c4-relationship-label-position=\"75\"");
            predecessorView.GetElementView(predecessorSource).X.ShouldBe(210);
        }

        [Fact]
        public void Render_WithPredecessor_PreservesExplicitSuccessorDimensions()
        {
            Workspace predecessor = new Workspace("Predecessor", "Description");
            SystemLandscapeView predecessorView = predecessor.Views.CreateSystemLandscapeView("landscape", "Landscape");
            predecessorView.Dimensions = new Dimensions(900, 700);
            Workspace successor = new Workspace("Successor", "Description");
            SystemLandscapeView successorView = successor.Views.CreateSystemLandscapeView("landscape", "Landscape");
            successorView.Dimensions = new Dimensions(400, 300);

            new SvgWorkspaceRenderer().Render(successor, predecessor);

            successorView.Dimensions.Width.ShouldBe(400);
            successorView.Dimensions.Height.ShouldBe(300);
        }

        [Fact]
        public void Render_WithPredecessor_OmitsDeletedObjectsAndUsesDeterministicFallbackForNewElements()
        {
            Workspace predecessor = new Workspace("Predecessor", "Description");
            SoftwareSystem sharedPredecessor = predecessor.Model.AddSoftwareSystem("Shared", "Description");
            SoftwareSystem removedPredecessor = predecessor.Model.AddSoftwareSystem("Removed", "Description");
            Relationship removedRelationship = sharedPredecessor.Uses(removedPredecessor, "Removed relationship");
            SystemLandscapeView predecessorView = predecessor.Views.CreateSystemLandscapeView("landscape", "Landscape");
            predecessorView.AddAllSoftwareSystems();
            predecessorView.GetElementView(sharedPredecessor).X = 300;
            predecessorView.GetElementView(sharedPredecessor).Y = 200;
            predecessorView.GetRelationshipView(removedRelationship).SetVertices(new[] { new Vertex(500, 200) });
            SystemLandscapeView removedView = predecessor.Views.CreateSystemLandscapeView("removed", "Removed view");
            removedView.Add(removedPredecessor);

            Workspace successor = new Workspace("Successor", "Description");
            SoftwareSystem sharedSuccessor = successor.Model.AddSoftwareSystem("Shared", "Description");
            SoftwareSystem newSuccessor = successor.Model.AddSoftwareSystem("New", "New description");
            SystemLandscapeView successorView = successor.Views.CreateSystemLandscapeView("landscape", "Landscape");
            successorView.AddAllSoftwareSystems();

            SvgWorkspaceRenderer renderer = new SvgWorkspaceRenderer();
            string first = renderer.Render(successor, predecessor)["landscape"];
            string second = renderer.Render(successor, predecessor)["landscape"];

            successorView.Elements.Count.ShouldBe(2);
            successorView.Relationships.ShouldBeEmpty();
            successorView.GetElementView(sharedSuccessor).X.ShouldBe(300);
            successorView.GetElementView(newSuccessor).X.ShouldBe(0);
            successorView.GetElementView(newSuccessor).Y.ShouldBe(0);
            first.ShouldBe(second);
            first.ShouldContain("New");
            first.ShouldNotContain("Removed");
            first.ShouldNotContain("Removed relationship");
            renderer.Render(successor, predecessor).ContainsKey("removed").ShouldBeFalse();
        }

        [Fact]
        public void Render_WithPredecessor_IgnoresUnmatchedViews()
        {
            Workspace predecessor = new Workspace("Predecessor", "Description");
            predecessor.Views.CreateSystemLandscapeView("predecessor", "Predecessor");
            Workspace successor = new Workspace("Successor", "Description");
            SoftwareSystem system = successor.Model.AddSoftwareSystem("System", "Description");
            SystemLandscapeView successorView = successor.Views.CreateSystemLandscapeView("successor", "Successor");
            successorView.Add(system);
            SvgWorkspaceRenderer renderer = new SvgWorkspaceRenderer();
            string expected = renderer.Render(successor)["successor"];

            string actual = renderer.Render(successor, predecessor)["successor"];

            actual.ShouldBe(expected);
            successorView.GetElementView(system).X.ShouldBe(0);
            successorView.GetElementView(system).Y.ShouldBe(0);
        }

        [Fact]
        public void Render_WithPredecessor_RejectsNullWorkspaces()
        {
            Workspace workspace = new Workspace("Workspace", "Description");
            SvgWorkspaceRenderer renderer = new SvgWorkspaceRenderer();

            Should.Throw<ArgumentNullException>(() => renderer.Render(null, workspace)).ParamName.ShouldBe("workspace");
            Should.Throw<ArgumentNullException>(() => renderer.Render(workspace, null)).ParamName.ShouldBe("predecessor");
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
        public void Render_PreservesDeterministicPositionsForUntouchedElementsAfterAMove()
        {
            Workspace workspace = new Workspace("Test", "Description");
            SoftwareSystem first = workspace.Model.AddSoftwareSystem("One", "Description");
            workspace.Model.AddSoftwareSystem("Two", "Description");
            SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
            view.AddAllSoftwareSystems();
            view.GetElementView(first).X = 400;
            view.GetElementView(first).Y = 300;

            string svg = new SvgWorkspaceRenderer().Render(workspace)["landscape"];

            svg.ShouldContain("x=\"325\" y=\"265\"");
            svg.ShouldContain("x=\"285\" y=\"65\"");
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
            svg.ShouldContain("data-c4-relationship-source-id=\"" + source.Id + "\"");
            svg.ShouldContain("data-c4-relationship-destination-id=\"" + destination.Id + "\"");
			svg.ShouldContain("data-c4-relationship-label-id=\"" + relationship.Id + "\"");
			svg.ShouldContain("data-c4-relationship-label-position=\"50\"");
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
        public void Render_UsesATwoPixelDefaultRelationshipStroke()
        {
            Workspace workspace = new Workspace("Test", "Description");
            SoftwareSystem source = workspace.Model.AddSoftwareSystem("Source", "Description");
            SoftwareSystem destination = workspace.Model.AddSoftwareSystem("Destination", "Description");
            source.Uses(destination, "Calls");
            SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
            view.AddAllSoftwareSystems();

            string svg = new SvgWorkspaceRenderer().Render(workspace)["landscape"];

            svg.ShouldContain("<polyline");
            svg.ShouldContain("stroke-width=\"2\"");
        }

        [Fact]
        public void Render_ClipsVisibleRelationshipEndpointsButKeepsCentreInteractionGeometry()
        {
            Workspace workspace = new Workspace("Test", "Description");
            SoftwareSystem source = workspace.Model.AddSoftwareSystem("Source", "Description");
            SoftwareSystem destination = workspace.Model.AddSoftwareSystem("Destination", "Description");
            source.Uses(destination, "Calls");
            SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
            view.AddAllSoftwareSystems();
            view.GetElementView(source).X = 100;
            view.GetElementView(source).Y = 100;
            view.GetElementView(destination).X = 500;
            view.GetElementView(destination).Y = 100;

            string svg = new SvgWorkspaceRenderer().Render(workspace)["landscape"];

            svg.ShouldContain("data-c4-relationship-visible=\"true\" points=\"175,100 425,100\"");
            svg.ShouldContain("data-c4-relationship-interaction=\"true\" points=\"100,100 500,100\"");
        }

        [Fact]
        public void Render_WrapsLongElementNamesInsideBoundedTextLines()
        {
            Workspace workspace = new Workspace("Test", "Description");
            SoftwareSystem spaced = workspace.Model.AddSoftwareSystem("A deliberately long software system name for wrapping", "Description");
            SoftwareSystem unbroken = workspace.Model.AddSoftwareSystem("AnExceptionallyLongUnbrokenSystemName", "Description");
            SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
            view.AddAllSoftwareSystems();

            string svg = new SvgWorkspaceRenderer().Render(workspace)["landscape"];

            svg.ShouldContain("<tspan");
            svg.ShouldContain("A deliberately long");
            svg.ShouldContain("AnExceptionallyLong");
        }

        [Fact]
        public void Render_ExpandsItsViewportAndRendersConnectorVertexHandles()
        {
            Workspace workspace = new Workspace("Test", "Description");
            SoftwareSystem source = workspace.Model.AddSoftwareSystem("Source", "Description");
            SoftwareSystem destination = workspace.Model.AddSoftwareSystem("Destination", "Description");
            Relationship relationship = source.Uses(destination, "Calls");
            SystemLandscapeView view = workspace.Views.CreateSystemLandscapeView("landscape", "Landscape");
            view.AddAllSoftwareSystems();
            view.GetElementView(source).X = 100;
            view.GetElementView(source).Y = 100;
            view.GetElementView(destination).X = 1200;
            view.GetElementView(destination).Y = 900;
            view.GetRelationshipView(relationship).SetVertices(new[] { new Vertex(1400, 1100) });

            string svg = new SvgWorkspaceRenderer().Render(workspace)["landscape"];

            svg.ShouldContain("width=\"1500\"");
            svg.ShouldContain("height=\"1200\"");
            svg.ShouldContain("data-c4-relationship-vertex-index=\"0\"");
            svg.ShouldContain("cx=\"1400\" cy=\"1100\"");
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
