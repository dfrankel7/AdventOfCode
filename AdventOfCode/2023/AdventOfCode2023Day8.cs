using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Console = VSConsole.Console;

namespace AdventOfCode;

internal class AdventOfCode2023Day8 : IAdventOfCodeDayAsync
{
    public async Task<string> Part1()
    {
        List<string> input = AdventOfCodeParser.ParseInputFile("Day8.txt", 2023);
        Dictionary<string, List<string>> directionOptions = GenerateDirectionOptions(input);
        string directions = input[0];
        string currentPosition = "AAA";
        int stringIndex = 0;
        long turnsMade = 0;

        while (!currentPosition.Equals("ZZZ"))
        {
            switch (directions[stringIndex])
            {
                case 'L':
                    currentPosition = directionOptions[currentPosition][0];
                    break;
                case 'R':
                    currentPosition = directionOptions[currentPosition][1];
                    break;
            }
            
            stringIndex = (stringIndex + 1) % directions.Length;
            turnsMade++;
        }

        return turnsMade.ToString();
    }

    public async Task<string> Part2()
    {
        List<string> input = AdventOfCodeParser.ParseInputFile("Day8.txt", 2023);
        Dictionary<string, List<string>> directionOptions = GenerateDirectionOptions(input);
        string directions = input[0];
        int stringIndex = 0;
        long turnsMade = 1;
        List<PathFollower> pathFollowers = new List<PathFollower>();

        foreach (string directionToCheck in directionOptions.Keys) 
        {
            if (directionToCheck[2] == 'A')
            {
                pathFollowers.Add(new PathFollower(directions, directionToCheck, directionOptions));
            }
        }

        while (!pathFollowers.All(follower => follower.IsAtDestination()))
        {
            for (int i = 0; i < pathFollowers.Count; i++)
            {
                if (!pathFollowers[i].IsAtDestination())
                {
                    pathFollowers[i].FindNextPosition(stringIndex);
                }
            }

            stringIndex = (stringIndex + 1) % directions.Length;
        }

        HashSet<long> factors = new HashSet<long>();

        for (int i = 0; i < pathFollowers.Count; i++)
        {
            long stepsTaken = pathFollowers[i].stepsTaken;
            factors.UnionWith(FindFactors(stepsTaken));
        }

        foreach (long factor in factors)
        {
            turnsMade *= factor;
        }

        return turnsMade.ToString();
    }

    public List<long> FindFactors(long number)
    {
        List<long> factors = new List<long>();
        long max = (long)Math.Sqrt(number);  // Round down

        for (long factor = 2; factor <= max; ++factor) // Test from 1 to the square root, or the int below it, inclusive.
        {
            if (number % factor == 0)
            {
                factors.Add(factor);
                if (factor != number / factor) // Don't add the square root twice!  Thanks Jon
                {
                    factors.Add(number / factor);
                }
            }
        }

        return factors;
    }

    private Dictionary<string, List<string>> GenerateDirectionOptions(List<string> input)
    {
        Dictionary<string, List<string>> directionOptions = new Dictionary<string, List<string>>();

        for (int i = 2; i < input.Count; i++)
        {
            string[] directionLineStrings = input[i].Split('=');
            string source = directionLineStrings[0].Trim();
            string[] optionStrings = directionLineStrings[1].Trim().Split(',');
            List<string> options = new List<string>
            {
                optionStrings[0].Substring(1),
                optionStrings[1].Substring(1, 3)
            };

            directionOptions.Add(source, options);
        }

        return directionOptions;
    }

    class PathFollower
    {
        private string _directions;
        private string _currentPosition;
        private Dictionary<string, List<string>> _directionOptions;
        public long stepsTaken { get; private set; }

        public PathFollower(string directions, string startingLocation, Dictionary<string, List<string>> directionOptions)
        {
            _directions = directions;
            _currentPosition = startingLocation;
            _directionOptions = directionOptions;
            stepsTaken = 0;
        }

        public void FindNextPosition(int nextDirectionIndex)
        {
            switch (_directions[nextDirectionIndex])
            {
                case 'L':
                    _currentPosition = _directionOptions[_currentPosition][0];
                    break;
                case 'R':
                    _currentPosition = _directionOptions[_currentPosition][1];
                    break;
            }

            stepsTaken++;
        }

        public bool IsAtDestination()
        {
            return _currentPosition[2] == 'Z';
        }
    }
}
