using Solitaire.Cards;
using Solitaire.Game.Enums;
using Solitaire.Renderers;
using Solitaire.UI;

namespace Solitaire.Game
{
    /// <summary>
    /// Main game class that controls the flow and logic of Solitaire.
    /// </summary>
    public class GameHandler
    {
        private readonly SolitaireBoard _board;
        private readonly SolitaireRenderer _renderer;
        private readonly InputHandler _inputHandler;
        private readonly MovesHandler _movesHandler;

        /// <summary>
        /// Initializes the game, including deck creation.
        /// </summary>
        public GameHandler(bool isHardMode)
        {
            // Deck and board creation
            List<Card> deck = Deck.Shuffle(Deck.Create());
            _board = new(deck);

            SelectionInfo selection = new();
            GC.KeepAlive(selection); // Prevents selection from being garbage collected

            _renderer = new(_board, selection);
            _inputHandler = new(_board, selection);
            _movesHandler = new(_board, selection, _renderer, isHardMode);
        }

        /// <summary>
        /// Starts and runs the game loop.
        /// </summary>
        /// <returns>
        /// The number of moves taken if the game is won, or -1 if the game is quit by the user.
        /// </returns>
        public int Run()
        {
            // Match game loop
            while (true)
            {
                // Get player input and update selection
                InputState state = _inputHandler.HandleInput();

                if (state == InputState.None)
                    continue;

                // If user chooses to quit, confirm and exit
                if (state == InputState.QuitGame && UIHandler.ConfirmQuitGame())
                    return -1;

                // Handle other states (SelectionMoved, Click, SelectionCanceled, MoveUndone)
                _movesHandler.Handle(state);

                _renderer.Render();

                if (IsGameWon())
                    return _board.MovesCount;
            }
        }

        private bool IsGameWon()
        {
            foreach (Stack<Card> pile in _board.Foundations)
            {
                // A full foundation pile contains 13 cards
                if (pile.Count != 13)
                    return false;
            }

            // All piles are full, game is won
            return true;
        }
    }
}