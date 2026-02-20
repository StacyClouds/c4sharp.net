using Xunit;
using Shouldly;

namespace StacyClouds.C4Sharp.Core.Tests
{
    public class PaperSizeTests
    {
        [Fact]
        public void Test_A6_Portrait_HasCorrectProperties()
        {
            PaperSize.A6_Portrait.Key.ShouldBe("A6_Portrait");
            PaperSize.A6_Portrait.Name.ShouldBe("A6");
            PaperSize.A6_Portrait.Orientation.ShouldBe(Orientation.Portrait);
            PaperSize.A6_Portrait.width.ShouldBe(1240);
            PaperSize.A6_Portrait.height.ShouldBe(1748);
        }

        [Fact]
        public void Test_A6_Landscape_HasCorrectProperties()
        {
            PaperSize.A6_Landscape.Key.ShouldBe("A6_Landscape");
            PaperSize.A6_Landscape.Name.ShouldBe("A6");
            PaperSize.A6_Landscape.Orientation.ShouldBe(Orientation.Landscape);
            PaperSize.A6_Landscape.width.ShouldBe(1748);
            PaperSize.A6_Landscape.height.ShouldBe(1240);
        }

        [Fact]
        public void Test_A5_Portrait_HasCorrectProperties()
        {
            PaperSize.A5_Portrait.Key.ShouldBe("A5_Portrait");
            PaperSize.A5_Portrait.Name.ShouldBe("A5");
            PaperSize.A5_Portrait.Orientation.ShouldBe(Orientation.Portrait);
            PaperSize.A5_Portrait.width.ShouldBe(1748);
            PaperSize.A5_Portrait.height.ShouldBe(2480);
        }

        [Fact]
        public void Test_A5_Landscape_HasCorrectProperties()
        {
            PaperSize.A5_Landscape.Key.ShouldBe("A5_Landscape");
            PaperSize.A5_Landscape.Name.ShouldBe("A5");
            PaperSize.A5_Landscape.Orientation.ShouldBe(Orientation.Landscape);
            PaperSize.A5_Landscape.width.ShouldBe(2480);
            PaperSize.A5_Landscape.height.ShouldBe(1748);
        }

        [Fact]
        public void Test_A4_Portrait_HasCorrectProperties()
        {
            PaperSize.A4_Portrait.Key.ShouldBe("A4_Portrait");
            PaperSize.A4_Portrait.Name.ShouldBe("A4");
            PaperSize.A4_Portrait.Orientation.ShouldBe(Orientation.Portrait);
            PaperSize.A4_Portrait.width.ShouldBe(2480);
            PaperSize.A4_Portrait.height.ShouldBe(3508);
        }

        [Fact]
        public void Test_A4_Landscape_HasCorrectProperties()
        {
            PaperSize.A4_Landscape.Key.ShouldBe("A4_Landscape");
            PaperSize.A4_Landscape.Name.ShouldBe("A4");
            PaperSize.A4_Landscape.Orientation.ShouldBe(Orientation.Landscape);
            PaperSize.A4_Landscape.width.ShouldBe(3508);
            PaperSize.A4_Landscape.height.ShouldBe(2480);
        }

        [Fact]
        public void Test_A3_Portrait_HasCorrectProperties()
        {
            PaperSize.A3_Portrait.Key.ShouldBe("A3_Portrait");
            PaperSize.A3_Portrait.Name.ShouldBe("A3");
            PaperSize.A3_Portrait.Orientation.ShouldBe(Orientation.Portrait);
            PaperSize.A3_Portrait.width.ShouldBe(3508);
            PaperSize.A3_Portrait.height.ShouldBe(4961);
        }

        [Fact]
        public void Test_A3_Landscape_HasCorrectProperties()
        {
            PaperSize.A3_Landscape.Key.ShouldBe("A3_Landscape");
            PaperSize.A3_Landscape.Name.ShouldBe("A3");
            PaperSize.A3_Landscape.Orientation.ShouldBe(Orientation.Landscape);
            PaperSize.A3_Landscape.width.ShouldBe(4961);
            PaperSize.A3_Landscape.height.ShouldBe(3508);
        }

        [Fact]
        public void Test_A2_Portrait_HasCorrectProperties()
        {
            PaperSize.A2_Portrait.Key.ShouldBe("A2_Portrait");
            PaperSize.A2_Portrait.Name.ShouldBe("A2");
            PaperSize.A2_Portrait.Orientation.ShouldBe(Orientation.Portrait);
            PaperSize.A2_Portrait.width.ShouldBe(4961);
            PaperSize.A2_Portrait.height.ShouldBe(7016);
        }

        [Fact]
        public void Test_A2_Landscape_HasCorrectProperties()
        {
            PaperSize.A2_Landscape.Key.ShouldBe("A2_Landscape");
            PaperSize.A2_Landscape.Name.ShouldBe("A2");
            PaperSize.A2_Landscape.Orientation.ShouldBe(Orientation.Landscape);
            PaperSize.A2_Landscape.width.ShouldBe(7016);
            PaperSize.A2_Landscape.height.ShouldBe(4961);
        }

        [Fact]
        public void Test_A1_Portrait_HasCorrectProperties()
        {
            PaperSize.A1_Portrait.Key.ShouldBe("A1_Portrait");
            PaperSize.A1_Portrait.Name.ShouldBe("A1");
            PaperSize.A1_Portrait.Orientation.ShouldBe(Orientation.Portrait);
            PaperSize.A1_Portrait.width.ShouldBe(7016);
            PaperSize.A1_Portrait.height.ShouldBe(9933);
        }

        [Fact]
        public void Test_A1_Landscape_HasCorrectProperties()
        {
            PaperSize.A1_Landscape.Key.ShouldBe("A1_Landscape");
            PaperSize.A1_Landscape.Name.ShouldBe("A1");
            PaperSize.A1_Landscape.Orientation.ShouldBe(Orientation.Landscape);
            PaperSize.A1_Landscape.width.ShouldBe(9933);
            PaperSize.A1_Landscape.height.ShouldBe(7016);
        }

        [Fact]
        public void Test_A0_Portrait_HasCorrectProperties()
        {
            PaperSize.A0_Portrait.Key.ShouldBe("A0_Portrait");
            PaperSize.A0_Portrait.Name.ShouldBe("A0");
            PaperSize.A0_Portrait.Orientation.ShouldBe(Orientation.Portrait);
            PaperSize.A0_Portrait.width.ShouldBe(9933);
            PaperSize.A0_Portrait.height.ShouldBe(14043);
        }

        [Fact]
        public void Test_A0_Landscape_HasCorrectProperties()
        {
            PaperSize.A0_Landscape.Key.ShouldBe("A0_Landscape");
            PaperSize.A0_Landscape.Name.ShouldBe("A0");
            PaperSize.A0_Landscape.Orientation.ShouldBe(Orientation.Landscape);
            PaperSize.A0_Landscape.width.ShouldBe(14043);
            PaperSize.A0_Landscape.height.ShouldBe(9933);
        }

        [Fact]
        public void Test_Letter_Portrait_HasCorrectProperties()
        {
            PaperSize.Letter_Portrait.Key.ShouldBe("Letter_Portrait");
            PaperSize.Letter_Portrait.Name.ShouldBe("Letter");
            PaperSize.Letter_Portrait.Orientation.ShouldBe(Orientation.Portrait);
            PaperSize.Letter_Portrait.width.ShouldBe(2550);
            PaperSize.Letter_Portrait.height.ShouldBe(3300);
        }

        [Fact]
        public void Test_Letter_Landscape_HasCorrectProperties()
        {
            PaperSize.Letter_Landscape.Key.ShouldBe("Letter_Landscape");
            PaperSize.Letter_Landscape.Name.ShouldBe("Letter");
            PaperSize.Letter_Landscape.Orientation.ShouldBe(Orientation.Landscape);
            PaperSize.Letter_Landscape.width.ShouldBe(3300);
            PaperSize.Letter_Landscape.height.ShouldBe(2550);
        }

        [Fact]
        public void Test_Legal_Portrait_HasCorrectProperties()
        {
            PaperSize.Legal_Portrait.Key.ShouldBe("Legal_Portrait");
            PaperSize.Legal_Portrait.Name.ShouldBe("Legal");
            PaperSize.Legal_Portrait.Orientation.ShouldBe(Orientation.Portrait);
            PaperSize.Legal_Portrait.width.ShouldBe(2550);
            PaperSize.Legal_Portrait.height.ShouldBe(4200);
        }

        [Fact]
        public void Test_Legal_Landscape_HasCorrectProperties()
        {
            PaperSize.Legal_Landscape.Key.ShouldBe("Legal_Landscape");
            PaperSize.Legal_Landscape.Name.ShouldBe("Legal");
            PaperSize.Legal_Landscape.Orientation.ShouldBe(Orientation.Landscape);
            PaperSize.Legal_Landscape.width.ShouldBe(4200);
            PaperSize.Legal_Landscape.height.ShouldBe(2550);
        }

        [Fact]
        public void Test_Slide_4_3_HasCorrectProperties()
        {
            PaperSize.Slide_4_3.Key.ShouldBe("Slide_4_3");
            PaperSize.Slide_4_3.Name.ShouldBe("Slide 4:3");
            PaperSize.Slide_4_3.Orientation.ShouldBe(Orientation.Landscape);
            PaperSize.Slide_4_3.width.ShouldBe(3306);
            PaperSize.Slide_4_3.height.ShouldBe(2480);
        }

        [Fact]
        public void Test_Slide_16_9_HasCorrectProperties()
        {
            PaperSize.Slide_16_9.Key.ShouldBe("Slide_16_9");
            PaperSize.Slide_16_9.Name.ShouldBe("Slide 16:9");
            PaperSize.Slide_16_9.Orientation.ShouldBe(Orientation.Landscape);
            PaperSize.Slide_16_9.width.ShouldBe(3508);
            PaperSize.Slide_16_9.height.ShouldBe(1973);
        }

        [Fact]
        public void Test_Slide_16_10_HasCorrectProperties()
        {
            PaperSize.Slide_16_10.Key.ShouldBe("Slide_16_10");
            PaperSize.Slide_16_10.Name.ShouldBe("Slide 16:10");
            PaperSize.Slide_16_10.Orientation.ShouldBe(Orientation.Landscape);
            PaperSize.Slide_16_10.width.ShouldBe(3508);
            PaperSize.Slide_16_10.height.ShouldBe(2193);
        }

        [Fact]
        public void Test_GetPaperSize_ReturnsA4Portrait_WhenKeyIsNull()
        {
            PaperSize.GetPaperSize(null).ShouldBeSameAs(PaperSize.A4_Portrait);
        }

        [Fact]
        public void Test_GetPaperSize_ReturnsA4Portrait_WhenKeyDoesNotExist()
        {
            PaperSize.GetPaperSize("Unknown").ShouldBeSameAs(PaperSize.A4_Portrait);
        }

        [Fact]
        public void Test_GetPaperSize_ReturnsPaperSize_WhenKeyExists()
        {
            PaperSize.GetPaperSize("A3_Landscape").ShouldBeSameAs(PaperSize.A3_Landscape);
        }
    }
}
