using System.Threading.Tasks;

namespace AdventOfCode.Common;

public interface IAdventOfCodeDayAsync
{
    public Task<string> Part1();
    public Task<string> Part2();
}