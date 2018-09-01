using System.Drawing;

namespace WindowsFormsApp
{
    public class ColorTable
    {
        public readonly int[][][] Backing;

        public static readonly ColorTable Mono = new ColorTable(Color.Black.ToArgb(), new[]
        {
            Color.White.ToArgb(),
        });

        public static readonly ColorTable GreenLowerThird = new ColorTable(Color.Black.ToArgb(), new[]
        {
            Color.LawnGreen.ToArgb(),
            Color.White.ToArgb(),
            Color.White.ToArgb(),
        });

        public static readonly ColorTable Rainbow = new ColorTable(Color.Black.ToArgb(), new[]
        {
            Color.White.ToArgb(),
            Color.Red.ToArgb(),
            Color.MediumPurple.ToArgb(),
            Color.Yellow.ToArgb(),
            Color.LawnGreen.ToArgb(),
            Color.Cyan.ToArgb(),
            Color.White.ToArgb(),
        });

        private ColorTable(int backgroundColor, int[] palette)
        {
            Palette = palette;
            Backing = new int[0x100][][];

            for (var i = 0; i <= 0xff; i++)
            {
                Backing[i] = new int[palette.Length][];

                for (var colorIndex = 0; colorIndex < palette.Length; colorIndex++)
                {
                    Backing[i][colorIndex] = new int[8];

                    for (var bitIndex = 0; bitIndex < 8; bitIndex++)
                    {
                        var isOn = (i & (1 << bitIndex)) > 0;
                        Backing[i][colorIndex][bitIndex] = isOn ? palette[colorIndex] : backgroundColor;
                    }
                }
            }
        }

        public int[] Palette { get; }
    }
}