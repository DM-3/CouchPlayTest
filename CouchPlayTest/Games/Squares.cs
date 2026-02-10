using Color = Raylib_cs.Color;
using CouchPlayTest.Drawing.Font;
using CouchPlayTest.Utilities;

namespace CouchPlayTest.Games;

public class Squares(Player[] players) : Game(players, "Squares")
{
    readonly Player[] _players = players;
    
    public override void Update(double delta)
    {
        foreach (var player in _players) {
            player.Update(delta);
            player.Transform.Position += player.GetInput();
        }
    }
    public override void Render()
    {
        foreach (var player in _players) {
            player.Render();
            player.DrawPlayer();
        }
        FontUtility.DrawString(FontUtility.GetStringCenteredPos("Welcome to Squares!", Program.LowRough), 10, "Welcome to Squares!", Program.LowRough, new Color(0, 255, 255));
    }
}