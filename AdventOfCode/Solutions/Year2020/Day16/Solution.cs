using System.Collections.Generic;
using System.Linq;
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
            var min = GetMinimumValue();
            var max = GetMaximumValue();
            return _tickets.SelectMany(t => t).Where(t => t < min || t > max).Sum().ToString();
        }

        protected override string SolvePartTwo()
        {
            // All the valid tickets
            var validTickets = GetValidTickets();

            // All the valid tickets, but now grouped by the index of each ticket
            // [0] contains all the first tickets of all valid tickets
            // [1] contains all the second tickets of all valid tickets
            // etc...
            var validTicketsPerField = new List<List<int>>();
            for (var i = 0; i < _ticketField.Count; i++)
            {
                var ticketsPerFieldList = new List<int>();
                for(var j = 0; j < validTickets.Count; j++)
                {
                    ticketsPerFieldList.Add(validTickets[j][i]);
                }
                validTicketsPerField.Add(ticketsPerFieldList);
            }

            // Go through all ticket indexes and find which one maps to each title
            var mapping = _ticketField.ToDictionary(ticket => ticket.Name, ticket => new List<int>());
            for(var i = 0; i < _ticketField.Count; i++)
            {
                var currentTicketField = _ticketField[i];

                for(var j = 0; j < validTicketsPerField.Count; j++)
                {
                    if(validTicketsPerField[j].All(t =>
                            t >= currentTicketField.Range1.Min && t <= currentTicketField.Range1.Max
                            || t >= currentTicketField.Range2.Min && t <= currentTicketField.Range2.Max))
                    {
                        mapping[currentTicketField.Name].Add(j);
                    }
                }
            }

            // Now clean up the mapping by removing values used by multiple tickets
            var cleanedMapping = new Dictionary<string, int>();
            foreach(var map in mapping.OrderBy(m => m.Value.Count))
            {
                cleanedMapping.Add(map.Key, map.Value.FirstOrDefault(v => !cleanedMapping.ContainsValue(v)));
            }

            return cleanedMapping.Where(kvp => kvp.Key.Contains("departure"))
                .Select(kvp => _myTicket.ElementAt(kvp.Value))
                .Aggregate(1L, (t1, t2) => t1 * t2).ToString();
        }

        private List<List<int>> GetValidTickets()
        {
            var min = GetMinimumValue();
            var max = GetMaximumValue();
            return _tickets.Where(ticket => ticket.All(t => t >= min && t <= max)).ToList();
        }

        private int GetMinimumValue() => _ticketField.Aggregate((v1, v2) => v1.Range1.Min < v2.Range1.Min ? v1 : v2).Range1.Min;
        private int GetMaximumValue() => _ticketField.Aggregate((v1, v2) => v1.Range2.Max > v2.Range2.Max ? v1 : v2).Range2.Max;

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
                    Range2 = (int.Parse(ranges[1].Split("-")[0]), int.Parse(ranges[1].Split("-")[1]))
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
