using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Common;

public class AdventOfCodeParser
{
    public static List<string> ParseInputFile(string fileName, int adventOfCodeYear)
    {
        //Directory.SetCurrentDirectory($"/Users/danfrankel/Projects/AdventOfCode/AdventOfCode/{adventOfCodeYear}/Input/");
        Directory.SetCurrentDirectory($"D:\\Code Projects\\AdventOfCode\\AdventOfCode\\{adventOfCodeYear}\\Input\\");
        List<string> input = new List<string>();
        StreamReader reader = new StreamReader(File.OpenRead(fileName));

        while (!reader.EndOfStream)
        {
            input.Add(reader.ReadLine().Trim());
        }

        return input;
    }
}