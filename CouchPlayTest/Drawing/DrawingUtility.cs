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

    //Checks if an x, y coord is within the screen bounds;
    public static bool IsOnScreen(int x, int y)
        => x >= 0 && x < Program.ScreenSize && y >= 0 && y < Program.ScreenSize;

    //Todo: Comment explanations and optimize.
    //Unused, maybe used later;
    //File subject to change and optimizations;
    //If possible, use raylib functions directly
    public static void DrawLine(int xA, int yA, int xB, int yB, Color color)
    {
        int dx    = Math.Abs(xB - xA);
        int sx    = xA < xB ? 1 : -1;
        int dy    = -Math.Abs(yB - yA);
        int sy    = yA < yB ? 1 : -1;
        int error = dx + dy;

        while (true) {
            if(IsOnScreen(xA, yA)) DrawPixel(xA, yA, color);
            int e2 = error * 2;
            if (e2 >= dy) {
                if (xA == xB) break;
                error += dy;
                xA += sx;
            }
            if (e2 <= dx) {
                if (yA == yB) break;
                error += dx;
                yA += sy;
            }
        }
    }

    public static void DrawCircle(int x, int y, int radius, Color color)
    {
        float t1 = radius / 16;
        int xc = radius; int yc = 0;
        while (xc > yc) {
            if(IsOnScreen(xc  + x, yc  + y)) DrawPixel(xc  + x, yc  + y, color);
            if(IsOnScreen(yc  + x, xc  + y)) DrawPixel(yc  + x, xc  + y, color);
            if(IsOnScreen(xc  + x, -yc + y)) DrawPixel(xc  + x, -yc + y, color);
            if(IsOnScreen(yc  + x, -xc + y)) DrawPixel(yc  + x, -xc + y, color);
            if(IsOnScreen(-xc + x, yc  + y)) DrawPixel(-xc + x, yc  + y, color);
            if(IsOnScreen(-yc + x, xc  + y)) DrawPixel(-yc + x, xc  + y, color);
            if(IsOnScreen(-xc + x, -yc + y)) DrawPixel(-xc + x, -yc + y, color);
            if(IsOnScreen(-yc + x, -xc + y)) DrawPixel(-yc + x, -xc + y, color);
            
            yc++;
            t1 += yc;
            float t2 = t1 - xc; 
            if (t2 >= 0) {
                t1 = t2;
                xc--;
            }
        }
    }
}