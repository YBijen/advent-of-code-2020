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
            AssertPartTwo();
        }

        protected override string SolvePartOne() => CreateOutputForResult(PlayGame(ParseInput(), 100));

        protected override string SolvePartTwo()
        {
            var cups = ParseInput();
            foreach (var i in Enumerable.Range(base.Input.Length + 1, 1_000_000 - base.Input.Length))
            {
                cups.AddLast(i);
            }

            PlayGame(cups, 10_000_000);

            var cup1 = cups.Find(1);
            return ((long)cup1.Next.Value * (long)cup1.Next.Next.Value).ToString();
        }

        private LinkedList<int> PlayGame(LinkedList<int> cups, int moves)
        {
            var cupsCount = cups.Count;

            var currentCup = cups.First;

            // Create index of LinkedList
            var index = new Dictionary<int, LinkedListNode<int>>();
            while(index.Count != cups.Count)
            {
                index.Add(currentCup.Value, currentCup);
                currentCup = currentCup.NextOrFirst();
            }

            Assert.AreEqual(currentCup, cups.First);

            for (var i = 1; i <= moves; i++)
            {
                var removedCups = new List<LinkedListNode<int>>(CUPS_TO_TAKE);
                for (var j = 0; j < CUPS_TO_TAKE; j++)
                {
                    removedCups.Insert(0, currentCup.NextOrFirst());
                    cups.Remove(currentCup.NextOrFirst());
                }

                var destinationCup = GetNextDestinationCup(removedCups, cupsCount, currentCup.Value);
                var destinationCupNode = index[destinationCup];

                removedCups.ForEach(cup => cups.AddAfter(destinationCupNode ?? cups.First, cup));

                currentCup = currentCup.NextOrFirst();
            }

            return cups;
        }

        private int GetNextDestinationCup(List<LinkedListNode<int>> removedCups, int listLength, int currentCup)
        {
            var removedCupsValue = removedCups.Select(cup => cup.Value);

            var destinationCupValue = currentCup;
            while(true)
            {
                if (--destinationCupValue <= 0)
                {
                    destinationCupValue = listLength;
                }

                if (!removedCupsValue.Contains(destinationCupValue))
                {
                    return destinationCupValue;
                }
            }
        }

        private string CreateOutputForResult(LinkedList<int> cups)
        {
            var cupsList = cups.ToList();
            var indexOfLabel1 = cupsList.IndexOf(1);
            return string.Join("", cupsList.Skip(indexOfLabel1 + 1).Union(cupsList.Take(indexOfLabel1)));
        }


        private void AssertPartOne()
        {
            base.DebugInput = "389125467";
            Assert.AreEqual("92658374", CreateOutputForResult(PlayGame(ParseInput(), 10)));
            Assert.AreEqual("67384529", CreateOutputForResult(PlayGame(ParseInput(), 100)));
            base.DebugInput = null;
        }

        private void AssertPartTwo()
        {
            base.DebugInput = "389125467";
            Assert.AreEqual("149245887792", SolvePartTwo());
            base.DebugInput = null;
        }

        private LinkedList<int> ParseInput()
        {
            var ll = new LinkedList<int>();
            base.Input.Select(c => int.Parse(c.ToString())).ToList().ForEach(v => ll.AddLast(v));
            return ll;
        }
    }
}
