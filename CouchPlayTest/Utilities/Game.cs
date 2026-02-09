namespace CouchPlayTest.Utilities;

public abstract class Game(Player[] players, string gameName)
{
    public readonly string GameName = gameName;
    public Player[] Players = players;
    public abstract void Update(double delta);

    public abstract void Render();
}