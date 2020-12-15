using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Solutions.Year2020
{
    class Day15 : ASolution
    {
        private List<int> _startingNumbers;
        private List<int> _game;

        public Day15() : base(15, 2020, "")
        {
            AssertDebugInputsPart1();

            // My input
            _startingNumbers = new List<int> { 12, 1, 16, 3, 11, 0 };
        }

        protected override string SolvePartOne()
        {
            const int AMOUNT_OF_TURNS = 2020;

            _game = new List<int>(_startingNumbers);

            for (var turnCount = _startingNumbers.Count; turnCount < AMOUNT_OF_TURNS; turnCount++)
            {
                _game.Add(GetNextNumber());
            }
            return _game.Last().ToString();
        }

        private int GetNextNumber()
        {
            var lastNumber = _game.Last();
            var lastNumberIndex = _game.Count() - 1;
            var beforeLastNumberIndex = -1;
            for (var i = _game.Count - 2; i >= 0; i--)
            {
                if (_game[i] == lastNumber)
                {
                    beforeLastNumberIndex = i;
                    break;
                }
            }

            return beforeLastNumberIndex >= 0
                ? lastNumberIndex - beforeLastNumberIndex
                : 0;
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private void AssertDebugInputsPart1()
        {
            AssertListAndAnswer(new List<int> { 0, 3, 6 }, "436");
            AssertListAndAnswer(new List<int> { 1, 3, 2 }, "1");
            AssertListAndAnswer(new List<int> { 2, 1, 3 }, "10");
            AssertListAndAnswer(new List<int> { 1, 2, 3 }, "27");
            AssertListAndAnswer(new List<int> { 2, 3, 1 }, "78");
            AssertListAndAnswer(new List<int> { 3, 2, 1 }, "438");
            AssertListAndAnswer(new List<int> { 3, 1, 2 }, "1836");
        }

        private void AssertListAndAnswer(List<int> numbers, string expectedResult)
        {
            _startingNumbers = numbers;
            Assert.AreEqual(SolvePartOne(), expectedResult);
        }
    }
}
