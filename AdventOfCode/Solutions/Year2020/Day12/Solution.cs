using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode.Solutions.Year2020.Day12;

namespace AdventOfCode.Solutions.Year2020
{

    class Day12Solution : ASolution
    {
        private List<Direction> _directions = new List<Direction> { Direction.N, Direction.E, Direction.S, Direction.W };
        private List<(Direction Direction, int Value)> _instructions;
        private Direction _currentDirection = Direction.E;
        private (int X, int Y) _currentLocation = (0, 0);
        private (int X, int Y) _waypoint = (10, 1);

        public Day12Solution() : base(12, 2020, "")
        {
            //SetDebugInput();
            ParseInput();
        }

        protected override string SolvePartOne()
        {
            ResetGlobals();
            foreach (var instr in _instructions)
            {
                switch(instr.Direction)
                {
                    case Direction.L:
                    case Direction.R:
                        MakeTurn(instr.Direction, instr.Value);
                        break;
                    case Direction.F:
                        _currentLocation = GetNewCurrentLocation(_currentDirection, instr.Value);
                        break;
                    default:
                        _currentLocation = GetNewCurrentLocation(instr.Direction, instr.Value);
                        break;
                }
            }
            return (Math.Abs(_currentLocation.X) + Math.Abs(_currentLocation.Y)).ToString();
        }

        private (int X, int Y) GetNewCurrentLocation(Direction direction, int value) => direction switch
        {
            Direction.N => (_currentLocation.X, _currentLocation.Y + value),
            Direction.E => (_currentLocation.X + value, _currentLocation.Y),
            Direction.S => (_currentLocation.X, _currentLocation.Y - value),
            Direction.W => (_currentLocation.X - value, _currentLocation.Y),
            _ => throw new Exception("Invalid direction for this method")
        };

        private void MakeTurn(Direction direction, int value)
        {
            var indexModifier = value / 90;
            var currentDirectionIndex = _directions.IndexOf(_currentDirection);

            if(direction == Direction.R)
            {
                var newDirectionIndex = (indexModifier + currentDirectionIndex) % _directions.Count;
                _currentDirection = _directions[newDirectionIndex];
                return;
            }
            else if(direction == Direction.L)
            {
                var newDirectionIndex = currentDirectionIndex - indexModifier;
                if(newDirectionIndex < 0)
                {
                    newDirectionIndex += _directions.Count;
                }
                _currentDirection = _directions[newDirectionIndex];
                return;
            }

            throw new Exception("This should not happen");
        }

        protected override string SolvePartTwo()
        {
            ResetGlobals();
            foreach (var instr in _instructions)
            {
                switch (instr.Direction)
                {
                    case Direction.L:
                    case Direction.R:
                        RotateWaypoint(instr.Direction, instr.Value);
                        break;
                    case Direction.F:
                        _currentLocation = GetNewLocationAfterMovingToWaypoint(instr.Value);
                        break;
                    default:
                        _waypoint = UpdateWaypoint(instr.Direction, instr.Value);
                        break;
                }
            }
            return (Math.Abs(_currentLocation.X) + Math.Abs(_currentLocation.Y)).ToString();
        }

        private (int X, int Y) GetNewLocationAfterMovingToWaypoint(int timesToMove)
        {
            var newX = _currentLocation.X + (_waypoint.X * timesToMove);
            var newY = _currentLocation.Y + (_waypoint.Y * timesToMove);
            return (newX, newY);
        }

        private (int X, int Y) UpdateWaypoint(Direction direction, int value) => direction switch
        {
            Direction.N => (_waypoint.X, _waypoint.Y + value),
            Direction.E => (_waypoint.X + value, _waypoint.Y),
            Direction.S => (_waypoint.X, _waypoint.Y - value),
            Direction.W => (_waypoint.X - value, _waypoint.Y),
            _ => throw new Exception("Invalid direction for this method")
        };

        private void RotateWaypoint(Direction direction, int value)
        {
            if(value == 0)
            {
                return;
            }

            if (direction == Direction.R)
            {
                _waypoint = (_waypoint.Y, _waypoint.X * -1);
            }
            else if (direction == Direction.L)
            {
                _waypoint = (_waypoint.Y * -1, _waypoint.X);
            }

            RotateWaypoint(direction, value - 90);
        }

        private void ParseInput()
        {
            _instructions = new List<(Direction Direction, int Value)>();
            foreach (var line in base.Input.SplitByNewline())
            {
                var dir = Enum.Parse<Direction>(line[0].ToString());
                var val = int.Parse(line.Substring(1));
                _instructions.Add((dir, val));
            }
        }

        private void SetDebugInput()
        {
            base.DebugInput = "F10\nN3\nF7\nR90\nF11";
        }

        private void ResetGlobals()
        {
            _currentDirection = Direction.E;
            _currentLocation = (0, 0);
            _waypoint = (10, 1);
        }
    }
}
