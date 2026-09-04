using System;
using System.Collections.Generic;

namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Represents a named paper or slide size that can be assigned to a view.
    /// </summary>
    public class PaperSize
    {

        private static Dictionary<string, PaperSize> paperSizes = new Dictionary<string, PaperSize>();

        /// <summary>
        /// Represents an A6 page in portrait orientation.
        /// </summary>
        public static readonly PaperSize A6_Portrait = new PaperSize("A6_Portrait", "A6", Orientation.Portrait, 1240, 1748);
        /// <summary>
        /// Represents an A6 page in landscape orientation.
        /// </summary>
        public static readonly PaperSize A6_Landscape = new PaperSize("A6_Landscape", "A6", Orientation.Landscape, 1748, 1240);

        /// <summary>
        /// Represents an A5 page in portrait orientation.
        /// </summary>
        public static readonly PaperSize A5_Portrait = new PaperSize("A5_Portrait", "A5", Orientation.Portrait, 1748, 2480);
        /// <summary>
        /// Represents an A5 page in landscape orientation.
        /// </summary>
        public static readonly PaperSize A5_Landscape = new PaperSize("A5_Landscape", "A5", Orientation.Landscape, 2480, 1748);

        /// <summary>
        /// Represents an A4 page in portrait orientation.
        /// </summary>
        public static readonly PaperSize A4_Portrait = new PaperSize("A4_Portrait", "A4", Orientation.Portrait, 2480, 3508);
        /// <summary>
        /// Represents an A4 page in landscape orientation.
        /// </summary>
        public static readonly PaperSize A4_Landscape = new PaperSize("A4_Landscape", "A4", Orientation.Landscape, 3508, 2480);

        /// <summary>
        /// Represents an A3 page in portrait orientation.
        /// </summary>
        public static readonly PaperSize A3_Portrait = new PaperSize("A3_Portrait", "A3", Orientation.Portrait, 3508, 4961);
        /// <summary>
        /// Represents an A3 page in landscape orientation.
        /// </summary>
        public static readonly PaperSize A3_Landscape = new PaperSize("A3_Landscape", "A3", Orientation.Landscape, 4961, 3508);

        /// <summary>
        /// Represents an A2 page in portrait orientation.
        /// </summary>
        public static readonly PaperSize A2_Portrait = new PaperSize("A2_Portrait", "A2", Orientation.Portrait, 4961, 7016);
        /// <summary>
        /// Represents an A2 page in landscape orientation.
        /// </summary>
        public static readonly PaperSize A2_Landscape = new PaperSize("A2_Landscape", "A2", Orientation.Landscape, 7016, 4961);

        /// <summary>
        /// Represents an A1 page in portrait orientation.
        /// </summary>
        public static readonly PaperSize A1_Portrait = new PaperSize("A1_Portrait", "A1", Orientation.Portrait, 7016, 9933);
        /// <summary>
        /// Represents an A1 page in landscape orientation.
        /// </summary>
        public static readonly PaperSize A1_Landscape = new PaperSize("A1_Landscape", "A1", Orientation.Landscape, 9933, 7016);

        /// <summary>
        /// Represents an A0 page in portrait orientation.
        /// </summary>
        public static readonly PaperSize A0_Portrait = new PaperSize("A0_Portrait", "A0", Orientation.Portrait, 9933, 14043);
        /// <summary>
        /// Represents an A0 page in landscape orientation.
        /// </summary>
        public static readonly PaperSize A0_Landscape = new PaperSize("A0_Landscape", "A0", Orientation.Landscape, 14043, 9933);

        /// <summary>
        /// Represents US Letter paper in portrait orientation.
        /// </summary>
        public static readonly PaperSize Letter_Portrait = new PaperSize("Letter_Portrait", "Letter", Orientation.Portrait, 2550, 3300);
        /// <summary>
        /// Represents US Letter paper in landscape orientation.
        /// </summary>
        public static readonly PaperSize Letter_Landscape = new PaperSize("Letter_Landscape", "Letter", Orientation.Landscape, 3300, 2550);

        /// <summary>
        /// Represents US Legal paper in portrait orientation.
        /// </summary>
        public static readonly PaperSize Legal_Portrait = new PaperSize("Legal_Portrait", "Legal", Orientation.Portrait, 2550, 4200);
        /// <summary>
        /// Represents US Legal paper in landscape orientation.
        /// </summary>
        public static readonly PaperSize Legal_Landscape = new PaperSize("Legal_Landscape", "Legal", Orientation.Landscape, 4200, 2550);

        /// <summary>
        /// Represents a 4:3 slide canvas.
        /// </summary>
        public static readonly PaperSize Slide_4_3 = new PaperSize("Slide_4_3", "Slide 4:3", Orientation.Landscape, 3306, 2480);
        /// <summary>
        /// Represents a 16:9 slide canvas.
        /// </summary>
        public static readonly PaperSize Slide_16_9 = new PaperSize("Slide_16_9", "Slide 16:9", Orientation.Landscape, 3508, 1973);
        /// <summary>
        /// Represents a 16:10 slide canvas.
        /// </summary>
        public static readonly PaperSize Slide_16_10 = new PaperSize("Slide_16_10", "Slide 16:10", Orientation.Landscape, 3508, 2193);

        /// <summary>
        /// Identifies the paper size in serialized view data.
        /// </summary>
        public string Key { get; }
        /// <summary>
        /// Provides the human-readable paper size name.
        /// </summary>
        public String Name { get; }
        /// <summary>
        /// Indicates whether the size is portrait or landscape.
        /// </summary>
        public Orientation Orientation { get; }
        /// <summary>
        /// Stores the width in pixels.
        /// </summary>
        public int width { get; }
        /// <summary>
        /// Stores the height in pixels.
        /// </summary>
        public int height { get; }

        private PaperSize(String key, String name, Orientation orientation, int width, int height)
        {
            this.Key = key;
            this.Name = name;
            this.Orientation = orientation;
            this.width = width;
            this.height = height;

            paperSizes.Add(key, this);
        }

        /// <summary>
        /// Resolves a paper size by key.
        /// </summary>
        /// <param name="key">The serialized paper size key.</param>
        /// <returns>The matching paper size, or <see cref="A4_Portrait"/> when the key is null or unknown.</returns>
        public static PaperSize GetPaperSize(string key)
        {
            PaperSize paperSize;
            if (key == null || !paperSizes.TryGetValue(key, out paperSize))
            {
                paperSize = A4_Portrait;
            }

            return paperSize;
        }

    }

    /// <summary>
    /// Identifies the orientation of a paper size.
    /// </summary>
    public enum Orientation
    {
        /// <summary>
        /// Uses portrait orientation.
        /// </summary>
        Portrait,
        /// <summary>
        /// Uses landscape orientation.
        /// </summary>
        Landscape
    }

}
