using System;

namespace AdventOfCode.Solutions.Year2020.Day24.Models
{
    public class Coordinate : IEquatable<Coordinate>
    {
        public Coordinate() { }

        public Coordinate(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Coordinate(Coordinate coordinate)
        {
            this.X = coordinate.X;
            this.Y = coordinate.Y;
            this.Z = coordinate.Z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public bool Equals(Coordinate other) => other.X == this.X && other.Y == this.Y && other.Z == this.Z;

        public override int GetHashCode()
        {
            return 31 * X + 17 * Y + 334 * Z;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate && Equals((Coordinate)obj);
        }
    }
}
