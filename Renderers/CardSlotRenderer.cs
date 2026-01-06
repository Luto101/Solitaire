using Solitaire.Builders;
using Solitaire.Builders.Enums;
using Solitaire.Cards;
using Solitaire.UI;

namespace Solitaire.Renderers
{
    /// <summary>
    /// Responsible for rendering card slots and cards on the console screen.
    /// Provides methods for drawing empty slots, shuffle slots, and cards with proper coloring.
    /// </summary>
    public static class CardSlotRenderer
    {
        // Define colors for different card states
        private const ConsoleColor FaceDownColor = ConsoleColor.DarkYellow;
        private const ConsoleColor FaceUpRedColor = ConsoleColor.Red;
        private const ConsoleColor FaceUpBlackColor = ConsoleColor.DarkGray;
        private const ConsoleColor SlotColor = ConsoleColor.White;

        /// <summary>
        /// Draws a card slot based on its type at the specified coordinates.
        /// </summary>
        public static void DrawSlot(SlotType slotType, int x, int y, ConsoleColor? customColor = null)
        {
            // Use custom color if provided, otherwise use default slot color
            ConsoleColor color = customColor ?? SlotColor;

            switch (slotType)
            {
                case SlotType.Empty:
                    UIHelper.WriteLinesAt(BoardSlots.EmptySlot, x, y, color);
                    break;
                case SlotType.Shuffle:
                    UIHelper.WriteLinesAt(BoardSlots.ShuffleSlot, x, y, color);
                    break;
            }
        }

        /// <summary>
        /// Draws a card at the specified coordinates, with the appropriate color and cover status.
        /// </summary>
        public static void DrawCard(Card card, int x, int y, ConsoleColor? customColor = null, CoverStatus coverStatus = CoverStatus.None)
        {
            // Use custom color if provided, otherwise determine color based on card properties
            ConsoleColor color = customColor ?? GetCardColor(card);

            UIHelper.WriteLinesAt(CardBuilder.Build(card, coverStatus), x, y, color);
        }

        /// <summary>
        /// Determines the appropriate color for a card.
        /// </summary>
        private static ConsoleColor GetCardColor(Card card)
        {
            if (card.IsFaceUp)
            {
                if (card.IsRed())
                    return FaceUpRedColor;
                else
                    return FaceUpBlackColor;
            }
            else
                return FaceDownColor;
        }
    }
}
