using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day11 : ASolution
    {
        private readonly Dictionary<(int X, int Y), bool> _seats = new Dictionary<(int X, int Y), bool>();
        private int _maxX;
        private int _maxY;

        public Day11() : base(11, 2020, "")
        {
            //SetDebugInput();
        }

        protected override string SolvePartOne()
        {
            FillSeatsFromInput();
            while (PerformSeatRound()) { }
            return _seats.Values.Count(v => v).ToString();
        }

        protected override string SolvePartTwo()
        {
            FillSeatsFromInput();
            while (PerformSeatRoundPart2()) { }
            return _seats.Values.Count(v => v).ToString();
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

        private bool PerformSeatRoundPart2()
        {
            var startingSeats = new Dictionary<(int X, int Y), bool>(_seats);

            foreach (var currentSeat in startingSeats)
            {
                var occupiedSeatsCount = CountOccupiedSeatsInSight(startingSeats, currentSeat.Key.X, currentSeat.Key.Y);
                if (occupiedSeatsCount >= 5)
                {
                    _seats[currentSeat.Key] = false;
                }
                else if (occupiedSeatsCount == 0)
                {
                    _seats[currentSeat.Key] = true;
                }
            }

            return !startingSeats.Values.SequenceEqual(_seats.Values);
        }

        private int CountOccupiedSeatsInSight(Dictionary<(int X, int Y), bool> seats, int x, int y)
        {
            var occupiedSeats = 0;
            for (var yModifier = -1; yModifier <= 1; yModifier++)
            {
                for (var xModifier = -1; xModifier <= 1; xModifier++)
                {
                    if (yModifier == 0 && xModifier == 0)
                    {
                        continue;
                    }

                    var seatToCheck = (x + xModifier, y + yModifier);
                    var loop = 1;
                    while(InSeatRange(seatToCheck))
                    {
                        if (seats.ContainsKey(seatToCheck))
                        {
                            // Check if the spotted seat is taken or not
                            if(seats[seatToCheck])
                            {
                                occupiedSeats++;
                            }

                            break;
                        }

                        // This seat would crash the loops by making it infinite
                        // After this seat is checked it would be the end of the range anyway
                        if(seatToCheck == (0, 0))
                        {
                            break;
                        }

                        loop++;
                        seatToCheck = (x + (xModifier * loop), y + (yModifier * loop));
                    }
                }
            }
            return occupiedSeats;
        }

        private bool InSeatRange((int X, int Y) seat) => InSeatRange(seat.X, seat.Y);
        private bool InSeatRange(int x, int y) => x >= 0 && x <= _maxX && y >= 0 && y <= _maxY;

        private void FillSeatsFromInput()
        {
            _seats.Clear();
            var splitInput = base.Input.SplitByNewline();
            for(var y = 0; y < splitInput.Length; y++)
            {
                for(var x = 0; x < splitInput[y].Length; x++)
                {
                    if(splitInput[y][x] == 'L')
                    {
                        _seats.Add((x, y), false);
                    }
                    else if(splitInput[y][x] == '#')
                    {
                        _seats.Add((x, y), true);
                    }
                }
            }

            _maxX = _seats.Keys.Max(key => key.X);
            _maxY = _seats.Keys.Max(key => key.Y);
        }

        private void PrintSeats()
        {
            for(var y = 0; y <= _maxY; y++)
            {
                for(var x = 0; x <= _maxX; x++)
                {
                    if(_seats.ContainsKey((x, y)))
                    {
                        var value = _seats[(x, y)];
                        Console.Write(value ? '#' : 'L');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
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
