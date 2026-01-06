using Solitaire.Cards;

namespace Solitaire.Game
{
    /// <summary>
    /// Represents the Solitaire game board.
    /// </summary>
    public class SolitaireBoard
    {
        public List<Stack<Card>> Tableau { get; set; }        // 7 piles
        public Stack<Card> StockPile { get; set; }            // 1 pile
        public Stack<Card> TalonPile { get; set; }            // 1 pile
        public List<Stack<Card>> Foundations { get; set; }    // 4 piles
        public int MovesCount { get; set; }

        /// <summary>
        /// Initializes a new Solitaire board with the given deck of cards.
        /// </summary>
        public SolitaireBoard(List<Card> deck)
        {
            Tableau = [new(), new(), new(), new(), new(), new(), new()];
            Foundations = [new(), new(), new(), new()];
            StockPile = new Stack<Card>();
            TalonPile = new Stack<Card>();
            MovesCount = 0;

            InitializeBoard(deck);
        }

        /// <summary>
        /// Creates a new instance of the Solitaire board by copying an existing board.
        /// </summary>
        public SolitaireBoard(SolitaireBoard oldBoard)
        {
            Tableau = [];
            foreach (var pile in oldBoard.Tableau)
                Tableau.Add(CloneStack(pile));

            Foundations = [];
            foreach (var pile in oldBoard.Foundations)
                Foundations.Add(CloneStack(pile));

            StockPile = CloneStack(oldBoard.StockPile);
            TalonPile = CloneStack(oldBoard.TalonPile);
            MovesCount = oldBoard.MovesCount;
        }

        private void InitializeBoard(List<Card> deck)
        {
            int cardIndex = 0;

            // Generates 7 piles. In first pile 1 card, in second 2 cards, etc...
            for (int i = 0; i < 7; i++)
            {
                Stack<Card> pile = new();

                for (int j = 0; j <= i; j++)
                {
                    // Flips last card in pile
                    if (i == j)
                        deck[cardIndex].IsFaceUp = true;

                    pile.Push(deck[cardIndex]);
                    cardIndex++;
                }

                Tableau[i] = pile;
            }

            // Rest cards goes to draw pile
            for (int i = cardIndex; i < deck.Count; i++)
                StockPile.Push(deck[i]);
        }

        /// <summary>
        /// Creates a clone of the provided stack of cards.
        /// </summary>
        private static Stack<Card> CloneStack(Stack<Card> original)
        {
            var cards = original.Reverse().Select(card => new Card(card.Rank, card.Suit, card.IsFaceUp)).ToList();

            return new Stack<Card>(cards);
        }

    }
}
