using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

internal class AdventOfCode2023Day6 : IAdventOfCodeDay
{
    public string Part1()
    {
        List<string> input = AdventOfCodeParser.ParseInputFile("Day6.txt", 2023);
        List<Race> races = CreatePart1Races(input[0], input[1]);
        int product = 1;

        for (int i = 0; i < races.Count; i++)
        {
            product *= races[i].CalculateWaysToWin();
        }

        return product.ToString();
    }

    public string Part2()
    {
        List<string> input = AdventOfCodeParser.ParseInputFile("Day6.txt", 2023);
        long raceTime = long.Parse(input[0].Split(":")[1].Trim().Replace(" ", ""));
        long recordDistance = long.Parse(input[1].Split(":")[1].Trim().Replace(" ", ""));
        Race race = new Race(raceTime, recordDistance);

        return race.CalculateWaysToWin().ToString();
    }

    private List<Race> CreatePart1Races(string totalTimesString, string recordDistancesString)
    {
        List<Race> part1Races = new List<Race>();
        List<int> raceTimes = CreateIntListsFromInput(totalTimesString);
        List<int> recordDistances = CreateIntListsFromInput(recordDistancesString);

        for (int i = 0; i < raceTimes.Count; i++)
        {
            part1Races.Add(new Race(raceTimes[i], recordDistances[i]));
        }

        return part1Races;
    }

    private List<int> CreateIntListsFromInput(string input)
    {
        string[] raceTimeStrings = input.Split(':')[1].Trim().Split(" ");
        List<int> raceTimes = new List<int>();

        for (int i = 0; i < raceTimeStrings.Length; i++)
        {
            if (!string.IsNullOrEmpty(raceTimeStrings[i]))
            {
                raceTimes.Add(int.Parse(raceTimeStrings[i]));
            }
        }

        return raceTimes;
    }

    private class Race(long _totalTime, long _recordDistance)
    {
        public int CalculateWaysToWin()
        {
            int waysToWin = 0;

            for (int i = 1; i < _totalTime; i++)
            {
                if (i * (_totalTime - i) > _recordDistance)
                {
                    waysToWin++;
                }
            }

            return waysToWin;
        }
    }
}
