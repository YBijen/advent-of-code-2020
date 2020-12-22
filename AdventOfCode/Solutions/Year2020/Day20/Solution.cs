using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Solutions.Year2020
{
    class Day20 : ASolution
    {
        private const char EMPTY = '.';
        private const char FILLED = '#';

        private const int LENGTH_IMAGE = 10;
        private Dictionary<int, (List<char> Standard, List<char> Flipped)> _images;
        private readonly Dictionary<int, List<char>> _validLists = new Dictionary<int, List<char>>();
        private readonly Dictionary<(int X, int Y), int> _correctImages = new Dictionary<(int X, int Y), int>();

        private List<(int, List<char>)> _fullImage = new List<(int, List<char>)>();

        public Day20() : base(20, 2020, "")
        {
            SetDebugInput();
            ParseInput();
            
            if(base.DebugInput == null)
            {
                Assert.AreEqual(2699020245973.ToString(), SolvePartOne());
            }
            else
            {
                Assert.AreEqual(20899048083289.ToString(), SolvePartOne());
            }
        }

        protected override string SolvePartOne() => FindSolvesForEachImage()
                .Where(kvp => kvp.Value.Count == 2)
                .Select(kvp => (long)kvp.Key)
                .Aggregate(1L, (v1, v2) => v1 * v2)
                .ToString();

        protected override string SolvePartTwo()
        {
            var lengthAllImages = base.DebugInput == null ? 12 : 3; // TODO: Resolve by code

            var solvables = FindSolvesForEachImage();

            var topLeftKey = FindTopLeftStartingTile(solvables);
            _correctImages.Add((0, 0), topLeftKey);

            _fullImage.Add((topLeftKey, _images[topLeftKey].Standard));

            // TODO: Update the 1 after the first row works
            for(var y = 0; y < 1; y++)
            {
                for(var x = 0; x < lengthAllImages; x++)
                {
                    // Skip top left, because we have it already
                    if(x == 0 && x == 0)
                    {
                        continue;
                    }

                    // Look for y - 1 for the correct image to check
                    if(x == 0)
                    {
                        // Not needed yet
                    }

                    var lastImage = _correctImages.Last();
                    var solves = solvables[lastImage.Value];

                    if(solves.Any(s => s.Rotation == 270)) // TODO: Improve
                    {
                        var correctSolve = solves.Find(s => s.Rotation == 270);
                        if(correctSolve.IsFlipped)
                        {
                            _fullImage.Add((correctSolve.SolvableByImage, RotateImage(_images[correctSolve.SolvableByImage].Flipped, correctSolve.Rotation)));
                        }
                        else
                        {
                            _fullImage.Add((correctSolve.SolvableByImage, RotateImage(_images[correctSolve.SolvableByImage].Standard, correctSolve.Rotation)));
                        }
                        _correctImages.Add((x, y), correctSolve.SolvableByImage);
                    }
                    else
                    {
                        var correctSolve = solves.Find(s => s.Rotation == 90);
                        if (correctSolve.IsFlipped)
                        {
                            _fullImage.Add((correctSolve.SolvableByImage, RotateImage(_images[correctSolve.SolvableByImage].Standard, correctSolve.Rotation)));
                        }
                        else
                        {
                            _fullImage.Add((correctSolve.SolvableByImage, RotateImage(_images[correctSolve.SolvableByImage].Flipped, correctSolve.Rotation)));
                        }
                        _correctImages.Add((x, y), correctSolve.SolvableByImage);

                    }
                }
            }

            foreach(var x in _fullImage)
            {
                PrintImage(x.Item1, x.Item2);
            }

            return null;
        }

        /// <summary>
        /// Find the starting tile, we're looking for a tile in the top-left corner
        /// </summary>
        private int FindTopLeftStartingTile(Dictionary<int, List<(int SolvableByImage, int Rotation, bool IsFlipped)>> solvables)
        {
            // For the top-left corner we need a solve for rotation 90 and rotation 180
            var expectedSolvedRotation = new List<int> { 90, 180 };

            var possibleTopLeftCorner = solvables.Where(kvp => kvp.Value.All(value => expectedSolvedRotation.Contains(value.Rotation)));
            if (possibleTopLeftCorner.Count() != 1)
            {
                throw new Exception("Your input needs further processing");
            }

            return possibleTopLeftCorner.First().Key;
        }

        /// <summary>
        /// For each image, count how many times it can be solved
        /// Returns a list of the possible solves with a rotation
        /// </summary>
        private Dictionary<int, List<(int SolvableByImage, int Rotation, bool IsFlipped)>> FindSolvesForEachImage()
        {
            var solvables = new Dictionary<int, List<(int SolvableByImage, int Rotation, bool IsFlipped)>>();
            foreach(var baseImage in _images)
            {
                solvables.Add(baseImage.Key, new List<(int SolvableByImage, int Rotation, bool IsFlipped)>());

                var baseTileTopRows = new List<string>();
                for (var r = 0; r <= 270; r += 90)
                {
                    baseTileTopRows.Add(GetTopRowForRotatedImage(baseImage.Value.Standard, r));
                }

                foreach (var compareTile in _images)
                {
                    if(baseImage.Key == compareTile.Key)
                    {
                        continue;
                    }

                    for(var r = 0; r <= 270; r += 90)
                    {
                        foreach(var topRow in baseTileTopRows)
                        {
                            if(topRow == GetTopRowForRotatedImage(compareTile.Value.Standard, r))
                            {
                                solvables[baseImage.Key].Add((compareTile.Key, r, false));
                            }

                            if (topRow == GetTopRowForRotatedImage(compareTile.Value.Flipped, r))
                            {
                                solvables[baseImage.Key].Add((compareTile.Key, r, true));
                            }
                        }
                    }
                }
            }

            return solvables;
        }

        /// <summary>
        /// Calculate the index for an array which contains an x-y grid
        /// For info about the magic numbers see: https://youtu.be/8OK8_tHeCIA
        /// </summary>
        public static int CalcIndexForRotation(int r, int x, int y) => r switch
        {
            0 => y * LENGTH_IMAGE + x,
            90 => 90 + y - (x * LENGTH_IMAGE),
            180 => 99 - (y * LENGTH_IMAGE) - x,
            270 => 9 - y + (x * LENGTH_IMAGE),
            _ => throw new Exception("Invalid rotation value: " + r)
        };

        private List<char> RotateImage(List<char> image, int rotation)
        {
            return Enumerable.Range(0, image.Count).Select(x => image[CalcIndexForRotation(rotation, x % LENGTH_IMAGE, x / LENGTH_IMAGE)]).ToList();
        }

        private string GetTopRowForRotatedImage(List<char> image, int rotation) =>
            string.Join("", Enumerable.Range(0, LENGTH_IMAGE).Select(x => image[CalcIndexForRotation(rotation, x, 0)]));

        private static List<char> FlipImage(List<char> image)
        {
            var tileFlipped = new List<char>();
            for (var i = (image.Count - LENGTH_IMAGE); i >= 0; i -= LENGTH_IMAGE)
            {
                tileFlipped.AddRange(image.Skip(i).Take(LENGTH_IMAGE));
            }
            return tileFlipped;
        }

        private static void PrintImage(int key, List<char> image, string description = "")
        {
            Console.WriteLine($"\nTile {key} ({description}):");
            for (var i = 1; i < image.Count + 1; i++)
            {
                Console.Write(image[i - 1]);
                if (i % LENGTH_IMAGE == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }

        private void ParseInput()
        {
            _images = new Dictionary<int, (List<char> Standard, List<char> Flipped)>();
            var splitOn = base.Input.Contains("\r\n\r\n") ? "\r\n\r\n" : "\n\n";
            foreach (var inputImage in base.Input.Split(splitOn))
            {
                var splittedInputImage = inputImage.SplitByNewline();
                var title = int.Parse(splittedInputImage[0].Replace("Tile", "").Replace(":", "").Trim());
                var standardImage = string.Join("", splittedInputImage.Skip(1)).ToCharArray().ToList();
                _images.Add(title, (standardImage, FlipImage(standardImage)));
            }
        }

        private void SetDebugInput()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AdventOfCode.Solutions.Year2020.Day20.debuginput";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            base.DebugInput = reader.ReadToEnd();
        }
    }
}
