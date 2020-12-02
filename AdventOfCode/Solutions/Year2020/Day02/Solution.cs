using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Year2020.Day02.Models;
using AdventOfCode.Solutions.Year2020.Day02.Models.Part1;

namespace AdventOfCode.Solutions.Year2020
{

    class Day02Solution : ASolution
    {
        private readonly List<InputModel> _input;

        public Day02Solution() : base(02, 2020, "")
        {
            _input = base.Input.Split('\n').Select(CreateInputModel).ToList();
        }

        protected override string SolvePartOne() =>
            _input.Where(Part1_HasPasswordValidPolicy).Count().ToString();

        private bool Part1_HasPasswordValidPolicy(InputModel input)
        {
            var charCountInPassword = input.Password.Count(c => c == input.Policy.Character);
            return charCountInPassword >= input.Policy.Minimum && charCountInPassword <= input.Policy.Maximum;
        }

        protected override string SolvePartTwo() =>
            _input.Where(Part2_HasPasswordValidPolicy).Count().ToString();

        private bool Part2_HasPasswordValidPolicy(InputModel input) =>
            input.Password[input.Policy.Minimum - 1] == input.Policy.Character
            ^ input.Password[input.Policy.Maximum - 1] == input.Policy.Character;

        /// <summary>
        /// Parse the InputLine to a Model
        /// Example input line: 1-3 a: abcde
        /// </summary>
        /// <param name="inputLine"></param>
        /// <returns></returns>
        private InputModel CreateInputModel(string inputLine)
        {
            var values = inputLine.Split(' ');
            var minMax = values[0].Split('-');
            return new InputModel
            {
                Policy = new PolicyModel
                {
                    Minimum = int.Parse(minMax[0]),
                    Maximum = int.Parse(minMax[1]),
                    Character = values[1].First()
                },
                Password = values[2]
            };
        }
    }
}
