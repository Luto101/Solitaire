using Solitaire.Cards;
using Solitaire.Cards.Enums;

namespace Solitaire.Game
{
    /// <summary>
    /// Provides methods to check if the move is possible.
    /// </summary>
    public static class MovesValidator
    {
        /// <summary>
        /// Determines if the card can be placed.
        /// </summary>
        public static bool IsCardLayable(Card pickedCard, Card? targetTop)
        {
            // If a pile in Tableau is empty 
            if (targetTop == null)
                return pickedCard.Rank == CardRank.King; // If the picked card is king

            bool descending = (int)targetTop.Rank - 1 == (int)pickedCard.Rank; // Cards in correct order
            bool differentColor = targetTop.IsRed() != pickedCard.IsRed();

            return targetTop.IsFaceUp && differentColor && descending;
        }

        /// <summary>
        /// Determines if the card can be moved to Foundations.
        /// </summary>
        public static bool CanMoveToFoundation(SolitaireBoard board, Card card)
        {
            int suitIndex = (int)card.Suit - 1;
            return board.Foundations[suitIndex].Count + 1 == (int)card.Rank;
        }
    }
}
