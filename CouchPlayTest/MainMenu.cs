using System.Numerics;
using CouchPlayTest.Drawing;
using CouchPlayTest.Drawing.Font;
using CouchPlayTest.Games;
using CouchPlayTest.Utilities;
using CouchPlayTest.Players;
using CouchPlayTest.Utilities;
using Raylib_cs;
using Font = CouchPlayTest.Drawing.Font.Font;

namespace CouchPlayTest;

public class MainMenu
{
    const int VotePoolWidth = 20;
    const int VotePoolHeight = 60;
    const int VotePoolYPos = 112;
    readonly int _numberOfPools = GameTypes.Length;

    int _peopleConnected;
    readonly List<Player> _players = [];

    static readonly Type[] GameTypes = [ typeof(Squares), typeof(Test)];
    static Game? _selectedGame = null;
    static double _gameCountDown = 0;
    const int CountDownSeconds = 2;
    static int _playersVoted = 0;
    
    public void Update(double delta)
    {
        SideMenu.Update(delta);
        
        if (_selectedGame != null) {
            _selectedGame.Update(delta);
            return;
        }
        
        if(Raylib.IsKeyDown(KeyboardKey.W) && _players.All(p => p.GetType() != new Wasd().GetType())) _players.Add(new Wasd());
        if(Raylib.IsKeyDown(KeyboardKey.Up) && _players.All(p => p.GetType() != new Arrows().GetType())) _players.Add(new Arrows());
        if((Raylib.IsGamepadButtonDown(0, GamepadButton.LeftFaceUp) || (Raylib.GetGamepadAxisMovement(0, GamepadAxis.LeftY) < -0.5f )) && _players.All(p => p.GetType() != new Controller().GetType())) _players.Add(new Controller());
        _peopleConnected = _players.Count;

        foreach (var player in _players) {
            PlayerMenuController(player, delta);
        }

        if (_playersVoted == _players.Count && _players.Count > 0) {
            _gameCountDown += delta;
        } else {
            _gameCountDown = 0;
        }

        if (_gameCountDown >= CountDownSeconds) {
            _playersVoted = 0;
            var votes = new int[GameTypes.Length];
            foreach (var player in _players) {
                votes[player.MenuVoted]++;
                player.MenuVoted = -1;
            }
            var mostVotedIndex = -1;
            for (var i = 0; i < votes.Length; i++) {
                if (votes[i] > mostVotedIndex) mostVotedIndex = i;
            }
            _selectedGame = (Activator.CreateInstance(GameTypes[mostVotedIndex], [_players.ToArray()]) as Game);
        }
    }
    public void Render()
    {
        if (_selectedGame != null) {
            _selectedGame.Render();
            SideMenu.RenderGameSideMenu(_selectedGame.GameName);
        }
        else {
            RenderMainMenu();
            SideMenu.RenderMainSideMenu();
        }
    }

    void PlayerMenuController(Player? player, double deltaTime)
    {
        #region Voting Button
        switch (player!.MenuVoted) {
            case -1 when player.GetSpecialInput() && !player.MenuVotedRecently:
                player.MenuVoted = player.MenuVotePoolIndex;
                player.MenuVoteDelay = 0;
                player.MenuVotedRecently = true;
                _playersVoted++;
                break;
            case >= 0 when player.GetSpecialInput() && !player.MenuVotedRecently:
                player.MenuVoted = -1;
                player.MenuVoteDelay = 0;
                player.MenuVotedRecently = true;
                _playersVoted--;
                break;
        }
        
        switch (player.MenuVotedRecently) {
            case true when player.MenuVoteDelay > 0.2f:
                player.MenuVotedRecently = false;
                player.MenuVoteDelay = 0;
                break;
            case true:
                player.MenuVoteDelay += deltaTime;
                break;
        }
        
        if (player.MenuVoted != -1) return;
        #endregion

        #region Voting Selection
        int x = (int)Math.Clamp(player!.GetInput().X*2, -1, 1);

        switch (player.MenuMoved) {
            case false when x != 0:
                player.MenuVotePoolIndex = LoopVotePool(player.MenuVotePoolIndex + x);
                player.MenuMoved = true;
                player.MenuDelay += deltaTime;
                return;
            case true:
                player.MenuDelay += deltaTime;
                break;
        }

        switch (player.MenuDelay) {
            case > 0.1f when x == 0:
                player.MenuMoved = false;
                player.MenuDelay = 0;
                return;
            case > 0.2f: //when x != 0:
                player.MenuVotePoolIndex = LoopVotePool(player.MenuVotePoolIndex + x);
                player.MenuMoved = true;
                player.MenuDelay = 0;
                return;
        }
        #endregion
    }

    int LoopVotePool(int currentVotePoolIndex)
    {
        return currentVotePoolIndex > _numberOfPools-1 ? 0 : currentVotePoolIndex < 0 ? _numberOfPools-1 : currentVotePoolIndex;
    }

    void RenderMainMenu()
    {
        FontUtility.DrawString(FontUtility.GetStringCenteredPos("Input UP to connect.", Program.LowRough), 70, "Input UP to connect.", Program.LowRough, Program.White);
        FontUtility.DrawString(FontUtility.GetStringCenteredPos("Players Connected: " + _peopleConnected, Program.LowRough), 80, "Players Connected: " + _peopleConnected, Program.LowRough, Program.White);
        FontUtility.DrawString(FontUtility.GetStringCenteredPos(Program.Title, Program.TitleRough), 90, Program.Title, Program.TitleRough, [160, 255, 255, 255]);

        FontUtility.DrawString(10, 200, "Players Voted: " + _playersVoted, Program.LowRough, Program.White);

        FontUtility.DrawString(10, 10, "Controllers Connected:", Program.LowRough, Program.White);
        if (_players.Any(p => p is Wasd)) FontUtility.DrawString(10, 18, "WASD", Program.LowRough, Program.White);
        if (_players.Any(p => p is Arrows)) FontUtility.DrawString(10, 26, "Arrows", Program.LowRough, Program.White);
        if (_players.Any(p => p is Controller)) FontUtility.DrawString(10, 34, "Controller", Program.LowRough, Program.White);

        for (var g = 0; g < GameTypes.Length; g++)
            DrawVotePool(g, GameTypes[g].Name, Program.White);

        for (var p = 0; p < _players.Count; p++)
            DrawPoolVote(_players[p].MenuVotePoolIndex, p, _players[p].Color);

        if (_playersVoted != _players.Count || _playersVoted == 0)
            return;

        Utility.DrawRectangle(0, Program.ScreenSize / 2 - 40, Program.ScreenSize, 40, [150,150,150,255]);
        FontUtility.DrawString(FontUtility.GetStringCenteredPos("All players have voted.", Program.LowRough), Program.ScreenSize / 2 - 33, "All players have voted.", Program.LowRough, Program.White);
        FontUtility.DrawString(FontUtility.GetStringCenteredPos("Game Starting in " + (CountDownSeconds - _gameCountDown).ToString("0.0") + " seconds.", Program.LowRough), Program.ScreenSize / 2 - 18, "Game Starting in " + (CountDownSeconds - _gameCountDown).ToString("0.0") + " seconds.", Program.LowRough, Program.White);
    }
    
    void DrawVotePool(int poolIndex, string gameName, byte[] color)
    {
        int x = (int)Math.Round(Program.ScreenSize / (float)(_numberOfPools + 1) * (poolIndex + 1) - VotePoolWidth / 2f);
        Utility.DrawRectangle(x, VotePoolYPos, VotePoolWidth, VotePoolHeight + 2, color);
        Utility.DrawRectangle(x + 1, VotePoolYPos + 1, VotePoolWidth - 2, VotePoolHeight, Program.Black);
        FontUtility.DrawString(x + (VotePoolWidth - ((Program.LowRough.FontData.dimensions[0] * gameName.Length) + (gameName.Length - 1))) / 2, VotePoolYPos + VotePoolHeight + 4, gameName, Program.LowRough, Program.White);
    }

    void DrawPoolVote(int poolIndex, int voteIndex, byte[] color)
    {
        int x = (int)Math.Round(Program.ScreenSize / (float)(_numberOfPools + 1) * (poolIndex + 1) - VotePoolWidth / 2f);
        Utility.DrawRectangle(x + 1, VotePoolYPos + 1 + VotePoolHeight/_peopleConnected * voteIndex, VotePoolWidth - 2, (VotePoolHeight/_peopleConnected), color);
    }
}