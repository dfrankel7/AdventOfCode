using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    class Puzzle2020_10
    {
        private const string INPUT_FILE_PATH = "C:\\Users\\Dan\\Documents\\Code Projects\\AdventOfCode\\AdventOfCode\\2020\\Input\\Puzzle2020_10_input.txt";

        private static List<int> GetData()
        {
            List<int> data = new List<int>();
            StreamReader streamReader = new StreamReader(INPUT_FILE_PATH);

            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine().Trim();
                data.Add(int.Parse(line));
            }

            return data;
        }

        public static int Part1()
        {
            int differencesBy1 = 0;
            int differencesBy3 = 0;
            List<int> data = GetData();
            data.Add(0);
            data.Add(data.Max() + 3);
            data.Sort();

            for (int i = 0; i < data.Count -1; i++)
            {
                if (data[i + 1] - data[i] == 1)
                {
                    differencesBy1++;
                }

                if (data[i + 1] - data[i] == 3)
                {
                    differencesBy3++;
                }
            }

            return differencesBy1 * differencesBy3;
        }

        public static string Part2()
        {
            List<int> data = GetData();
            data.Add(0);
            data.Add(data.Max() + 3);
            data.Sort();
            Dictionary<int, long> memoize = new Dictionary<int, long>
            {
                [data.Count - 1] = 1
            };

            for (int k = data.Count - 2; k >= 0; k--)
            {
                long connections = 0L;
                for (int connected = k + 1; connected < data.Count && data[connected] - data[k] <= 3; connected++)
                {
                    connections += memoize[connected];
                }

                memoize[k] = connections;
            }

            return memoize[0].ToString();
        }
    }
}
