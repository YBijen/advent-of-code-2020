using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day01 : ASolution
    {
        private readonly List<int> _input;

        public Day01() : base(01, 2020, "")
        {
            _input = base.Input.Split('\n').Select(str => int.Parse(str)).ToList();
        }

        protected override string SolvePartOne()
        {
            for (var i = 0; i < _input.Count; i++)
            {
                for (var j = 0; j < _input.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    if (_input[i] + _input[j] == 2020)
                    {
                        return (_input[i] * _input[j]).ToString();
                    }
                }
            }
            return null;
        }

        protected override string SolvePartTwo()
        {
            for (var i = 0; i < _input.Count; i++)
            {
                for (var j = 0; j < _input.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    for (var k = 0; k < _input.Count; k++)
                    {
                        if (k == i || k == j)
                        {
                            continue;
                        }

                        if (_input[i] + _input[j] + _input[k] == 2020)
                        {
                            return (_input[i] * _input[j] * _input[k]).ToString();
                        }
                    }
                }
            }
            return null;
        }
    }
}
