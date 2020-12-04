using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Day04() : base(04, 2020, "")
        {
            //base.DebugInput = "" +
            //    "ecl:gry pid:860033327 eyr: 2020 hcl:#fffffd\n" +
            //    "byr: 1937 iyr: 2017 cid: 147 hgt: 183cm\n" +
            //    "\n" +
            //    "    iyr:2013 ecl: amb cid:350 eyr: 2023 pid: 028048884\n" +
            //    "hcl:#cfa07d byr:1929\n" +
            //    "\n" +
            //    "hcl:#ae17e1 iyr:2013\n" +
            //    "eyr: 2024\n" +
            //    "ecl: brn pid:760753108 byr: 1931\n" +
            //    "hgt: 179cm\n" +
            //    "\n" +
            //    " hcl:#cfa07d eyr:2025 pid:166559648\n" +
            //    "iyr: 2011 ecl: brn hgt:59in\n";
        }

        private string[] GetPassportsFromInput() => base.Input.Split("\n\n");

        protected override string SolvePartOne()
        {
            return GetPassportsFromInput()
                .Count(passport => _fieldList
                    .Where(field => field.IsRequired)
                    .All(field => passport.Contains(field.Name))
                    )
                .ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }
    }
}
