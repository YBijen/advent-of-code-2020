namespace AdventOfCode.Solutions.Year2020.Day14.Models
{
    public class MaskMemoryModel
    {
        public (char Character, int Index)[] Mask { get; set; }
        public (int Index, long Value)[] MemoryAdjustements { get; set; }
    }
}
