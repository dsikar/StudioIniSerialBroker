namespace ConsoleApplication8
{
  static class Constants
  {
    // Dimensions
    public static readonly int ConsoleWidth = 100; // 150; // 150
    public static readonly int ConsoleHeight = 50; // 

    // Y Markers
    public static readonly int ConsoleMidY = 24;
    public static readonly int ConsoleMaxY = 40;

    // X Markers
    public static readonly int ConsoleMidX = 70;
     
    // Horizontal and vertical lines
    public static readonly string HorizontalLine = "----------------------------------------------------------------------------------------------------------------------------------------------";
    // Number of white spaces = ConsoleMidX - 1
    public static readonly string VerticalLine = " |\n";

    public static bool bPaused = true;
    public static bool bColour = false;
  }
}
