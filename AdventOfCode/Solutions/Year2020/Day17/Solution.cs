using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Year2020.Day17.Models;

namespace AdventOfCode.Solutions.Year2020
{
    class Day17Solution : ASolution
    {
        private const int CYCLES = 6;
        private HashSet<(int X, int Y, int Z)> _pocketDimensionCubes;
        private HashSet<(int X, int Y, int Z, int W)> _pocketDimensionHyperCubes;

        public Day17Solution() : base(17, 2020, "")
        {
            //base.DebugInput = ".#.\n..#\n###";
            ParseInput();
        }

        protected override string SolvePartOne()
        {
            HashSet<(int X, int Y, int Z)> temporaryPocketDimension = new HashSet<(int X, int Y, int Z)>();
            for (var i = 0; i < CYCLES; i++)
            {
                temporaryPocketDimension.Clear();
                var borders = GetCubeBorders();
                for (var x = borders.X.Min; x <= borders.X.Max; x++)
                {
                    for (var y = borders.Y.Min; y <= borders.Y.Max; y++)
                    {
                        for (var z = borders.Z.Min; z <= borders.Z.Max; z++)
                        {
                            var currentCube = (x, y, z);
                            var activeNeighbourCount = CountActiveNeighboursForCube(currentCube);
                            if (activeNeighbourCount == 3)
                            {
                                temporaryPocketDimension.Add(currentCube);
                            }
                            else if (activeNeighbourCount == 2 && _pocketDimensionCubes.Contains(currentCube))
                            {
                                temporaryPocketDimension.Add(currentCube);
                            }
                        }
                    }
                }
                _pocketDimensionCubes = new HashSet<(int X, int Y, int Z)>(temporaryPocketDimension);
            }
            return _pocketDimensionCubes.Count.ToString();
        }

        private int CountActiveNeighboursForCube((int X, int Y, int Z) cube)
        {
            var count = 0;
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    for (var z = -1; z <= 1; z++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                        {
                            continue;
                        }

                        var cubeToCheck = (cube.X + x, cube.Y + y, cube.Z + z);
                        if (_pocketDimensionCubes.Contains(cubeToCheck))
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        private (MinMax X, MinMax Y, MinMax Z) GetCubeBorders()
        {
            MinMax x = new MinMax(), y = new MinMax(), z = new MinMax();
            foreach (var cube in _pocketDimensionCubes)
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

        protected override string SolvePartTwo()
        {
            HashSet<(int X, int Y, int Z, int W)> temporaryPocketDimension = new HashSet<(int X, int Y, int Z, int W)>();
            for (var i = 0; i < CYCLES; i++)
            {
                temporaryPocketDimension.Clear();
                var borders = GetHyperCubeBorders();
                for (var x = borders.X.Min; x <= borders.X.Max; x++)
                {
                    for (var y = borders.Y.Min; y <= borders.Y.Max; y++)
                    {
                        for (var z = borders.Z.Min; z <= borders.Z.Max; z++)
                        {
                            for (var w = borders.W.Min; w <= borders.W.Max; w++)
                            {
                                var currentCube = (x, y, z, w);

                                var activeNeighbourCount = CountActiveNeighboursForHyperCube(currentCube);
                                if (activeNeighbourCount == 3)
                                {
                                    temporaryPocketDimension.Add(currentCube);
                                }
                                else if (activeNeighbourCount == 2 && _pocketDimensionHyperCubes.Contains(currentCube))
                                {
                                    temporaryPocketDimension.Add(currentCube);
                                }
                            }
                        }
                    }
                }
                _pocketDimensionHyperCubes = new HashSet<(int X, int Y, int Z, int W)>(temporaryPocketDimension);
            }
            return _pocketDimensionHyperCubes.Count.ToString();
        }

        private int CountActiveNeighboursForHyperCube((int X, int Y, int Z, int W) cube)
        {
            var count = 0;
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    for (var z = -1; z <= 1; z++)
                    {
                        for (var w = -1; w <= 1; w++)
                        {
                            if (x == 0 && y == 0 && z == 0 && w == 0)
                            {
                                continue;
                            }

                            var cubeToCheck = (cube.X + x, cube.Y + y, cube.Z + z, cube.W + w);
                            if (_pocketDimensionHyperCubes.Contains(cubeToCheck))
                            {
                                count++;
                            }
                        }
                    }
                }
            }
            return count;
        }

        private (MinMax X, MinMax Y, MinMax Z, MinMax W) GetHyperCubeBorders()
        {
            MinMax x = new MinMax(), y = new MinMax(), z = new MinMax(), w = new MinMax();
            foreach (var cube in _pocketDimensionHyperCubes)
            {
                UpdateMinMax(ref x, cube.X);
                UpdateMinMax(ref y, cube.Y);
                UpdateMinMax(ref z, cube.Z);
                UpdateMinMax(ref w, cube.W);
            }

            // Update the borders to add a +1
            // This forces a check to extra cubes which may become active
            x.Min--;
            x.Max++;
            y.Min--;
            y.Max++;
            z.Min--;
            z.Max++;
            w.Min--;
            w.Max++;

            return (x, y, z, w);
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
            _pocketDimensionCubes = base.Input.SplitByNewline()
                .SelectMany((line, y) =>
                    line.Select((cube, x) => (X: x, Y: y, IsActive: cube == '#'))
                )
                .Where(cube => cube.IsActive)
                .Select(cube => (cube.X, cube.Y, Z: 0))
                .ToHashSet();

            _pocketDimensionHyperCubes = _pocketDimensionCubes.Select(cube => (cube.X, cube.Y, cube.Z, 0)).ToHashSet();
        }
    }
}
