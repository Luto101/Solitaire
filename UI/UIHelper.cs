namespace Solitaire.UI
{
    /// <summary>
    /// Provides helper methods for rendering text in the console UI.
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// Writes lines at specific possition and with a given text color.
        /// </summary>
        public static void WriteLinesAt(string[] lines, int x, int y, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);

            foreach (string line in lines)
            {
                Console.Write(line);
                Console.SetCursorPosition(x, Console.GetCursorPosition().Top + 1); // Next row
            }

            Console.ResetColor();
        }
    }
}
