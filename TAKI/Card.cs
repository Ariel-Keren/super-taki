using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAKI
{
    internal class Card
    {
        readonly string figure;
        string color;

        public Card(string figure, string color)
        {
            this.figure = figure;
            this.color = color;
        }

        public string GetFigure()
        {
            return figure;
        }

        public string GetColor()
        {
            return color;
        }

        public void SetColor(string color)
        {
            if (figure != "Change Color" && figure != "Super TAKI")
            {
                throw new Exception("Only the cards Change Color and Super TAKI can change their color");
            }

            this.color = color;
        }

        public bool IsPossibleToPut(Card leadingCard, bool isPlusTwoActive, bool isTakiActive)
        {
            bool doesFigureMatch = !isTakiActive && figure == leadingCard.GetFigure();
            bool doesColorMatch = !isPlusTwoActive && color != null && color == leadingCard.GetColor();
            bool isSpecialStart = leadingCard.GetFigure() == "+3" || leadingCard.GetFigure() == "+3 Breaker" || leadingCard.GetFigure() == "Super Taki" && leadingCard.GetColor() == null;
            bool isSuperTaki = figure == "Super TAKI";
            bool isChangeColor = figure == "Change Color";
            bool isKing = figure == "King";
            bool isFreeTurn = leadingCard.GetFigure() == "King";
            bool isPlusThree = figure == "+3";
            bool isPlusThreeBreaker = !isPlusTwoActive && figure == "+3 Breaker";
            bool canBePut = doesFigureMatch || doesColorMatch || isSpecialStart || isSuperTaki || isChangeColor || isKing || isFreeTurn || isPlusThree || isPlusThreeBreaker;
            return canBePut;
        }

        public override string ToString()
        {
            if (color == null)
            {
                return figure;
            }
            string cardString = $"{color} {figure}";
            return cardString;
        }
    }
}
