using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    class Day22 : ASolution
    {
        private Queue<long> _cardsPlayer1 = new Queue<long>();
        private Queue<long> _cardsPlayer2 = new Queue<long>();

        public Day22() : base(22, 2020, "")
        {
            //SetDebugInput();
            ParseCardsForPlayersFromInput();
        }

        protected override string SolvePartOne()
        {
            while(_cardsPlayer1.Count > 0 && _cardsPlayer2.Count > 0)
            {
                var cardPlayer1 = _cardsPlayer1.Dequeue();
                var cardPlayer2 = _cardsPlayer2.Dequeue();
                if(cardPlayer1 > cardPlayer2)
                {
                    _cardsPlayer1.Enqueue(cardPlayer1);
                    _cardsPlayer1.Enqueue(cardPlayer2);
                }
                else
                {
                    _cardsPlayer2.Enqueue(cardPlayer2);
                    _cardsPlayer2.Enqueue(cardPlayer1);
                }
            }

            var winningCards = _cardsPlayer2.Count > 0 ? _cardsPlayer2 : _cardsPlayer1;
            var result = 0L;
            for(var i = winningCards.Count; i > 0; i--)
            {
                result += winningCards.Dequeue() * i;
            }
            return result.ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private void ParseCardsForPlayersFromInput()
        {
            var players = base.Input.Split("\n\n");
            foreach(var card in players[0].SplitByNewline().Skip(1).Select(card => long.Parse(card)))
            {
                _cardsPlayer1.Enqueue(card);
            }
            foreach (var card in players[1].SplitByNewline().Skip(1).Select(card => long.Parse(card)))
            {
                _cardsPlayer2.Enqueue(card);
            }
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
