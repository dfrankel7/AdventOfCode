using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

internal class AdventOfCode2023Day5 : IAdventOfCodeDay
{
    public string Part1()
    {
        List<string> input = AdventOfCodeParser.ParseInputFile("Day5.txt", 2023);
        List<long> seeds = GetSeedsPart1(input[0]);

        return GetMinimumLocation(input, seeds);
    }

    public List<long> GetSeedsPart1(string seedsLine)
    {
        List<long> seeds = new List<long>();
        string seedsString = seedsLine.Split(':')[1].Trim();
        string[] seedStrings = seedsString.Split(" ");

        for (int i = 0; i < seedStrings.Length; i++)
        {
            seeds.Add(long.Parse(seedStrings[i].Trim()));
        }

        return seeds;
    }

    public string Part2()
    {
        List<string> input = AdventOfCodeParser.ParseInputFile("Day5.txt", 2023);
        List<long> seeds = GetSeedsPart2(input[0]);

        return GetMinimumLocation(input, seeds);
    }

    public List<long> GetSeedsPart2(string seedsLine)
    {
        List<long> seeds = new List<long>();
        string seedsString = seedsLine.Split(':')[1].Trim();
        string[] seedStrings = seedsString.Split(" ");

        for (int i = 0; i < seedStrings.Length; i += 2)
        {
            long seedStartingValue = long.Parse(seedStrings[i].Trim());
            long seedRange = long.Parse(seedStrings[i + 1].Trim());

            for (long j = 0; j < seedRange; j++)
            {
                seeds.Add(long.Parse(seedStrings[i].Trim()) + j);
            }
        }

        return seeds;
    }

    public string GetMinimumLocation(List<string> input, List<long> seeds)
    {
        List<Map> maps = new List<Map>();

        int mapStartIndex = 3;

        for (int i = mapStartIndex; i < input.Count; i++)
        {
            if (string.IsNullOrEmpty(input[i]))
            {
                Map map = new Map(input.GetRange(mapStartIndex, i - mapStartIndex));
                maps.Add(map);

                mapStartIndex = i += 2;
            }
        }

        Map finalMap = new Map(input.GetRange(mapStartIndex, input.Count - mapStartIndex));
        maps.Add(finalMap);

        long minimumLocation = long.MaxValue;
        long currentValue = 0;

        for (int i = 0; i < seeds.Count; i++)
        {
            currentValue = seeds[i];

            for (int j = 0; j < maps.Count; j++)
            {
                currentValue = maps[j].GetMapValue(currentValue);
            }

            if (currentValue < minimumLocation)
            {
                minimumLocation = currentValue;
            }
        }

        return minimumLocation.ToString();
    }

    private class Map
    {
        private List<MapRange> _mapRanges;

        public Map(List<string> mapDataStrings)
        {
            _mapRanges = new List<MapRange>();

            for (int i = 0; i < mapDataStrings.Count; i++)
            {
                string[] numberStrings = mapDataStrings[i].Trim().Split(' ');
                long sourceRangeStart = long.Parse(numberStrings[1].Trim());
                long destinationRangeStart = long.Parse(numberStrings[0].Trim());
                long rangeLength = long.Parse(numberStrings[2].Trim());

                _mapRanges.Add(new MapRange(sourceRangeStart, destinationRangeStart, rangeLength));
            }
        }

        public long GetMapValue(long mapSource)
        {
            for (int i = 0; i < _mapRanges.Count; i++)
            {
                long mapDestination = _mapRanges[i].GetMapValue(mapSource);

                if (mapDestination >= 0)
                {
                    return mapDestination;
                }    
            }

            return mapSource;
        }

        private class MapRange
        {
            long _sourceRangeStart;
            long _destinationRangeStart;
            long _rangeLength;

            public MapRange(long sourceRangeStart, long destinationRangeStart, long rangeLength)
            {
                _sourceRangeStart = sourceRangeStart;
                _destinationRangeStart = destinationRangeStart;
                _rangeLength = rangeLength;
            }

            public long GetMapValue(long sourceValue)
            {
                if (sourceValue >= _sourceRangeStart && sourceValue < _sourceRangeStart + _rangeLength)
                {
                    long offset = sourceValue - _sourceRangeStart;
                    return _destinationRangeStart + offset;
                }

                return -1;
            }
        }
    }
}
