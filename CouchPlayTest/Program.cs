using System.Numerics;
using CouchPlayTest.Drawing;
using CouchPlayTest.Drawing.Font;
using Raylib_cs;
using Font = CouchPlayTest.Drawing.Font.Font;

namespace CouchPlayTest;

class Program
{
    public const string Title = "Couch Play Test";
    public const int ScreenSize = 256;
    public const int PixelScale = 3;
    
    public static readonly byte[] White   = [255, 255, 255];
    public static readonly byte[] Black   = [0,   0,     0];
    public static readonly byte[] Cyan    = [0,   255, 255];
    public static readonly byte[] Magenta = [255, 0,   255];
    public static readonly byte[] Yellow  = [255, 255, 0];
    public static readonly byte[] Red     = [255, 0,   0];
    public static readonly byte[] Blue    = [0,   0,   255];
    public static readonly byte[] Green   = [0,   255, 0];

    static readonly (byte, byte)[] PlayerV = [(0, 0), (0, 1), (1, 1), (1, 0)];
    static readonly (byte, byte)[] PlayerI = [(0, 1), (0, 3), (2, 1), (2, 3)];

    public static readonly Font LowRough = new(@"Data\FontAtlases\5_7_1_LowRough.png");
    public static readonly Font TitleRough = new(@"Data\FontAtlases\10_14_1_TitleRough.png");
    
    public struct Transform(float[] position, float[] scale)
    {
        public float[] Position = position;
        public float[] Scale = scale;
    }

    
    static void Main(string[] args)
    {
        double currentTime = 0;
        double previousTime = Raylib.GetTime();
        double deltaTime = 0;
        
        Raylib.InitWindow(ScreenSize*PixelScale, ScreenSize*PixelScale, "Couch Play Test");
        Raylib.ClearBackground(Color.Black);
        Raylib.SetTargetFPS(60);
        
        Transform playerA = new Transform([64,64],[10,10]);
        Transform playerB = new Transform([64,64],[10,10]);
        Transform playerC = new Transform([64,64],[10,10]);
        Transform playerD = new Transform([64,64],[10,10]);
        Transform playerE = new Transform([64,64],[10,10]);

        MainMenu mainMenu = new();
        
        while (!Raylib.WindowShouldClose())
        {
            playerA.Position[0] += Raylib.GetGamepadAxisMovement(0, GamepadAxis.LeftX)*2;
            playerA.Position[1] += Raylib.GetGamepadAxisMovement(0, GamepadAxis.LeftY)*2;
            
            playerB.Position[0] += Raylib.GetGamepadAxisMovement(1, GamepadAxis.LeftX)*2;
            playerB.Position[1] += Raylib.GetGamepadAxisMovement(1, GamepadAxis.LeftY)*2;
            
            if(Raylib.IsKeyDown(KeyboardKey.W)) playerC.Position[1] -= 2;
            if(Raylib.IsKeyDown(KeyboardKey.S)) playerC.Position[1] += 2;
            if(Raylib.IsKeyDown(KeyboardKey.D)) playerC.Position[0] += 2;
            if(Raylib.IsKeyDown(KeyboardKey.A)) playerC.Position[0] -= 2;
            
            if(Raylib.IsKeyDown(KeyboardKey.Up))    playerD.Position[1] -= 2;
            if(Raylib.IsKeyDown(KeyboardKey.Down))  playerD.Position[1] += 2;
            if(Raylib.IsKeyDown(KeyboardKey.Right)) playerD.Position[0] += 2;
            if(Raylib.IsKeyDown(KeyboardKey.Left))  playerD.Position[0] -= 2;

            playerE.Position[0] = Raylib.GetMousePosition().X/PixelScale - playerE.Scale[0]/2;
            playerE.Position[1] = Raylib.GetMousePosition().Y/PixelScale - playerE.Scale[1]/2;
            
            mainMenu.Update(deltaTime);

            #region Rendering
            {
                Raylib.ClearBackground(Color.Black);
                Raylib.BeginDrawing();
                
                mainMenu.Render();
                
                /*
                DrawMesh(PlayerV, PlayerI, Blue,    playerA);
                DrawMesh(PlayerV, PlayerI, Red,     playerB);
                DrawMesh(PlayerV, PlayerI, Green,   playerC);
                DrawMesh(PlayerV, PlayerI, Magenta, playerD);
                DrawMesh(PlayerV, PlayerI, Yellow,  playerE);
                
                FontUtility.DrawCharacter(4,  5, 'H', LowRough, White);
                FontUtility.DrawCharacter(10, 5, 'E', LowRough, Cyan);
                FontUtility.DrawCharacter(16, 5, 'L', LowRough, Blue);
                FontUtility.DrawCharacter(22, 5, 'L', LowRough, Green);
                FontUtility.DrawCharacter(28, 5, 'O', LowRough, Magenta);
                FontUtility.DrawCharacter(34, 5, ' ', LowRough, Red);
                FontUtility.DrawCharacter(40, 5, 'W', LowRough, Yellow);
                FontUtility.DrawCharacter(46, 5, 'O', LowRough, Cyan);
                FontUtility.DrawCharacter(52, 5, 'R', LowRough, Blue);
                FontUtility.DrawCharacter(58, 5, 'L', LowRough, Green);
                FontUtility.DrawCharacter(64, 5, 'D', LowRough, Red);
                
                FontUtility.DrawString(4, 15, "String Test!()", LowRough, White);
                FontUtility.DrawString(4, 23, "ABCDEFGHIJKLMNOPQRSTUVWXYZ.!?&*+/\\\'\":;", LowRough, White);
                FontUtility.DrawString(4, 31, "DeltaTime = " + Math.Round(deltaTime, 4), LowRough, White);
                */
                Raylib.EndDrawing();
            }
            #endregion
            
            currentTime = Raylib.GetTime();
            deltaTime = currentTime - previousTime;
            previousTime = currentTime;
        }
    }

    public static void DrawMesh((byte, byte)[] vertices, (byte, byte)[] indices, byte[] color, Transform t)
    {
        for (int i = 0; i < indices.Length; i++) {
            WireFrame.DrawLine(
                (byte)(vertices[indices[i].Item1].Item1 * t.Scale[0] + t.Position[0]), (byte)(vertices[indices[i].Item1].Item2 * t.Scale[1] + t.Position[1]),
                (byte)(vertices[indices[i].Item2].Item1 * t.Scale[0] + t.Position[0]), (byte)(vertices[indices[i].Item2].Item2 * t.Scale[1] + t.Position[1]), 
                color
            );
        }
    }
}