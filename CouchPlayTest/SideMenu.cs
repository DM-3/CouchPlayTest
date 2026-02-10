using CouchPlayTest.Drawing;
using CouchPlayTest.Drawing.Font;
using CouchPlayTest.Utilities;
using CouchPlayTest.Utilities.UI;
using Raylib_cs;
using Font = CouchPlayTest.Drawing.Font.Font;

namespace CouchPlayTest;

public static class SideMenu
{
    public static bool IsOpen = false;
    static double _windowAnimation = 0;
    const float WindowAnimationTime = 0.1f;

    static int _numberOfOptions = 1;
    static int _selectedOption = 0;
    static Player? _player;

    public static void Init()
    {
        MainMenu.GlobalUi.Ui.Add("SideMenuSelection", new UiInteractable());
        MainMenu.GlobalUi.Ui.Add("SideMenuButton", new UiInteractable());
    }

    public static bool Update(double delta, bool inGame)
    {
        if(_player == null && MainMenu.Players.Count != 0) _player = MainMenu.Players[0]; 
        
        if (Raylib.IsKeyPressed(KeyboardKey.Tab) || Raylib.IsGamepadButtonPressed(0, GamepadButton.MiddleRight)) {
            IsOpen = !IsOpen;
            _selectedOption = 0;
        }

        switch (IsOpen) {
            case true when _windowAnimation < WindowAnimationTime:
                _windowAnimation += delta;
                break;
            case false when _windowAnimation > 0:
                _windowAnimation -= delta;
                break;
        }

        if (!IsOpen || _player == null) return false;

        UiInteractable.UiResult selection;
        UiInteractable.UiResult button;
        
        _numberOfOptions = 1;
        switch (inGame) {
            case true:
                _numberOfOptions += 1;
                
                selection = MainMenu.GlobalUi.Ui["SideMenuSelection"].UpdateInputSelection(_player, delta, UiInteractable.UiAxis.Y, (0, _numberOfOptions-1), _selectedOption, false, true);
                button = MainMenu.GlobalUi.Ui["SideMenuButton"].UpdateInputButton(_player, delta);
                
                _selectedOption = (int)selection.Value;
                switch (_selectedOption) {
                    case 0 when button.Triggered:
                        Raylib.CloseWindow();
                        Environment.Exit(0);
                        break;
                    case 1 when button.Triggered:
                        MainMenu.ReturnToMainMenu();
                        return true;
                }
                
                break;
            
            case false:
                selection = MainMenu.GlobalUi.Ui["SideMenuSelection"].UpdateInputSelection(_player, delta, UiInteractable.UiAxis.Y, (0, _numberOfOptions-1), _selectedOption, false, true);
                button = MainMenu.GlobalUi.Ui["SideMenuButton"].UpdateInputButton(_player, delta);
                
                _selectedOption = (int)selection.Value;
                switch (_selectedOption) {
                    case 0 when button.Triggered:
                        Raylib.CloseWindow();
                        Environment.Exit(0);
                        break;
                }
                break;
        }

        return false;
    }

    public static int GetTempCursorRom()
    {
        switch (_selectedOption) {
            case 0:
                return Program.ScreenSize-16;
            case 1:
                return 26;
        }

        return 0;
    }
    
    public static void RenderSideMenu(string displayName, bool inGame)
    {
        var animation = RenderWindow(displayName, GetTempCursorRom());

        if (!inGame || !IsOpen && _windowAnimation <= 0) return;
        
        DrawingUtility.DrawRectangle(animation.textAnimation-2, 26, animation.xAnimation-6, Program.LowRough.FontData.dimensions[1]+4, new Color(100,100,100));
        FontUtility.DrawString(FontUtility.GetStringCenteredPos("Return", Program.LowRough, (animation.textAnimation-2, animation.textAnimation-2+animation.xAnimation-6)),28, "Return", Program.LowRough, Color.White);
    }
    
    static (int textAnimation, int xAnimation) RenderWindow(string windowDisplayTitle, int cursorY)
    {
        if (!IsOpen && _windowAnimation <= 0) return (0,0);
        int windowWidth = FontUtility.GetStringWidth(windowDisplayTitle, Program.LowRough) + 10;
        int xAnimation = (int)Utility.Lerp(0, windowWidth, (float)_windowAnimation / WindowAnimationTime);
        DrawingUtility.DrawRectangle(0, 0, xAnimation, Program.ScreenSize, new Color(160,160,160,200));

        int textAnimation = 5 - windowWidth + xAnimation;
        
        if(_player != null) DrawingUtility.DrawRectangle(textAnimation-3, cursorY-1, xAnimation-4, Program.LowRough.FontData.dimensions[1]+6, _player.Color);
        
        DrawingUtility.DrawRectangle(textAnimation-2, 8, xAnimation-6, Program.LowRough.FontData.dimensions[1]+4, new Color(100,100,100));
        FontUtility.DrawString(textAnimation, 10, windowDisplayTitle, Program.LowRough, Color.White);
        
        DrawingUtility.DrawRectangle(textAnimation-2, Program.ScreenSize-16, xAnimation-6, Program.LowRough.FontData.dimensions[1]+4, new Color(100,100,100));
        FontUtility.DrawString(FontUtility.GetStringCenteredPos("Exit", Program.LowRough, (textAnimation-2, textAnimation-2+xAnimation-6)), Program.ScreenSize-16+2, "Exit", Program.LowRough, Color.White);

        return (textAnimation, xAnimation);
    }
}