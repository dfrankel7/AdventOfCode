using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

internal class AdventOfCode2023Day4 : IAdventOfCodeDay
{
    public string Part1()
    {
        int sum = 0;
        List<string> input = AdventOfCodeParser.ParseInputFile("Day4.txt", 2023);

        for (int cardIndex = 0; cardIndex < input.Count; cardIndex++)
        {
            input[cardIndex] = input[cardIndex].Replace("  ", " ");
            string cardNumbers = input[cardIndex].Split(":")[1].Trim();
            string[] numberStrings = cardNumbers.Split('|');

            GameCard gameCard = new GameCard(numberStrings[0].Trim(), numberStrings[1].Trim());
            sum += gameCard.CalculateCardValue();
        }

        return sum.ToString();
    }

    public string Part2()
    {
        int sum = 0;
        List<string> input = AdventOfCodeParser.ParseInputFile("Day4.txt", 2023);
        Dictionary<int, int> gameCardCopies = new Dictionary<int, int>();

        for (int i = 0; i < input.Count; i++)
        {
            gameCardCopies[i] = 1;
        }

        for (int cardIndex = 0; cardIndex < input.Count; cardIndex++)
        {
            input[cardIndex] = input[cardIndex].Replace("  ", " ");
            string cardNumbers = input[cardIndex].Split(":")[1].Trim();
            string[] numberStrings = cardNumbers.Split('|');

            GameCard gameCard = new GameCard(numberStrings[0].Trim(), numberStrings[1].Trim());
            int winningNumberCount = gameCard.GetNumberOfMatchingNumbers();

            for (int i = 0; i < winningNumberCount; i++)
            {
                if (gameCardCopies.ContainsKey(cardIndex + i + 1))
                {
                    gameCardCopies[cardIndex + i + 1] += gameCardCopies[cardIndex];
                }
            }
        }

        foreach (KeyValuePair<int, int> pair in gameCardCopies)
        {
            sum += pair.Value;
        }

        return sum.ToString();
    }

    private class GameCard
    {
        private HashSet<int> _winningNumbers;
        private List<int> _cardNumbers;
        
        public GameCard(string winningNumbersString, string cardNumbers)
        {
            _winningNumbers = new HashSet<int>();
            _cardNumbers = new List<int>();

            string[] numberStrings = winningNumbersString.Trim().Split(" ");
            foreach (string numberString in numberStrings)
            {
                _winningNumbers.Add(int.Parse(numberString.Trim()));
            }

            numberStrings = cardNumbers.Trim().Split(" ");
            foreach (string numberString in numberStrings)
            {
                _cardNumbers.Add(int.Parse(numberString.Trim()));
            }
        }

        public int GetNumberOfMatchingNumbers()
        {
            int matchingNumbers = 0;

            for (int i = 0; i < _cardNumbers.Count; i++)
            {
                if (_winningNumbers.Contains(_cardNumbers[i]))
                {
                    matchingNumbers++;
                }
            }

            return matchingNumbers;
        }

        public int CalculateCardValue()
        {
            int matchingNumbers = GetNumberOfMatchingNumbers();

            if (matchingNumbers == 0)
            {
                return 0;
            }

            return (int)Math.Pow(2, matchingNumbers - 1);
        }
    }
}
