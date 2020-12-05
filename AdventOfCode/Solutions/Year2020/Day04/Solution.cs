using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020
{

    class Day04 : ASolution
    {
        private readonly List<(string Name, bool IsRequired)> _fieldList = new List<(string FieldName, bool IsRequired)>
        {
            ("byr", true),
            ("iyr", true),
            ("eyr", true),
            ("hgt", true),
            ("hcl", true),
            ("ecl", true),
            ("pid", true),
            ("cid", false)
        };

        private List<string> _possibleEyeColors = new List<string>
        {
            "amb",
            "blu",
            "brn",
            "gry",
            "grn",
            "hzl",
            "oth"
        };

        public Day04() : base(04, 2020, "") { }

        private string[] GetPassportsFromInput() => base.Input.Split("\n\n");

        private IEnumerable<string> GetPassportsWithAllRequiredProperties() => GetPassportsFromInput()
            .Where(passport => _fieldList
                .Where(field => field.IsRequired)
                .All(field => passport.Contains(field.Name))
                );

        protected override string SolvePartOne() => GetPassportsWithAllRequiredProperties().Count().ToString();

        protected override string SolvePartTwo() => GetPassportsWithAllRequiredProperties()
                .Count(passport => IsPassportValid(passport))
                .ToString();

        private bool IsPassportValid(string passport)
        {
            var passportKeyValuePairs = passport.Replace('\n', ' ').Split(' ');
            return passportKeyValuePairs.All(kvp =>
            {
                var key = kvp.Split(':')[0];
                var value = kvp.Split(':')[1];
                return IsKeyValuePairValid(key, value);
            });
        }

        private bool IsKeyValuePairValid(string key, string value)
        {
            int.TryParse(value, out var intValue);
            if (key == "byr")
            {
                return intValue >= 1920 && intValue <= 2002;
            }
            else if (key == "iyr")
            {
                return intValue >= 2010 && intValue <= 2020;
            }
            else if (key == "eyr")
            {
                return intValue >= 2020 && intValue <= 2030;
            }
            else if (key == "hgt" && value.Contains("cm"))
            {
                value = value.Replace("cm", "");
                intValue = int.Parse(value);
                return intValue >= 150 && intValue <= 193;
            }
            else if (key == "hgt" && value.Contains("in"))
            {
                value = value.Replace("in", "");
                intValue = int.Parse(value);
                return intValue >= 59 && intValue <= 76;
            }
            else if(key == "hcl")
            {
                return Regex.IsMatch(value, "#[a-f0-9]{6}");
            }
            else if(key == "ecl")
            {
                return _possibleEyeColors.Contains(value);
            }
            else if(key == "pid")
            {
                return value.Length == 9 && intValue != 0;
            }
            else if(key == "cid")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
