namespace Solitaire.Builders
{
    /// <summary>
    /// Provides representations of different board slot types.
    /// </summary>
    public static class BoardSlots
    {
        public static readonly string[] ShuffleSlot =
            [
                "┌─────────┐",
                "│         │",
                "│         │",
                "│ Shuffle │",
                "│         │",
                "│         │",
                "└─────────┘"
            ];

        public static readonly string[] EmptySlot =
            [
                "┌─────────┐",
                "│         │",
                "│         │",
                "│         │",
                "│         │",
                "│         │",
                "└─────────┘"
            ];
    }
}
