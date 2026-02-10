using System;
using Xunit;

namespace StacyClouds.C4Sharp.Core.Tests
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

    }

}
