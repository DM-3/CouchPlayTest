namespace CouchPlayTest.Utilities;

public abstract class Game(Player[] players, string gameName)
{
    //All games must inherit this.
    public readonly string GameName = gameName;
    public Player[] Players = players;
    public abstract void Update(double delta);

    public abstract void Render();
}