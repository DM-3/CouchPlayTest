using System.Numerics;
using CouchPlayTest.Drawing;
using CouchPlayTest.Drawing.Font;
using CouchPlayTest.Interfaces;
using CouchPlayTest.Players;
using Raylib_cs;

namespace CouchPlayTest;

public class MainMenu
{
    const int VotePoolWidth = 20;
    const int VotePoolHeight = 60;
    const int VotePoolYPos = 112;
    const int NumberOfPools = 3;

    int _peopleConnected;

    readonly List<Player> _players = new();
    
    public void Update(double delta)
    {
        if(Raylib.IsKeyDown(KeyboardKey.W) && _players.All(p => p.GetType() != new Wasd().GetType())) _players.Add(new Wasd());
        if(Raylib.IsKeyDown(KeyboardKey.Up) && _players.All(p => p.GetType() != new Arrows().GetType())) _players.Add(new Arrows());
        if((Raylib.IsGamepadButtonDown(0, GamepadButton.LeftFaceUp) || (Raylib.GetGamepadAxisMovement(0, GamepadAxis.LeftY) < -0.5f )) && _players.All(p => p.GetType() != new Controller().GetType())) _players.Add(new Controller());
        _peopleConnected = _players.Count;
        
        foreach (var player in _players) {
            PlayerMenuController(player, delta);
        }
    }
    public void Render()
    {
        FontUtility.DrawString(Program.ScreenSize/2 - "Input UP to connect.".Length * (Program.LowRough.fontData.dimensions[0]/2) - ("Players Connected: " + _peopleConnected).Length-1, 70, "Input UP to connect.", Program.LowRough, Program.White);
        FontUtility.DrawString(Program.ScreenSize/2 - ("Players Connected: " + _peopleConnected).Length * (Program.LowRough.fontData.dimensions[0]/2) - ("Players Connected: " + _peopleConnected).Length-1, 80, ("Players Connected: " + _peopleConnected), Program.LowRough, Program.White);
        FontUtility.DrawString(
            Program.ScreenSize/2 - (
                (Program.TitleRough.fontData.dimensions[0] * Program.Title.Length)
                + (Program.Title.Length - 1)) / 2, 90,
            Program.Title,
            Program.TitleRough,
            [160, 255, 255]
        );

        FontUtility.DrawString(10, 10, "Controllers Connected: ", Program.LowRough, Program.White);
        if(_players.Any(p => p is Wasd)) FontUtility.DrawString(10, 18, "WASD", Program.LowRough, Program.White);
        if(_players.Any(p => p is Arrows)) FontUtility.DrawString(10, 26, "Arrows", Program.LowRough, Program.White);
        if(_players.Any(p => p is Controller)) FontUtility.DrawString(10, 34, "Controller", Program.LowRough, Program.White);
        
        DrawVotePool(0, Program.White);
        DrawVotePool(1, Program.White);
        DrawVotePool(2, Program.White);

        for (var p = 0; p < _players.Count; p++) {
            var player = _players[p];
            DrawPoolVote(player.VotePoolIndex, p, _players[p].Color);
        }
    }

    void PlayerMenuController(Player? player, double deltaTime)
    {
        int x = (int)Math.Clamp(player!.GetInput().X, -1, 1);

        switch (player.MenuHasMoved) {
            case false when x != 0:
                player.VotePoolIndex = LoopVotePool(player.VotePoolIndex + x);
                player.MenuHasMoved = true;
                player.MenuDelay += deltaTime;
                return;
            case true:
                player.MenuDelay += deltaTime;
                break;
        }

        switch (player.MenuDelay) {
            case > 0.1f when x == 0:
                player.MenuHasMoved = false;
                player.MenuDelay = 0;
                return;
            case > 0.2f: //when x != 0:
                player.VotePoolIndex = LoopVotePool(player.VotePoolIndex + x);
                player.MenuHasMoved = true;
                player.MenuDelay = 0;
                return;
        }
    }

    int LoopVotePool(int currentVotePoolIndex)
    {
        return currentVotePoolIndex > NumberOfPools-1 ? 0 : currentVotePoolIndex < 0 ? NumberOfPools-1 : currentVotePoolIndex;
    }
    
    void DrawVotePool(int poolIndex, byte[] color)
    {
        int x = (int)Math.Round(Program.ScreenSize / (float)(NumberOfPools + 1) * (poolIndex + 1) - VotePoolWidth / 2f);
        Utility.DrawRectangle(x,     VotePoolYPos,          VotePoolWidth, VotePoolHeight + 2,  color);
        Utility.DrawRectangle(x + 1, VotePoolYPos + 1, VotePoolWidth - 2,   VotePoolHeight, Program.Black);
    }

    void DrawPoolVote(int poolIndex, int voteIndex, byte[] color)
    {
        int x = (int)Math.Round(Program.ScreenSize / (float)(NumberOfPools + 1) * (poolIndex + 1) - VotePoolWidth / 2f);
        Utility.DrawRectangle(x + 1, VotePoolYPos + 1 + VotePoolHeight/_peopleConnected * voteIndex, VotePoolWidth - 2, (VotePoolHeight/_peopleConnected), color);
    }
}