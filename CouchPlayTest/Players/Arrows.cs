using System.Numerics;
using CouchPlayTest.Interfaces;
using Raylib_cs;
using Transform = CouchPlayTest.Interfaces.Transform;

namespace CouchPlayTest.Players;

public class Arrows : Player
{
    public Arrows() : base(Program.Green)
    {
        Transform = new Transform(Vector2.Zero);
    }
    public override void Update(double deltaTime)
    {
        
    }
    public override void Render()
    {
        
    }

    public override Vector2 GetInput()
    {
        Vector2 input = Vector2.Zero;
        if(Raylib.IsKeyDown(KeyboardKey.Up)) input.Y = -2;
        if(Raylib.IsKeyDown(KeyboardKey.Down)) input.Y = 2;
        if(Raylib.IsKeyDown(KeyboardKey.Left)) input.X = -2;
        if(Raylib.IsKeyDown(KeyboardKey.Right)) input.X = 2;
        return Vector2.Normalize(input);
    }
}