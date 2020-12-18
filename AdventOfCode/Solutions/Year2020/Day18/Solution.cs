using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Solutions.Year2020
{
    class Day18 : ASolution
    {
        private Regex _parenthesesRegex = new Regex(@"\([^\(]+?\)", RegexOptions.Compiled);

        public Day18() : base(18, 2020, "")
        {
            PerformTestCases();
        }

        protected override string SolvePartOne() => base.Input.SplitByNewline().Sum(ResolveExpression).ToString();

        private long ResolveExpression(string expression)
        {
            while(_parenthesesRegex.IsMatch(expression))
            {
                foreach (Match match in _parenthesesRegex.Matches(expression))
                {
                    var matchResult = ResolveExpression(match.Value.Replace("(", "").Replace(")", "")).ToString();
                    expression = expression.Replace(match.Value, matchResult);
                }
            }

            long currentValue = 0;
            var currentOperator = Operator.None;
            foreach(var value in expression.Split(' '))
            {
                if (long.TryParse(value, out var longValue))
                {
                    switch(currentOperator)
                    {
                        case Operator.None:
                            currentValue = longValue;
                            break;
                        case Operator.Addition:
                            currentValue += longValue;
                            break;
                        case Operator.Mulitplier:
                            currentValue *= longValue;
                            break;
                        default:
                            break;
                    }
                }
                else if(value == "+")
                {
                    currentOperator = Operator.Addition;
                }
                else if(value == "*")
                {
                    currentOperator = Operator.Mulitplier;
                }
            }

            return currentValue;
        }


        protected override string SolvePartTwo()
        {
            return null;
        }

        private void PerformTestCases()
        {
            Assert.AreEqual(71, ResolveExpression("1 + 2 * 3 + 4 * 5 + 6"));
            Assert.AreEqual(51, ResolveExpression("1 + (2 * 3) + (4 * (5 + 6))"));
            Assert.AreEqual(26, ResolveExpression("2 * 3 + (4 * 5)"));
            Assert.AreEqual(437, ResolveExpression("5 + (8 * 3 + 9 + 3 * 4 * 3)"));
            Assert.AreEqual(12240, ResolveExpression("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"));
            Assert.AreEqual(13632, ResolveExpression("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"));
        }
    }

    public enum Operator
    {
        None,
        Addition,
        Mulitplier
    }
}
