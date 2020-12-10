using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    class Day10 : ASolution
    {
        public Day10() : base(10, 2020, "")
        {
            //SetDebugInput();
        }

        protected override string SolvePartOne()
        {
            var adapters = base.Input.ToIntArray("\n").OrderBy(i => i).ToList();

            // Add the charging outlet
            adapters.Insert(0, 0);

            var joltDifferences = new List<int>();
            for (var i = 0; i < adapters.Count - 1; i++)
            {
                joltDifferences.Add(adapters[i + 1] - adapters[i]);
            }

            // Add the adapter on the device
            joltDifferences.Add(3);

            // Sanity check
            if(!joltDifferences.All(diff => diff >= 1 || diff <= 3))
            {
                throw new Exception($"Something went wrong in calculating the differences or the input does not support this method.");
            }

            return (joltDifferences.Count(diff => diff == 1) * joltDifferences.Count(diff => diff == 3)).ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

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
        #endregion
    }
}
