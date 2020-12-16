using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Year2020.Day16.Models;

namespace AdventOfCode.Solutions.Year2020
{

    class Day16Solution : ASolution
    {
        private List<TicketField> _ticketField = new List<TicketField>();
        private List<List<int>> _tickets = new List<List<int>>();
        private List<int> _myTicket = new List<int>();

        public Day16Solution() : base(16, 2020, "")
        {
            ParseInput();
        }

        protected override string SolvePartOne()
        {
            var min = _ticketField.Aggregate((v1, v2) => v1.Range1.Min < v2.Range1.Min ? v1 : v2).Range1.Min;
            var max = _ticketField.Aggregate((v1, v2) => v1.Range2.Max > v2.Range2.Max ? v1 : v2).Range2.Max;
            return _tickets.SelectMany(t => t).Where(t => t < min || t > max).Sum().ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private void ParseInput()
        {
            var inputLines = base.Input.SplitByNewline();
            
            // Parse Ticket Information
            foreach(var field in inputLines.TakeWhile(line => line != "your ticket:"))
            {
                var name = field.Split(':')[0];
                var ranges = field.Split(':')[1].Trim().Split(" or ");

                _ticketField.Add(new TicketField
                {
                    Name = field.Split(":")[0],
                    Range1 = (int.Parse(ranges[0].Split("-")[0]), int.Parse(ranges[0].Split("-")[1])),
                    Range2 = (int.Parse(ranges[0].Split("-")[1]), int.Parse(ranges[1].Split("-")[1]))
                });
            }

            // Parse MyTicket information
            var ticketInputIndex = inputLines.ToList().IndexOf("your ticket:") + 1;
            _myTicket = inputLines[ticketInputIndex].Split(',').Select(v => int.Parse(v)).ToList();

            // Parse Other Tickets
            var otherTicketStartIndex = inputLines.ToList().IndexOf("nearby tickets:") + 1;
            _tickets = inputLines.Skip(otherTicketStartIndex).Select(otherTicket => otherTicket.Split(',').Select(v => int.Parse(v)).ToList()).ToList();
        }
    }
}
