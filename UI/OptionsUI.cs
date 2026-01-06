namespace Solitaire.UI
{
    /// <summary>
    /// Provides functionality to display and handle user selection.
    /// </summary>
    public static class OptionsUI
    {
        /// <summary>
        /// Displays a list of options.
        /// </summary>
        /// <returns>index (1-based) of the selected option.</returns>
        public static int GetChoice(string[] options, int xOffset = 0, int yOffset = 1)
        {
            int selectedOption = 1;
            ConsoleColor[] optionColors = new ConsoleColor[options.Length];

            while (true)
            {
                // Reset all option colors to white
                for (int i = 0; i < options.Length; i++)
                    optionColors[i] = ConsoleColor.White;

                // Highlight the selected option
                optionColors[selectedOption - 1] = ConsoleColor.Green;

                // Draw all options on screen
                for (int i = 0; i < options.Length; i++)
                {
                    string[] optionBox = CreateOption(options[i], 10, 7);
                    int x = i * 12 + xOffset; // Horizontal spacing between options
                    UIHelper.WriteLinesAt(optionBox, x, yOffset, optionColors[i]);
                }

                // Handle user input
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.RightArrow or ConsoleKey.D:
                        selectedOption++;
                        if (selectedOption > options.Length)
                            selectedOption = 1;
                        break;
                    case ConsoleKey.LeftArrow or ConsoleKey.A:
                        selectedOption--;
                        if (selectedOption < 1)
                            selectedOption = options.Length;
                        break;
                    case ConsoleKey.Enter or ConsoleKey.Spacebar:
                        return selectedOption;
                }
            }
        }

        /// <summary>
        /// Creates a framed string array representing the option.
        /// </summary>
        private static string[] CreateOption(string optionText, int width, int height)
        {
            // Ensure the box is wide enough to fit the text
            if (width - 2 < optionText.Length)
                width = optionText.Length + 2;

            string[] lines = new string[height];

            for (int i = 0; i < height; i++)
            {
                lines[i] = "";

                for (int j = 0; j < width; j++)
                {
                    char ch;

                    // Box corners
                    if (i == 0 && j == 0)
                        ch = '┌';
                    else if (i == 0 && j == width - 1)
                        ch = '┐';
                    else if (i == height - 1 && j == 0)
                        ch = '└';
                    else if (i == height - 1 && j == width - 1)
                        ch = '┘';
                    // Box borders
                    else if (i == 0 || i == height - 1)
                        ch = '─';
                    else if (j == 0 || j == width - 1)
                        ch = '│';
                    else
                        ch = ' ';

                    lines[i] += ch;
                }
            }

            // Center the option text inside the box
            int textRow = height / 2;
            int textStart = (width - optionText.Length) / 2;

            lines[textRow] = lines[textRow].Remove(textStart, optionText.Length);
            lines[textRow] = lines[textRow].Insert(textStart, optionText);

            return lines;
        }
    }
}
