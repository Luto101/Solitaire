using Solitaire.Builders.Enums;
using Solitaire.Cards;
using Solitaire.Cards.Enums;

namespace Solitaire.Builders
{
    /// <summary>
    /// Builds visual (Unicode) representations of card.
    /// </summary>
    public static class CardBuilder
    {
        // Indicators:
        // r - card rank,
        // s - card suit,
        // h - horizontally covered suit,
        // v - vertically covered suit
        private static readonly string[] FaceUpTemplate =
            [
                "┌─────────┐",
                "│r       h│",
                "│v        │",
                "│    s    │",
                "│         │",
                "│        r│",
                "└─────────┘"
            ];

        private static readonly string[] FaceDownTemplate =
            [
                "┌─────────┐",
                "│░░░░░░░░░│",
                "│░░░░░░░░░│",
                "│░░░░░░░░░│",
                "│░░░░░░░░░│",
                "│░░░░░░░░░│",
                "└─────────┘"
            ];

        /// <summary>
        /// Builds a card representation.
        /// </summary>
        public static string[] Build(Card card, CoverStatus coverStatus)
        {
            if (card.IsFaceUp)
                return BuildFaceUpCard(card.Rank, card.Suit, coverStatus);
            else
                return FaceDownTemplate;
        }

        private static string[] BuildFaceUpCard(CardRank rank, CardSuit suit, CoverStatus coverStatus)
        {
            string[] card = FaceUpTemplate.ToArray();

            // Adjust spacing for two-character rank
            if (rank == CardRank.Ten)
            {
                card[1] = card[1].Remove(2, 1);
                card[5] = card[5].Remove(2, 1);
            }

            // Replace horizontal or vertical cover indicators
            if (coverStatus == CoverStatus.Horizontally)
                card[1] = card[1].Replace('h', CardEnumConverter.SuitToChar(suit));
            else if (coverStatus == CoverStatus.Vertically)
                card[2] = card[2].Replace('v', CardEnumConverter.SuitToChar(suit));

            card[1] = card[1].Replace("h", " ");
            card[2] = card[2].Replace("v", " ");

            card = ReplaceIndicators(card, rank, suit);

            return card;
        }

        private static string[] ReplaceIndicators(string[] card, CardRank rank, CardSuit suit)
        {
            // Insert rank and suit
            for (int i = 0; i < card.Length; i++)
            {
                card[i] = card[i].Replace("r", CardEnumConverter.RankToString(rank));
                card[i] = card[i].Replace('s', CardEnumConverter.SuitToChar(suit));
            }

            return card;
        }
    }
}
