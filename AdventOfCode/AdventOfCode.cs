using Console = VSConsole.Console;

namespace AdventOfCode
{
    class AdventOfCode
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new AdventOfCode2023Day9().Part2());
            //RunAsync();
        }

        private static async void RunAsync()
        {
            string output = await new AdventOfCode2023Day8().Part2();
            Console.WriteLine(output);
        }
    }
}
