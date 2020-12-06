using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day05 : ASolution
    {
        private const int INDEX_LAST_ROW = 6;
        private const int INDEX_LAST_COLUMN = 9;

        public Day05() : base(05, 2020, "")
        {
            //base.DebugInput = "FBFBBFFRLR\nBFFFBBFRRR\nFFFBBBFRRR\nBBFFBBFRLL";
        }

        protected override string SolvePartOne() => base.Input.SplitByNewline().Max(CalculateSeatInfoForBoardingPass).SeatId.ToString();

        private (int Row, int Column, int SeatId) CalculateSeatInfoForBoardingPass(string boardingPass)
        {
            var row = 0;
            var rowMin = 0;
            var rowMax = 127;
            var column = 0;
            var columnMin = 0;
            var columnMax = 7;

            for (var i = 0; i < boardingPass.Length; i++)
            {
                if (i <= INDEX_LAST_ROW)
                {
                    (rowMin, rowMax) = CalculateNewRange(rowMin, rowMax, boardingPass[i]);
                    if (i == INDEX_LAST_ROW && boardingPass[i] == 'F')
                    {
                        row = rowMin;
                    }
                    else if (i == INDEX_LAST_ROW && boardingPass[i] == 'B')
                    {
                        row = rowMax;
                    }
                }
                else if (i <= INDEX_LAST_COLUMN)
                {
                    (columnMin, columnMax) = CalculateNewRange(columnMin, columnMax, boardingPass[i]);
                    if (i == INDEX_LAST_COLUMN && boardingPass[i] == 'L')
                    {
                        column = columnMin;
                    }
                    else if (i == INDEX_LAST_COLUMN && boardingPass[i] == 'R')
                    {
                        column = columnMax;
                    }
                }
            }

            var seatId = row * 8 + column;
            //Console.WriteLine($"Row: {row} && Column: {column} && SeatId: {seatId}");
            return (row, column, seatId);
        }

        private (int min, int max) CalculateNewRange(int min, int max, char input)
        {
            var rangeModifier = (max - min) / 2d;

            return input switch
            {
                'F' or 'L' => (min, (int)Math.Floor(max - rangeModifier)),
                'B' or 'R' => ((int)Math.Ceiling(min + rangeModifier), max),
                _ => throw new Exception($"The input \"{input}\" is not supported (yet).")
            };
        }

        protected override string SolvePartTwo()
        {
            var rowWithOneEmptySeat = base.Input.SplitByNewline().Select(CalculateSeatInfoForBoardingPass)
                .GroupBy(info => info.Row)
                .FirstOrDefault(rowGroups => rowGroups.Count() == 7);
            foreach(var column in Enumerable.Range(0, 8))
            {
                var hasSeatWithColumn = rowWithOneEmptySeat.Any(info => info.Column == column);
                if(hasSeatWithColumn)
                {
                    continue;
                }

                return (rowWithOneEmptySeat.Key * 8 + column).ToString();
            }

            throw new Exception("Could not find my seat.");
        }
    }
}