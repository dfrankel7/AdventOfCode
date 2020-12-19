using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Puzzle2020_6
    {
        private const string INPUT_FILE_PATH = "C:\\Users\\Dan\\Documents\\Code Projects\\AdventOfCode\\AdventOfCode\\2020\\Input\\Puzzle2020_6_input.txt";

        public static int SumAnswers()
        {
            int sum = 0;
            string answerSet = string.Empty;
            StreamReader streamReader = new StreamReader(INPUT_FILE_PATH);

            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine().Trim();

                if (string.IsNullOrEmpty(line))
                {
                    HashSet<char> answers = new HashSet<char>();

                    for (int i = 0; i < answerSet.Length; i++)
                    {
                        answers.Add(answerSet[i]);
                    }

                    sum += answers.Count;
                    answerSet = string.Empty;
                }
                else
                {
                    answerSet += line;
                }
            }

            if (!string.IsNullOrEmpty(answerSet))
            {
                HashSet<char> answers = new HashSet<char>();

                for (int i = 0; i < answerSet.Length; i++)
                {
                    answers.Add(answerSet[i]);
                }

                sum += answers.Count;
            }

            return sum;
        }

        public static int SumCommonAnswers()
        {
            int sum = 0;
            string answerSet = string.Empty;
            StreamReader streamReader = new StreamReader(INPUT_FILE_PATH);

            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine().Trim();

                if (string.IsNullOrEmpty(line))
                {
                    List<string> answersGroup = new List<string>(answerSet.Trim().Split(" "));
                    answerSet = answerSet.Replace(" ", "");
                    HashSet<char> answers = new HashSet<char>();

                    for (int i = 0; i < answerSet.Length; i++)
                    {
                        char answer = answerSet[i];
                        if (!answers.Contains(answer))
                        {
                            answers.Add(answer);

                            if (answersGroup.All(s => s.Contains(answer)))
                            {
                                sum++;
                            }
                        }
                    }

                    answerSet = string.Empty;
                }
                else
                {
                    answerSet += line + " ";
                }
            }

            if (!string.IsNullOrEmpty(answerSet))
            {
                List<string> answersGroup = new List<string>(answerSet.Trim().Split(" "));
                answerSet = answerSet.Replace(" ", "");
                HashSet<char> answers = new HashSet<char>();

                for (int i = 0; i < answerSet.Length; i++)
                {
                    char answer = answerSet[i];
                    if (!answers.Contains(answer))
                    {
                        answers.Add(answer);

                        if (answersGroup.All(s => s.Contains(answer)))
                        {
                            sum++;
                        }
                    }
                }
            }

            return sum;
        }
    }
}
