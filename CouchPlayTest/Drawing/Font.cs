#pragma warning disable CA1416
using System.Drawing;
using Color = Raylib_cs.Color;

namespace CouchPlayTest.Drawing;

public class Font
{
    //Store invalid characters to not spam console with warnings;
    static HashSet<char> _unrecognizedCharacters = [];
    
    //Dictionary to loop up a character via char and get character id (int) back.
    public static readonly Dictionary<char, int> CharacterMap = new Dictionary<char, int>()
    {
        {'A',  0 }, {'B', 1 }, {'C', 2 },
        {'D',  3 }, {'E', 4 }, {'F', 5 },
        {'G',  6 }, {'H', 7 }, {'I', 8 },
        {'J',  9 }, {'K', 10}, {'L', 11},
        {'M',  12}, {'N', 13}, {'O', 14},
        {'P',  15}, {'Q', 16}, {'R', 17},
        {'S',  18}, {'T', 19}, {'U', 20},
        {'V',  21}, {'W', 22}, {'X', 23},
        {'Y',  24}, {'Z', 25}, 
        {'1',  26}, {'2', 27}, {'3', 28},
        {'4',  29}, {'5', 30}, {'6', 31},
        {'7',  32}, {'8', 33}, {'9', 34},
        {'0',  35}, {'.', 36}, {'!', 37},
        {'?',  38}, {'&', 39}, {'*', 40},
        {'+',  41}, {'/', 42}, {'\\',43},
        {'\'', 44}, {'"', 45}, {':', 46},
        {';',  47}, {'=', 48}, {'<', 49},
        {'>',  50}, {'(', 51}, {')', 52},
        {'[',  61}, {']', 62}, {'{', 63},
        {'}',  64}, {'^', 65}, {'~', 66},
        {'|',  67}, {' ', 68}
    };

    public string FontFilePath;
    public (int[] dimensions, List<byte[]> characterSet) FontData;

    public Font(string fontFilePath)
    {
        //A probably unoptimized method to Convert a font atlas (custom texture atlas) to a list of characters;
        int[] dimensions = new int[3];
        string[] parseDimensions = fontFilePath.Split('\\').Last().Split('_');
        for(var i = 0; i < 3; i++) {
            int.TryParse(parseDimensions[i], out dimensions[i]);
        }

        Bitmap fontAtlas = (Bitmap)Image.FromFile(fontFilePath);
        byte[] fontAtlasBytes = DrawingUtility.GetPixelData(fontAtlas);

        List<byte[]> characterSet = [];

        for (float i = 0; i < Font.CharacterMap.Count-1; i++) {
            byte[] character = new byte[dimensions[0]*dimensions[1]];
            for (var x = 0; x < dimensions[0]; x++) {
                for (var y = 0; y < dimensions[1]; y++) {
                    character[x + y * dimensions[0]]
                        = fontAtlasBytes[
                            ((int)(x + dimensions[0] * (i % 12)) +
                             (int)(y + dimensions[1] * Math.Floor(i / 12) + (Math.Floor(i / 12) * dimensions[2]))
                             * fontAtlas.Width) * 4 + 3];
                }
            }
            characterSet.Add(character);
        }
        
        this.FontFilePath = fontFilePath;
        this.FontData = (dimensions, characterSet);
    }

    //User-friendly method of drawing a single character to the screen;
    //Writes a console warning if it can not find the character in the character map;
    public void DrawCharacter(int x, int y, char c, Color color)
    {
        if (char.IsWhiteSpace(c)) return;

        if (CharacterMap.TryGetValue(c, out int charId))
            DrawCharacter(x, y, charId, color);
        else if (_unrecognizedCharacters.Add(c))
            Console.WriteLine("Warning: \'" + c + "\' is not a recognized character.");
    }

    //Draws a character from a character id to x, y: anchor top left, from font in color.
    private void DrawCharacter(int x, int y, int charId, Color color)
    {
        for (int i = 0; i < FontData.dimensions[0] * FontData.dimensions[1]; i++) 
        {
            byte charColor = (byte)(FontData.characterSet[charId][i] / 255);
            DrawingUtility.DrawPixel(
                i % FontData.dimensions[0] + x, 
                i / FontData.dimensions[0] + y, 
                new(charColor * color.R, charColor * color.G, charColor * color.B, charColor * 255));
        }
    }
    
    //Turns a string into a list of characters gets the character id from each and draws that character at x + (character width + 1) * character position in string, y
    public void DrawString(int x, int y, string str, Color color)
    {
        foreach (char c in str.ToUpper())
        {
            DrawCharacter(x, y, c, color);
            x += FontData.dimensions[0] + 1;
        }
    }

    public void DrawStringCentered(int y, string str, Color color)
        => DrawString(GetStringCenteredPos(str), y, str, color);

    public int GetStringWidth(string str)
        => FontData.dimensions[0] * str.Length + (str.Length - 1);
    
    public int GetStringCenteredPos(string str)
        => Program.ScreenSize / 2 - GetStringWidth(str) / 2;
    
    public int GetStringCenteredPos(string str, (int left, int right) of)
        => of.left + (of.right - of.left - GetStringWidth(str)) / 2;
}