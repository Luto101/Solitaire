using Solitaire.Cards;

namespace Solitaire.Game
{
    /// <summary>
    /// Provides methods to manage the Stock pile.
    /// </summary>
    public static class StockManager
    {
        /// <summary>
        /// Draws or Shuffles cards from Stock pile.
        /// </summary>
        public static void DrawOrShuffle(SolitaireBoard board, bool isHardMode)
        {
            int cardsCount = 1;

            // In hard mode draw 3 cards
            if (isHardMode)
                cardsCount = 3;

            // When Stock pile hasn't enough cards
            if (cardsCount > board.StockPile.Count)
                cardsCount = board.StockPile.Count;

            if (cardsCount > 0) // Draw
            {
                for (int i = 0; i < cardsCount; i++)
                {
                    // Move from Stock to Talon pile
                    Card card = board.StockPile.Pop();
                    card.IsFaceUp = true;
                    board.TalonPile.Push(card);
                }
            }
            else // Shuffle
            {
                foreach (Card card in board.TalonPile)
                    card.IsFaceUp = false;

                List<Card> cardsFromTalon = Deck.Shuffle(board.TalonPile.ToList());

                board.StockPile = new Stack<Card>(cardsFromTalon);
                board.TalonPile.Clear();
            }
        }
    }

}
