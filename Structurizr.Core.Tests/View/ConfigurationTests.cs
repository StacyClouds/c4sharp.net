using System;
using Xunit;
using Shouldly;

namespace Structurizr.Core.Tests
{

    
    public class ViewConfigurationTests : AbstractTestBase
    {

        [Fact]
        public void test_defaultView_DoesNothing_WhenPassedNull()
        {
            ViewConfiguration configuration = new ViewConfiguration();
            configuration.SetDefaultView(null);
            Assert.Null(configuration.DefaultView);
        }

        [Fact]
        public void test_defaultView()
        {
            SystemLandscapeView view = Views.CreateSystemLandscapeView("key", "Description");
            ViewConfiguration configuration = new ViewConfiguration();
            configuration.SetDefaultView(view);
            Assert.Equal("key", configuration.DefaultView);
        }

        [Fact]
        public void test_copyConfigurationFrom()
        {
            ViewConfiguration source = new ViewConfiguration();
            source.LastSavedView = "someKey";

            ViewConfiguration destination = new ViewConfiguration();
            destination.CopyConfigurationFrom(source);
            Assert.Equal("someKey", destination.LastSavedView);
        } 
        
        [Fact]
        public void Test_SetTheme_WithAUrl()
        {
            ViewConfiguration configuration = new ViewConfiguration();
            configuration.Theme = "https://example.com/theme.json";
            Assert.Equal("https://example.com/theme.json", configuration.Theme);
        }

        [Fact]
        public void Test_SetTheme_WithAUrlThatHasATrailingSpaceCharacter()
        {
            ViewConfiguration configuration = new ViewConfiguration();
            configuration.Theme = "https://example.com/theme.json ";
            Assert.Equal("https://example.com/theme.json", configuration.Theme);
        }

        [Fact]
        public void Test_SetTheme_ThrowsAnIllegalArgumentException_WhenAnInvalidUrlIsSpecified()
        {
            ViewConfiguration configuration = new ViewConfiguration();
            Assert.Throws<ArgumentException>(() =>
                configuration.Theme = "blah"
            );
        }

        [Fact]
        public void Test_SetTheme_DoesNothing_WhenANullUrlIsSpecified()
        {
            ViewConfiguration configuration = new ViewConfiguration();
            configuration.Theme = null;
            Assert.Null(configuration.Theme);
        }

        [Fact]
        public void Test_SetTheme_DoesNothing_WhenAnEmptyUrlIsSpecified()
        {
            ViewConfiguration configuration = new ViewConfiguration();
            configuration.Theme = " ";
            Assert.Null(configuration.Theme);
        }

        [Fact]
        public void Test_Theme_ReturnsNull_WhenNoThemesAreSet()
        {
            ViewConfiguration configuration = new ViewConfiguration();
            configuration.Theme.ShouldBeNull();
        }

        [Fact]
        public void Test_SetThemes_WithValidUrls_StoresThemes()
        {
            ViewConfiguration configuration = new ViewConfiguration();
            configuration.Themes = new[] { "https://example.com/theme1.json", "https://example.com/theme2.json" };
            configuration.Themes.Length.ShouldBe(2);
            configuration.Theme.ShouldBe("https://example.com/theme1.json");
        }

        [Fact]
        public void Test_SetThemes_WithNull_SetsEmptyArray()
        {
            ViewConfiguration configuration = new ViewConfiguration();
            configuration.Themes = null;
            configuration.Themes.Length.ShouldBe(0);
        }

        [Fact]
        public void Test_SetThemes_ThrowsAnException_WhenInvalidUrlIsIncluded()
        {
            ViewConfiguration configuration = new ViewConfiguration();
            Should.Throw<ArgumentException>(() =>
                configuration.Themes = new[] { "notaurl" }
            ).Message.ShouldContain("not a valid URL");
        }

        [Fact]
        public void Test_SetThemes_IgnoresEmptyStrings()
        {
            ViewConfiguration configuration = new ViewConfiguration();
            configuration.Themes = new[] { "  ", "https://example.com/theme.json" };
            configuration.Themes.Length.ShouldBe(1);
        }

    }

}
