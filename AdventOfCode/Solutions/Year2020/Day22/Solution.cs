using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Year2020.Day22.Models;

namespace AdventOfCode.Solutions.Year2020
{
    class Day22Solution : ASolution
    {
        public Day22Solution() : base(22, 2020, "")
        {
            //SetDebugInput();
        }

        protected override string SolvePartOne()
        {
            var (cardsPlayer1, cardsPlayer2) = ParseCardsForPlayersFromInput();

            while(cardsPlayer1.Count > 0 && cardsPlayer2.Count > 0)
            {
                var cardPlayer1 = cardsPlayer1.Dequeue();
                var cardPlayer2 = cardsPlayer2.Dequeue();
                if(cardPlayer1 > cardPlayer2)
                {
                    cardsPlayer1.Enqueue(cardPlayer1);
                    cardsPlayer1.Enqueue(cardPlayer2);
                }
                else
                {
                    cardsPlayer2.Enqueue(cardPlayer2);
                    cardsPlayer2.Enqueue(cardPlayer1);
                }
            }

            return CalculateScore(cardsPlayer1, cardsPlayer2).ToString();
        }

        protected override string SolvePartTwo()
        {
            var (cardsPlayer1, cardsPlayer2) = ParseCardsForPlayersFromInput();
            PlayGame(ref cardsPlayer1, ref cardsPlayer2, new HashSet<string>());
            return CalculateScore(cardsPlayer1, cardsPlayer2).ToString();
        }

        private void PlayGame(ref Queue<long> cardsPlayer1, ref Queue<long> cardsPlayer2, HashSet<string> history)
        {
            while (cardsPlayer1.Count > 0 && cardsPlayer2.Count > 0)
            {
                // Perform 
                var id = CardsToString(cardsPlayer1, cardsPlayer2);
                if (!history.Contains(id))
                {
                    history.Add(id);
                }
                else
                {
                    // To prevent an infinite loop player 1 won, we clean the cards of player 2
                    cardsPlayer2 = new Queue<long>();
                    break;
                }

                var cardPlayer1 = cardsPlayer1.Dequeue();
                var cardPlayer2 = cardsPlayer2.Dequeue();

                if (cardsPlayer1.Count >= cardPlayer1 && cardsPlayer2.Count >= cardPlayer2)
                {
                    var winnerSubGame = PlaySubGame(cardsPlayer1.Take((int)cardPlayer1).ToQueue(), cardsPlayer2.Take((int)cardPlayer2).ToQueue());
                    if (winnerSubGame == Player.One)
                    {
                        cardsPlayer1.Enqueue(cardPlayer1);
                        cardsPlayer1.Enqueue(cardPlayer2);
                    }
                    else
                    {
                        cardsPlayer2.Enqueue(cardPlayer2);
                        cardsPlayer2.Enqueue(cardPlayer1);
                    }
                }
                else if (cardPlayer1 > cardPlayer2)
                {
                    cardsPlayer1.Enqueue(cardPlayer1);
                    cardsPlayer1.Enqueue(cardPlayer2);
                }
                else
                {
                    cardsPlayer2.Enqueue(cardPlayer2);
                    cardsPlayer2.Enqueue(cardPlayer1);
                }
            }   
        }

        private string CardsToString(Queue<long> cardsPlayer1, Queue<long> cardsPlayer2) =>
            $"{string.Join("|", cardsPlayer1)}_{string.Join("|", cardsPlayer2)}";

        private Player PlaySubGame(Queue<long> cardsPlayer1, Queue<long> cardsPlayer2)
        {
            PlayGame(ref cardsPlayer1, ref cardsPlayer2, new HashSet<string>());
            return cardsPlayer1.Count > 0 ? Player.One : Player.Two;
        }

        private long CalculateScore(Queue<long> cardsPlayer1, Queue<long> cardsPlayer2)
        {
            var winningCards = cardsPlayer2.Count > 0 ? cardsPlayer2 : cardsPlayer1;
            var result = 0L;
            for (var i = winningCards.Count; i > 0; i--)
            {
                result += winningCards.Dequeue() * i;
            }
            return result;
        }

        private (Queue<long>, Queue<long>) ParseCardsForPlayersFromInput()
        {
            var players = base.Input.Split("\n\n");
            var cardsPlayer1 = players[0].SplitByNewline().Skip(1).Select(card => long.Parse(card)).ToQueue();
            var cardsPlayer2 = players[1].SplitByNewline().Skip(1).Select(card => long.Parse(card)).ToQueue();
            return (cardsPlayer1, cardsPlayer2);
        }

        private void SetDebugInput()
        {
            base.DebugInput = "" +
                "Player 1:\n" +
                "9\n" +
                "2\n" +
                "6\n" +
                "3\n" +
                "1\n" +
                "\n" +
                "Player 2:\n" +
                "5\n" +
                "8\n" +
                "4\n" +
                "7\n" +
                "10";

            base.DebugInput = "" +
                "Player 1:\n" +
                "43\n" +
                "19\n" +
                "\n" +
                "Player 2:\n" +
                "2\n" +
                "29\n" +
                "14";
        }
    }
}
