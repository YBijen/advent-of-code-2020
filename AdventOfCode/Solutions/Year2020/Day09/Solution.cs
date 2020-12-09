using System;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    class Day09 : ASolution
    {
        private int _preamble = 25;

        private readonly int[] _data;

        public Day09() : base(09, 2020, "")
        {
           // SetDebugInput();
            _data = base.Input.ToIntArray("\n");
        }

        protected override string SolvePartOne()
        {
            for(var i = _preamble; i < _data.Length; i++)
            {
                var numbers = _data.Skip(i - _preamble).Take(_preamble).ToArray();
                if(!AreAnyPairOfNumbersEqualTo(numbers, _data[i]))
                {
                    return _data[i].ToString();
                }
            }

            throw new Exception("No invalid number could be found.");
        }

        private static bool AreAnyPairOfNumbersEqualTo(int[] numbers, int numberToFind)
        {
            for(var i = 0; i < numbers.Length; i++)
            {
                for(var j = 0; j < numbers.Length; j++)
                {
                    if(i == j)
                    {
                        continue;
                    }

                    if(numbers[i] + numbers[j] == numberToFind)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        #region DebugInput
        private void SetDebugInput()
        {
            _preamble = 5;
            base.DebugInput = "" +
                "35\n" +
                "20\n" +
                "15\n" +
                "25\n" +
                "47\n" +
                "40\n" +
                "62\n" +
                "55\n" +
                "65\n" +
                "95\n" +
                "102\n" +
                "117\n" +
                "150\n" +
                "182\n" +
                "127\n" +
                "219\n" +
                "299\n" +
                "277\n" +
                "309\n" +
                "576";
        }
        #endregion
    }
}
