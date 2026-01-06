using Solitaire.Cards;

namespace Solitaire.Game
{
    /// <summary>
    /// Manages the game board history.
    /// </summary>
    public class BoardsHistoryManager(SolitaireBoard _mainBoard)
    {
        private readonly List<SolitaireBoard> lastBoards = []; // History of boards for undo

        /// <summary>
        /// Adds a new board to history. Should be called before player action. 
        /// </summary>
        public void AddBoard()
        {
            lastBoards.Add(new SolitaireBoard(_mainBoard));

            _mainBoard.MovesCount++;
        }

        /// <summary>
        /// Confirms the current move by limiting the history to 3 states
        /// </summary>
        public void ConfirmBoard()
        {
            if (lastBoards.Count > 3)
                lastBoards.RemoveAt(0); // Keep only last 3 states
        }

        /// <summary>
        /// Restores the board to the most recent previous state from the history.
        /// </summary>
        public void RestoreBoard()
        {
            if (lastBoards.Count == 0)
                return;

            // The most recent board
            SolitaireBoard board = lastBoards.Last();

            _mainBoard.Tableau = new List<Stack<Card>>(board.Tableau);
            _mainBoard.Foundations = new List<Stack<Card>>(board.Foundations);
            _mainBoard.StockPile = new Stack<Card>(board.StockPile.Reverse());
            _mainBoard.TalonPile = new Stack<Card>(board.TalonPile.Reverse());
            _mainBoard.MovesCount = board.MovesCount;

            lastBoards.Remove(board);
        }
    }
}
