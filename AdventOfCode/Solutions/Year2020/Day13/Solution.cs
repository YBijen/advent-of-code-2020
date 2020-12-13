using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    class Day13 : ASolution
    {
        private readonly int _takeBusAtTime = 0;
        private readonly List<int> _busIds = new List<int>();

        public Day13() : base(13, 2020, "")
        {
            //SetDebugInput();
            var input = base.Input.SplitByNewline();
            _takeBusAtTime = int.Parse(input[0]);
            _busIds = input[1].Split(',').Where(id => id != "x").Select(id => int.Parse(id)).ToList();
        }

        protected override string SolvePartOne()
        {
            (int busId, int timeToWait) shortestWaitTime = (0, _takeBusAtTime);
            foreach(var currentBusId in _busIds)
            {
                var timeToWait = currentBusId - (_takeBusAtTime % currentBusId);
                if(timeToWait < shortestWaitTime.timeToWait)
                {
                    shortestWaitTime = (currentBusId, timeToWait);
                }
            }
            return (shortestWaitTime.busId * shortestWaitTime.timeToWait).ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private void SetDebugInput()
        {
            base.DebugInput = "939\n7,13,x,x,59,x,31,19";
        }
    }
}
