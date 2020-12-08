using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Year2020.Day08.Models;

namespace AdventOfCode.Solutions.Year2020
{
    class Day08Solution : ASolution
    {
        private int _accumulator;
        private HashSet<int> _processedIndexes;

        private readonly List<(Operation operation, int modifier)> _bootCode;

        public Day08Solution() : base(08, 2020, "")
        {
            //SetDebugInput();
            _bootCode = base.Input.SplitByNewline().Select(ParseInputLine).ToList();
        }

        protected override string SolvePartOne()
        {
            ResetGlobals();

            int index = 0;
            while(true)
            {
                _processedIndexes.Add(index);
                var (operation, modifier) = _bootCode[index];

                index += PerformOperation(operation, modifier);
                if(_processedIndexes.Contains(index))
                {
                    break;
                }
            }

            return _accumulator.ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        /// <summary>
        /// Perform the operation and return the index modifier
        /// </summary>
        /// <param name="operation">The operation</param>
        /// <param name="modifier">The modifier of the operation</param>
        /// <returns>The index modifier for the next operation</returns>
        private int PerformOperation(Operation operation, int modifier)
        {
            var indexModifier = 1;
            switch(operation)
            {
                case Operation.acc:
                    _accumulator += modifier;
                    break;
                case Operation.jmp:
                    indexModifier = modifier;
                    break;
                case Operation.nop:
                default:
                    break;
            }

            return indexModifier;
        }

        private (Operation operation, int modifier) ParseInputLine(string inputLine)
        {
            var splittedLine = inputLine.Split(' ');
            if (!Enum.TryParse(splittedLine[0], out Operation operation)
                || !int.TryParse(splittedLine[1], out var intValue))
            {
                throw new Exception($"Couldn't parse the value \"{inputLine}\" succesfully.");
            }
            return (operation, intValue);
        }

        private void ResetGlobals()
        {
            _accumulator = 0;
            _processedIndexes = new HashSet<int>();
        }

        #region DebugInput
        private void SetDebugInput()
        {
            base.DebugInput = "" +
                "nop +0\n" +
                "acc +1\n" +
                "jmp +4\n" +
                "acc +3\n" +
                "jmp -3\n" +
                "acc -99\n" +
                "acc +1\n" +
                "jmp -4\n" +
                "acc +6";
        }
        #endregion
    }
}
