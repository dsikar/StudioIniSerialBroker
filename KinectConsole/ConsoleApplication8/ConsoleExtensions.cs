using System;

namespace ConsoleExtensions
{
  static class ConsoleEx
  {
    public static void DrawAt(int x, int y, string text, ConsoleColor color)
    {
      Console.CursorLeft = x;
      Console.CursorTop = y;
      Console.ForegroundColor = color;
      Console.Write(text);
    }
    public static void Clear(Tuple<int,int> drawCoords)
    {
      DrawAt(drawCoords.Item1, drawCoords.Item2, " ", ConsoleColor.Black);
    }
  }
}
