using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AdventOfCode.Solutions.Year2020.Day20.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Solutions.Year2020
{
    class Day20Solution : ASolution
    {
        private const int LENGTH_INPUT_IMAGE = 10;
        private const int LENGTH_SEA_MONSTER = 20;
        private const int HEIGHT_SEA_MONSTER = 3;

        private readonly List<(int X, int Y)> _seaMonsterCoords = new List<(int X, int Y)>
        {
            (0, -1),
            (1, -2),
            (4, -2),
            (5, -1),
            (6, -1),
            (7, -2),
            (10, -2),
            (11, -1),
            (12, -1),
            (13, -2),
            (16, -2),
            (17, -1),
            (18, 0),
            (18, -1),
            (19, -1)
        };

        private readonly int _lengthFullImage;
        private readonly int _lengthAllImages;

        private readonly Dictionary<int, (List<char> Standard, List<char> Flipped)> _images = new Dictionary<int, (List<char> Standard, List<char> Flipped)>();
        private readonly List<(int ImageId, List<char> Image)> _fullImage = new List<(int, List<char>)>();
        private readonly List<char> _trimmedFullImage = new List<char>();

        public Day20Solution() : base(20, 2020, "")
        {
            //SetDebugInput();
            ParseInput();

            _lengthAllImages = (int)Math.Sqrt(_images.Count);

            // Set the Full Image Length by removing the borders
            _lengthFullImage = (LENGTH_INPUT_IMAGE * _lengthAllImages) - (_lengthAllImages * 2);

            if (base.DebugInput == null)
            {
                Assert.AreEqual(2699020245973.ToString(), SolvePartOne());
                _fullImage.Clear();
                _trimmedFullImage.Clear();
                Assert.AreEqual(2012.ToString(), SolvePartTwo());
            }
            else
            {
                Assert.AreEqual(20899048083289.ToString(), SolvePartOne());
                _fullImage.Clear();
                _trimmedFullImage.Clear();
                Assert.AreEqual(273.ToString(), SolvePartTwo());
            }

            _fullImage.Clear();
            _trimmedFullImage.Clear();
        }

        protected override string SolvePartOne() => FindSolvesForEachImage()
                .Where(kvp => kvp.Value.Count == 2)
                .Select(kvp => (long)kvp.Key)
                .Aggregate(1L, (v1, v2) => v1 * v2)
                .ToString();

        protected override string SolvePartTwo()
        {
            FillFullImage();
            TrimFullImage();
            var amountOfSeaMonsters = CountSeaMonsters();
            return (_trimmedFullImage.Count(tfi => tfi == '#') - (amountOfSeaMonsters * _seaMonsterCoords.Count)).ToString();

        }

        private int CountSeaMonsters()
        {
            // Go through all rotations to find the Sea Monster
            // if none are found then flip the _trimmedFullImage, for me it was not needed
            for (var r = 0; r < 360; r += 90)
            {
                var count = CountSeaMonsterInRotation(r, false);
                if(count > 0)
                {
                    return count;
                }
            }

            throw new Exception("SeaMonsters are not found yet, try flipping the image");
        }

        private int CountSeaMonsterInRotation(int rotation, bool flipped)
        {
            var seaMonsterCount = 0;
            for (var y = 0; y < (_lengthFullImage - HEIGHT_SEA_MONSTER + 1); y++)
            {
                for (var x = 0; x < (_lengthFullImage - LENGTH_SEA_MONSTER + 1); x++)
                {
                    var isSeaMonster = true;
                    foreach (var smc in _seaMonsterCoords)
                    {
                        var result = _trimmedFullImage[CalcIndexForFullImageRotation(rotation, x + smc.X, y + Math.Abs(smc.Y))];
                        if (result != '#')
                        {
                            isSeaMonster = false;
                            break;
                        }
                    }
                    if (isSeaMonster)
                    {
                        seaMonsterCount++;
                    }
                }
            }
            return seaMonsterCount;
        }

        private void TrimFullImage()
        {
            var fullImages = _fullImage.Select(fi => fi.Image).ToList();
            for (var y = 1; y < _lengthAllImages * LENGTH_INPUT_IMAGE; y++)
            {
                var listIndexY = y / LENGTH_INPUT_IMAGE;
                var listY = y % LENGTH_INPUT_IMAGE;

                if (listY == 0 || listY == LENGTH_INPUT_IMAGE - 1)
                {
                    continue;
                }

                for (var x = 0; x < _lengthAllImages * LENGTH_INPUT_IMAGE; x++)
                {
                    var listIndexX = x / LENGTH_INPUT_IMAGE;
                    var listX = x % LENGTH_INPUT_IMAGE;

                    if (listX == 0 || listX == LENGTH_INPUT_IMAGE - 1)
                    {
                        continue;
                    }

                    var c = fullImages[(listIndexY * _lengthAllImages + listIndexX)][CalcIndexForRotation(0, listX, listY)];
                    _trimmedFullImage.Add(c);
                }
            }
        }

        private void FillFullImage()
        {
            var imageSolves = FindSolvesForEachImage();

            // Initialize full image
            var topLeftImageId = FindTopLeftStartingTile(imageSolves);
            _fullImage.Add((topLeftImageId, _images[topLeftImageId].Standard));

            // Prepare a list of Image Ids which are solved
            var addedImages = new List<int>();

            for (var i = 1; i < (_lengthAllImages * _lengthAllImages); i++)
            {
                addedImages.Add(_fullImage.Last().ImageId);

                int previousImageId;
                string borderToFind;
                Border borderToTake;

                // If the next image to add is on a new y
                if (i % _lengthAllImages == 0)
                {
                    var (imageId, image) = _fullImage[i - _lengthAllImages];
                    previousImageId = imageId;
                    borderToFind = GetBorderForImage(image, Border.Bottom);
                    borderToTake = Border.Top;

                }
                // If the next image to add is on the same y
                else
                {
                    var (imageId, image) = _fullImage.Last();
                    previousImageId = imageId;
                    borderToFind = GetBorderForImage(image, Border.Right);
                    borderToTake = Border.Left;
                }

                var foundImage = false;
                foreach(var possibleImage in imageSolves[previousImageId].Where(i => !addedImages.Contains(i.ImageId)))
                {
                    for (var r = 0; r < 360; r += 90)
                    {
                        var standardImage = RotateImage(_images[possibleImage.ImageId].Standard, r);
                        if (borderToFind == GetBorderForImage(standardImage, borderToTake))
                        {
                            _fullImage.Add((possibleImage.ImageId, standardImage));
                            foundImage = true;
                            break;
                        }

                        var flippedImage = RotateImage(_images[possibleImage.ImageId].Flipped, r);
                        if (borderToFind == GetBorderForImage(flippedImage, borderToTake))
                        {
                            _fullImage.Add((possibleImage.ImageId, flippedImage));
                            foundImage = true;
                            break;
                        }
                    }

                    if(foundImage)
                    {
                        break;
                    }
                }

                if(!foundImage)
                {
                    throw new Exception($"Something is going wrong in deciding the correct rotation for the current image.");
                }
            }
        }

        /// <summary>
        /// Find the starting tile, we're looking for a tile in the top-left corner
        /// </summary>
        private int FindTopLeftStartingTile(Dictionary<int, List<ImageMatch>> solvables)
        {
            // For the top-left corner we need a solve for rotation 90 and rotation 180
            var expectedSolvedRotation = new List<int> { 90, 180 };

            var possibleTopLeftCorner = solvables.Where(kvp => kvp.Value.Count == 2 && kvp.Value.All(value => expectedSolvedRotation.Contains(value.Rotation)));
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
        private Dictionary<int, List<ImageMatch>> FindSolvesForEachImage()
        {
            var solvables = new Dictionary<int, List<ImageMatch>>();
            foreach(var baseImage in _images)
            {
                solvables.Add(baseImage.Key, new List<ImageMatch>());

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
                        var matchedAtRotation = 0;
                        foreach(var topRow in baseTileTopRows)
                        {
                            if(topRow == GetTopRowForRotatedImage(compareTile.Value.Standard, r))
                            {
                                solvables[baseImage.Key].Add(new ImageMatch(compareTile.Key, false, r, matchedAtRotation));
                            }

                            if (topRow == GetTopRowForRotatedImage(compareTile.Value.Flipped, r))
                            {
                                solvables[baseImage.Key].Add(new ImageMatch(compareTile.Key, false, r, matchedAtRotation));
                            }

                            matchedAtRotation += 90;
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
            0 => y * LENGTH_INPUT_IMAGE + x,
            90 => 90 + y - (x * LENGTH_INPUT_IMAGE),
            180 => 99 - (y * LENGTH_INPUT_IMAGE) - x,
            270 => 9 - y + (x * LENGTH_INPUT_IMAGE),
            _ => throw new Exception("Invalid rotation value: " + r)
        };

        private static List<char> RotateImage(List<char> image, int rotation) =>
            Enumerable.Range(0, image.Count)
            .Select(x => image[CalcIndexForRotation(rotation, x % LENGTH_INPUT_IMAGE, x / LENGTH_INPUT_IMAGE)])
            .ToList();

        private static string GetBorderForImage(List<char> image, Border border) => border switch
        {
            Border.Top => string.Join("", image.Take(LENGTH_INPUT_IMAGE)),
            Border.Right => string.Join("", image.Where((c, idx) => idx > 0 && idx % LENGTH_INPUT_IMAGE == (LENGTH_INPUT_IMAGE - 1))),
            Border.Left => string.Join("", image.Where((c, idx) => idx % LENGTH_INPUT_IMAGE == 0)),
            Border.Bottom => string.Join("", image.Skip((LENGTH_INPUT_IMAGE * LENGTH_INPUT_IMAGE) - LENGTH_INPUT_IMAGE)),
            _ => throw new Exception("Invalid border")
        };

        private string GetTopRowForRotatedImage(List<char> image, int rotation) =>
            string.Join("", Enumerable.Range(0, LENGTH_INPUT_IMAGE).Select(x => image[CalcIndexForRotation(rotation, x, 0)]));

        private static List<char> FlipImage(List<char> image)
        {
            var tileFlipped = new List<char>();
            for (var i = (image.Count - LENGTH_INPUT_IMAGE); i >= 0; i -= LENGTH_INPUT_IMAGE)
            {
                tileFlipped.AddRange(image.Skip(i).Take(LENGTH_INPUT_IMAGE));
            }
            return tileFlipped;
        }

        /// <summary>
        /// Calculate the index for an array which contains an x-y grid
        /// For info about the magic numbers see: https://youtu.be/8OK8_tHeCIA
        /// </summary>
        public int CalcIndexForFullImageRotation(int r, int x, int y) => r switch
        {
            0 => y * _lengthFullImage + x,
            90 => (_lengthFullImage * _lengthFullImage - _lengthFullImage) + y - (x * _lengthFullImage),
            180 => (_lengthFullImage * _lengthFullImage - 1) - (y * _lengthFullImage) - x,
            270 => (_lengthFullImage - 1) - y + (x * _lengthFullImage),
            _ => throw new Exception("Invalid rotation value: " + r)
        };

        #region Print Logic
        private static void PrintImage(int key, List<char> image, string description = "")
        {
            Console.WriteLine($"\nTile {key} ({description}):");
            for (var i = 1; i < image.Count + 1; i++)
            {
                Console.Write(image[i - 1]);
                if (i % LENGTH_INPUT_IMAGE == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }

        private void PrintFullImage(List<char> image)
        {
            Console.WriteLine($"== Full Image ==");
            for (var i = 1; i < image.Count + 1; i++)
            {
                Console.Write(image[i - 1]);
                if (i % _lengthFullImage == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();

        }

        private void PrintFullImageWithBorders()
        {
            Console.WriteLine("The full image (including borders):");
            var fullImages = _fullImage.Select(fi => fi.Image).ToList();
            for (var y = 0; y < _lengthAllImages * LENGTH_INPUT_IMAGE; y++)
            {
                var listIndexY = y / LENGTH_INPUT_IMAGE;
                var listY = y % LENGTH_INPUT_IMAGE;
                for (var x = 0; x < _lengthAllImages * LENGTH_INPUT_IMAGE; x++)
                {
                    var listIndexX = x / LENGTH_INPUT_IMAGE;
                    var listX = x % LENGTH_INPUT_IMAGE;
                    Console.Write(fullImages[(listIndexY * _lengthAllImages + listIndexX)][CalcIndexForRotation(0, listX, listY)]);
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region Initialize
        private void ParseInput()
        {
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
        #endregion
    }
}
