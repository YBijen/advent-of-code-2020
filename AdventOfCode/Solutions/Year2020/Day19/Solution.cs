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
            AssertDebugInput();
            Initialize();
        }

        protected override string SolvePartOne()
        {
            return null;
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
                    // Haal de startwaarden op van de eerste value
                    var subruleResult = new List<string>();

                    // Als alle subrules van de rule processed zijn
                    if (rule.Value.Item1.All(subrule => _processedRules.ContainsKey(subrule)))
                    {
                        var set1Result = new List<string>(_processedRules[rule.Value.Item1[0]]);

                        // rule.value.item1 bevat { 4, 5 }
                        foreach (var subrule in rule.Value.Item1.Skip(1))
                        {
                            var newSet = new List<string>();

                            for(var i = 0; i < set1Result.Count; i++)
                            {
                                var currValue = set1Result[i];
                                foreach (var processedSubruleResult in _processedRules[subrule])
                                {
                                    // Bevat { "a" } of { "b" }
                                    newSet.Add(currValue += processedSubruleResult);
                                }
                            }

                            set1Result = new List<string>(newSet);
                        }

                        subruleResult.AddRange(set1Result);
                    }

                    if (rule.Value.Item2 != null && rule.Value.Item2.All(subrule => _processedRules.ContainsKey(subrule)))
                    {
                        var set2Result = new List<string>(_processedRules[rule.Value.Item2[0]]);

                        // rule.value.item2 bevat { 4, 5 }
                        foreach (var subrule in rule.Value.Item2.Skip(1))
                        {
                            var newSet = new List<string>();

                            for (var i = 0; i < set2Result.Count; i++)
                            {
                                var currValue = set2Result[i];
                                foreach (var processedSubruleResult in _processedRules[subrule])
                                {
                                    // Bevat { "a" } of { "b" }
                                    newSet.Add(currValue += processedSubruleResult);
                                }
                            }

                            set2Result = new List<string>(newSet);
                        }

                        subruleResult.AddRange(set2Result);
                    }

                    if(subruleResult.Count > 0)
                    {
                        _processedRules.Add(rule.Key, subruleResult);
                    }
                }
            }

            //foreach (var pr in _processedRules)
            //{
            //    Console.WriteLine($"[{pr.Key}] {string.Join(" | ", pr.Value)}");
            //}
        }

        private void Initialize(bool isDebug = false)
        {
            if(!isDebug)
            {
                base.DebugInput = string.Empty;
            }

            _rules.Clear();
            _processedRules.Clear();
            _messages.Clear();

            FillMessagesFromInput();
            FillRulesFromInput();
            ProcessEachRule();
        }

        private void AssertDebugInput()
        {
            base.DebugInput = "" +
                "0: 1 2\n" +
                "1: \"a\"\n" +
                "2: 1 3 | 3 1\n" +
                "3: \"b\"";

            //base.DebugInput = "" +
            //    "0: 2 1\n" +
            //    "1: \"a\"\n" +
            //    "2: 1 3 | 3 1\n" +
            //    "3: \"b\"";

            //base.DebugInput = "" +
            //    "0: \"a\"\n" +
            //    "1: \"b\"\n" +
            //    "2: 0 1 3 4 5 6\n" +
            //    "3: 0\n" +
            //    "4: 0 1\n" +
            //    "5: 0 1 0 1\n" +
            //    "6: 0 1 0 1 0 1";

            Initialize(true);

            //CollectionAssert.AreEqual(_processedRules[2], new List<string> { "abaabababababab" });

            //Console.WriteLine($"Debug input is valid!");
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
