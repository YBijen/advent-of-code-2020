using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    class Day22 : ASolution
    {
        public Day22() : base(22, 2020, "")
        {
            SetDebugInput();
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
            return null;
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
            var cardsPlayer1 = new Queue<long>();
            var cardsPlayer2 = new Queue<long>();

            var players = base.Input.Split("\n\n");
            foreach(var card in players[0].SplitByNewline().Skip(1).Select(card => long.Parse(card)))
            {
                cardsPlayer1.Enqueue(card);
            }
            foreach (var card in players[1].SplitByNewline().Skip(1).Select(card => long.Parse(card)))
            {
                cardsPlayer2.Enqueue(card);
            }

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
        }
    }
}
