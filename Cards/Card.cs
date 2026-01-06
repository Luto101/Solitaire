using Solitaire.Cards.Enums;

namespace Solitaire.Cards
{
    /// <summary>
    /// Represents a single playing card.
    /// </summary>
    public class Card(CardRank rank, CardSuit suit, bool isFaceUp = false)
    {
        public CardRank Rank { get; private set; } = rank;
        public CardSuit Suit { get; private set; } = suit;
        public bool IsFaceUp { get; set; } = isFaceUp;

        public bool IsRed() => (int)Suit % 2 == 0;
    }
}
