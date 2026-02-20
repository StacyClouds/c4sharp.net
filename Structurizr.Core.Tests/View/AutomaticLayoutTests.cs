using System;
using Xunit;

namespace Structurizr.Core.Tests.View
{
    public class AutomaticLayoutTests
    {
        [Fact]
        public void Test_AutomaticLayout()
        {
            AutomaticLayout automaticLayout = new AutomaticLayout(RankDirection.LeftRight, 100, 200, 300, true);

            Assert.Equal(RankDirection.LeftRight, automaticLayout.RankDirection);
            Assert.Equal(100, automaticLayout.RankSeparation);
            Assert.Equal(200, automaticLayout.NodeSeparation);
            Assert.Equal(300, automaticLayout.EdgeSeparation);
            Assert.True(automaticLayout.Vertices);
        }

        [Fact]
        public void Test_RankSeparation_ThrowsAnException_WhenANegativeIntegerIsSpecified()
        {
            try
            {
                AutomaticLayout automaticLayout = new AutomaticLayout();
                automaticLayout.RankSeparation = -100;
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("The rank separation must be a positive integer.", iae.Message);
            }
        }

        [Fact]
        public void Test_NodeSeparation_ThrowsAnException_WhenANegativeIntegerIsSpecified()
        {
            try
            {
                AutomaticLayout automaticLayout = new AutomaticLayout();
                automaticLayout.NodeSeparation = -100;
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("The node separation must be a positive integer.", iae.Message);
            }
        }

        [Fact]
        public void Test_EdgeSeparation_ThrowsAnException_WhenANegativeIntegerIsSpecified()
        {
            try
            {
                AutomaticLayout automaticLayout = new AutomaticLayout();
                automaticLayout.EdgeSeparation = -100;
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("The edge separation must be a positive integer.", iae.Message);
            }
        }
        
    }
}