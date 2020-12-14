using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Solutions.Year2020
{
    class Day13 : ASolution
    {
        private int _takeBusAtTime = 0;
        private List<(int busId, int departOffset)> _busIds = new List<(int busId, int departOffset)>();

        public Day13() : base(13, 2020, "")
        {
            //DrawSchedule();
            //SetDebugInput(0);
            //AssertDebugInputsPart2();
            SetGlobalsFromInput();
        }

        protected override string SolvePartOne()
        {
            (int busId, int timeToWait) shortestWaitTime = (0, _takeBusAtTime);
            foreach(var currentBus in _busIds)
            {
                var currentBusId = currentBus.busId;
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
            // Initalize
            long time = 0;
            long currentIncrement = _busIds[0].busId;
            var solvedBusCount = 1;
            var busToCheck = _busIds[1];

            while (true)
            {
                time += currentIncrement;

                if(busToCheck.busId - (time % busToCheck.busId) == (busToCheck.departOffset % busToCheck.busId))
                {
                    if(_busIds.Count - 1 == solvedBusCount)
                    {
                        break;
                    }

                    currentIncrement *= busToCheck.busId;
                    busToCheck = _busIds[++solvedBusCount];
                }
            }

            return time.ToString();
        }

        private void SetGlobalsFromInput()
        {
            var input = base.Input.SplitByNewline();
            _takeBusAtTime = int.Parse(input[0]);
            _busIds.Clear();
            var splitInput = input[1].Split(',');
            for (var i = 0; i < splitInput.Length; i++)
            {
                if(splitInput[i] == "x")
                {
                    continue;
                }
                _busIds.Add((int.Parse(splitInput[i]), i));
            }
        }

        private void SetDebugInput(int inputNumber)
        {
            switch(inputNumber)
            {
                case 0:
                    base.DebugInput = "939\n7,13,x,x,59,x,31,19"; // 1068781
                    break;
                case 1:
                    base.DebugInput = "0\n17,x,13,19"; // 3417
                    break;
                case 2:
                    base.DebugInput = "0\n67,7,59,61"; // 754018
                    break;
                case 3:
                    base.DebugInput = "0\n67,x,7,59,61"; // 779210
                    break;
                case 4:
                    base.DebugInput = "0\n67,7,x,59,61"; // 1261476
                    break;
                case 5:
                    base.DebugInput = "0\n1789,37,47,1889"; // 1202161486
                    break;
                default:
                    throw new Exception("The input number is invalid");
            }
        }

        private void DrawSchedule()
        {
            var loopsNeeded = 170;
            //var bus = new List<(int id, int offset)> { (3, 0), (5, 1), (7, 2) };
            //var bus = new List<(int id, int offset)> { (17, 0), (13, 2), (19, 3) };
            var bus = new List<(int id, int offset)> { (7, 0), (13, 1), (59, 4), (31, 6), (19, 7) };

            Console.WriteLine($"Time\t{string.Join("\t", bus.Select(b => "bus " + b.id))}");
            for (var i = 0; i <= loopsNeeded; i++)
            {
                Console.WriteLine($"{i.ToString("0000")}:\t  {string.Join("\t  ", bus.Select(b => DoesDepart(b, i) ? "D" : "."))}");
            }
        }

        private bool DoesDepart((int id, int offset) bus, int time) => time == 0 || time % bus.id == 0;

        private void AssertDebugInputsPart2()
        {
            UseInputAndExpectedResult(0, "1068781");
            UseInputAndExpectedResult(1, "3417");
            UseInputAndExpectedResult(2, "754018");
            UseInputAndExpectedResult(3, "779210");
            UseInputAndExpectedResult(4, "1261476");
            UseInputAndExpectedResult(5, "1202161486");
        }

        private void UseInputAndExpectedResult(int input, string expectedResult)
        {
            SetDebugInput(input);
            SetGlobalsFromInput();
            Assert.AreEqual(SolvePartTwo(), expectedResult);
        }
    }
}
