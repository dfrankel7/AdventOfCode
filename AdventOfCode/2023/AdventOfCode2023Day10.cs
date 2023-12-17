using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Console = VSConsole.Console;

namespace AdventOfCode
{
    internal class AdventOfCode2023Day10 : IAdventOfCodeDay
    {
        public enum Direction
        {
            None,
            North,
            South,
            East,
            West
        }

        public string Part1()
        {
            List<string> input = AdventOfCodeParser.ParseInputFile("Day10.txt", 2023);

            return new PathFollower(input).FindMaxPathDistance().ToString();
        }

        public string Part2()
        {
            List<string> input = AdventOfCodeParser.ParseInputFile("Day10.txt", 2023);

            PathFollower pathFollower = new PathFollower(input, false);
            pathFollower.FindMaxPathDistance();

            return pathFollower.CalculateLoopArea().ToString();
        }

        private class PathFollower
        {
            private readonly Dictionary<char, PathPipe> CHARACTER_TO_PATH_LOOKUP = new Dictionary<char, PathPipe>
            {
                { '|', new PathPipe(Direction.North, Direction.South) },
                { '-', new PathPipe(Direction.East , Direction.West)  },
                { 'L', new PathPipe(Direction.North, Direction.East)  },
                { 'J', new PathPipe(Direction.North, Direction.West)  },
                { '7', new PathPipe(Direction.South, Direction.West)  },
                { 'F', new PathPipe(Direction.South, Direction.East)  },
            };

            private readonly Dictionary<Direction, List<int>> DIRECTIONS_TO_CHECK = new Dictionary<Direction, List<int>>
            {
                { Direction.North, new List<int> {  0, -1 } },
                { Direction.South, new List<int> {  0,  1 } },
                { Direction.East,  new List<int> {  1,  0 } },
                { Direction.West,  new List<int> { -1,  0 } },
            };

            private char[,] _characterMap;
            private int[,] _pathDistances;
            private int _xMax, _yMax;
            private int _x1Position, _x2Position, _y1Position, _y2Position;
            private Direction _path1Direction, _path2Direction;
            private bool _followSecondPath;
            private List<int[]> _pathPositions;

            public int _pathLength { get; private set; }
            public HashSet<int[]> pathCoords { get; private set; }

            class ArrayEqualityComparer : IEqualityComparer<int[]>
            {
                public bool Equals(int[] x, int[] y)
                {
                    return x[0] == y[0] && x[1] == y[1];
                }

                public int GetHashCode([DisallowNull] int[] obj)
                {
                    return Tuple.Create(obj[0], obj[1]).GetHashCode();
                }
            }

            public PathFollower(List<string> input, bool followSecondPath = true)
            {
                _yMax = input.Count;
                _xMax = input[0].Length;
                _pathLength = 1;
                _characterMap = new char[_xMax, _yMax];
                _pathDistances = new int[_xMax, _yMax];
                _path1Direction = _path2Direction = Direction.None;
                pathCoords = new HashSet<int[]>(new ArrayEqualityComparer());
                _followSecondPath = followSecondPath;
                _pathPositions = new List<int[]>();

                for (int y = 0; y < _yMax; y++)
                {
                    for (int x = 0; x < _xMax; x++)
                    {
                        _characterMap[x, y] = input[y][x];
                        _pathDistances[x, y] = -1;

                        if (input[y][x] == 'S')
                        {
                            _x1Position = _x2Position = x;
                            _y1Position = _y2Position = y;
                            _pathDistances[x, y] = 0;
                            pathCoords.Add(new int[] { x, y });
                            _pathPositions.Add(new int[] { x, y });
                        }
                    }
                }

                int xStart = _x1Position;
                int yStart = _y1Position;

                foreach (var pair in DIRECTIONS_TO_CHECK)
                {
                    int x = xStart + pair.Value[0];
                    int y = yStart + pair.Value[1];

                    if (x < 0 || x > _xMax - 1 || y < 0 || y > _yMax - 1)
                    {
                        continue;
                    }

                    char character = _characterMap[x, y];

                    if (character == '.')
                    {
                        continue;
                    }

                    PathPipe pipe = CHARACTER_TO_PATH_LOOKUP[character];

                    foreach (Direction direction in pipe.availableDirections)
                    {
                        int xToCheck = x + DIRECTIONS_TO_CHECK[direction][0];
                        int yToCheck = y + DIRECTIONS_TO_CHECK[direction][1];

                        if (xStart == xToCheck && yStart == yToCheck)
                        {
                            if (_path1Direction == Direction.None)
                            {
                                _x1Position = x;
                                _y1Position = y;
                                _pathDistances[_x1Position, _y1Position] = _pathLength;
                                _path1Direction = pipe.GetNextDirection(direction);
                                pathCoords.Add(new int[] { x, y });
                                _pathPositions.Add(new int[] { x, y });
                            }
                            else
                            {
                                if (_followSecondPath)
                                {
                                    _x2Position = x;
                                    _y2Position = y;
                                    _pathDistances[_x2Position, _y2Position] = _pathLength;
                                    _path2Direction = pipe.GetNextDirection(direction);
                                    pathCoords.Add(new int[] { x, y });
                                }
                            }
                        }
                    }
                }
            }

            public int FindMaxPathDistance()
            {
                while (_x1Position != _x2Position || _y1Position != _y2Position)
                {
                    _pathLength++;
                    List<int> positionOffsets = DIRECTIONS_TO_CHECK[_path1Direction];
                    _x1Position += positionOffsets[0];
                    _y1Position += positionOffsets[1];
                    _pathDistances[_x1Position, _y1Position] = _pathLength;
                    char character = _characterMap[_x1Position, _y1Position];

                    if (character == 'S')
                    {
                        break;
                    }

                    PathPipe pipe = CHARACTER_TO_PATH_LOOKUP[character];
                    _path1Direction = pipe.GetNextDirection(PathPipe.GetInverseDirection(_path1Direction));
                    pathCoords.Add(new int[] { _x1Position, _y1Position });
                    _pathPositions.Add(new int[] { _x1Position, _y1Position });

                    if (_followSecondPath)
                    {
                        positionOffsets = DIRECTIONS_TO_CHECK[_path2Direction];
                        _x2Position += positionOffsets[0];
                        _y2Position += positionOffsets[1];
                        _pathDistances[_x2Position, _y2Position] = _pathLength;
                        pipe = CHARACTER_TO_PATH_LOOKUP[_characterMap[_x2Position, _y2Position]];
                        _path2Direction = pipe.GetNextDirection(PathPipe.GetInverseDirection(_path2Direction));
                        pathCoords.Add(new int[] { _x2Position, _y2Position });
                    }
                }

                return _pathLength;
            }

            // Shoestring Formula + Pick's Theorm
            public int CalculateLoopArea()
            {
                int area = 0;

                for (int i = 0; i < _pathPositions.Count; i++)
                {
                    int[] position1 = new int[]
                    {
                        _pathPositions[i][0],
                        _pathPositions[i][1],
                    };

                    int[] position2 = new int[2];
                    
                    if (i == _pathPositions.Count - 1)
                    {
                        position2[0] = _pathPositions[0][0];
                        position2[1] = _pathPositions[0][1];
                    }
                    else
                    {
                        position2[0] = _pathPositions[i + 1][0];
                        position2[1] = _pathPositions[i + 1][1];
                    }

                    area += (position1[0] * position2[1]) - (position1[1] * position2[0]);
                }

                area /= 2;
                int inside = area - (_pathPositions.Count / 2) + 1;

                return inside;
            }
        }

        private class PathPipe
        {
            public List<Direction> availableDirections { get; private set; }

            public PathPipe(Direction direction1, Direction direction2)
            {
                availableDirections = new List<Direction>
                {
                    direction1,
                    direction2
                };
            }

            public bool CanReachStartPosition(Direction directionToStartPosition)
            {
                return availableDirections.Contains(directionToStartPosition);
            }

            public Direction GetNextDirection(Direction directionWeCameFrom)
            {
                if (availableDirections.Contains(directionWeCameFrom))
                {
                    int directionIndex = availableDirections.IndexOf(directionWeCameFrom);
                    directionIndex = (directionIndex + 1) % availableDirections.Count;

                    return availableDirections[directionIndex];
                }

                return Direction.None;
            }

            public static Direction GetInverseDirection(Direction direction)
            {
                switch (direction)
                {
                    case Direction.North:
                        return Direction.South;
                    case Direction.South:
                        return Direction.North;
                    case Direction.East:
                        return Direction.West;
                    default:
                        return Direction.East;
                }
            }
        }
    }
}
