using Solitaire.Cards;
using Solitaire.Renderers.Enums;

namespace Solitaire.Game
{
    /// <summary>
    /// Handles moving cards between different piles in the Solitaire game.
    /// </summary>
    public class CardMover(SolitaireBoard _board)
    {
        /// <summary>
        /// Moves a single card to the corresponding foundation pile based on its suit.
        /// </summary>
        public void MoveToFoundation(Card card, SlotSelectionType sourceSlot, int sourceIndex)
        {
            int suitIndex = (int)card.Suit - 1;
            _board.Foundations[suitIndex].Push(card); // Add the card to its appropriate foundation pile

            // Remove the card from its original pile
            if (sourceSlot == SlotSelectionType.Tableau)
                _board.Tableau[sourceIndex].Pop();
            else
                _board.TalonPile.Pop();
        }

        /// <summary>
        /// Moves one or more cards from a source pile (Talon, Foundation, or Tableau) to a target Tableau pile.
        /// </summary>
        public void MoveCards(SlotSelectionType sourceSlot, int sourceIndex, int targetIndex, int count)
        {
            Stack<Card> buffer = new();

            for (int i = 0; i < count; i++)
            {
                Card card;

                if (sourceSlot == SlotSelectionType.Talon)
                    card = _board.TalonPile.Pop();
                else if (sourceSlot == SlotSelectionType.Foundation)
                    card = _board.Foundations[sourceIndex].Pop();
                else // Tableau
                    card = _board.Tableau[sourceIndex].Pop();

                buffer.Push(card);
            }

            // Move all cards from buffer to the destination tableau pile
            while (buffer.Count > 0)
                _board.Tableau[targetIndex].Push(buffer.Pop());
        }

        /// <summary>
        /// Reveals the top card in all Tableau piles.
        /// </summary>
        public void RevealTopCards()
        {
            foreach (var pile in _board.Tableau)
                if (pile.Count > 0)
                    pile.Peek().IsFaceUp = true; // Last card always face-up
        }
    }
}
