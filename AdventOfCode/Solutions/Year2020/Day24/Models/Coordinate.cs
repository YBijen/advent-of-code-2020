namespace AdventOfCode.Solutions.Year2020.Day24.Models
{
    public class Coordinate
    {
        public Coordinate()
        {

        }

        public Coordinate((int X, int Y, int Z) tupleCoords)
        {
            this.X = tupleCoords.X;
            this.Y = tupleCoords.Y;
            this.Z = tupleCoords.Z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}
