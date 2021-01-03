using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Year2020.Day24.Models;

namespace AdventOfCode.Solutions.Year2020
{
    class Day24Solution : ASolution
    {
        public Day24Solution() : base(24, 2020, "")
        {
            //SetDebugInput();
        }

        protected override string SolvePartOne()
        {
            var flippedTiles = FlipTiles(ParseInput());
            var blackTiles = flippedTiles.Count(tile => tile.Value % 2 != 0);
            return blackTiles.ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private Dictionary<(int X, int Y, int Z), int> FlipTiles(List<List<Direction>> tilePaths)
        {
            var result = new Dictionary<(int X, int Y, int Z), int>();
            foreach(var tilePathDirections in tilePaths)
            {
                var currentTile = new Coordinate() { X = 3, Y = 3, Z = 3 };
                foreach(var direction in tilePathDirections)
                {
                    switch(direction)
                    {
                        case Direction.NorthEast:
                            currentTile.X += 1;
                            currentTile.Z -= 1;
                            break;
                        case Direction.East:
                            currentTile.X += 1;
                            currentTile.Y -= 1;
                            break;
                        case Direction.SouthEast:
                            currentTile.Z += 1;
                            currentTile.Y -= 1;
                            break;
                        case Direction.SouthWest:
                            currentTile.X -= 1;
                            currentTile.Z += 1;
                            break;
                        case Direction.West:
                            currentTile.X -= 1;
                            currentTile.Y += 1;
                            break;
                        case Direction.NorthWest:
                            currentTile.Z -= 1;
                            currentTile.Y += 1;
                            break;
                        default:
                            throw new Exception("This should not happen.");
                    }
                    //Console.WriteLine($"Current Tile: {currentTile.X},{currentTile.Y},{currentTile.Z}. After Direction: {direction.ToString()}");
                }
                var foundTile = (currentTile.X, currentTile.Y, currentTile.Z);

                if (result.ContainsKey(foundTile))
                {
                    result[foundTile]++;
                }
                else
                {
                    result.Add(foundTile, 1);
                }
            }
            return result;
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
