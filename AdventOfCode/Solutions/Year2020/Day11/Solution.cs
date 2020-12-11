using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day11 : ASolution
    {
        private readonly Dictionary<(int X, int Y), bool> _seats = new Dictionary<(int X, int Y), bool>();

        public Day11() : base(11, 2020, "")
        {
            //SetDebugInput();
            FillSeatsFromInput();
        }

        protected override string SolvePartOne()
        {
            while(true)
            {
                if (!PerformSeatRound())
                {
                    return _seats.Values.Count(v => v).ToString();
                }
            }
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        /// <summary>
        /// Perform a round on the seats, make a copy of the seats to start and use that one to check if seats are occupied or not
        /// </summary>
        /// <returns>True if anything changed, otherwise false</returns>
        private bool PerformSeatRound()
        {
            var startingSeats = new Dictionary<(int X, int Y), bool>(_seats);

            foreach(var currentSeat in startingSeats)
            {
                var occupiedSeatsCount = CountNearbyOccupiedSeats(startingSeats, currentSeat.Key.X, currentSeat.Key.Y);
                if(occupiedSeatsCount >= 4)
                {
                    _seats[currentSeat.Key] = false;
                }
                else if(occupiedSeatsCount == 0)
                {
                    _seats[currentSeat.Key] = true;
                }
            }

            return !startingSeats.Values.SequenceEqual(_seats.Values);
        }

        private int CountNearbyOccupiedSeats(Dictionary<(int X, int Y), bool> seats, int x, int y)
        {
            var occupiedSeats = 0;
            for(var yModifier = -1; yModifier <= 1; yModifier++)
            {
                for (var xModifier = -1; xModifier <= 1; xModifier++)
                {
                    if(yModifier == 0 && xModifier == 0)
                    {
                        continue;
                    }

                    var seatToCheck = (x + xModifier, y + yModifier);
                    if(seats.ContainsKey(seatToCheck) && seats[seatToCheck])
                    {
                        occupiedSeats++;
                    }
                }
            }
            return occupiedSeats;
        }

        private void FillSeatsFromInput()
        {
            var splitInput = base.Input.SplitByNewline();
            for(var y = 0; y < splitInput.Length; y++)
            {
                for(var x = 0; x < splitInput[y].Length; x++)
                {
                    if(splitInput[y][x] == 'L')
                    {
                        _seats.Add((x, y), false);
                    }
                }
            }
        }

        private void SetDebugInput()
        {
            base.DebugInput = "" +
                "L.LL.LL.LL\n" +
                "LLLLLLL.LL\n" +
                "L.L.L..L..\n" +
                "LLLL.LL.LL\n" +
                "L.LL.LL.LL\n" +
                "L.LLLLL.LL\n" +
                "..L.L.....\n" +
                "LLLLLLLLLL\n" +
                "L.LLLLLL.L\n" +
                "L.LLLLL.LL";
        }
    }
}
