using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode;

public class AdventOfCode2023Day1 : IAdventOfCodeDay
{
    private enum Numbers
    {
        one = 1,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine
    }

    public string Part1()
    {
        List<string> input = AdventOfCodeParser.ParseInputFile("Day1.txt", 2023);
        List<int> numbers = new List<int>();

        for (int i = 0; i < input.Count; i++)
        {
            numbers.Add(FindCalibrationValue(input[i]));
        }

        return FindSum(numbers).ToString();
    }

    public string Part2()
    {
        List<string> input = ReplaceAllWordNumbersWithNumbers(AdventOfCodeParser.ParseInputFile("Day1Modified.txt", 2023));
        List<int> numbers = new List<int>();

        for (int i = 0; i < input.Count; i++)
        {
            numbers.Add(FindCalibrationValue(input[i]));
        }

        return FindSum(numbers).ToString();
    }

    private int FindFirstNumberLeftToRight(string input, bool doWordCheck = true)
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (int.TryParse(input.Substring(i, 1), out int number))
            {
                if (number > 0)
                {
                    return number;
                }
            }

            if (doWordCheck && i > 2)
            {
                string subString = input.Substring(0, i + 1);

                for (int j = 1; j < (int)Numbers.nine + 1; j++)
                {
                    subString = subString.Replace(((Numbers)j).ToString(), j.ToString());
                    string lastCharacterString = subString.Substring(subString.Length - 1);

                    if (int.TryParse(lastCharacterString, out int numberToTest))
                    {
                        if (numberToTest > 0)
                        {
                            return numberToTest;
                        }
                    }
                }
            }
        }

        return -1;
    }

    private int FindFirstNumberRightToLeft(string input, bool doWordCheck = true)
    {
        for (int i = input.Length - 1; i >= 0; i--)
        {
            if (int.TryParse(input.Substring(i, 1), out int number))
            {
                if (number > 0)
                {
                    return number;
                }
            }

            if (doWordCheck && input.Length - 1 - i > 2)
            {
                string subString = input.Substring(i);

                for (int j = 1; j < (int)Numbers.nine + 1; j++)
                {
                    subString = subString.Replace(((Numbers)j).ToString(), j.ToString());

                    int numberToTest = FindFirstNumberRightToLeft(subString, false);

                    if (numberToTest > 0)
                    {
                        return numberToTest;
                    }
                }
            }
        }

        return -1;
    }

    private List<string> ReplaceAllWordNumbersWithNumbers(List<string> input)
    {
        List<string> output = new List<string>();

        for (int i = 0; i < input.Count; i++)
        {
            string inputLine = input[i];

            for(int j = 1; j < (int)Numbers.nine + 1; j++)
            {
                inputLine = inputLine.Replace(((Numbers)j).ToString(), j.ToString());
            }

            output.Add(inputLine);
            Console.WriteLine($"{input[i]}: {output[i]}");
        }

        return output;
    }

    private int FindCalibrationValue(string stringToSearch)
    {
        Regex regex = new Regex(@"[0-9]+");
        MatchCollection matches = regex.Matches(stringToSearch);
        string numbersString = string.Empty;

        for (int j = 0; j < matches.Count; j++)
        {
            numbersString += matches[j].Value;
        }

        string numberString = $"{numbersString[0]}{numbersString[numbersString.Length - 1]}";
        return int.Parse(numberString);
    }

    private int FindSum(List<int> numbers)
    {
        int sum = 0;
        foreach (int number in numbers)
        {
            sum += number;
        }

        return sum;
    }
}