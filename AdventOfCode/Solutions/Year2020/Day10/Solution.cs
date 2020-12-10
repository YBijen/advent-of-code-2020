using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    class Day10 : ASolution
    {
        public Day10() : base(10, 2020, "")
        {
            //SetDebugInput2();
        }

        protected override string SolvePartOne()
        {
            var joltDifferences = GetJoltDifferencesForAdapters();
            return (joltDifferences.Count(diff => diff == 1) * joltDifferences.Count(diff => diff == 3)).ToString();
        }

        protected override string SolvePartTwo()
        {
            var joltDifferences = GetJoltDifferencesForAdapters();
            var chainOfOneDifferences = FindChainOfOneJoltDifference(joltDifferences);
            var amountOfCombinations = chainOfOneDifferences.Aggregate(1L, (v1, v2) => v1 * CalculatePossibilities(v2));
            return amountOfCombinations.ToString();
        }

        private List<int> GetJoltDifferencesForAdapters() => GetJoltDifferencesForAdapters(GetAdapters());

        private List<int> GetJoltDifferencesForAdapters(List<int> adapters)
        {
            var joltDifferences = new List<int>();
            for (var i = 0; i < adapters.Count - 1; i++)
            {
                joltDifferences.Add(adapters[i + 1] - adapters[i]);
            }

            // Sanity check
            if (!joltDifferences.All(difference => difference == 1 || difference == 3))
            {
                throw new Exception($"Something went wrong in calculating the differences or the input does not support this method.");
            }

            return joltDifferences;
        }

        /// <summary>
        /// Find all chained jolt differences of "1"
        /// Only return those that are chained by more than 1
        /// </summary>
        /// <param name="joltDifferences"></param>
        /// <returns></returns>
        private List<int> FindChainOfOneJoltDifference(List<int> joltDifferences)
        {
            var chainOfOneDifferences = new List<int>();
            var currentChain = 0;
            foreach (var difference in joltDifferences)
            {
                if (difference == 1)
                {
                    currentChain++;
                }
                else if (difference == 3)
                {
                    if (currentChain > 1)
                    {
                        chainOfOneDifferences.Add(currentChain);
                    }
                    currentChain = 0;
                }
            }
            return chainOfOneDifferences;
        }

        private List<int> GetAdapters()
        {
            var adapters = base.Input.ToIntArray("\n").OrderBy(i => i).ToList();

            // Add the charging outlet
            adapters.Insert(0, 0);
            // Add the device
            adapters.Add(adapters.Max() + 3);

            return adapters;
        }

        /// <summary>
        /// The amount of Possibilities were counted by hand on paper
        /// TODO: I have to read up on how to find the unique possibilities for each amount
        /// </summary>
        /// <param name="chainLength"></param>
        /// <returns></returns>
        private int CalculatePossibilities(int chainLength) => chainLength switch
        {
            2 => 2,
            3 => 4,
            4 => 7,
            _ => throw new Exception(chainLength + " is not calculated yet.")
        };

        #region Debug Input
        private void SetDebugInput()
        {
            base.DebugInput = "" +
                "16\n" +
                "10\n" +
                "15\n" +
                "5\n" +
                "1\n" +
                "11\n" +
                "7\n" +
                "19\n" +
                "6\n" +
                "12\n" +
                "4";
        }

        private void SetDebugInput2()
        {
            base.DebugInput = "" +
                "28\n" +
                "33\n" +
                "18\n" +
                "42\n" +
                "31\n" +
                "14\n" +
                "46\n" +
                "20\n" +
                "48\n" +
                "47\n" +
                "24\n" +
                "23\n" +
                "49\n" +
                "45\n" +
                "19\n" +
                "38\n" +
                "39\n" +
                "11\n" +
                "1\n" +
                "32\n" +
                "25\n" +
                "35\n" +
                "8\n" +
                "17\n" +
                "7\n" +
                "9\n" +
                "4\n" +
                "2\n" +
                "34\n" +
                "10\n" +
                "3";
        }
        #endregion
    }
}
