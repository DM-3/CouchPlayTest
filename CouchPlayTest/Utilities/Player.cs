using System.Data;
using System.Numerics;

namespace CouchPlayTest.Interfaces;

public abstract class Player(byte[] color)
{
    public Transform Transform;
    
    public int VotePoolIndex;
    public double MenuDelay;
    public bool MenuHasMoved = false;
    public byte[] Color = color;
    public abstract void Update(double deltaTime);
    public abstract void Render();

    public abstract Vector2 GetInput();
}