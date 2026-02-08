using CouchPlayTest.Drawing;
using CouchPlayTest.Drawing.Font;
using Raylib_cs;
using Font = CouchPlayTest.Drawing.Font.Font;

namespace CouchPlayTest;

public static class SideMenu
{
    public static bool IsOpen = false;
    static double _windowAnimation = 0;
    const float WindowAnimationTime = 0.1f;
    
    public static void Update(double delta)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Tab)) {
            IsOpen = !IsOpen;
        }

        switch (IsOpen) {
            case true when _windowAnimation < WindowAnimationTime:
                _windowAnimation += delta;
                break;
            case false when _windowAnimation > 0:
                _windowAnimation -= delta;
                break;
        }
    }

    public static void RenderGameSideMenu(string gameName)
    {
        RenderWindow(gameName);
    }

    public static void RenderMainSideMenu()
    {
        RenderWindow("Main Menu");
    }

    public static void RenderWindow(string windowDisplayTitle)
    {
        if (!IsOpen && _windowAnimation <= 0) return;
        int windowWidth = FontUtility.GetStringWidth(windowDisplayTitle, Program.LowRough) + 10;
        int xAnimation = (int)Utility.Lerp(0, windowWidth, (float)_windowAnimation / WindowAnimationTime);
        Utility.DrawRectangle(0, 0, xAnimation, Program.ScreenSize, [160,160,160,230]);

        int textAnimation = 5 - windowWidth + xAnimation;
        
        Utility.DrawRectangle(textAnimation-2, 8, xAnimation-6, Program.LowRough.FontData.dimensions[1]+4, [100,100,100,255]);
        
        FontUtility.DrawString(textAnimation, 10, windowDisplayTitle, Program.LowRough, Program.White);
    }
}