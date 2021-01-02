using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Solutions.Year2020
{
    class Day23 : ASolution
    {
        const int CUPS_TO_TAKE = 3;

        public Day23() : base(23, 2020, "")
        {
            AssertPartOne();
        }

        protected override string SolvePartOne() => PlayGame(ParseInput(), 100);

        protected override string SolvePartTwo()
        {
            return null;
        }

        private string PlayGame(LinkedList<int> cups, int moves)
        {
            var currentCup = cups.First;
            for (var i = 1; i <= moves; i++)
            {
                var removedCups = new List<int>();
                for (var j = 0; j < CUPS_TO_TAKE; j++)
                {
                    removedCups.Insert(0, currentCup.Next?.Value ?? cups.First.Value);
                    cups.Remove(currentCup?.Next ?? cups.First);
                }

                var destinationCup = GetNextDestinationCup(removedCups, currentCup.Value);
                var destinationCupNode = cups.Find(destinationCup);

                removedCups.ForEach(cup => cups.AddAfter(destinationCupNode ?? cups.First, cup));

                currentCup = currentCup.Next ?? cups.First;
            }

            return CreateOutputForResult(cups);
        }


        private string CreateOutputForResult(LinkedList<int> cups)
        {
            var cupsList = cups.ToList();
            var indexOfLabel1 = cupsList.IndexOf(1);
            return string.Join("", cupsList.Skip(indexOfLabel1 + 1).Union(cupsList.Take(indexOfLabel1)));
        }

        private int GetNextDestinationCup(List<int> removedCups, int currentCup)
        {
            var destinationCupValue = currentCup;
            while(true)
            {
                if (--destinationCupValue <= 0)
                {
                    destinationCupValue = base.Input.Length;
                }

                if (!removedCups.Contains(destinationCupValue))
                {
                    return destinationCupValue;
                }
            }
        }

        private void AssertPartOne()
        {
            base.DebugInput = "389125467";
            Assert.AreEqual("92658374", PlayGame(ParseInput(), 10));
            Assert.AreEqual("67384529", PlayGame(ParseInput(), 100));
            base.DebugInput = null;
        }

        private LinkedList<int> ParseInput()
        {
            var ll = new LinkedList<int>();
            base.Input.Select(c => int.Parse(c.ToString())).Reverse().ToList().ForEach(v => ll.AddFirst(v));
            return ll;
        }
    }
}
