using Solitaire.Cards.Enums;

namespace Solitaire.Cards
{
    /// <summary>
    /// Provides functionality to create and shuffle decks of cards.
    /// </summary>
    public static class Deck
    {
        /// <summary>
        /// Creates a standard 52-card deck.
        /// </summary>
        public static List<Card> Create()
        {
            List<Card> deck = [];

            // Adds to list all 52 cards
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                foreach (CardRank rank in Enum.GetValues(typeof(CardRank)))
                    deck.Add(new Card(rank, suit));

            return deck;
        }

        /// <summary>
        /// Shuffles a deck using the Fisher–Yates algorithm.
        /// </summary>
        public static List<Card> Shuffle(List<Card> originalDeck)
        {
            List<Card> shuffledDeck = new(originalDeck);
            Random random = new();

            for (int i = shuffledDeck.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (shuffledDeck[i], shuffledDeck[j]) = (shuffledDeck[j], shuffledDeck[i]); // Switchs card position
            }

            return shuffledDeck;
        }
    }
}
