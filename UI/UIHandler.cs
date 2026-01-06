using Solitaire.Scores;

namespace Solitaire.UI
{
    /// <summary>
    /// Provides methods to handle UI in console.
    /// </summary>
    public static class UIHandler
    {
        /// <summary>
        /// Displays the main menu.
        /// </summary>
        /// <returns>
        /// true if player chooses Play,
        /// false if player chooses Exit.
        /// </returns>
        public static bool DisplayMainMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("  _________      .__  .__  __         .__                ");
                Console.WriteLine(" /   _____/ ____ |  | |__|/  |______  |__|______   ____  ");
                Console.WriteLine(" \\_____  \\ /  _ \\|  | |  \\   __\\__  \\ |  \\_  __ \\_/ __ \\ ");
                Console.WriteLine(" /        (  <_> )  |_|  ||  |  / __ \\|  ||  | \\/\\  ___/ ");
                Console.WriteLine("/_______  /\\____/|____/__||__| (____  /__||__|    \\___  >");
                Console.WriteLine("        \\/                          \\/                \\/ ");

                int choice = OptionsUI.GetChoice(["Play", "Controls", "Scores", "Exit"], 5, 7);

                Console.Clear();

                switch (choice)
                {
                    case 1:
                        InformAboutFullScreen();
                        return true;
                    case 2:
                        DisplayControls();
                        break;
                    case 3:
                        DisplayBestScores();
                        break;
                    case 4:
                        return false;
                }
            }
        }

        /// <summary>
        /// Asks the player to choose the difficulty level.
        /// </summary>
        /// <returns>
        /// true for Hard,
        /// false for Easy,
        /// null for back to main menu.
        /// </returns>
        public static bool? PromptDifficultySelection()
        {
            Console.WriteLine("         Choose difficulty:");
            int choice = OptionsUI.GetChoice(["Easy", "Hard", "Back"]);

            if (choice == 1)
                return false; // Easy
            else if (choice == 2)
                return true; // Hard
            else
                return null;

        }

        /// <summary>
        /// Displays victory banner and bests scores.
        /// </summary>
        public static void DisplayVictoryScreen()
        {
            Console.Clear();

            Console.WriteLine(" __      ___      _                   ");
            Console.WriteLine(" \\ \\    / (_)    | |                  ");
            Console.WriteLine("  \\ \\  / / _  ___| |_ ___  _ __ _   _ ");
            Console.WriteLine("   \\ \\/ / | |/ __| __/ _ \\| '__| | | |");
            Console.WriteLine("    \\  /  | | (__| || (_) | |  | |_| |");
            Console.WriteLine("     \\/   |_|\\___|\\__\\___/|_|   \\__, |");
            Console.WriteLine("                                 __/ |");
            Console.WriteLine("                                |___/ ");
            Console.WriteLine();

            DisplayBestScores();
        }

        /// <summary>
        /// Asks the player if they want to quit the game.
        /// </summary>
        public static bool ConfirmQuitGame()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Are you sure to quit a game?");
                int choose = OptionsUI.GetChoice(["Yes", "No"], 3);

                if (choose == 1)
                    return true;
                else
                    return false;
            }
        }

        private static void DisplayBestScores()
        {
            // List ordered by moves
            List<GameScore> allScores = ScoresFileHandler.GetScores().OrderBy(x => x.Moves).ToList();

            if (allScores.Count == 0)
                Console.WriteLine("No scores available");
            else
            {
                // Latest score
                GameScore latestScore = allScores.OrderBy(x => x.StartDateTime).Last();

                Console.WriteLine("The best scores: ");

                for (int i = 0; i < allScores.Count; i++)
                {
                    GameScore score = allScores[i];

                    // Latest score is in a green color
                    if (score == latestScore)
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    else
                        Console.ResetColor();

                    // Set the difficulty to display
                    string difficultyString;
                    if (score.IsHardMode)
                        difficultyString = "Hard";
                    else
                        difficultyString = "Easy";

                    TimeSpan time = score.EndDateTime - score.StartDateTime;
                    Console.WriteLine($"{i + 1}. {score.StartDateTime} - time: {time.ToString("hh':'mm':'ss")}, difficulty: {difficultyString}, moves: {score.Moves}");
                }

                Console.ResetColor();
            }

            // Offset to return button
            int offset = Console.GetCursorPosition().Top;

            OptionsUI.GetChoice(["Return"], 30, offset + 1);
        }

        private static void DisplayControls()
        {
            Console.Clear();

            Console.WriteLine("← / A            Move left between piles");
            Console.WriteLine("→ / D            Move right between piles");
            Console.WriteLine("↑ / W            Move up within a Tableau column");
            Console.WriteLine("↓ / S            Move down within a Tableau column");
            Console.WriteLine("1–7              Jump directly to Tableau column 1–7");
            Console.WriteLine("Enter / Space    Select or place a card");
            Console.WriteLine("Tab              Focus on Stock");
            Console.WriteLine("Esc              Cancel current move");
            Console.WriteLine("Ctrl + Z         Undo last move");
            Console.WriteLine("Q                Quit a game");

            OptionsUI.GetChoice(["Return"], 16, 11);
        }

        private static void InformAboutFullScreen()
        {
            Console.Clear();

            Console.WriteLine("Ensure that the console in in a full screen mode (press F11)");
            OptionsUI.GetChoice(["OK"], 24);

            Console.Clear();
        }
    }
}