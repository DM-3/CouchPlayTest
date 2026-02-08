using System.Numerics;

namespace CouchPlayTest.Interfaces;

public struct Transform(Vector2 position)
{
    public Vector2 Position { get; set; } = position;
    public Vector2 Scale { get; set; } = Vector2.One;
}