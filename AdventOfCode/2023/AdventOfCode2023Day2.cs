using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

internal class AdventOfCode2023Day2 : IAdventOfCodeDay
{
    public string Part1()
    {
        int sum = 0;
        Part1Game game = new Part1Game(12, 13, 14);
        List<string> input = AdventOfCodeParser.ParseInputFile("Day2.txt", 2023);

        for (int i = 0; i < input.Count; i++)
        {
            string[] gameLineSections = input[i].Split(':');

            if (game.IsValidGame(gameLineSections[1]))
            {
                sum += i + 1;
            }
        }

        return sum.ToString();
    }

    public string Part2()
    {
        int sum = 0;
        List<string> input = AdventOfCodeParser.ParseInputFile("Day2.txt", 2023);

        for (int i = 0; i < input.Count; i++)
        {
            string[] gameLineSections = input[i].Split(':');
            Part2Game game = new Part2Game(gameLineSections[1]);

            sum += game.CalculateGamePower();
        }

        return sum.ToString();
    }

    public class Part1Game
    {
        private enum Colors
        {
            red,
            green,
            blue
        }

        private Dictionary<string, int> _maxCubesByColor;

        public Part1Game(int maxRedCubes, int maxGreenCubes, int maxBlueCubes)
        {
            _maxCubesByColor = new Dictionary<string, int>
            {
                {"red", maxRedCubes},
                {"green", maxGreenCubes},
                {"blue", maxBlueCubes}
            };
        }

        public bool IsValidGame(string gameString)
        {
            bool isValid = true;
            string[] rounds = gameString.Trim().Split(';');

            for (int i = 0; i < rounds.Length; i++)
            {
                isValid = IsValidRound(rounds[i]);

                if (!isValid)
                {
                    break;
                }
            }

            return isValid;
        }

        private bool IsValidRound(string roundString)
        {
            bool isValid = true;
            string[] cubeCounts = roundString.Trim().Split(",");

            for (int i = 0; i < cubeCounts.Length; i++)
            {
                string[] cubeCountPair = cubeCounts[i].Trim().Split(" ");
                int cubeCount = int.Parse(cubeCountPair[0]);
                string color = cubeCountPair[1];

                if (_maxCubesByColor[color] < cubeCount)
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }
    }

    public class Part2Game
    {
        private Dictionary<string, int> _maxCubesByColor;

        public Part2Game(string gameString)
        {
            _maxCubesByColor = new Dictionary<string, int>();
            gameString = gameString.Trim().Replace(";", ",");
            string[] cubeStrings = gameString.Split(",");

            for (int i = 0; i < cubeStrings.Length; i++)
            {
                string[] cubeCountPair = cubeStrings[i].Trim().Split(" ");
                int cubeCount = int.Parse(cubeCountPair[0]);
                string color = cubeCountPair[1];

                if (!_maxCubesByColor.ContainsKey(color))
                {
                    _maxCubesByColor[color] = cubeCount;
                }
                else
                {
                    if (_maxCubesByColor[color] < cubeCount)
                    {
                        _maxCubesByColor[color] = cubeCount;
                    }
                }
            }
        }

        public int CalculateGamePower()
        {
            int power = 1;

            foreach (KeyValuePair<string, int> pair in _maxCubesByColor)
            {
                power *= pair.Value;
            }

            return power;
        }
    }
}
