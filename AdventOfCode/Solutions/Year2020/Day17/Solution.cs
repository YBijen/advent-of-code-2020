using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Year2020.Day17.Models;

namespace AdventOfCode.Solutions.Year2020
{
    class Day17Solution : ASolution
    {
        private HashSet<(int X, int Y, int Z)> _pocketDimension;

        public Day17Solution() : base(17, 2020, "")
        {
            //base.DebugInput = ".#.\n..#\n###";
            ParseInput();
        }

        protected override string SolvePartOne()
        {
            PerformCycles(6);
            return _pocketDimension.Count.ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private void PerformCycles(int cycles)
        {
            HashSet<(int X, int Y, int Z)> temporaryPocketDimension = new HashSet<(int X, int Y, int Z)>();
            for (var i = 0; i < cycles; i++)
            {
                temporaryPocketDimension.Clear();
                var borders = GetCurrentBorders();
                for (var x = borders.X.Min; x <= borders.X.Max; x++)
                {
                    for (var y = borders.Y.Min; y <= borders.Y.Max; y++)
                    {
                        for (var z = borders.Z.Min; z <= borders.Z.Max; z++)
                        {
                            var currentCube = (x, y, z);
                            var activeNeighbourCount = CountActiveNeighboursForCube(currentCube);
                            if(activeNeighbourCount == 3)
                            {
                                temporaryPocketDimension.Add(currentCube);
                            }
                            else if (activeNeighbourCount == 2 && _pocketDimension.Contains(currentCube))
                            {
                                temporaryPocketDimension.Add(currentCube);
                            }
                        }
                    }
                }
                _pocketDimension = new HashSet<(int X, int Y, int Z)>(temporaryPocketDimension);
            }
        }

        private int CountActiveNeighboursForCube((int X, int Y, int Z) cube)
        {
            var count = 0;
            for(var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    for (var z = -1; z <= 1; z++)
                    {
                        if(x == 0 && y == 0 && z == 0)
                        {
                            continue;
                        }

                        var cubeToCheck = (cube.X + x, cube.Y + y, cube.Z + z);
                        if(_pocketDimension.Contains(cubeToCheck))
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        private (MinMax X, MinMax Y, MinMax Z) GetCurrentBorders()
        {
            MinMax x = new MinMax(), y = new MinMax(), z = new MinMax();
            foreach(var cube in _pocketDimension)
            {
                UpdateMinMax(ref x, cube.X);
                UpdateMinMax(ref y, cube.Y);
                UpdateMinMax(ref z, cube.Z);
            }

            // Update the borders to add a +1
            // This forces a check to extra cubes which may become active
            x.Min--;
            x.Max++;
            y.Min--;
            y.Max++;
            z.Min--;
            z.Max++;

            return (x, y, z);
        }

        private void UpdateMinMax(ref MinMax minMax, int value)
        {
            if(value < minMax.Min)
            {
                minMax.Min = value;
            }
            if(value > minMax.Max)
            {
                minMax.Max = value;
            }
        }

        private void ParseInput()
        {
            _pocketDimension = base.Input.SplitByNewline()
                .SelectMany((line, y) =>
                    line.Select((cube, x) => (X: x, Y: y, IsActive: cube == '#'))
                )
                .Where(cube => cube.IsActive)
                .Select(cube => (cube.X, cube.Y, Z: 0))
                .ToHashSet();
        }
    }
}
