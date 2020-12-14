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

        public Day14Solution() : base(14, 2020, "")
        {
            //SetDebugInput();
            //SetDebugInput2();
            _program = ParseInput().ToList();
        }

        protected override string SolvePartOne()
        {
            var maxMemoryValue = _program.SelectMany(p => p.MemoryAdjustements.Select(ma => ma.Index)).Max();
            long[] memory = new long[maxMemoryValue + 1];

            foreach (var p in _program)
            {
                foreach(var memoryAdjustement in p.MemoryAdjustements)
                {
                    var strValue = ConvertIntToBitString(memoryAdjustement.Value);
                    foreach(var m in p.Mask)
                    {
                        if(m.Character == 'X')
                        {
                            continue;
                        }

                        strValue = ReplaceCharInString(strValue, m.Index, m.Character);
                    }
                    var intValue = Convert.ToInt64(strValue, 2);
                    memory[memoryAdjustement.Index] = intValue;
                }
            }

            return memory.Sum().ToString();
        }

        private string ConvertIntToBitString(long value) => Convert.ToString(value, 2).PadLeft(BIT_LENGTH, '0');

        private string ReplaceCharInString(string str, int index, char character) =>
            new StringBuilder(str).Replace(str[index], character, index, 1).ToString();

        protected override string SolvePartTwo()
        {
            var memory = new Dictionary<long, long>();

            foreach (var p in _program)
            {
                foreach (var memoryAdjustement in p.MemoryAdjustements)
                {
                    var strValue = ConvertIntToBitString(memoryAdjustement.Index);
                    foreach (var m in p.Mask)
                    {
                        switch(m.Character)
                        {
                            case '1':
                            case 'X':
                                strValue = ReplaceCharInString(strValue, m.Index, m.Character);
                                break;
                            case '0':
                            default:
                                continue;

                        }
                    }

                    var amountOfFloats = strValue.Count(c => c == 'X');
                    foreach(var replacement in CreatePossibilities(new List<string>(), amountOfFloats))
                    {
                        var adjustAtIndexString = strValue;

                        foreach(var c in replacement)
                        {
                            adjustAtIndexString = ReplaceCharInString(adjustAtIndexString, adjustAtIndexString.IndexOf('X'), c);
                        }

                        var adjustAtIndexLong = Convert.ToInt64(adjustAtIndexString, 2);

                        if(memory.ContainsKey(adjustAtIndexLong))
                        {
                            memory[adjustAtIndexLong] = memoryAdjustement.Value;
                        }
                        else
                        {
                            memory.Add(adjustAtIndexLong, memoryAdjustement.Value);
                        }
                    }
                }
            }

            return memory.Values.Sum().ToString();
        }

        private List<string> CreatePossibilities(List<string> possibilities, int count)
        {
            if(count == 0)
            {
                return possibilities;
            }

            if(possibilities.Count == 0)
            {
                possibilities.Add("0");
                possibilities.Add("1");
                return CreatePossibilities(possibilities, --count);
            }
            else
            {
                var currentPossibilitiesCount = possibilities.Count;
                for(var i = 0; i < currentPossibilitiesCount; i++)
                {
                    possibilities.Add("0" + possibilities[i]);
                    possibilities.Add("1" + possibilities[i]);
                }
                return CreatePossibilities(possibilities.Skip(currentPossibilitiesCount).ToList(), --count);
            }
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

        private void SetDebugInput2()
        {
            base.DebugInput = "" +
                "mask = 000000000000000000000000000000X1001X\n" +
                "mem[42] = 100\n" +
                "mask = 00000000000000000000000000000000X0XX\n" +
                "mem[26] = 1";
        }
    }
}
