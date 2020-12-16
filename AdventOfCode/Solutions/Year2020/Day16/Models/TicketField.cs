namespace AdventOfCode.Solutions.Year2020.Day16.Models
{
    public class TicketField
    {
        public string Name { get; set; }
        public (int Min, int Max) Range1 { get; set; }
        public (int Min, int Max) Range2 { get; set; }
    }
}
