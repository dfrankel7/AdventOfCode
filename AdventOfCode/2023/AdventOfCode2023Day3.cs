using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode;

internal class AdventOfCode2023Day3 : IAdventOfCodeDay
{
    public string Part1()
    {
        List<string> inputMap = AdventOfCodeParser.ParseInputFile("Day3.txt", 2023);
        HashSet<PartNumber> partNumbers = new HashSet<PartNumber>();
        int[,] positionsToCheck =
        {
            { -1, -1 },
            {  0, -1 },
            {  1, -1 },
            {  1,  0 },
            {  1,  1 },
            {  0,  1 },
            { -1,  1 },
            { -1,  0 }
        };

        for (int col = 0; col < inputMap.Count; col++)
        {
            for (int row = 0; row < inputMap[col].Length; row++)
            {
                char character = inputMap[col][row];

                if (character == '.')
                {
                    continue;
                }  
                
                if (character >= '0' && character <= '9')
                {
                    continue;
                }

                for (int i = 0; i < positionsToCheck.GetLength(0); i++)
                {
                    int x = row + positionsToCheck[i, 0];
                    int y = col + positionsToCheck[i, 1];
                    char characterToTest = inputMap[y][x];

                    if (characterToTest >= '0' && characterToTest <= '9')
                    {
                        // find left most bounds
                        int startIndex = x;
                        int endIndex = x;
                        char searchCharacter = inputMap[y][startIndex];

                        while (startIndex > 0 && searchCharacter >= '0' && searchCharacter <= '9')
                        {
                            searchCharacter = inputMap[y][startIndex - 1];

                            if (searchCharacter >= '0' && searchCharacter <= '9')
                            {
                                startIndex--;
                            }
                        }

                        searchCharacter = inputMap[y][endIndex];

                        while (endIndex < inputMap[y].Length - 1 && searchCharacter >= '0' && searchCharacter <= '9')
                        {
                            searchCharacter = inputMap[y][endIndex + 1];

                            if (searchCharacter >= '0' && searchCharacter <= '9')
                            {
                                endIndex++;
                            }
                        }

                        string numberString = inputMap[y].Substring(startIndex, endIndex - startIndex + 1);
                        partNumbers.Add(new PartNumber(int.Parse(numberString), startIndex, y));
                    }
                }
            }
        }

        int sum = 0;

        foreach (PartNumber partNumber in partNumbers)
        {
            sum += partNumber.number;
        }

        return sum.ToString();
    }

    public string Part2()
    {
        List<string> inputMap = AdventOfCodeParser.ParseInputFile("Day3.txt", 2023);
        List<Gear> gears = new List<Gear>();
        HashSet<PartNumber> partNumbers = new HashSet<PartNumber>();
        int[,] positionsToCheck =
        {
            { -1, -1 },
            {  0, -1 },
            {  1, -1 },
            {  1,  0 },
            {  1,  1 },
            {  0,  1 },
            { -1,  1 },
            { -1,  0 }
        };

        for (int col = 0; col < inputMap.Count; col++)
        {
            for (int row = 0; row < inputMap[col].Length; row++)
            {
                char character = inputMap[col][row];

                if (character != '*')
                {
                    continue;
                }

                Gear gear = new Gear();
                gears.Add(gear);

                for (int i = 0; i < positionsToCheck.GetLength(0); i++)
                {
                    int x = row + positionsToCheck[i, 0];
                    int y = col + positionsToCheck[i, 1];
                    char characterToTest = inputMap[y][x];

                    if (characterToTest >= '0' && characterToTest <= '9')
                    {
                        // find left most bounds
                        int startIndex = x;
                        int endIndex = x;
                        char searchCharacter = inputMap[y][startIndex];

                        while (startIndex > 0 && searchCharacter >= '0' && searchCharacter <= '9')
                        {
                            searchCharacter = inputMap[y][startIndex - 1];

                            if (searchCharacter >= '0' && searchCharacter <= '9')
                            {
                                startIndex--;
                            }
                        }

                        searchCharacter = inputMap[y][endIndex];

                        while (endIndex < inputMap[y].Length - 1 && searchCharacter >= '0' && searchCharacter <= '9')
                        {
                            searchCharacter = inputMap[y][endIndex + 1];

                            if (searchCharacter >= '0' && searchCharacter <= '9')
                            {
                                endIndex++;
                            }
                        }

                        string numberString = inputMap[y].Substring(startIndex, endIndex - startIndex + 1);
                        gear.AddPartNumber(new PartNumber(int.Parse(numberString), startIndex, y));
                    }
                }
            }
        }

        int sum = 0;

        foreach (Gear gear in gears)
        {
            sum += gear.CalculateGearRatio();
        }

        return sum.ToString();
    }

    private class PartNumber
    {
        public int number;
        public int x;
        public int y;

        public PartNumber(int number, int x, int y)
        {
            this.number = number;
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is PartNumber partNumber && number == partNumber.number && x == partNumber.x && y == partNumber.y)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(number, x, y).GetHashCode();
        }
    }

    private class Gear
    {
        public int x, y;
        private HashSet<PartNumber> partNumbers;

        public Gear()
        {
            partNumbers = new HashSet<PartNumber>();
        }

        public void AddPartNumber(PartNumber partNumber)
        {
            partNumbers.Add(partNumber);
        }

        public int CalculateGearRatio()
        {
            if (partNumbers.Count != 2)
            {
                return 0;
            }

            return partNumbers.First().number * partNumbers.Last().number;          
        }
    }
}
