using System.Numerics;

namespace CouchPlayTest.Utilities;

public static class Utility
{
    //General use math utilities;
    
    public static Vector2 ClampMagnitude(Vector2 v, float maxLength)
        => v.Length() <= maxLength ? v : v / v.Length() * maxLength;
    
    public static float Lerp(float a, float b, float t)
        => a + (b - a) * Math.Clamp(t, 0, 1);
}
