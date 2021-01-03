using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Year2020.Day24.Models;

namespace AdventOfCode.Solutions.Year2020
{
    class Day24Solution : ASolution
    {
        const int DAYS = 100;
        private HashSet<Coordinate> _flippedTiles;

        public Day24Solution() : base(24, 2020, "")
        {
            SetDebugInput();
        }

        protected override string SolvePartOne() => GetFlippedTiles(ParseInput()).Count.ToString();

        protected override string SolvePartTwo()
        {
            _flippedTiles = GetFlippedTiles(ParseInput()).ToHashSet();
            for (var i = 1; i <= DAYS; i++)
            {
                var (minimum, maximum) = GetFloorBorders();
                var newFlippedTiles = new List<Coordinate>(_flippedTiles);
                for (var x = minimum.X; x <= maximum.X; x++)
                {
                    for (var y = minimum.Y; y <= maximum.Y; y++)
                    {
                        for (var z = minimum.Z; z <= maximum.Z; z++)
                        {
                            var currentTile = new Coordinate(x, y, z);
                            var amountOfFlippedNeighbours = CountFlippedNeighbours(currentTile);
                            if(_flippedTiles.Contains(currentTile))
                            {
                                if(amountOfFlippedNeighbours == 0 || amountOfFlippedNeighbours > 2)
                                {
                                    newFlippedTiles.Remove(currentTile);
                                }
                            }
                            else
                            {
                                if(amountOfFlippedNeighbours == 2)
                                {
                                    newFlippedTiles.Add(currentTile);
                                }
                            }
                        }
                    }
                }
                _flippedTiles = new HashSet<Coordinate>(newFlippedTiles);
                Console.WriteLine($"Flipped tiles after day {i}: {_flippedTiles.Count}");
            }
            return _flippedTiles.Count.ToString();
        }

        private int CountFlippedNeighbours(Coordinate tile)
        {
            var flippedNeighbours = 0;

            foreach(var direction in (Direction[])Enum.GetValues(typeof(Direction)))
            {
                var neighbour = GetTileInDirection(direction, tile);
                if(_flippedTiles.Contains(neighbour))
                {
                    flippedNeighbours++;
                }
            }

            return flippedNeighbours;
        }

        private (Coordinate min, Coordinate max) GetFloorBorders()
        {
            Coordinate minimum = new Coordinate(), maximum = new Coordinate();

            foreach(var tile in _flippedTiles)
            {
                if(tile.X < minimum.X)
                {
                    minimum.X = tile.X;
                }
                if (tile.X > maximum.X)
                {
                    maximum.X = tile.X;
                }

                if (tile.Y < minimum.Y)
                {
                    minimum.Y = tile.Y;
                }
                if (tile.Y > maximum.Y)
                {
                    maximum.Y = tile.Y;
                }

                if (tile.Z < minimum.Z)
                {
                    minimum.Z = tile.Z;
                }
                if (tile.Z > maximum.Z)
                {
                    maximum.Z = tile.Z;
                }
            }

            minimum.X--;
            minimum.Y--;
            minimum.Z--;
            maximum.X++;
            maximum.Y++;
            maximum.Z++;

            return (minimum, maximum);
        }

        private List<Coordinate> GetFlippedTiles(List<List<Direction>> tilePaths)
        {
            var result = new Dictionary<Coordinate, int>();
            foreach(var tilePathDirections in tilePaths)
            {
                var currentTile = new Coordinate();
                foreach(var direction in tilePathDirections)
                {
                    currentTile = GetTileInDirection(direction, currentTile);
                }

                if (result.ContainsKey(currentTile))
                {
                    result[currentTile]++;
                }
                else
                {
                    result.Add(currentTile, 1);
                }
            }
            return result.Where(tile => tile.Value % 2 != 0).Select(tile => tile.Key).ToList();
        }

        private Coordinate GetTileInDirection(Direction direction, Coordinate currentTile)
        {
            var nextTile = new Coordinate(currentTile);
            switch (direction)
            {
                case Direction.NorthEast:
                    nextTile.X += 1;
                    nextTile.Z -= 1;
                    break;
                case Direction.East:
                    nextTile.X += 1;
                    nextTile.Y -= 1;
                    break;
                case Direction.SouthEast:
                    nextTile.Z += 1;
                    nextTile.Y -= 1;
                    break;
                case Direction.SouthWest:
                    nextTile.X -= 1;
                    nextTile.Z += 1;
                    break;
                case Direction.West:
                    nextTile.X -= 1;
                    nextTile.Y += 1;
                    break;
                case Direction.NorthWest:
                    nextTile.Z -= 1;
                    nextTile.Y += 1;
                    break;
                default:
                    throw new Exception("This should not happen.");

            }
            return nextTile;
        }

        private List<List<Direction>> ParseInput()
        {
            var parsedTilePathList = new List<List<Direction>>();
            foreach(var tilePath in base.Input.SplitByNewline())
            {
                var parsedTilePath = new List<Direction>();
                for(var i = 0; i < tilePath.Length; i++)
                {
                    if(tilePath[i] == 'e')
                    {
                        parsedTilePath.Add(Direction.East);
                    }
                    else if(tilePath[i] == 'w')
                    {
                        parsedTilePath.Add(Direction.West);
                    }
                    else if(tilePath[i] == 's')
                    {
                        if(tilePath[i+1] == 'e')
                        {
                            parsedTilePath.Add(Direction.SouthEast);
                        }
                        else if (tilePath[i + 1] == 'w')
                        {
                            parsedTilePath.Add(Direction.SouthWest);
                        }
                        i++;
                    }
                    else if (tilePath[i] == 'n')
                    {
                        if (tilePath[i + 1] == 'e')
                        {
                            parsedTilePath.Add(Direction.NorthEast);
                        }
                        else if (tilePath[i + 1] == 'w')
                        {
                            parsedTilePath.Add(Direction.NorthWest);
                        }
                        i++;
                    }
                }
                parsedTilePathList.Add(parsedTilePath);
            }
            return parsedTilePathList;
        }

        private void SetDebugInput()
        {
            base.DebugInput = "sesenwnenenewseeswwswswwnenewsewsw\n" +
                "neeenesenwnwwswnenewnwwsewnenwseswesw\n" +
                "seswneswswsenwwnwse\n" +
                "nwnwneseeswswnenewneswwnewseswneseene\n" +
                "swweswneswnenwsewnwneneseenw\n" +
                "eesenwseswswnenwswnwnwsewwnwsene\n" +
                "sewnenenenesenwsewnenwwwse\n" +
                "wenwwweseeeweswwwnwwe\n" +
                "wsweesenenewnwwnwsenewsenwwsesesenwne\n" +
                "neeswseenwwswnwswswnw\n" +
                "nenwswwsewswnenenewsenwsenwnesesenew\n" +
                "enewnwewneswsewnwswenweswnenwsenwsw\n" +
                "sweneswneswneneenwnewenewwneswswnese\n" +
                "swwesenesewenwneswnwwneseswwne\n" +
                "enesenwswwswneneswsenwnewswseenwsese\n" +
                "wnwnesenesenenwwnenwsewesewsesesew\n" +
                "nenewswnwewswnenesenwnesewesw\n" +
                "eneswnwswnwsenenwnwnwwseeswneewsenese\n" +
                "neswnwewnwnwseenwseesewsenwsweewe\n" +
                "wseweeenwnesenwwwswnew";
        }
    }
}
