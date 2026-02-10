namespace CouchPlayTest;

public interface IScene
{
    public string Name { get; }
    
    public void Update(double delta);

    public void Render();
}
