using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    class Puzzle2020_8
    {
        private const string INPUT_FILE_PATH = "C:\\Users\\Dan\\Documents\\Code Projects\\AdventOfCode\\AdventOfCode\\2020\\Input\\Puzzle2020_8_input.txt";

        public static int FindAcculatorValueAtLoopingInstruction()
        {
            List<string> commandStringList = new List<string>();
            int currentIndex = 0;
            int accumulator = 0;
            StreamReader streamReader = new StreamReader(INPUT_FILE_PATH);

            while (!streamReader.EndOfStream)
            {
                commandStringList.Add(streamReader.ReadLine().Trim());
            }

            List<int> processedIndicies = new List<int>();

            while (!processedIndicies.Contains(currentIndex))
            {
                processedIndicies.Add(currentIndex);
                string commandLine = commandStringList[currentIndex];
                string[] commandPair = commandLine.Split(" ");
                int modifier = int.Parse(commandPair[1]);

                switch (commandPair[0])
                {
                    case "jmp":
                        if (modifier != 0)
                        {
                            currentIndex += modifier;
                        }
                        break;
                    case "acc":
                        accumulator += modifier;
                        goto default;
                    default:
                        currentIndex++;
                        break;
                }

                currentIndex++;
            }
            
            return accumulator;
        }

        public static string FindAcculatorValueAfterFinalInstruction()
        {
            var instructions = ParseInput();
            var res = 0;
            foreach (var t in instructions)
            {
                var command = t.command;
                t.command = command switch
                {
                    "jmp" => "nop",
                    "nop" => "jmp",
                    _ => command
                };
                var (halts, acc) = RunProgram(instructions);
                if (halts)
                {
                    res = acc;
                    break;
                }
                t.command = command;
            }

            return res.ToString();
        }

        private static (bool halts, int acc) RunProgram(IList<Instruction> instructions)
        {
            var seen = new HashSet<int>();
            var acc = 0;
            var pos = 0;
            while (pos < instructions.Count)
            {
                if (seen.Contains(pos))
                {
                    return (false, acc);
                }
                seen.Add(pos);
                switch (instructions[pos].command)
                {
                    case "acc":
                        {
                            acc += instructions[pos].argument;
                            pos++;
                            break;
                        }
                    case "jmp":
                        {
                            pos += instructions[pos].argument;
                            break;
                        }
                    case "nop":
                        {
                            pos++;
                            break;
                        }
                }
            }
            return (true, acc);
        }

        private static IList<Instruction> ParseInput()
        {
            List<Instruction> commandList = new List<Instruction>();
            StreamReader streamReader = new StreamReader(INPUT_FILE_PATH);
            
            while (!streamReader.EndOfStream)
            {
                string commandLine = streamReader.ReadLine().Trim();
                string[] commandPair = commandLine.Split(" ");
                Instruction instruction = new Instruction
                {
                    command = commandPair[0],
                    argument = int.Parse(commandPair[1])
                };

                commandList.Add(instruction);
            }

            return commandList;
        }

        private class Instruction
        {
            public string command { get; set; }
            public int argument { get; set; }
        }
    }
}
