namespace CouchPlayTest.Drawing;

public static class WireFrame
{
    public static void DrawLine(int xA, int yA, int xB, int yB, byte[] color)
    {
        int dx    = Math.Abs(xB - xA);
        int sx    = xA < xB ? 1 : -1;
        int dy    = -Math.Abs(yB - yA);
        int sy    = yA < yB ? 1 : -1;
        int error = dx + dy;

        while (true) {
            if(Utility.WithinBounds(xA, yA)) Utility.DrawPixel(xA, yA, color);
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

    public static void DrawCircle(int x, int y, int radius, byte[] color)
    {
        float t1 = radius / 16;
        int xc = radius; int yc = 0;
        while (xc > yc) {
            if(Utility.WithinBounds(xc  + x, yc  + y)) Utility.DrawPixel(xc  + x, yc  + y, color);
            if(Utility.WithinBounds(yc  + x, xc  + y)) Utility.DrawPixel(yc  + x, xc  + y, color);
            if(Utility.WithinBounds(xc  + x, -yc + y)) Utility.DrawPixel(xc  + x, -yc + y, color);
            if(Utility.WithinBounds(yc  + x, -xc + y)) Utility.DrawPixel(yc  + x, -xc + y, color);
            if(Utility.WithinBounds(-xc + x, yc  + y)) Utility.DrawPixel(-xc + x, yc  + y, color);
            if(Utility.WithinBounds(-yc + x, xc  + y)) Utility.DrawPixel(-yc + x, xc  + y, color);
            if(Utility.WithinBounds(-xc + x, -yc + y)) Utility.DrawPixel(-xc + x, -yc + y, color);
            if(Utility.WithinBounds(-yc + x, -xc + y)) Utility.DrawPixel(-yc + x, -xc + y, color);
            
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