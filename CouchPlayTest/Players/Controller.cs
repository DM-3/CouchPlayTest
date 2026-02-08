using System.Numerics;
using CouchPlayTest.Interfaces;
using Raylib_cs;
using Transform = CouchPlayTest.Interfaces.Transform;

namespace CouchPlayTest.Players;

public class Controller : Player
{
    public Controller() : base(Program.Blue)
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
        input.X = Raylib.GetGamepadAxisMovement(0, GamepadAxis.LeftX)*2;
        input.Y = Raylib.GetGamepadAxisMovement(0, GamepadAxis.LeftY)*2;
        return input;
    }
}