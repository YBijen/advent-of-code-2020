using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    class Day07 : ASolution
    {
        private const string BAG_SHINY_GOLD = "shiny gold";

        private readonly Dictionary<string, Dictionary<string, int>> _luggageRules;

        public Day07() : base(07, 2020, "")
        {
            _luggageRules = ParseLuggageRules();
            Extra();
        }

        private void SetDebugInputPart1()
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

        private void SetDebugInputPart2()
        {
            base.DebugInput = "" +
                "shiny gold bags contain 2 dark red bags.\n" +
                "dark red bags contain 2 dark orange bags.\n" +
                "dark orange bags contain 2 dark yellow bags.\n" +
                "dark yellow bags contain 2 dark green bags.\n" +
                "dark green bags contain 2 dark blue bags.\n" +
                "dark blue bags contain 2 dark violet bags.\n" +
                "dark violet bags contain no other bags.";
        }

        protected override string SolvePartOne()
        {
            var checkedBags = 0;
            var alreadyChecked = new List<string>();
            var bagsToFind = new List<string> { BAG_SHINY_GOLD };
            while(bagsToFind.Count > 0)
            {
                var currentBag = bagsToFind.First();
                bagsToFind.RemoveAt(0);

                foreach (var rule in _luggageRules)
                {
                    if(rule.Value.ContainsKey(currentBag) && !bagsToFind.Contains(rule.Key) && !alreadyChecked.Contains(rule.Key))
                    {
                        alreadyChecked.Add(rule.Key);
                        bagsToFind.Add(rule.Key);
                        checkedBags++;
                    }
                }

            }

            return checkedBags.ToString();
        }

        protected override string SolvePartTwo()
        {
            var bagsInGivenBag = CountBagsInBag(BAG_SHINY_GOLD);
            return (bagsInGivenBag - 1).ToString();
        }

        private void Extra()
        {
            Console.WriteLine("== Method 1 ==");
            var sw1 = Stopwatch.StartNew();
            var methodOne = _luggageRules.ToDictionary(kvp => kvp.Key, kvp => CountBagsInBag(kvp.Key) - 1);
            sw1.Stop();
            Console.WriteLine($"Took {sw1.Elapsed}");

            Console.WriteLine("== Method 2 ==");
            var sw2 = Stopwatch.StartNew();
            var methodTwo = BuildBagCountFromBottom();
            sw2.Stop();
            Console.WriteLine($"Took {sw2.Elapsed}");

            Console.WriteLine();
            Console.WriteLine($"Method 2 is {sw1.ElapsedMilliseconds / sw2.ElapsedMilliseconds} times faster than method 1.");

            var areEqual = methodOne.All(methodOneKvp => methodTwo[methodOneKvp.Key] == methodOneKvp.Value);
            Console.WriteLine($"The two methods {(areEqual ? "ARE" : " ARE NOT ")} providing the same result");
        }

        /// <summary>
        /// Build a lookup of all the bags with the amount of bags it contains next to it
        /// Starts from the bottom and moves to the top
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, long> BuildBagCountFromBottom()
        {
            var result = new Dictionary<string, long>();
            while(result.Count < _luggageRules.Count)
            {
                foreach (var rule in _luggageRules)
                {
                    if(result.ContainsKey(rule.Key))
                    {
                        continue;
                    }

                    if(rule.Value.Count == 0)
                    {
                        result.Add(rule.Key, 0);
                    }
                    else if(rule.Value.All(kvp => result.ContainsKey(kvp.Key)))
                    {
                        result.Add(rule.Key, rule.Value.Sum(kvp => (result[kvp.Key] * kvp.Value) + kvp.Value));
                    }
                }
            }
            return result;
        }

        private long CountBagsInBag(string bag)
        {
            var amountOfBagsInContainedBags = _luggageRules[bag].Sum(b => CountBagsInBag(b.Key) * b.Value);
            return amountOfBagsInContainedBags > 0 ? amountOfBagsInContainedBags + 1 : 1;
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
    }
}
