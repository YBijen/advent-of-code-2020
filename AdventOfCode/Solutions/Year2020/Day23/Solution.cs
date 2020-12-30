using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    class Day23 : ASolution
    {
        const int CUPS_TO_TAKE = 3;
        const int LOOPS = 8;

        public Day23() : base(23, 2020, "")
        {
            base.DebugInput = "389125467";
        }

        protected override string SolvePartOne()
        {

            var cups = ParseInput();
            var currentCupIndex = 0;
            
            Console.WriteLine($"Round 0: " + string.Join(" ", cups));

            for (var i = 1; i <= LOOPS; i++)
            {
                var currentCup = cups[currentCupIndex++];

                var nextDest = GetNextDestinationCup(cups, currentCup);
                var cupsToTake = GetCupsToTake(cups, currentCup);
                foreach (var cupToTake in cupsToTake)
                {
                    cups.Remove(cupToTake);
                }

                var destinationIndex = cups.IndexOf(nextDest) + 1;
                for(var j = 0; j < cupsToTake.Count; j++)
                {
                    cups.Insert(destinationIndex + j, cupsToTake[j]);
                }

                Console.WriteLine($"Round {i}: " + string.Join(" ", cups));
                Console.WriteLine($"Current cup: {currentCup}; Destination cup: {nextDest}");
            }
            return null;
        }

        private List<int> GetCupsToTake(List<int> cups, int currentCup)
        {
            var currentCupIndex = cups.IndexOf(currentCup);
            return cups.Skip(currentCupIndex + 1).Take(CUPS_TO_TAKE).ToList();
        }

        private int GetNextDestinationCup(List<int> cups, int currentCup)
        {
            var destinationCup = currentCup - 1;
            var cupsToTake = GetCupsToTake(cups, currentCup);
            while (true)
            {
                if(cupsToTake.Contains(destinationCup))
                {
                    destinationCup--;
                }
                else
                {
                    return destinationCup;
                }
            }
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private List<int> ParseInput() => base.Input.Select(c => int.Parse(c.ToString())).ToList();
    }
}
