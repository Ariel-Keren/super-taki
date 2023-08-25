using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAKI
{
    internal class Game
    {
        readonly Random random;
        readonly DrawPile drawPile;
        readonly Player[] players;
        Card leadingCard;

        const int MINIMUM_PLAYER_COUNT = 2;
        const int MAXIMUM_PLAYER_COUNT = 10;
        readonly string[] COLORS = { "red", "blue", "green", "yellow" };

        public Game()
        {
            random = new Random();
            drawPile = new DrawPile();
            leadingCard = drawPile.DrawCard(random);
            players = PopulatePlayers();
        }

        public void Start()
        {
            int turn = 0, numberOfCardsToDraw = 1;
            bool[] shouldDrawThreeCards = new bool[players.Length];
            bool isTakiActive = false;

            while (true)
            {
                Console.Clear();
                int playingUserIndex = turn % players.Length;
                Player playingUser = players[playingUserIndex];
                turn++;

                if (shouldDrawThreeCards[playingUserIndex])
                {
                    Console.WriteLine($"Someone put a +3!");
                    DrawCard(playingUser, 3);
                    Console.WriteLine();
                    shouldDrawThreeCards[playingUserIndex] = false;
                }

                TurnInformation(playingUser);

                while (true)
                {
                    Console.WriteLine("\nWhich card would you like to put? (Enter the number to its left)");
                    Console.WriteLine("Or enter \"draw\" to draw a card from the draw pile");

                    if (isTakiActive)
                    {
                        Console.WriteLine("You can press enter to end the TAKI");
                    }

                    string userInput = Console.ReadLine();
                    bool isPlusTwoActive = numberOfCardsToDraw > 1;

                    if (userInput.ToLower() == "draw" && !isTakiActive)
                    {
                        DrawCard(playingUser, numberOfCardsToDraw);
                        if (isPlusTwoActive)
                        {
                            numberOfCardsToDraw = 1;
                        }
                        break;
                    }

                    if (userInput == "" && isTakiActive)
                    {
                        PerformCardAction(leadingCard, playingUserIndex, shouldDrawThreeCards, ref turn, ref numberOfCardsToDraw, ref isTakiActive);
                        isTakiActive = false;
                        break;
                    }

                    if (!int.TryParse(userInput, out int chosenCardNumber) || chosenCardNumber < 0 || chosenCardNumber > playingUser.GetCardCount())
                    {
                        Console.WriteLine("Invalid input");
                        continue;
                    }

                    int chosenCardIndex = chosenCardNumber - 1;
                    Card chosenCard = playingUser.GetCardByIndex(chosenCardIndex);

                    if (isTakiActive && leadingCard.GetFigure() == "+3")
                    {
                        Console.WriteLine("+3 can't be hidden inside a TAKI run");
                    }

                    if (!chosenCard.IsPossibleToPut(leadingCard, isPlusTwoActive, isTakiActive))
                    {
                        Console.WriteLine("You can't put this card");
                        continue;
                    }

                    playingUser.RemoveCard(chosenCard);

                    if (isTakiActive)
                    {
                        turn--;
                    }
                    else
                    {
                        PerformCardAction(chosenCard, playingUserIndex, shouldDrawThreeCards, ref turn, ref numberOfCardsToDraw, ref isTakiActive);
                    }

                    if (chosenCard.GetFigure() != "+3" && chosenCard.GetFigure() != "+3 Breaker")
                    {
                        leadingCard = chosenCard;
                    }

                    break;
                }

                if (playingUser.GetCardCount() == 0)
                {
                    Console.WriteLine($"\nThe winner is {playingUser.GetName()}!");
                    break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        void TurnInformation(Player playingUser)
        {
            Console.WriteLine($"{playingUser.GetName()}'s turn");
            Console.WriteLine($"\nThe Leading Card is a {leadingCard}");
            playingUser.ShowHand();
        }

        void DrawCard(Player player, int numberOfCardsToDraw)
        {
            for (int i = 0; i < numberOfCardsToDraw; i++)
            {
                Card drawnCard = drawPile.DrawCard(random);
                player.AddCard(drawnCard);
                Console.WriteLine($"You've drawn a {drawnCard}");
            }
        }

        void PerformCardAction(Card card, int playerIndex, bool[] shouldDrawThreeCards, ref int turn, ref int numberOfCardsToDraw, ref bool isTakiActive)
        {
            switch (card.GetFigure())
            {
                case "Change Direction":
                    players.Reverse();
                    break;
                case "Stop":
                    turn++;
                    break;
                case "Plus":
                    turn--;
                    break;
                case "Change Color":
                    ChangeColor(card);
                    break;
                case "+2":
                    if (numberOfCardsToDraw == 1)
                    {
                        numberOfCardsToDraw = 2;
                        break;
                    }
                    numberOfCardsToDraw += 2;
                    break;
                case "King":
                    turn--;
                    numberOfCardsToDraw = 1;
                    break;
                case "+3":
                    for (int currentPlayerIndex = 0; currentPlayerIndex < players.Length; currentPlayerIndex++)
                    {
                        if (currentPlayerIndex != playerIndex)
                        {
                            shouldDrawThreeCards[currentPlayerIndex] = true;
                        }
                    }
                    break;
                case "+3 Breaker":
                    Player player = players[playerIndex];
                    DrawCard(player, 3);
                    break;
                case "TAKI":
                    if (!isTakiActive)
                    {
                        turn--;
                        isTakiActive = true;
                    }
                    break;
                case "Super TAKI":
                    card.SetColor(leadingCard.GetColor());
                    if (!isTakiActive)
                    {
                        turn--;
                        isTakiActive = true;
                    }
                    break;
            }
        }

        void ChangeColor(Card card)
        {
            while (true)
            {
                Console.WriteLine("Which color would you like to change to? (Enter \"red\", \"blue\", \"green\" or \"yellow\")");
                string userInput = Console.ReadLine().ToLower();
                if (COLORS.Contains(userInput))
                {
                    card.SetColor(userInput);
                    break;
                }
            }
        }

        Player[] PopulatePlayers()
        {
            int currentPlayerNumber = 1;
            List<Player> playerList = new List<Player>();

            Console.WriteLine("Super TAKI");

            for (int i = 0; i < MAXIMUM_PLAYER_COUNT; i++)
            {
                Console.Write($"\nWhat's the name of player {currentPlayerNumber}?");

                if (currentPlayerNumber > MINIMUM_PLAYER_COUNT)
                {
                    Console.WriteLine(" (Enter \"start\" to begin the game)");
                }
                else
                {
                    Console.WriteLine();
                }

                string userInput = Console.ReadLine();
                
                if (userInput.ToLower() == "start" && currentPlayerNumber > MINIMUM_PLAYER_COUNT)
                {
                    break;
                }
                
                if (userInput.ToLower() == "start")
                {
                    Console.WriteLine($"Add at least {MINIMUM_PLAYER_COUNT} players to start the game");
                    continue;
                }

                if (userInput.ToLower().Trim() == "")
                {
                    Console.WriteLine("Unable to add a player without a name");
                    continue;
                }

                Player newPlayer = new Player(userInput, drawPile, random);
                playerList.Add(newPlayer);
                currentPlayerNumber++;
            }

            return playerList.ToArray();
        }
    }
}
