using Solitaire.Renderers.Enums;

namespace Solitaire.Renderers
{
    /// <summary>
    /// Represents the selection state of a card slot.
    /// </summary>
    public class SelectionInfo(SlotSelectionType slot = SlotSelectionType.Stock, int index = 0, int count = 1)
    {
        public SlotSelectionType Slot { get; set; } = slot;
        public int Index { get; set; } = index;
        public int Count { get; set; } = count;
        public bool IsCardPicked { get; set; } = false;
    }
}
