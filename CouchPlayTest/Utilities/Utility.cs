using System.Numerics;

namespace CouchPlayTest.Utilities;

public class Utility
{
    public static Vector2 ClampMagnitude(Vector2 v, float maxLength)
    {
        var magnitude = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);

        if (!(magnitude > maxLength) || !(magnitude > 0)) return new Vector2(v.X, v.Y);
        var scale = maxLength / magnitude;
        return new Vector2(v.X * scale, v.Y * scale);
    }
    
    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * Math.Clamp(t, 0, 1);
    }
}