#pragma warning disable CA1416
using System.Drawing;
using Color = Raylib_cs.Color;

namespace CouchPlayTest.Drawing.Font;

public static class FontUtility
{
    static HashSet<char> _unrecognizedCharacters = [];
    static readonly Dictionary<char, int> CharacterMap = new Dictionary<char, int>()
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

    
    public static (int[] dimensions, List<byte[]> characterSet) GetCharacterSet(string fontPath)
    {
        int[] dimensions = new int[3];
        string[] parseDimensions = fontPath.Split('\\').Last().Split('_');
        for(var i = 0; i < 3; i++) {
            int.TryParse(parseDimensions[i], out dimensions[i]);
        }

        Bitmap fontAtlas = (Bitmap)Image.FromFile(fontPath);
        byte[] fontAtlasBytes = DrawingUtility.GetPixelData(fontAtlas);

        List<byte[]> characterSet = [];

        for (float i = 0; i < CharacterMap.Count-1; i++) {
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
        
        return (dimensions, characterSet);
    }

    #region Drawing
    public static void DrawCharacter(int x, int y, char c, Font font, Color color)
    {
        if (c == ' ') return;
        if(CharacterMap.TryGetValue(c, out int charId)) { }
        else if(!_unrecognizedCharacters.Contains(c)) {
            Console.WriteLine("Warning: \'" + c + "\' is not a recognized character.");
            _unrecognizedCharacters.Add(c);
        }
        DrawCharacter(x, y, charId, font, color);
    }

    static void DrawCharacter(int x, int y, int c, Font font, Color color)
    {
        for (int i = 0; i < font.FontData.dimensions[0] * font.FontData.dimensions[1]; i++) {
            byte charColor = (byte)(font.FontData.characterSet[c][i] / 255);
            DrawingUtility.DrawPixel(
                i%font.FontData.dimensions[0] + x, 
                i/font.FontData.dimensions[0] + y, 
                new(charColor * color.R, charColor * color.G, charColor * color.B, color.A));
        }
    }
    
    
    public static void DrawString(int x, int y, string str, Font font, Color color)
    {
        ushort index = 0;
        foreach (char c in str.ToUpper()) {
            if (c == ' ' || _unrecognizedCharacters.Contains(c)) {index++; continue;}
            if(CharacterMap.TryGetValue(c, out int charId)) { }
            else if(!_unrecognizedCharacters.Contains(c)) {
                Console.WriteLine("Warning: \'" + c + "\' is not a recognized character.");
                _unrecognizedCharacters.Add(c);
            }
            DrawCharacter(x + (font.FontData.dimensions[0] + 1) * index++, y, charId, font, color);
        }
    }
    #endregion

    public static int GetStringWidth(string str, Font font)
    {
        return  font.FontData.dimensions[0] * str.Length + (str.Length - 1);
    }
    
    public static int GetStringCenteredPos(string str, Font font)
    {
        return Program.ScreenSize / 2 - GetStringWidth(str, font) / 2;
    }
    public static int GetStringCenteredPos(string str, Font font, (int left, int right) of)
    {
        return of.left + ((of.right - of.left) - GetStringWidth(str, font)) / 2;
    }

}
