using Solitaire.Builders.Enums;
using Solitaire.Cards;
using Solitaire.Game;
using Solitaire.Renderers.Enums;

namespace Solitaire.Renderers
{
    /// <summary>
    /// Responsible for rendering the game board on the console.
    /// </summary>
    public class SolitaireRenderer
    {
        private ConsoleColor highlightColor;

        private readonly SolitaireBoard _board;
        private readonly SelectionInfo _selection;

        /// <summary>
        /// Initializes the renderer with the specified board and selection information.
        /// </summary>
        public SolitaireRenderer(SolitaireBoard board, SelectionInfo selection)
        {
            _board = board;
            _selection = selection;
            highlightColor = (ConsoleColor)HighlightState.Cursor;

            // Initial board render
            Render();
        }

        /// <summary>
        /// Changes the highlight color to indicate a different selection state.
        /// </summary>
        public void ChangeHighlightState(HighlightState state) => highlightColor = (ConsoleColor)state;

        /// <summary>
        /// Renders the entire game board, including all slots and cards.
        /// </summary>
        public void Render()
        {
            Console.Clear();

            CheckBuffer();

            DrawStockPile();
            DrawTalonPile();
            DrawFoundations();
            DrawTableau();
            DrawMoveCount();
        }
        /// <summary>
        /// Ensures that the console buffer width is large enough for the game.
        /// </summary>
        private static void CheckBuffer()
        {
            while (Console.BufferWidth < 120 || Console.BufferHeight < 40)
            {
                Console.WriteLine("Increase the window size!!!");

                Thread.Sleep(1000);

                Console.Clear();
            }
        }

        private void DrawStockPile()
        {
            ConsoleColor? highlightColor = GetHighlightColor(SlotSelectionType.Stock);

            // If Stock pile is selected
            if (_board.StockPile.Count == 0 && _board.TalonPile.Count == 0) // No cards left in either pile
                CardSlotRenderer.DrawSlot(SlotType.Empty, 0, 0, highlightColor);
            else if (_board.StockPile.Count == 0 && _board.TalonPile.Count != 0) // Stock is empty, but Talon is not: show shuffle slot
                CardSlotRenderer.DrawSlot(SlotType.Shuffle, 0, 0, highlightColor);
            else
                CardSlotRenderer.DrawCard(_board.StockPile.Peek(), 0, 0, highlightColor); // Draw top card of the Stock pile
        }

        /// <summary>
        /// Draws the top 3 most recent cards from the Talon pile.
        /// </summary>
        private void DrawTalonPile()
        {
            Stack<Card> recentCards = new(_board.TalonPile.Take(3)); // Up to 3 most recent cards
            int x = 15;
            int i = 0;

            foreach (Card card in recentCards)
            {
                // If Talon pile is selected, change color of last card in pile
                ConsoleColor? highlightColor = GetHighlightColor(SlotSelectionType.Talon, -1, i, recentCards.Count);
                CoverStatus coverStatus = CoverStatus.None;

                // Determine whether card is covered
                if (i != recentCards.Count - 1)
                    coverStatus = CoverStatus.Vertically;

                CardSlotRenderer.DrawCard(card, x, 0, highlightColor, coverStatus);
                x += 4; // Offset for overlapping appearance
                i++;
            }
        }

        private void DrawFoundations()
        {
            int x = 45;
            int i = 0;

            foreach (Stack<Card> cards in _board.Foundations)
            {
                ConsoleColor? highlightColor = GetHighlightColor(SlotSelectionType.Foundation, i);

                if (cards.Count == 0)
                    CardSlotRenderer.DrawSlot(SlotType.Empty, x, 0, highlightColor);
                else
                    CardSlotRenderer.DrawCard(cards.Peek(), x, 0, highlightColor);

                x += 15; // Offset for next Foundation slot
                i++;
            }
        }

        private void DrawTableau()
        {
            List<Stack<Card>> Tableau = _board.Tableau;
            int x = 0;

            for (int i = 0; i < Tableau.Count; i++)
            {
                Stack<Card> cards = new(Tableau[i]);
                int columnHeight = cards.Count;
                int y = 10;

                // Empty slot
                if (columnHeight == 0)
                {
                    ConsoleColor? highlightColor = GetHighlightColor(SlotSelectionType.Tableau, i, 0, columnHeight);
                    CardSlotRenderer.DrawSlot(SlotType.Empty, x, y, highlightColor);
                }

                for (int j = 0; j < columnHeight; j++)
                {
                    ConsoleColor? highlightColor = GetHighlightColor(SlotSelectionType.Tableau, i, j, columnHeight);
                    CoverStatus coverStatus = CoverStatus.None;

                    // Determine whether card is covered
                    if (j != columnHeight - 1)
                        coverStatus = CoverStatus.Horizontally;

                    // Highlight cards that are part of the current Tableau selection
                    CardSlotRenderer.DrawCard(cards.Pop(), x, y, highlightColor, coverStatus);
                    y += 2; // Move down for next card
                }
                x += 15; // Move right for next Tableau column
            }
        }

        private void DrawMoveCount()
        {
            Console.SetCursorPosition(105, 1);
            Console.WriteLine("Moves: " + _board.MovesCount);
        }

        /// <summary>
        /// Determines the highlight color, based on the current selection.
        /// </summary>
        private ConsoleColor? GetHighlightColor(SlotSelectionType slot, int column = -1, int cardIndex = -1, int totalCards = -1)
        {
            // Default color for card slot
            if (_selection.Slot != slot)
                return null;

            // Check conditions for each slot type
            switch (slot)
            {
                case SlotSelectionType.Stock:
                    return highlightColor;

                case SlotSelectionType.Talon:
                    if (cardIndex == totalCards - 1)
                        return highlightColor;
                    break;

                case SlotSelectionType.Tableau:
                    if (column == _selection.Index && cardIndex >= totalCards - _selection.Count)
                        return highlightColor;
                    break;

                case SlotSelectionType.Foundation:
                    if (column == _selection.Index)
                        return highlightColor;
                    break;
            }

            return null;
        }
    }
}
