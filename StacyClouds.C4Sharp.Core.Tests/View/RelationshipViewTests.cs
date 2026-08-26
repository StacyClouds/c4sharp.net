using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using Xunit;
using Shouldly;

namespace StacyClouds.C4Sharp.Core.Tests
{
    public class RelationshipViewTests : AbstractTestBase
    {
        private SoftwareSystem softwareSystem1;
        private SoftwareSystem softwareSystem2;
        private Relationship relationship;

        public RelationshipViewTests()
        {
            softwareSystem1 = Model.AddSoftwareSystem("Software System 1", "Description");
            softwareSystem2 = Model.AddSoftwareSystem("Software System 2", "Description");
            relationship = softwareSystem1.Uses(softwareSystem2, "Uses");
        }

        [Fact]
        public void Test_Position_IsClampedToZero_WhenNegativeValueIsSet()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.Position = -10;
            view.Position.ShouldBe(0);
        }

        [Fact]
        public void Test_Position_IsClampedTo100_WhenValueExceeds100()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.Position = 150;
            view.Position.ShouldBe(100);
        }

        [Fact]
        public void Test_Position_IsSetCorrectly_WhenValueIsWithinRange()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.Position = 50;
            view.Position.ShouldBe(50);
        }

        [Fact]
        public void Test_Position_RemainsNull_WhenNotSet()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.Position.ShouldBeNull();
        }

        [Fact]
        public void Test_Equals_ReturnsFalse_WhenComparingWithNull()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.Equals(null).ShouldBeFalse();
        }

        [Fact]
        public void Test_Equals_ReturnsTrue_WhenComparingWithSelf()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.Equals(view).ShouldBeTrue();
        }

        [Fact]
        public void Test_Equals_ReturnsTrue_WhenTwoViewsHaveTheSameIdAndOrderAndDescription()
        {
            RelationshipView view1 = new RelationshipView(relationship);
            view1.Order = "1";
            view1.Description = "desc";

            RelationshipView view2 = new RelationshipView(relationship);
            view2.Order = "1";
            view2.Description = "desc";

            view1.Equals(view2).ShouldBeTrue();
        }

        [Fact]
        public void Test_Equals_ReturnsFalse_WhenDescriptionsDiffer()
        {
            RelationshipView view1 = new RelationshipView(relationship);
            view1.Description = "desc1";

            RelationshipView view2 = new RelationshipView(relationship);
            view2.Description = "desc2";

            view1.Equals(view2).ShouldBeFalse();
        }

        [Fact]
        public void Test_Equals_ReturnsFalse_WhenOrdersDiffer()
        {
            RelationshipView view1 = new RelationshipView(relationship);
            view1.Order = "1";

            RelationshipView view2 = new RelationshipView(relationship);
            view2.Order = "2";

            view1.Equals(view2).ShouldBeFalse();
        }

        [Fact]
        public void Test_CopyLayoutInformationFrom_CopiesVerticesRoutingAndPosition()
        {
            RelationshipView source = new RelationshipView(relationship);
            source.Routing = Routing.Orthogonal;
            source.Position = 75;

            RelationshipView destination = new RelationshipView(relationship);
            destination.CopyLayoutInformationFrom(source);

            destination.Routing.ShouldBe(Routing.Orthogonal);
            destination.Position.ShouldBe(75);
        }

        [Fact]
        public void Test_ConnectorVertices_CanBeCreatedAndEditedWithoutExposingInternalState()
        {
            RelationshipView view = new RelationshipView(relationship);
            Vertex first = new Vertex(100, 200);
            Vertex second = new Vertex(300, 400);
            List<Vertex> replacement = new List<Vertex> { first, second };

            view.SetVertices(replacement);
            view.Vertices.Count.ShouldBe(2);
            view.Vertices[0].X.ShouldBe(100);
            view.Vertices[1].Y.ShouldBe(400);

            replacement.Clear();
            view.Vertices.Count.ShouldBe(2);

            view.AddVertex(new Vertex(500, 600));
            view.Vertices.Count.ShouldBe(3);
            view.RemoveVertex(second).ShouldBeTrue();
            view.Vertices.Count.ShouldBe(2);
            view.Vertices[0].X.ShouldBe(100);
            view.Vertices[1].X.ShouldBe(500);

            List<Vertex> returnedVertices = view.Vertices;
            returnedVertices.Clear();
            view.Vertices.Count.ShouldBe(2);

            view.ClearVertices();
            view.Vertices.ShouldBeEmpty();
        }

        [Fact]
        public void Test_ConnectorVertices_RejectNullValuesWithoutChangingTheLayout()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.AddVertex(new Vertex(100, 200));

            Should.Throw<ArgumentNullException>(() => view.AddVertex(null));
            Should.Throw<ArgumentNullException>(() => view.SetVertices(null));
            Should.Throw<ArgumentException>(() => view.SetVertices(new List<Vertex> { null }));

            view.Vertices.Count.ShouldBe(1);
            view.Vertices[0].X.ShouldBe(100);
            view.Vertices[0].Y.ShouldBe(200);
        }

        [Fact]
        public void Test_ConnectorVertices_ArePreservedBySerializationAndLayoutCopying()
        {
            RelationshipView source = new RelationshipView(relationship);
            source.SetVertices(new[] { new Vertex(100, 200), new Vertex(300, 400) });

            RelationshipView copy = new RelationshipView(relationship);
            copy.CopyLayoutInformationFrom(source);
            copy.Vertices.Count.ShouldBe(2);
            copy.Vertices[1].Y.ShouldBe(400);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RelationshipView));
            using MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, source);
            stream.Position = 0;
            RelationshipView deserialized = (RelationshipView)serializer.ReadObject(stream);

            deserialized.Vertices.Count.ShouldBe(2);
            deserialized.Vertices[0].X.ShouldBe(100);
            deserialized.Vertices[1].Y.ShouldBe(400);
        }
    }
}
