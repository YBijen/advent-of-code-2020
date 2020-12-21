using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Solutions.Year2020
{
    class Day19 : ASolution
    {
        private const int AMOUNT_OF_PRESOLVED_VALUES = 2;

        private readonly Dictionary<int, (List<int>, List<int>)> _rules = new Dictionary<int, (List<int>, List<int>)>();
        private readonly List<string> _messages = new List<string>();

        private readonly Dictionary<int, List<string>> _processedRules = new Dictionary<int, List<string>>();

        public Day19() : base(19, 2020, "")
        {
            AssertPart1();
            Initialize();
        }

        protected override string SolvePartOne()
        {
            ProcessEachRule();
            return _processedRules[0].Count(rule => _messages.Contains(rule)).ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private void ProcessEachRule()
        {
            // Zolang nog niet alle rules processed zijn
            while ((_processedRules.Count - AMOUNT_OF_PRESOLVED_VALUES) < _rules.Count)
            {
                // Voor elke rule die nog niet processed is
                foreach (var rule in _rules.Where(r => !_processedRules.ContainsKey(r.Key)))
                {
                    // Als nog niet alle subrules opgelost zijn, sla deze dan over
                    if(!AreAllSubrulesProcessed(rule.Value.Item1, rule.Value.Item2))
                    {
                        continue;
                    }

                    var processedSubruleResult = ProcessList(rule.Value.Item1).Union(ProcessList(rule.Value.Item2)).ToList();
                    _processedRules.Add(rule.Key, processedSubruleResult);
                }
            }
        }

        private bool AreAllSubrulesProcessed(List<int> list1, List<int> list2)
        {
            var allRules = list2 != null
                ? list1.Union(list2)
                : list1;

            return allRules.All(r => _processedRules.ContainsKey(r));
        }

        private List<string> ProcessList(List<int> list)
        {
            if (list == null || list.Count == 0)
            {
                return new List<string>();
            }

            // Start by filling the WorkingList with the values from the first rule
            var workingList = new List<string>(_processedRules[list[0]]);

            // Go through all the other subrules
            foreach (var subrule in list.Skip(1))
            {
                // Create a new list which will be filled with appended values
                var appendedWorkingList = new List<string>();

                // Go through all Working List values and append the lists of the subrules to them
                foreach (var currentValue in workingList)
                {
                    foreach (var processedSubruleResult in _processedRules[subrule])
                    {
                        appendedWorkingList.Add(currentValue + processedSubruleResult);
                    }
                }

                // Update the workinglist by setting it to the latest version of the appended working list
                workingList = new List<string>(appendedWorkingList);
            }

            return workingList;
        }

        private void Initialize(bool isDebug = false)
        {
            if(!isDebug)
            {
                base.DebugInput = null;
            }

            _rules.Clear();
            _processedRules.Clear();
            _messages.Clear();

            FillMessagesFromInput();
            FillRulesFromInput();
        }

        private void AssertPart1()
        {
            base.DebugInput = "" +
                "0: 4 1 5\n" +
                "1: 2 3 | 3 2\n" +
                "2: 4 4 | 5 5\n" +
                "3: 4 5 | 5 4\n" +
                "4: \"a\"\n" +
                "5: \"b\"\n" +
                "\n" +
                "ababbb\n" +
                "bababa\n" +
                "abbbab\n" +
                "aaabbb\n" +
                "aaaabbb";

            Initialize(true);
            ProcessEachRule();
            Assert.AreEqual("2", SolvePartOne());

            // Make sure that the answer for part 1 is still valid
            Initialize(false);
            ProcessEachRule();
            Assert.AreEqual("144", SolvePartOne());
        }

        private void FillMessagesFromInput()
        {
            var splittedInput = base.Input.Split("\n\n");
            if (splittedInput.Length > 1)
            {
                _messages.AddRange(splittedInput[1].SplitByNewline());
            }
        }

        private void FillRulesFromInput()
        {
            var splittedInput = base.Input.Split("\n\n");
            foreach (var rule in splittedInput[0].SplitByNewline())
            {
                var splittedRule = rule.Split(':', StringSplitOptions.TrimEntries);
                var index = int.Parse(splittedRule[0]);

                if (splittedRule[1].Contains("\""))
                {
                    _processedRules.Add(index, new List<string> { splittedRule[1][1].ToString() });
                }
                else if (splittedRule[1].Contains("|"))
                {
                    var subRules = splittedRule[1].Split('|', StringSplitOptions.TrimEntries);
                    var parsedRules = subRules
                        .Select(ruleList =>
                            ruleList.Split(" ")
                                .Select(rule => int.Parse(rule))
                                .ToList())
                        .ToList();

                    _rules.Add(index, (parsedRules[0], parsedRules[1]));
                }
                else
                {
                    var parsedRules = splittedRule[1].Split(" ").Select(rule => int.Parse(rule)).ToList();
                    _rules.Add(index, (parsedRules, null));
                }
            }
        }
    }
}
