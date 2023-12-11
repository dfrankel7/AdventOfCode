using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

internal class AdventOfCode2023Day7 : IAdventOfCodeDay
{
    public string Part1()
    {
        List<string> input = AdventOfCodeParser.ParseInputFile("Day7.txt", 2023);
        List<Hand> hands = new List<Hand>();
        Dictionary<int, Hand> handsByRank = new Dictionary<int, Hand>();

        for (int i = 0; i < input.Count; i++)
        {
            hands.Add(new Hand(input[i]));
        }

        hands.Sort(Hand.CompareTo);

        long sum = 0;

        for (int i = 0; i < hands.Count; i++)
        {
            sum += (i + 1) * hands[i].bet;
        }

        return sum.ToString();
    }

    public string Part2()
    {
        List<string> input = AdventOfCodeParser.ParseInputFile("Day7.txt", 2023);
        List<Hand> hands = new List<Hand>();
        Dictionary<int, Hand> handsByRank = new Dictionary<int, Hand>();

        for (int i = 0; i < input.Count; i++)
        {
            hands.Add(new Hand(input[i], true));
        }

        hands.Sort(Hand.CompareTo);

        long sum = 0;

        for (int i = 0; i < hands.Count; i++)
        {
            sum += (i + 1) * hands[i].bet;
        }

        return sum.ToString();
    }

    private class Hand
    {
        public enum Card
        {
            Joker = 1,
            Two = 2,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace
        }

        public List<Card> cards { get; private set; }

        public int bet { get; private set; }

        private bool _jokersInPlay;

        public Hand(string handString, bool jokersInPlay = false)
        {
            _jokersInPlay = jokersInPlay;
            string[] handStrings = handString.Split(" ");
            cards = new List<Card>();
            bet = int.Parse(handStrings[1]);

            for (int i = 0; i < handStrings[0].Length; i++)
            {
                cards.Add(ConvertStringToCard(handStrings[0].Substring(i, 1)));
            }
        }

        private Card ConvertStringToCard(string cardString)
        {
            switch (cardString)
            {
                case "A":
                    return Card.Ace;
                case "K":
                    return Card.King;
                case "Q":
                    return Card.Queen;
                case "J":
                    return _jokersInPlay ? Card.Joker : Card.Jack;
                case "T":
                    return Card.Ten;
                case "9":
                    return Card.Nine;
                case "8":
                    return Card.Eight;
                case "7":
                    return Card.Seven;
                case "6":
                    return Card.Six;
                case "5":
                    return Card.Five;
                case "4":
                    return Card.Four;
                case "3":
                    return Card.Three;
                default:
                    return Card.Two;
            }
        }

        public int CalculateHandRank()
        {
            Dictionary<Card, int> numberOfCardsByType = new Dictionary<Card, int>();
            int numberOfJokers = 0;

            for (int i = 0; i < cards.Count; i++)
            {
                Card card = cards[i];

                if (card == Card.Joker)
                {
                    numberOfJokers++;
                    continue;
                }    

                if (!numberOfCardsByType.ContainsKey(card))
                {
                    numberOfCardsByType.Add(card, 1);
                }
                else
                {
                    numberOfCardsByType[card]++;
                }
            }

            int highestCardCount = 0;
            int secondHightestCardCount = 0;

            foreach (var pair in numberOfCardsByType)
            {
                if (pair.Value > highestCardCount)
                {
                    secondHightestCardCount = highestCardCount;
                    highestCardCount = pair.Value;
                }
                else if (pair.Value > secondHightestCardCount)
                {
                    secondHightestCardCount = pair.Value;
                }
            }

            return 2 * (highestCardCount + numberOfJokers) + secondHightestCardCount;
        }

        public static int CompareTo(Hand handA, Hand handB)
        {
            int handARank = handA.CalculateHandRank();
            int handBRank = handB.CalculateHandRank();

            if (handARank == handBRank)
            {
                for (int i = 0; i < handA.cards.Count; i++)
                {
                    Card handACard = handA.cards[i];
                    Card handBCard = handB.cards[i];
                    if (handACard == handBCard)
                    {
                        continue;
                    }

                    return (int)handACard - (int)handBCard;
                }
            }
            else
            {
                return handARank - handBRank;
            }

            return 0;
        }
    }
}
