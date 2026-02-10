using System.Collections.Generic;
using CouchPlayTest.Utilities;
using Raylib_cs;
using Font = CouchPlayTest.Drawing.Font.Font;

namespace CouchPlayTest;

class Program
{
    public const string Title = "Couch Play Test";
    public const int ScreenSize = 256;
    public const int PixelScale = 3;
    
    public static readonly Font LowRough = new(@"Data/FontAtlases\5_7_1_LowRough.png");
    public static readonly Font TitleRough = new(@"Data/FontAtlases\10_14_1_TitleRough.png");

    private static Stack<IScene> _sceneStack = [];

    static void Main(string[] args)
    {
        double previousTime = Raylib.GetTime();
        double deltaTime = 0;
        
        Raylib.InitWindow(ScreenSize*PixelScale, ScreenSize*PixelScale, "Couch Play Test");
        Raylib.ClearBackground(Color.Black);
        Raylib.SetTargetFPS(60);

        _sceneStack.Push(new MainMenu());
        SideMenu.Init();
        
        while (!Raylib.WindowShouldClose())
        {
            var activeScene = _sceneStack.Peek();

            // update logic
            if (!SideMenu.Update(deltaTime, activeScene is Game) && !SideMenu.IsOpen)
                activeScene.Update(deltaTime);

            // update graphics
            Raylib.ClearBackground(Color.Black);
            Raylib.BeginDrawing();
            activeScene.Render();
            SideMenu.RenderSideMenu(activeScene.Name, activeScene is Game);
            Raylib.EndDrawing();
            
            var currentTime = Raylib.GetTime();
            deltaTime = currentTime - previousTime;
            previousTime = currentTime;
        }
    }

    public static void PushScene(IScene scene)
        => _sceneStack.Push(scene);
    
    public static void PopScene()
    {
        if (_sceneStack.Count > 1)
            _sceneStack.Pop();
    }
}