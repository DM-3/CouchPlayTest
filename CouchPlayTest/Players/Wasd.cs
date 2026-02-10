using System.Numerics;
using CouchPlayTest.Utilities;
using Raylib_cs;
using Transform = CouchPlayTest.Utilities.Transform;

namespace CouchPlayTest.Players;

public class Wasd : Player
{
    public Wasd() : base(Color.Red)
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
        if(Raylib.IsKeyDown(KeyboardKey.W)) input.Y = -2;
        if(Raylib.IsKeyDown(KeyboardKey.S)) input.Y = 2;
        if(Raylib.IsKeyDown(KeyboardKey.A)) input.X = -2;
        if(Raylib.IsKeyDown(KeyboardKey.D)) input.X = 2;
        return input == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(input);
    }
    
    public override bool GetSpecialInput()
    {
        return Raylib.IsKeyDown(KeyboardKey.Space);
    }
}