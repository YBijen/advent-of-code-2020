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
            AssertPart2();
            Initialize();
        }

        protected override string SolvePartOne()
        {
            ProcessEachRule();
            return _processedRules[0].Count(rule => _messages.Contains(rule)).ToString();
        }

        protected override string SolvePartTwo()
        {
            UpdateRulesForPart2();
            //ProcessEachRule();
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

                    // Process the rule and add them to the result
                    _processedRules.Add(rule.Key,
                        ProcessList(rule.Key, rule.Value.Item1)
                            .Union(ProcessList(rule.Key, rule.Value.Item2))
                            .ToList()
                    );
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

        private List<string> ProcessList(int key, List<int> list)
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

        private void AssertPart2()
        {
            #region SetDebugInput
            base.DebugInput = "" +
                "42: 9 14 | 10 1\n" +
                "9: 14 27 | 1 26\n" +
                "10: 23 14 | 28 1\n" +
                "1: \"a\"\n" +
                "11: 42 31\n" +
                "5: 1 14 | 15 1\n" +
                "19: 14 1 | 14 14\n" +
                "12: 24 14 | 19 1\n" +
                "16: 15 1 | 14 14\n" +
                "31: 14 17 | 1 13\n" +
                "6: 14 14 | 1 14\n" +
                "2: 1 24 | 14 4\n" +
                "0: 8 11\n" +
                "13: 14 3 | 1 12\n" +
                "15: 1 | 14\n" +
                "17: 14 2 | 1 7\n" +
                "23: 25 1 | 22 14\n" +
                "28: 16 1\n" +
                "4: 1 1\n" +
                "20: 14 14 | 1 15\n" +
                "3: 5 14 | 16 1\n" +
                "27: 1 6 | 14 18\n" +
                "14: \"b\"\n" +
                "21: 14 1 | 1 14\n" +
                "25: 1 1 | 1 14\n" +
                "22: 14 14\n" +
                "8: 42\n" +
                "26: 14 22 | 1 20\n" +
                "18: 15 15\n" +
                "7: 14 5 | 1 21\n" +
                "24: 14 1\n" +
                "\n" +
                "abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa\n" +
                "bbabbbbaabaabba\n" +
                "babbbbaabbbbbabbbbbbaabaaabaaa\n" +
                "aaabbbbbbaaaabaababaabababbabaaabbababababaaa\n" +
                "bbbbbbbaaaabbbbaaabbabaaa\n" +
                "bbbababbbbaaaaaaaabbababaaababaabab\n" +
                "ababaaaaaabaaab\n" +
                "ababaaaaabbbaba\n" +
                "baabbaaaabbaaaababbaababb\n" +
                "abbbbabbbbaaaababbbbbbaaaababb\n" +
                "aaaaabbaabaaaaababaa\n" +
                "aaaabbaaaabbaaa\n" +
                "aaaabbaabbaaaaaaabbbabbbaaabbaabaaa\n" +
                "babaaabbbaaabaababbaabababaaab\n" +
                "aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba";
            #endregion
            Initialize(true);
            ProcessEachRule();
            Assert.AreEqual("3", SolvePartOne());

            Initialize(true);
            UpdateRulesForPart2();
            //ProcessEachRule();
            //Assert.AreEqual("12", SolvePartTwo());
        }

        private void UpdateRulesForPart2()
        {
            // 8: 42 | 42 8
            _rules[8] = (new List<int> { 42 }, new List<int> { 42, 8 });
            // 11: 42 31 | 42 11 31
            _rules[11] = (new List<int> { 42, 31 }, new List<int> { 42, 11, 31 });
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
