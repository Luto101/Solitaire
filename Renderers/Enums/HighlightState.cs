namespace Solitaire.Renderers.Enums
{
    /// <summary>
    /// Represents the different states of slot selection with corresponding highlight colors.
    /// </summary>
    public enum HighlightState
    {
        Cursor = ConsoleColor.Blue,
        Picked = ConsoleColor.DarkBlue,
        Wrong = ConsoleColor.DarkRed,
        Good = ConsoleColor.Green
    }
}
