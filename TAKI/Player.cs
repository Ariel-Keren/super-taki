using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAKI
{
    internal class Player
    {
        readonly string name;
        readonly List<Card> hand;

        const int NUMBER_OF_STARTING_CARDS = 8;

        public Player(string name, DrawPile drawPile, Random random)
        {
            this.name = name;
            hand = new List<Card>();
            PopulateHand(drawPile, random);
        }

        public string GetName()
        {
            return name;
        }

        public Card GetCardByIndex(int index)
        {
            if (index < 0 || index >= hand.Count)
            {
                throw new Exception("Index out of range");
            }

            return hand[index];
        }

        public int GetCardCount()
        {
            return hand.Count;
        }

        void PopulateHand(DrawPile drawPile, Random random)
        {
            for (int i = 0; i < NUMBER_OF_STARTING_CARDS; i++)
            {
                Card drawnCard = drawPile.DrawCard(random);
                hand.Add(drawnCard);
            }
            SortHand();
        }

        int SortCardsByColor(Card firstCard, Card secondCard)
        {
            Dictionary<string, int> colorOrder = new Dictionary<string, int>
            {
                { "red", 1 },
                { "blue", 2 },
                { "green", 3 },
                { "yellow", 4 },
            };

            string firstCardColor = firstCard.GetColor();
            string secondCardColor = secondCard.GetColor();

            if (firstCardColor == null)
            {
                return -1;
            }

            if (secondCardColor == null)
            {
                return 1;
            }

            int firstCardOrder = colorOrder[firstCardColor];
            int secondCardOrder = colorOrder[secondCardColor];

            return firstCardOrder - secondCardOrder;
        }

        void SortHand()
        {
            hand.Sort(SortCardsByColor);
        }

        public void ShowHand()
        {
            int currentCardNumber = 1;
            hand.ForEach(delegate (Card card)
            {
                Console.WriteLine($"{currentCardNumber}: {card}");
                currentCardNumber++;
            });
        }

        public void AddCard(Card card)
        {
            hand.Add(card);
            SortHand();
        }

        public void RemoveCard(Card card)
        {
            hand.Remove(card);
        }
    }
}
