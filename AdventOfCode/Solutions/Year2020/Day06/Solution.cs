using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    class Day06 : ASolution
    {
        public Day06() : base(06, 2020, "") { }

        protected override string SolvePartOne() => GetAnswersFromInput()
            .SelectMany(GetDistinctAnswers)
            .Count()
            .ToString();

        protected override string SolvePartTwo()
        {
            var total = 0;
            foreach(var groupAnswers in GetAnswersFromInput())
            {
                var personsWhoAnswered = groupAnswers.Split('\n');
                if(personsWhoAnswered.Length == 1)
                {
                    total += personsWhoAnswered[0].Count();
                    continue;
                }

                foreach(var distinctAnswer in GetDistinctAnswers(groupAnswers))
                {
                    if(groupAnswers.Count(ga => ga == distinctAnswer) == personsWhoAnswered.Length)
                    {
                        total++;
                    }
                }
            }
            return total.ToString();
        }

        private string[] GetAnswersFromInput() => base.Input.Split("\n\n");

        private IEnumerable<char> GetDistinctAnswers(string answers) => answers.Replace("\n", "").Distinct();

    }
}
