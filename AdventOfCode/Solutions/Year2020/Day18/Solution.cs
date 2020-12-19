using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Solutions.Year2020
{
    class Day18 : ASolution
    {
        private Regex _parenthesesRegex = new Regex(@"\([^\(]+?\)", RegexOptions.Compiled);
        private Regex _additionRegex = new Regex(@"((\d+)\ \+\ (\d+))", RegexOptions.Compiled);
        private Regex _multiplyRegex = new Regex(@"((\d+)\ \*\ (\d+))", RegexOptions.Compiled);

        public Day18() : base(18, 2020, "")
        {
            PerformTestCasesPart1();
            PerformTestCasesPart2();
        }

        protected override string SolvePartOne() => base.Input.SplitByNewline().Sum(ResolveExpressionPart1).ToString();

        private long ResolveExpressionPart1(string expression)
        {
            while(_parenthesesRegex.IsMatch(expression))
            {
                foreach (Match match in _parenthesesRegex.Matches(expression))
                {
                    var matchResult = ResolveExpressionPart1(match.Value.Replace("(", "").Replace(")", "")).ToString();
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

        private void PerformTestCasesPart1()
        {
            Assert.AreEqual(71, ResolveExpressionPart1("1 + 2 * 3 + 4 * 5 + 6"));
            Assert.AreEqual(51, ResolveExpressionPart1("1 + (2 * 3) + (4 * (5 + 6))"));
            Assert.AreEqual(26, ResolveExpressionPart1("2 * 3 + (4 * 5)"));
            Assert.AreEqual(437, ResolveExpressionPart1("5 + (8 * 3 + 9 + 3 * 4 * 3)"));
            Assert.AreEqual(12240, ResolveExpressionPart1("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"));
            Assert.AreEqual(13632, ResolveExpressionPart1("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"));
        }

        // 88500956630893
        // 88549433800549 is too high
        // 351267568248230
        // 1729588977350389071
        protected override string SolvePartTwo() => base.Input.SplitByNewline().Sum(ResolveExpressionPart2).ToString();

        private long ResolveExpressionPart2(string expression)
        {
            while (_parenthesesRegex.IsMatch(expression))
            {
                var regexReplaceStartAtReducer = 0;
                foreach (Match match in _parenthesesRegex.Matches(expression))
                {
                    var matchResult = ResolveExpressionPart2(match.Value.Replace("(", "").Replace(")", "")).ToString();
                    expression = _parenthesesRegex.Replace(expression, matchResult.ToString(), 1, match.Index - regexReplaceStartAtReducer);
                    regexReplaceStartAtReducer += match.Length - matchResult.ToString().Length;
                }
            }

            while (_additionRegex.IsMatch(expression))
            {
                var regexReplaceStartAtReducer = 0;
                foreach (Match match in _additionRegex.Matches(expression))
                {
                    var matchResult = long.Parse(match.Groups[2].Value) + long.Parse(match.Groups[3].Value);
                    expression = _additionRegex.Replace(expression, matchResult.ToString(), 1, match.Index - regexReplaceStartAtReducer);
                    regexReplaceStartAtReducer += match.Length - matchResult.ToString().Length;
                }
            }

            while (_multiplyRegex.IsMatch(expression))
            {
                var regexReplaceStartAtReducer = 0;
                foreach (Match match in _multiplyRegex.Matches(expression))
                {
                    var matchResult = long.Parse(match.Groups[2].Value) * long.Parse(match.Groups[3].Value);
                    expression = _multiplyRegex.Replace(expression, matchResult.ToString(), 1, match.Index - regexReplaceStartAtReducer);
                    regexReplaceStartAtReducer += match.Length - matchResult.ToString().Length;
                }
            }

            return long.Parse(expression);
        }

        private void PerformTestCasesPart2()
        {
            Assert.AreEqual(231, ResolveExpressionPart2("1 + 2 * 3 + 4 * 5 + 6"));
            Assert.AreEqual(51, ResolveExpressionPart2("1 + (2 * 3) + (4 * (5 + 6))"));
            Assert.AreEqual(46, ResolveExpressionPart2("2 * 3 + (4 * 5)"));
            Assert.AreEqual(1445, ResolveExpressionPart2("5 + (8 * 3 + 9 + 3 * 4 * 3)"));
            Assert.AreEqual(669060, ResolveExpressionPart2("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"));
            Assert.AreEqual(23340, ResolveExpressionPart2("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"));
            Assert.AreEqual(108, ResolveExpressionPart2("1 + 2 + 1 + 2 + 1 + 2 * 1 + 2 + 1 * 2 + 1"));
            Assert.AreEqual(2027330665920, ResolveExpressionPart2("(6 + 9 * 9 + 7 * (5 * 7 * 6 + 9 * 5 + 2) + 3) * (2 + 4 * (8 * 3 + 2 * 4 * 2 * 8) + 9) * (6 + 6 * 2 * 6) + 5"));
        }
    }

    public enum Operator
    {
        None,
        Addition,
        Mulitplier
    }
}
