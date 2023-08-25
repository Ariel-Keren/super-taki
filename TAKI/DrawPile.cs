using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAKI
{
    internal class DrawPile
    {
        readonly List<Card> cards;

        readonly string[] COLORS = { "red", "blue", "green", "yellow" };
        readonly string[] COLORED_CARDS = { "1", "3", "4", "5", "6", "7", "8", "9", "+2", "Stop", "Change Direction", "Plus", "TAKI" };
        readonly string[] UNCOLORED_CARDS = { "Super TAKI", "King", "+3", "+3 Breaker", "Change Color" };

        public DrawPile()
        {
            cards = new List<Card>();
            AddColoredCards();
            AddColoredCards();
            AddUncoloredCards();
            AddUncoloredCards();
        }

        void AddColoredCards()
        {
            for (int currentColorIndex = 0; currentColorIndex < COLORS.Length; currentColorIndex++)
            {
                string currentColor = COLORS[currentColorIndex];
                for (int currentCardIndex = 0; currentCardIndex < COLORED_CARDS.Length; currentCardIndex++)
                {
                    string currentFigure = COLORED_CARDS[currentCardIndex];
                    Card currentCard = new Card(currentFigure, currentColor);
                    cards.Add(currentCard);
                }
            }
        }

        void AddUncoloredCards()
        {
            for (int currentCardIndex = 0; currentCardIndex < UNCOLORED_CARDS.Length; currentCardIndex++)
            {
                string currentFigure = UNCOLORED_CARDS[currentCardIndex];
                Card currentCard = new Card(currentFigure, null);
                cards.Add(currentCard);
            }
            Card ChangeColor = new Card("Change Color", null);
            cards.Add(ChangeColor);
        }

        int GenerateRandomIndex(Random random)
        {
            int randomIndex = random.Next(cards.Count);
            return randomIndex;
        }

        public Card DrawCard(Random random)
        {
            int randomIndex = GenerateRandomIndex(random);
            Card randomCard = cards[randomIndex];
            cards.Remove(randomCard);
            return randomCard;
        }
    }
}
