using Xunit;

namespace StacyClouds.C4Sharp.Documentation.Tests
{
    public class SectionTests
    {

        [Fact]
        public void Test_SetType_ProvidesBackwardsCompatibility()
        {
            Section section = new Section();
            section.SectionType = "Title"; // older clients use the type property
            Assert.Equal("Title", section.Title);
        }

    }
}