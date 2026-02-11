using System.Data;
using Color = Raylib_cs.Color;
using System.Numerics;
using CouchPlayTest.Drawing;
using CouchPlayTest.Utilities.UI;

namespace CouchPlayTest.Utilities;

public abstract class Player(Color color)
{
    public readonly Color Color = color;
    
    public Dictionary<string, UiInteractable> Ui = [];
    
    //Player vote information;
    public int VotePoolIndex;
    public int Voted = -1;

    public Transform Transform = new(Vector2.Zero);
    
    public virtual void Update(double deltaTime) {}
    public virtual void Render() {}
    public virtual void DrawPlayer()
    {
        //Draws generic player sprite, however games do not have to use this;
        var scaleX = 6 * Transform.Scale.X;   
        var scaleY = 6 * Transform.Scale.Y;   
        var x = (int)(Transform.Position.X - scaleX / 2);
        var y = (int)(Transform.Position.Y - scaleY / 2);
        DrawingUtility.DrawRectangle(x, y, (int)scaleX, (int)scaleY, Color);
        DrawingUtility.DrawRectangle(x + 1, y + 1, (int)scaleX - 2, (int)scaleY - 2, new Color(0, 0, 0, 255));
    }
    public abstract Vector2 GetInput();
    public abstract bool GetSpecialInput();
}