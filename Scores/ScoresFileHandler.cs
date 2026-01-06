using System.Text.Json;

namespace Solitaire.Scores
{
    /// <summary>
    /// Handles reading and writing game scores to a JSON file in the user's AppData directory.
    /// </summary>
    public static class ScoresFileHandler
    {
        private static readonly string FilePath;

        // Sets up the file path and ensures the scores file exists
        static ScoresFileHandler()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string solitaireDirectory = Path.Combine(appDataPath, "Solitaire");

            if (!Directory.Exists(solitaireDirectory))
                Directory.CreateDirectory(solitaireDirectory);

            FilePath = Path.Combine(solitaireDirectory, "scores.json");

            if (!File.Exists(FilePath))
                File.WriteAllText(FilePath, "[]"); // Empty JSON
        }

        /// <summary>
        /// Saves a new score to the file.
        /// </summary>
        public static void SaveScore(GameScore scoreInfo)
        {
            // Get scores that already exist
            List<GameScore> scores = GetScores();

            scores.Add(scoreInfo);

            File.WriteAllText(FilePath, JsonSerializer.Serialize(scores));
        }

        /// <summary>
        /// Retrieves all saved game scores.
        /// </summary>
        /// <returns>A list of <see cref="GameScore"/> representing previous game scores.</returns>
        public static List<GameScore> GetScores()
        {
            List<GameScore>? scores;

            try
            {
                string json = File.ReadAllText(FilePath);

                scores = JsonSerializer.Deserialize<List<GameScore>>(json);
            }
            catch // Bad JSON file
            {
                File.WriteAllText(FilePath, "[]"); // Create a new empty JSON file
                return [];
            }

            // If scores list is null then create a empty list
            scores ??= [];

            return scores;
        }
    }
}
