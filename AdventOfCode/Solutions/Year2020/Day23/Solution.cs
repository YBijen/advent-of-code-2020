using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private string PlayGame(List<int> cups, int moves)
        {
            var currentCupIndex = 0;

            var indexModifier = 0;

            for (var i = 1; i <= moves; i++)
            {

                cups = cups.Skip(indexModifier).Union(cups.Take(indexModifier)).ToList();
                indexModifier = 0;
                Console.WriteLine($"Round {i} start: " + string.Join(" ", cups));

                var currentCup = cups[currentCupIndex];



                var removedCups = GetAndRemoveNextCups(cups, currentCup);
                var destinationCup = GetNextDestinationCup(cups, currentCup);

                Console.WriteLine($"Current cup: {currentCup}; Destination cup: {destinationCup}");

                var destinationIndex = cups.IndexOf(destinationCup) + 1;
                for (var j = 0; j < removedCups.Count; j++)
                {
                    cups.Insert(destinationIndex + j, removedCups[j]);
                }

                Console.WriteLine("Moving: " + string.Join(" ", removedCups));




                if (currentCupIndex > destinationIndex)
                {
                    indexModifier = base.Input.Length - currentCupIndex > 3 ? 3 : base.Input.Length - currentCupIndex - 1;
                    //indexModifier = 3;
                }

                Console.WriteLine($"[CURRENT IDX] {currentCupIndex} || [DEST IDX] {destinationIndex} || {indexModifier}");
                Console.WriteLine($"Round {i} finish: " + string.Join(" ", cups));




                if (++currentCupIndex == 9)
                {
                    currentCupIndex = 0;
                }
                Console.WriteLine();
            }

            var indexOfLabel1 = cups.IndexOf(1);
            return string.Join("", cups.Skip(indexOfLabel1 + 1).Union(cups.Take(indexOfLabel1)));
        }

        private List<int> GetAndRemoveNextCups(List<int> cups, int currentCup)
        {
            var currentCupIndex = cups.IndexOf(currentCup);
            var takenCups = cups.Skip(currentCupIndex + 1).Take(CUPS_TO_TAKE).ToList();

            // Fill cups to take from the start if some are missing
            var i = 0;
            while(takenCups.Count != CUPS_TO_TAKE)
            {
                takenCups.Add(cups[i++]);
            }

            takenCups.ForEach(tc => cups.Remove(tc));
            return takenCups;
        }

        private int GetNextDestinationCup(List<int> cups, int currentCup)
        {
            var destinationCup = currentCup - 1;
            while(true)
            {
                if(cups.Contains(destinationCup))
                {
                    return destinationCup;
                }

                if(--destinationCup <= 0)
                {
                    destinationCup = 9;
                }
            }
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private void AssertPartOne()
        {
            base.DebugInput = "389125467";
            Assert.AreEqual("92658374", PlayGame(ParseInput(), 10));
            Assert.AreEqual("67384529", PlayGame(ParseInput(), 100));
            base.DebugInput = null;
        }

        private List<int> ParseInput() => base.Input.Select(c => int.Parse(c.ToString())).ToList();
    }
}
