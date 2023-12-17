using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

internal class AdventOfCode2023Day12 : IAdventOfCodeDay
{
    public string Part1()
    {
        List<string> input = AdventOfCodeParser.ParseInputFile("Day12.txt", 2023);
        List<SpringReport> reports = new List<SpringReport>();
        int sum = 0;

        for (int i = 0; i < input.Count; i++)
        {
            SpringReport report = new SpringReport(input[i]);
            sum += report.CalculateWorkingCombinations();
            reports.Add(report);
        }

        return sum.ToString();
    }

    public string Part2()
    {
        throw new NotImplementedException();
    }

    private class SpringReport
    {
        private string _startString;
        private int[] _counts;
        private HashSet<string> _combinations;

        public SpringReport(string input)
        {
            _combinations = new HashSet<string>();
            string[] strings = input.Split(' ');
            _startString = strings[0];
            string[] countStrings = strings[1].Split(",");
            _counts = new int[countStrings.Length];

            for (int i = 0; i < countStrings.Length; i++)
            {
                _counts[i] = int.Parse(countStrings[i]);
            }

            FindAllCombinations(_startString);
        }

        private void FindAllCombinations(string stringToCheck)
        {
            if (!stringToCheck.Contains('?'))
            {
                _combinations.Add(stringToCheck);
                return;
            }

            int index = stringToCheck.IndexOf('?');
            string prefixString = stringToCheck.Substring(0, index);
            string sufixString = index + 1 < stringToCheck.Length ? stringToCheck.Substring(index + 1) : string.Empty;

            FindAllCombinations($"{prefixString}.{sufixString}");
            FindAllCombinations($"{prefixString}#{sufixString}");
        }

        public int CalculateWorkingCombinations()
        {
            int workingCombinations = 0;

            foreach (string stringToCheck in _combinations)
            {
                List<int> stringLengths = new List<int>();
                string[] stringsToCheck = stringToCheck.Split(".");

                for (int i = 0; i < stringsToCheck.Length; i++)
                {
                    if (stringsToCheck[i].Length > 0)
                    {
                        stringLengths.Add(stringsToCheck[i].Length);
                    }
                }

                if (stringLengths.Count == _counts.Length)
                {
                    bool isWorkingCombinations = true;

                    for (int j = 0; j < _counts.Length; j++)
                    {
                        if (_counts[j] != stringLengths[j])
                        {
                            isWorkingCombinations = false;
                            break;
                        }
                    }

                    if (isWorkingCombinations)
                    {
                        workingCombinations++;
                    }
                }
            }

            return workingCombinations;
        }
    }
}
