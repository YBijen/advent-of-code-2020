using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Year2020.Day14.Models;

namespace AdventOfCode.Solutions.Year2020
{
    class Day14Solution : ASolution
    {
        private const int BIT_LENGTH = 36;
        private List<MaskMemoryModel> _program;
        private long[] _memory;

        public Day14Solution() : base(14, 2020, "")
        {
            //SetDebugInput();
            _program = ParseInput().ToList();

            var maxMemoryValue = _program.SelectMany(p => p.MemoryAdjustements.Select(ma => ma.Index)).Max();
            _memory = new long[maxMemoryValue + 1];
        }

        protected override string SolvePartOne()
        {
            foreach(var p in _program)
            {
                foreach(var memoryAdjustement in p.MemoryAdjustements)
                {
                    var strValue = Convert.ToString(memoryAdjustement.Value, 2).PadLeft(BIT_LENGTH, '0');
                    foreach(var m in p.Mask)
                    {
                        if(m.Character == 'X')
                        {
                            continue;
                        }

                        strValue = new StringBuilder(strValue).Replace(strValue[m.Index], m.Character, m.Index, 1).ToString();
                    }
                    var intValue = Convert.ToInt64(strValue, 2);
                    _memory[memoryAdjustement.Index] = intValue;
                }
            }

            return _memory.Sum().ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private IEnumerable<MaskMemoryModel> ParseInput()
        {
            var programs = base.Input.Split("mask = ", StringSplitOptions.RemoveEmptyEntries);
            foreach(var program in programs)
            {
                var p = program.SplitByNewline();
                yield return new MaskMemoryModel
                {
                    Mask = p[0].Select((character, index) => (character, index)).ToArray(),
                    MemoryAdjustements = p.Skip(1).Select(ProcessMemoryAssignment).ToArray()
                };
            }
        }

        private (int Index, long Value) ProcessMemoryAssignment(string memoryAssignment)
        {
            var splitted = memoryAssignment.Split(" = ");
            var indexString = splitted[0].Replace("mem[", "").Replace("]", "");
            return (int.Parse(indexString), long.Parse(splitted[1]));
        }

        private void SetDebugInput()
        {
            base.DebugInput = "" +
                "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X\n" +
                "mem[8] = 11\n" +
                "mem[7] = 101\n" +
                "mem[8] = 0";
        }
    }
}
