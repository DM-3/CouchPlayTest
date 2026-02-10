namespace CouchPlayTest.Utilities;

public abstract class Game(Player[] players, string gameName) : IScene
{
    public Player[] Players = players;

    private string _name = gameName;
    public string Name => _name;

    public abstract void Update(double delta);

    public abstract void Render();
}