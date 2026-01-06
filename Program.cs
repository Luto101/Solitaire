using Solitaire.Game;
using Solitaire.Scores;
using Solitaire.UI;
using System.Text;

// Ensure UTF-8 characters and hide the cursor for better visual presentation
Console.OutputEncoding = Encoding.UTF8;
Console.CursorVisible = false;

DateTime gameStart;
int movesCount;

// Main game loop
while (true)
{
    // Show the main menu. If the player chooses "Exit", terminate the app
    if (!UIHandler.DisplayMainMenu())
        return;

    bool? isHardMode = UIHandler.PromptDifficultySelection();

    // Back to main menu
    if (isHardMode == null)
        continue;

    gameStart = DateTime.Now;

    // Initialize and run the game
    GameHandler game = new(isHardMode.Value);
    movesCount = game.Run(); // -1 indicates the game was quit

    // If player successfully completed the game
    if (movesCount > 0)
    {
        ScoresFileHandler.SaveScore(new(gameStart, DateTime.Now, movesCount, isHardMode.Value));  // Save the score
        UIHandler.DisplayVictoryScreen(); // Show the victory screen and best scores
    }
}





