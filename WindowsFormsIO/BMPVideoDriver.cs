using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Intel8080;
using SpaceInvaders;

namespace WindowsFormsApp
{
    public class BMPVideoDriver
    {
        private readonly int[] bmpBits;
        public readonly Bitmap bmp;

        public BMPVideoDriver()
        {
            bmpBits = new int[Machine.VideoLogicalHeight * Machine.VideoLogicalWidth];
            var bmpBitsHnd = GCHandle.Alloc(bmpBits, GCHandleType.Pinned);
            bmp = new Bitmap(Machine.VideoLogicalHeight, Machine.VideoLogicalWidth, Machine.VideoLogicalHeight * 4, PixelFormat.Format32bppArgb, bmpBitsHnd.AddrOfPinnedObject());
        }

        public void Render(RAM ram, ColorTable colorTable)
        {
            for (var byteIndex = 0; byteIndex < Machine.VideoRAMLength; byteIndex++)
            {
                var loc = byteIndex + Machine.VideoRAMLocation;
       
                var value = ram[loc];
                var pixelIndex = byteIndex * 8;
                var sourceRow = pixelIndex / Machine.VideoLogicalWidth;
                var sourceCol = pixelIndex % Machine.VideoLogicalWidth;

                var destCol = sourceRow;
                var destRow = Machine.VideoLogicalWidth - 1 - sourceCol;

                var colorIndex = pixelIndex % Machine.VideoLogicalWidth / (Machine.VideoLogicalWidth / colorTable.Palette.Length);

                var vramColors = colorTable.Backing[value][colorIndex];

                for (var bitIndex = 0; bitIndex < 8; bitIndex++)
                {
                    bmpBits[(destRow - bitIndex) * Machine.VideoLogicalHeight + destCol] = vramColors[bitIndex];
                }
            }
        }
    }
}