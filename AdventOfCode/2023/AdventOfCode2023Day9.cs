using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class AdventOfCode2023Day9 : IAdventOfCodeDay
    {
        public string Part1()
        {
            List<string> input = AdventOfCodeParser.ParseInputFile("Day9.txt", 2023);
            List<Sequencer> sequences = new List<Sequencer>();
            int sum = 0;

            for (int i = 0; i < input.Count; i++)
            {
                Sequencer sequencer = new Sequencer(input[i]);
                sequences.Add(sequencer);
                int next = sequencer.FindNextNumberInSequence();
                sum += next;
            }

            return sum.ToString();
        }

        public string Part2()
        {
            List<string> input = AdventOfCodeParser.ParseInputFile("Day9.txt", 2023);
            List<Sequencer> sequences = new List<Sequencer>();
            int sum = 0;

            for (int i = 0; i < input.Count; i++)
            {
                Sequencer sequencer = new Sequencer(input[i]);
                sequences.Add(sequencer);
                int next = sequencer.FindNewStartingNumberInSequence();
                sum += next;
            }

            return sum.ToString();
        }

        private class Sequencer
        {
            List<List<int>> sequences;

            public Sequencer(string intString)
            {
                sequences = new List<List<int>>();
                List<int> startingNumbers = new List<int>();
                string[] numberStrings = intString.Split(' ');

                for (int i = 0; i < numberStrings.Length; i++)
                {
                    startingNumbers.Add(int.Parse(numberStrings[i]));
                }

                sequences.Add(startingNumbers);

                ProcessSequence();
            }

            private void ProcessSequence()
            {
                int sequenceIndex = 0;
                List<int> newSequence;

                do
                {
                    newSequence = new List<int>();
                    List<int> currentSequence = sequences[sequenceIndex];

                    for (int i = 0; i < currentSequence.Count - 1; i++)
                    {
                        newSequence.Add(currentSequence[i + 1] - currentSequence[i]);
                    }

                    sequences.Add(newSequence);
                    sequenceIndex++;
                }
                while (newSequence.Any(number => number != 0));
            }

            public int FindNextNumberInSequence()
            {
                for (int i = sequences.Count - 1; i >= 0; i--)
                {
                    if (i == sequences.Count - 1)
                    {
                        sequences[i].Add(0);
                    }
                    else
                    {
                        sequences[i].Add(sequences[i].Last() + sequences[i + 1].Last());
                    }
                }

                return sequences[0].Last();
            }

            public int FindNewStartingNumberInSequence()
            {
                for (int i = sequences.Count - 1; i >= 0; i--)
                {
                    if (i == sequences.Count - 1)
                    {
                        sequences[i].Add(sequences[i].First());
                    }
                    else
                    {
                        sequences[i].Insert(0, sequences[i].First() - sequences[i + 1].First());
                    }
                }

                return sequences[0].First();
            }
        }
    }
}
