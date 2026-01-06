namespace Solitaire.Cards.Enums
{
    /// <summary>
    /// Helper class for converting card enums to strings or characters.
    /// </summary>
    public static class CardEnumConverter
    {
        /// <summary>
        /// Converts a <see cref="CardSuit"/> to Unicode character.
        /// </summary>
        public static char SuitToChar(CardSuit suit)
        {
            return suit switch
            {
                CardSuit.Spade => '♠',
                CardSuit.Diamond => '♦',
                CardSuit.Heart => '♥',
                CardSuit.Club => '♣',
                _ => '\0',
            };
        }

        /// <summary>
        /// Converts a <see cref="CardRank"/> to its string representation ("A", "10", "K", ...).
        /// </summary>
        public static string RankToString(CardRank rank)
        {
            return rank switch
            {
                CardRank.Ace => "A",
                CardRank.Two => "2",
                CardRank.Three => "3",
                CardRank.Four => "4",
                CardRank.Five => "5",
                CardRank.Six => "6",
                CardRank.Seven => "7",
                CardRank.Eight => "8",
                CardRank.Nine => "9",
                CardRank.Ten => "10",
                CardRank.Jack => "J",
                CardRank.Queen => "Q",
                CardRank.King => "K",
                _ => ""
            };
        }
    }
}
