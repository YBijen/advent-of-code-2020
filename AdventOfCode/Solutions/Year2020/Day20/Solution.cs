using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Solutions.Year2020
{
    class Day20 : ASolution
    {
        private const int LENGTH_IMAGE = 10;
        private readonly int _lengthAllImages;

        private readonly Dictionary<int, (List<char> Standard, List<char> Flipped)> _images = new Dictionary<int, (List<char> Standard, List<char> Flipped)>();
        private readonly List<(int ImageId, List<char> Image)> _fullImage = new List<(int, List<char>)>();

        public Day20() : base(20, 2020, "")
        {
            SetDebugInput();
            ParseInput();

            _lengthAllImages = base.DebugInput == null ? 12 : 3; // TODO: Resolve by code

            if (base.DebugInput == null)
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
            FillFullImage();
            return null;
        }

        private void FillFullImage()
        {
            var imageSolves = FindSolvesForEachImage();

            // Initialize full image
            var topLeftImageId = FindTopLeftStartingTile(imageSolves);
            _fullImage.Add((topLeftImageId, _images[topLeftImageId].Standard));

            for (var i = 1; i < (_lengthAllImages * _lengthAllImages); i++)
            {
                ImageMatch previousImageSolve;
                string borderToFind;
                int toRotate = 0;
                Border borderToTake;

                // If the next image to add is on a new y
                if (i % _lengthAllImages == 0)
                {
                    var (imageId, image) = _fullImage[i - _lengthAllImages];
                    borderToFind = GetBorderForImage(image, Border.Bottom);
                    previousImageSolve = imageSolves[imageId].Find(i => i.MatchedAtRotation == 180);
                    borderToTake = Border.Top;

                    // Find the value to which the image should be rotated
                    toRotate = Math.Abs(180 - previousImageSolve.Rotation);

                }
                // If the next image to add is on the same y
                else
                {
                    var (imageId, image) = _fullImage.Last();
                    borderToFind = GetBorderForImage(image, Border.Right);
                    previousImageSolve = imageSolves[imageId].Find(i => i.MatchedAtRotation == 270);
                    borderToTake = Border.Left;

                    // Find the value to which the image should be rotated
                    toRotate = Math.Abs(90 - previousImageSolve.Rotation);
                }

                var standardImage = RotateImage(_images[previousImageSolve.ImageId].Standard, toRotate);
                if (borderToFind == GetBorderForImage(standardImage, borderToTake))
                {
                    _fullImage.Add((previousImageSolve.ImageId, standardImage));
                    continue;
                }

                var flippedImage = RotateImage(_images[previousImageSolve.ImageId].Flipped, toRotate);
                if (borderToFind == GetBorderForImage(flippedImage, borderToTake))
                {
                    _fullImage.Add((previousImageSolve.ImageId, flippedImage));
                    continue;
                }

                throw new Exception($"Something is going wrong in deciding the correct rotation for the current image.");
            }

            PrintFullImage();
        }

        /// <summary>
        /// Find the starting tile, we're looking for a tile in the top-left corner
        /// </summary>
        private int FindTopLeftStartingTile(Dictionary<int, List<ImageMatch>> solvables)
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
            0 => y * LENGTH_IMAGE + x,
            90 => 90 + y - (x * LENGTH_IMAGE),
            180 => 99 - (y * LENGTH_IMAGE) - x,
            270 => 9 - y + (x * LENGTH_IMAGE),
            _ => throw new Exception("Invalid rotation value: " + r)
        };

        private static List<char> RotateImage(List<char> image, int rotation) =>
            Enumerable.Range(0, image.Count)
            .Select(x => image[CalcIndexForRotation(rotation, x % LENGTH_IMAGE, x / LENGTH_IMAGE)])
            .ToList();

        private static string GetBorderForImage(List<char> image, Border border) => border switch
        {
            Border.Top => string.Join("", image.Take(LENGTH_IMAGE)),
            Border.Right => string.Join("", image.Where((c, idx) => idx > 0 && idx % LENGTH_IMAGE == (LENGTH_IMAGE - 1))),
            Border.Left => string.Join("", image.Where((c, idx) => idx % LENGTH_IMAGE == 0)),
            Border.Bottom => string.Join("", image.Skip((LENGTH_IMAGE * LENGTH_IMAGE) - LENGTH_IMAGE)),
            _ => throw new Exception("Invalid border")
        };

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

        private void PrintFullImage()
        {
            Console.WriteLine("The full image (including borders):");
            var fullImages = _fullImage.Select(fi => fi.Image).ToList();
            for (var y = 0; y < _lengthAllImages * LENGTH_IMAGE; y++)
            {
                var listIndexY = y / LENGTH_IMAGE;
                var listY = y % LENGTH_IMAGE;
                for (var x = 0; x < _lengthAllImages * LENGTH_IMAGE; x++)
                {
                    var listIndexX = x / LENGTH_IMAGE;
                    var listX = x % LENGTH_IMAGE;
                    Console.Write(fullImages[(listIndexY * _lengthAllImages + listIndexX)][CalcIndexForRotation(0, listX, listY)]);
                }
                Console.WriteLine();
            }
        }

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
    }

    public class ImageMatch
    {
        public ImageMatch(int imageId, bool isFlipped, int rotation, int matchedAtRotation)
        {
            ImageId = imageId;
            IsFlipped = isFlipped;
            Rotation = rotation;
            MatchedAtRotation = matchedAtRotation;
        }

        public int ImageId { get; set; }
        public bool IsFlipped { get; set; }
        public int Rotation { get; set; }
        public int MatchedAtRotation { get; set; }
    }

    public enum Border
    {
        Top,
        Right,
        Bottom,
        Left
    }
}
