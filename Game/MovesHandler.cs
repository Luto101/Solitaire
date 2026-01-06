using Solitaire.Cards;
using Solitaire.Game.Enums;
using Solitaire.Renderers;
using Solitaire.Renderers.Enums;

namespace Solitaire.Game
{
    /// <summary>
    /// Handles player actions (selecting, moving, and undoing).
    /// </summary>
    public class MovesHandler(SolitaireBoard _board, SelectionInfo _selection, SolitaireRenderer _renderer, bool isHardMode)
    {
        private readonly BoardsHistoryManager _historyManager = new(_board);
        private readonly CardMover _mover = new(_board);

        // Information to restore selection in selection cancellation case
        private int indexBeforeMove;
        private SlotSelectionType slotBeforeMove;

        private SelectionInfo lastSelection = new();    // Selection before move was done
        private bool wasLayable;                        // Determine if the picked card can be placed

        /// <summary>
        /// Handles the current input state.
        /// </summary>
        public void Handle(InputState state)
        {
            switch (state)
            {
                case InputState.SelectionMoved:
                    HandleSelectionMoved();
                    break;

                case InputState.Click:
                    HandleClick();
                    break;

                case InputState.SelectionCanceled:
                    HandleSelectionCanceled();
                    break;

                case InputState.MoveUndone:
                    HandleMoveUndone();
                    break;
            }

            // If the Talon pile is empty after undoing or moving, change the selection to Stock
            if (_selection.Slot == SlotSelectionType.Talon && _board.TalonPile.Count == 0)
                _selection.Slot = SlotSelectionType.Stock;


            // Save the current selection before next move
            lastSelection = new SelectionInfo(_selection.Slot, _selection.Index, _selection.Count);
        }

        private void HandleSelectionCanceled()
        {
            _renderer.ChangeHighlightState(HighlightState.Cursor);
            _historyManager.RestoreBoard();

            // Back to the previous state of selection
            _selection.IsCardPicked = false;
            _selection.Index = indexBeforeMove;
            _selection.Slot = slotBeforeMove;
        }

        private void HandleMoveUndone()
        {
            // If a card is picked, selection is canceled instead of undoing the move
            if (_selection.IsCardPicked)
            {
                HandleSelectionCanceled();
                return;
            }

            _historyManager.RestoreBoard();

            _selection.Count = 1; // Prevents selecting cards on face-down or empty piles.
        }

        private void HandleSelectionMoved()
        {
            // Selection was changed in input handler
            if (!_selection.IsCardPicked)
                return;

            Card pickedCard = GetPickedCard();
            Card? targetCard = GetTargetCard();

            wasLayable = MovesValidator.IsCardLayable(pickedCard, targetCard);

            // When card is on the source slot
            if (_selection.Slot == slotBeforeMove && _selection.Index == indexBeforeMove)
                wasLayable = true;

            _renderer.ChangeHighlightState(wasLayable ? HighlightState.Good : HighlightState.Wrong);

            _mover.MoveCards(lastSelection.Slot, lastSelection.Index, _selection.Index, _selection.Count);
        }

        private void HandleClick()
        {
            if (_selection.Slot == SlotSelectionType.Stock) // Draw or Shuffle
            {
                _historyManager.AddBoard();
                _historyManager.ConfirmBoard();

                StockManager.DrawOrShuffle(_board, isHardMode);
            }
            else // For other slots (Tableau, Talon, Foundation), try to pick or place a card
                PickOrPlaceCard();
        }

        private void PickOrPlaceCard()
        {
            if (!_selection.IsCardPicked)
            {
                _historyManager.AddBoard();

                Card card = GetPickedCard();

                // Auto-move to foundation if possible
                if (_selection.Count == 1 && _selection.Slot != SlotSelectionType.Foundation &&
                    MovesValidator.CanMoveToFoundation(_board, card))
                {
                    _mover.MoveToFoundation(card, _selection.Slot, _selection.Index);

                    _historyManager.ConfirmBoard();
                    _mover.RevealTopCards();
                    return;
                }

                // Pick the card
                _renderer.ChangeHighlightState(HighlightState.Picked);
                _selection.IsCardPicked = true;

                indexBeforeMove = _selection.Index;
                slotBeforeMove = _selection.Slot;
            }
            else
            {
                // If the player selects the same slot again, cancel the move
                if (_selection.Index == indexBeforeMove && _selection.Slot == slotBeforeMove)
                {
                    HandleSelectionCanceled();
                    return;
                }

                // Finalize the move if it's valid
                if (wasLayable)
                {
                    _renderer.ChangeHighlightState(HighlightState.Cursor);
                    _selection.IsCardPicked = false;

                    _historyManager.ConfirmBoard();
                    _mover.RevealTopCards();
                }
            }
        }

        /// <summary>
        /// Retrieves the card currently selected by the player, based on the last selection.
        /// In Tableau case retrieves the top selected card from the stack.
        /// </summary>
        private Card GetPickedCard()
        {
            if (lastSelection.Slot == SlotSelectionType.Talon)
                return _board.TalonPile.Peek();
            else if (lastSelection.Slot == SlotSelectionType.Foundation)
                return _board.Foundations[lastSelection.Index].Peek();
            else // Tableau
            {
                // Top card in selection stack
                Stack<Card> pile = _board.Tableau[lastSelection.Index];
                return pile.ElementAt(_selection.Count - 1);
            }
        }

        private Card? GetTargetCard()
        {
            if (_selection.Slot == SlotSelectionType.Tableau && _board.Tableau[_selection.Index].Count > 0)
                return _board.Tableau[_selection.Index].Peek(); // Last card in pile

            return null;
        }
    }
}
