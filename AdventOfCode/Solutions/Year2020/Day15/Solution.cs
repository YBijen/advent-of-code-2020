using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Solutions.Year2020
{
    class Day15 : ASolution
    {
        private List<int> _startingNumbers;
        private Dictionary<int, int[]> _numberIndexCollection;

        private List<int> _game;

        public Day15() : base(15, 2020, "")
        {
            AssertDebugInputsPart1();
            AssertDebugInputsPart2();

            // My input
            _startingNumbers = new List<int> { 12, 1, 16, 3, 11, 0 };
        }

        protected override string SolvePartOne()
        {
            const int AMOUNT_OF_TURNS = 2020;

            var sw = Stopwatch.StartNew();
            
            _game = new List<int>(_startingNumbers);

            for (var turnCount = _startingNumbers.Count; turnCount < AMOUNT_OF_TURNS; turnCount++)
            {
                _game.Add(GetNextNumber());
            }

            var result = _game.Last().ToString();
            sw.Stop();
            Console.WriteLine($"Solution 1 solved in {sw.Elapsed}.");



            sw.Restart();

            var currentTurn = 1;
            _numberIndexCollection = new Dictionary<int, int[]>();
            _startingNumbers.ForEach(number => _numberIndexCollection.Add(number, new int[2] { currentTurn++, -1 }));

            var lastAdded = _startingNumbers.Last();
            while (currentTurn <= AMOUNT_OF_TURNS)
            {
                var currentNumberIndex = _numberIndexCollection[lastAdded];
                lastAdded = (currentNumberIndex[1] != -1) ? currentNumberIndex[1] - currentNumberIndex[0] : 0;
                AddToNumberIndexCollection(lastAdded, currentTurn++);
            }

            var result2 = lastAdded.ToString();
            Console.WriteLine($"Solution 2 solved in {sw.Elapsed}.");

            return lastAdded.ToString();
        }

        protected override string SolvePartTwo()
        {
            const int AMOUNT_OF_TURNS = 30000000;

            var sw = Stopwatch.StartNew();

            var currentTurn = 1;
            _numberIndexCollection = new Dictionary<int, int[]>();
            _startingNumbers.ForEach(number => _numberIndexCollection.Add(number, new int[2] { currentTurn++, -1 }));

            var lastAdded = _startingNumbers.Last();
            while (currentTurn <= AMOUNT_OF_TURNS)
            {
                var currentNumberIndex = _numberIndexCollection[lastAdded];
                lastAdded = (currentNumberIndex[1] != -1) ? currentNumberIndex[1] - currentNumberIndex[0] : 0;
                AddToNumberIndexCollection(lastAdded, currentTurn++);
            }

            Console.WriteLine($"Part 2 solved in {sw.Elapsed}.");

            return lastAdded.ToString();
        }

        private void AddToNumberIndexCollection(int number, int currentTurn)
        {
            if (_numberIndexCollection.ContainsKey(number))
            {
                _numberIndexCollection[number] = new int[] { _numberIndexCollection[number].Max(), currentTurn++ };
            }
            else
            {
                _numberIndexCollection.Add(number, new int[] { currentTurn++, -1 });
            }
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

        private void AssertDebugInputsPart1()
        {
            AssertListAndAnswerPart1(new List<int> { 0, 3, 6 }, "436");
            AssertListAndAnswerPart1(new List<int> { 1, 3, 2 }, "1");
            AssertListAndAnswerPart1(new List<int> { 2, 1, 3 }, "10");
            AssertListAndAnswerPart1(new List<int> { 1, 2, 3 }, "27");
            AssertListAndAnswerPart1(new List<int> { 2, 3, 1 }, "78");
            AssertListAndAnswerPart1(new List<int> { 3, 2, 1 }, "438");
            AssertListAndAnswerPart1(new List<int> { 3, 1, 2 }, "1836");
        }

        private void AssertListAndAnswerPart1(List<int> numbers, string expectedResult)
        {
            _startingNumbers = numbers;
            Assert.AreEqual(SolvePartOne(), expectedResult);
        }

        private void AssertDebugInputsPart2()
        {
            AssertListAndAnswerPart2(new List<int> { 0, 3, 6 }, "175594");
            AssertListAndAnswerPart2(new List<int> { 1, 3, 2 }, "2578");
            AssertListAndAnswerPart2(new List<int> { 2, 1, 3 }, "3544142");
            AssertListAndAnswerPart2(new List<int> { 1, 2, 3 }, "261214");
            AssertListAndAnswerPart2(new List<int> { 2, 3, 1 }, "6895259");
            AssertListAndAnswerPart2(new List<int> { 3, 2, 1 }, "18");
            AssertListAndAnswerPart2(new List<int> { 3, 1, 2 }, "362");
        }

        private void AssertListAndAnswerPart2(List<int> numbers, string expectedResult)
        {
            _startingNumbers = numbers;
            Assert.AreEqual(SolvePartTwo(), expectedResult);
        }
    }
}
