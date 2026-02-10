using Color = Raylib_cs.Color;
using CouchPlayTest.Drawing.Font;
using CouchPlayTest.Utilities;

namespace CouchPlayTest.Games;

public class Test(Player[] players) : Game(players, "Test")
{
    readonly Player[] _players = players;
    
    public override void Update(double delta)
    {
        foreach (var player in _players) {
            player.Update(delta);
        }
    }
    public override void Render()
    {
        foreach (var player in _players) {
            player.Render();
        }
        FontUtility.DrawString(FontUtility.GetStringCenteredPos("Welcome to Test!", Program.LowRough), 10, "Welcome to Test!", Program.LowRough, new Color(0, 255, 255));
    }
}