using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    class Day07 : ASolution
    {
        private const string BAG_SHINY_GOLD = "shiny gold";

        public Day07() : base(07, 2020, "")
        {
            //SetDebugInput();
        }

        private void SetDebugInput()
        {
            base.DebugInput = "" +
            "light red bags contain 1 bright white bag, 2 muted yellow bags.\n" +
            "dark orange bags contain 3 bright white bags, 4 muted yellow bags.\n" +
            "bright white bags contain 1 shiny gold bag.\n" +
            "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.\n" +
            "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.\n" +
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.\n" +
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.\n" +
            "faded blue bags contain no other bags.\n" +
            "dotted black bags contain no other bags.";
        }

        protected override string SolvePartOne()
        {
            var parsedRules = ParseLuggageRules();

            var checkedBags = 0;
            var alreadyChecked = new List<string>();
            var bagsToFind = new List<string> { BAG_SHINY_GOLD };
            while(bagsToFind.Count > 0)
            {
                var currentBag = bagsToFind.First();

                foreach(var rule in parsedRules)
                {
                    if(rule.Value.ContainsKey(currentBag) && !bagsToFind.Contains(rule.Key) && !alreadyChecked.Contains(rule.Key))
                    {
                        alreadyChecked.Add(rule.Key);
                        bagsToFind.Add(rule.Key);
                        checkedBags++;
                    }
                }

                bagsToFind.RemoveAt(0);
            }

            return checkedBags.ToString();
        }

        private Dictionary<string, Dictionary<string, int>> ParseLuggageRules()
        {
            var parsedLuggageRules = new Dictionary<string, Dictionary<string, int>>();

            var luggageRules = base.Input.SplitByNewline();
            foreach(var rule in luggageRules)
            {
                var parsedRule = SplitRule(CleanRule(rule));

                var currentBagColor = $"{parsedRule[0]} {parsedRule[1]}";
                var currentBagContains = new Dictionary<string, int>();
                for(var i = 2; i < parsedRule.Length; i+= 3)
                {
                    // Add BagColor + Amount of times it is needed
                    currentBagContains.Add($"{parsedRule[i + 1]} {parsedRule[i + 2]}", int.Parse(parsedRule[i]));
                }

                parsedLuggageRules.Add(currentBagColor, currentBagContains);
            }

            return parsedLuggageRules;
        }

        private string CleanRule(string rule) =>
            rule.Replace("contain", "").Replace("bags", "").Replace("bag", "").Replace("no other", "").Replace(",", "").Replace(".", "");

        private string[] SplitRule(string rule) => rule.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        protected override string SolvePartTwo()
        {
            return null;
        }
    }
}
