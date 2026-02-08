using CouchPlayTest.Interfaces;

namespace CouchPlayTest.Games;

public class Squares : IGame
{
    readonly Player[] _players;
    Squares(Player[] players) => this._players = players;

    public void Update(double delta)
    {
        foreach (var player in _players) {
            player.Update(delta);
        }
    }
    public void Render()
    {
        foreach (var player in _players) {
            player.Render();
        }
    }
}