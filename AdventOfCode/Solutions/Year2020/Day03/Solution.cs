using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    class Day03Solution : ASolution
    {
        public Day03Solution() : base(03, 2020, "")
        {
            //SetDebugInput();
        }

        private void SetDebugInput()
        {
            base.DebugInput =
                "..##.......\n" +
                "#...#...#..\n" +
                ".#....#..#.\n" +
                "..#.#...#.#\n" +
                ".#...##..#.\n" +
                "..#.##.....\n" +
                ".#.#.#....#\n" +
                ".#........#\n" +
                "#.##...#...\n" +
                "#...##....#\n" +
                ".#..#...#.#";
        }

        protected override string SolvePartOne()
        {
            return CountEncounteredTrees(3, 1).ToString();
        }

        protected override string SolvePartTwo()
        {
            var slopeAngleModifiers = new List<(int X, int Y)>
            {
                (1, 1),
                (3, 1),
                (5, 1),
                (7, 1),
                (1, 2)
            };

            return slopeAngleModifiers
                .Select(modifier => CountEncounteredTrees(modifier.X, modifier.Y))
                .Aggregate((value1, value2) => value1 * value2)
                .ToString();
        }

        private long CountEncounteredTrees(int xModifier, int yModifier)
        {
            var input = base.Input.SplitByNewline();
            var singleInputLength = input[0].Length;
            var amountOfLoops = input.Length / yModifier;

            long encounteredTrees = 0;
            for (var loop = 0; loop < amountOfLoops; loop++)
            {
                var y = loop * yModifier;
                var x = (loop * xModifier) % singleInputLength;
                if (input[y][x] == '#')
                {
                    encounteredTrees++;
                }
            }

            return encounteredTrees;
        }
    }
}
