namespace Solitaire.Scores
{
    /// <summary>
    /// Represents a single game score, including start and end time, the number of moves made and difficulty.
    /// </summary>
    public class GameScore(DateTime startDateTime, DateTime endDateTime, int moves, bool isHardMode)
    {
        public DateTime StartDateTime { get; set; } = startDateTime;
        public DateTime EndDateTime { get; set; } = endDateTime;
        public bool IsHardMode { get; set; } = isHardMode;
        public int Moves { get; set; } = moves;
    }
}
