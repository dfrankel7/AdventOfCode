using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    class Puzzle2020_9
    {
        private const string INPUT_FILE_PATH = "C:\\Users\\Dan\\Documents\\Code Projects\\AdventOfCode\\AdventOfCode\\2020\\Input\\Puzzle2020_9_input.txt";

        public static long FindInvalidNumber(int preableLength)
        {
            List<long> data = GetData();
            long currentNumber = 0;

            for (int i = preableLength; i < data.Count; i++)
            {
                currentNumber = data[i];

                if (!IsNumberValid(data, preableLength, i))
                {
                    break;
                }    
            }

            return currentNumber;
        }

        private static List<long> GetData()
        {
            List<long> data = new List<long>();
            StreamReader streamReader = new StreamReader(INPUT_FILE_PATH);

            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine().Trim();
                data.Add(long.Parse(line));
            }

            return data;
        }

        private static bool IsNumberValid(List<long> data, int preableLength, int indexToCheck)
        {
            long currentNumber = data[indexToCheck];

            for (int i = indexToCheck - preableLength; i < indexToCheck - 1; i++)
            {
                for (int j = i + 1; j < indexToCheck; j++)
                {
                    if (data[i] + data[j] == currentNumber)
                    {
                        return true;
                    }    
                }
            }

            return false;
        }

        public static long FindWeaknessOfInvalidNumber(int preableLength)
        {
            List<long> data = GetData();
            long invalidNumber = FindInvalidNumber(preableLength);
            int startingIndex = preableLength;
            long sum = data[startingIndex];
            int indexOffset = 0;

            while (sum != invalidNumber)
            {
                indexOffset++;
                sum += data[startingIndex + indexOffset];

                if (sum > invalidNumber)
                {
                    startingIndex++;
                    indexOffset = 0;
                    sum = data[startingIndex];
                }
            }

            List<long> numbers = new List<long>();
            for (int i = startingIndex; i < startingIndex + indexOffset; i++)
            {
                numbers.Add(data[i]);
            }

            numbers.Sort();

            return numbers[0] + numbers[numbers.Count - 1];
        }
    }
}
