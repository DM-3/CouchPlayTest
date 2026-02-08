namespace CouchPlayTest.Drawing.Font;

public struct Font
{
    public string fontFilePath;
    public (int[] dimensions, List<byte[]> characterSet) fontData;

    public Font(string fontFilePath)
    {
        this.fontFilePath = fontFilePath;
        fontData = FontUtility.GetCharacterSet(fontFilePath);
    }
}