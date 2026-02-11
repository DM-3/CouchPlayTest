using Raylib_cs;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;
using Color = Raylib_cs.Color;
using Rectangle = System.Drawing.Rectangle;
namespace CouchPlayTest.Drawing;
#pragma warning disable CA1416

public static class DrawingUtility
{
    //Sets a scaled pixel at x, y to color;
    public static void DrawPixel(int x, int y, Color color) 
        => Raylib.DrawRectangle(x * Program.PixelScale, y * Program.PixelScale, Program.PixelScale, Program.PixelScale, color);

    //Sets an area of scaled pixels at x to x + width, y to y + height to color;
    public static void DrawRectangle(int x, int y, int width, int height, Color color)
        => Raylib.DrawRectangle(x * Program.PixelScale, y * Program.PixelScale, Program.PixelScale * width, Program.PixelScale * height, color);

    //Checks if an x, y coord is within the screen bounds;
    public static bool WithinBounds(int xA, int xB)
        => xA is >= 0 and < Program.ScreenSize && xB is >= 0 and < Program.ScreenSize;

    
    //Converts a Bitmap into a byte array. Reason: Much faster, and more recommended than GetPixel();
    public static byte[] GetPixelData(Bitmap bitmap)
    {
        BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
        try {
            int byteCount = bitmapData.Stride * bitmapData.Height;
            byte[] pixelData = new byte[byteCount];
            Marshal.Copy(bitmapData.Scan0, pixelData, 0, byteCount);
            return pixelData;
        } finally {
            bitmap.UnlockBits(bitmapData);
        } 
    }
}