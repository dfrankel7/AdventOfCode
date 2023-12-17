using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = VSConsole.Console;

namespace AdventOfCode;

internal class AdventOfCode2023Day11 : IAdventOfCodeDay
{
    public string Part1()
    {
        List<string> input = AdventOfCodeParser.ParseInputFile("Day11.txt", 2023);
        Universe universe = new Universe(input);
        List<long> pathLenghts = universe.FindPathLengths();
        long sum = 0;

        for (int i = 0; i < pathLenghts.Count; i++)
        {
            sum += pathLenghts[i];
        }

        return sum.ToString();
    }

    public string Part2()
    {
        List<string> input = AdventOfCodeParser.ParseInputFile("Day11.txt", 2023);
        Universe universe = new Universe(input, 999999);
        List<long> pathLenghts = universe.FindPathLengths();
        long sum = 0;

        for (int i = 0; i < pathLenghts.Count; i++)
        {
            sum += pathLenghts[i];
        }

        return sum.ToString();
    }

    private class Universe
    {
        private List<List<char>> _universe;
        private HashSet<int> _expandingRows;
        private HashSet<int> _expandingColumns;
        private int _expandedOffset;

        public Universe(List<string> input, int expandedOffset = 1)
        {
            _expandedOffset = expandedOffset;
            _universe = new List<List<char>>();
            _expandingRows = new HashSet<int>();
            _expandingColumns = new HashSet<int>();

            for (int y = 0; y < input.Count; y++)
            {
                List<char> list = new List<char>();
                _universe.Add(list);

                for (int x = 0; x < input[y].Length; x++)
                {
                    list.Add(input[y][x]);
                }
            }

            ExpandUniverse();
        }

        private void ExpandUniverse()
        {
            for (int y = 0; y < _universe.Count; y++)
            {
                List<char> universeSlice = _universe[y];
                bool shouldExpand = true;

                for (int x = 0; x < universeSlice.Count; x++)
                {
                    if (universeSlice[x] == '#') 
                    {
                        shouldExpand = false;
                        break;
                    }
                }

                if (shouldExpand)
                {
                    _expandingRows.Add(y);
                }
            }

            for (int x = 0; x < _universe[0].Count; x++)
            {
                bool shouldExpand = true;

                for (int y = 0; y < _universe.Count; y++)
                {
                    if (_universe[y][x] == '#')
                    {
                        shouldExpand = false;
                        break;
                    }
                }

                if (shouldExpand)
                {
                    _expandingColumns.Add(x);
                }
            }
        }

        private List<int[]> FindGalaxyPositions()
        {
            List<int[]> galaxyPositions = new List<int[]>();

            for (int y = 0; y < _universe.Count; y++)
            {
                for (int x = 0; x < _universe[y].Count; x++)
                {
                    if (_universe[y][x] == '#')
                    {
                        galaxyPositions.Add(new int[] { x, y });
                    }
                }
            }

            return galaxyPositions;
        }

        public List<long> FindPathLengths()
        {
            List<long> pathLengths = new List<long>();
            List<int[]> galaxyPositions = FindGalaxyPositions();

            for (int i = 0; i < galaxyPositions.Count; i++)
            {
                for (int j = i + 1; j < galaxyPositions.Count; j++)
                {
                    long pathLength = Math.Abs(galaxyPositions[i][0] - galaxyPositions[j][0]);
                    pathLength += Math.Abs(galaxyPositions[i][1] - galaxyPositions[j][1]);

                    int expandedAreas = _expandingColumns.Where(row => row >= Math.Min(galaxyPositions[i][0], galaxyPositions[j][0]) &&
                                                          row <= Math.Max(galaxyPositions[i][0], galaxyPositions[j][0])).Count();
                    expandedAreas += _expandingRows.Where(row => row >= Math.Min(galaxyPositions[i][1], galaxyPositions[j][1]) &&
                                                          row <= Math.Max(galaxyPositions[i][1], galaxyPositions[j][1])).Count();

                    pathLength += expandedAreas * _expandedOffset;
                    pathLengths.Add(pathLength);
                }
            }

            return pathLengths;
        }
    }
}
