using Solitaire.Cards;
using Solitaire.Game.Enums;
using Solitaire.Renderers;
using Solitaire.Renderers.Enums;

namespace Solitaire.Game
{
    /// <summary>
    /// Handles user input for interacting with the Solitaire game.
    /// </summary>
    public class InputHandler(SolitaireBoard _board, SelectionInfo _selection)
    {
        /// <summary>
        /// Processes the user input and updates the selection.
        /// </summary>
        /// <returns><see cref="InputState"/> to indicate the result of the action.</returns>
        public InputState HandleInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            ConsoleKey key = keyInfo.Key;
            bool ctrl = keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control);

            switch (key)
            {
                case ConsoleKey.LeftArrow or ConsoleKey.A:
                    return HandleLeftArrow();

                case ConsoleKey.RightArrow or ConsoleKey.D:
                    return HandleRightArrow();

                case ConsoleKey.UpArrow or ConsoleKey.W:
                    return HandleUpArrow();

                case ConsoleKey.DownArrow or ConsoleKey.S:
                    return HandleDownArrow();

                case ConsoleKey.D1 or ConsoleKey.NumPad1:
                    return HandleNumbers(1);

                case ConsoleKey.D2 or ConsoleKey.NumPad2:
                    return HandleNumbers(2);

                case ConsoleKey.D3 or ConsoleKey.NumPad3:
                    return HandleNumbers(3);

                case ConsoleKey.D4 or ConsoleKey.NumPad4:
                    return HandleNumbers(4);

                case ConsoleKey.D5 or ConsoleKey.NumPad5:
                    return HandleNumbers(5);

                case ConsoleKey.D6 or ConsoleKey.NumPad6:
                    return HandleNumbers(6);

                case ConsoleKey.D7 or ConsoleKey.NumPad7:
                    return HandleNumbers(7);

                case ConsoleKey.Enter or ConsoleKey.Spacebar:
                    return HandleEnter();

                case ConsoleKey.Q:
                    return InputState.QuitGame;

                case ConsoleKey.Escape:
                    if (_selection.IsCardPicked)
                        return InputState.SelectionCanceled;
                    else
                        return InputState.None;

                case ConsoleKey.Z when ctrl:
                    return InputState.MoveUndone;

                case ConsoleKey.Tab:
                    // If no card is picked, cycle focus to the Stock
                    if (!_selection.IsCardPicked)
                    {
                        _selection.Count = 1;
                        _selection.Slot = SlotSelectionType.Stock;
                        return InputState.SelectionMoved;
                    }
                    return InputState.None;

                default:
                    return InputState.None;
            }
        }

        private InputState HandleLeftArrow()
        {
            switch (_selection.Slot)
            {
                case SlotSelectionType.Tableau:
                    // Move Tableau
                    if (_selection.Index > 0)
                        _selection.Index--;
                    else
                        _selection.Index = 6;

                    if (!_selection.IsCardPicked)
                        _selection.Count = 1; // Reset card count when changing columns
                    break;

                case SlotSelectionType.Talon:
                    // From Talon to Stock
                    if (!_selection.IsCardPicked)
                        _selection.Slot = SlotSelectionType.Stock;
                    else
                        return InputState.None;
                    break;

                case SlotSelectionType.Foundation:
                    if (!_selection.IsCardPicked)
                    {
                        // Move left in Foundation if not at first pile
                        if (_selection.Index > 0)
                        {
                            _selection.Index--;
                        }
                        // From Foundation to Talon if at first pile and Talon has cards
                        else if (_selection.Index == 0 && _board.TalonPile.Count > 0)
                        {
                            _selection.Slot = SlotSelectionType.Talon;
                        }
                        // From Foundation to Stock if at first pile and Talon is empty
                        else if (_selection.Index == 0 && _board.TalonPile.Count == 0)
                        {
                            _selection.Slot = SlotSelectionType.Stock;
                        }
                        else
                            return InputState.None;
                    }
                    else
                        return InputState.None;
                    break;

                case SlotSelectionType.Stock:
                    _selection.Slot = SlotSelectionType.Foundation;
                    _selection.Index = 3;
                    break;
            }

            return InputState.SelectionMoved;
        }

        private InputState HandleRightArrow()
        {
            switch (_selection.Slot)
            {
                case SlotSelectionType.Tableau:
                    // Move in Tableau
                    if (_selection.Index < 6)
                        _selection.Index++;
                    else
                        _selection.Index = 0;

                    if (!_selection.IsCardPicked)
                        _selection.Count = 1;
                    break;

                case SlotSelectionType.Stock:
                    // From Stock to Talon if Talon has cards
                    if (_board.TalonPile.Count > 0)
                    {
                        _selection.Slot = SlotSelectionType.Talon;
                    }
                    // From Stock to Foundation if Talon is empty
                    else
                    {
                        _selection.Slot = SlotSelectionType.Foundation;
                        _selection.Index = 0;
                    }
                    break;

                case SlotSelectionType.Talon:
                    // From Talon to Foundation
                    if (!_selection.IsCardPicked)
                    {
                        _selection.Slot = SlotSelectionType.Foundation;
                        _selection.Index = 0;
                    }
                    else
                        return InputState.None;
                    break;

                case SlotSelectionType.Foundation:
                    if (!_selection.IsCardPicked)
                    {
                        // Move right in Foundation or back to Stock
                        if (_selection.Index < 3)
                            _selection.Index++;
                        else
                            _selection.Slot = SlotSelectionType.Stock;
                    }
                    else
                        return InputState.None;
                    break;

                default:
                    return InputState.None;
            }

            return InputState.SelectionMoved;
        }

        private InputState HandleUpArrow()
        {
            switch (_selection.Slot)
            {
                case SlotSelectionType.Tableau:
                    List<Card> columnCards = new(_board.Tableau[_selection.Index]);

                    // Move to the next face-up card in the stack
                    if (columnCards.Count > _selection.Count && columnCards[_selection.Count].IsFaceUp && !_selection.IsCardPicked)
                    {
                        _selection.Count++;
                    }
                    // If card is not picked, jump to the Stock
                    else if (!_selection.IsCardPicked)
                    {
                        _selection.Slot = SlotSelectionType.Stock;
                        _selection.Count = 1;
                    }
                    else
                    {
                        return InputState.None;
                    }
                    break;

                default:
                    return InputState.None;
            }

            return InputState.SelectionMoved;
        }

        private InputState HandleDownArrow()
        {
            // Move from upper row (Stock, Talon, Foundation) to Tableau
            if (_selection.Slot != SlotSelectionType.Tableau)
            {
                _selection.Slot = SlotSelectionType.Tableau;
                _selection.Index = 0;
                _selection.Count = 1;
            }
            // Unselect picked card
            else if (_selection.Slot == SlotSelectionType.Tableau && _selection.Count > 1 && !_selection.IsCardPicked)
            {
                _selection.Count--;
            }
            else
            {
                return InputState.None;
            }

            return InputState.SelectionMoved;
        }

        private InputState HandleEnter()
        {
            switch (_selection.Slot)
            {
                case SlotSelectionType.Stock: // Clicking Stock if it has cards (or Talon has leftovers)
                    if (_board.StockPile.Count > 0 || _board.TalonPile.Count > 0)
                        return InputState.Click;
                    break;

                case SlotSelectionType.Talon:
                    return InputState.Click;

                case SlotSelectionType.Foundation: // Clicking Foundation pile if it’s not empty
                    if (_board.Foundations[_selection.Index].Count != 0)
                        return InputState.Click;
                    break;

                case SlotSelectionType.Tableau: // Clicking Tableau pile if valid
                    Stack<Card> pile = _board.Tableau[_selection.Index];

                    // Ensure there are enough cards to click
                    if (pile.Count >= _selection.Count)
                        return InputState.Click;
                    else
                        return InputState.None;
            }

            return InputState.None;
        }

        private InputState HandleNumbers(int number)
        {
            _selection.Slot = SlotSelectionType.Tableau;
            _selection.Index = number - 1;

            if (!_selection.IsCardPicked)
                _selection.Count = 1;

            return InputState.SelectionMoved;
        }
    }
}
