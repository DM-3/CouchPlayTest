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
    
    static void Main(string[] args)
    {
        double previousTime = Raylib.GetTime();
        double deltaTime = 0;
        
        Raylib.InitWindow(ScreenSize*PixelScale, ScreenSize*PixelScale, "Couch Play Test");
        Raylib.ClearBackground(Color.Black);
        Raylib.SetTargetFPS(60);

        MainMenu mainMenu = new();
        SideMenu.Init();
        
        while (!Raylib.WindowShouldClose())
        {
            mainMenu.Update(deltaTime);
            
            Raylib.ClearBackground(Color.Black);
            
            Raylib.BeginDrawing();
            mainMenu.Render();
            Raylib.EndDrawing();
            
            var currentTime = Raylib.GetTime();
            deltaTime = currentTime - previousTime;
            previousTime = currentTime;
        }
    }
}