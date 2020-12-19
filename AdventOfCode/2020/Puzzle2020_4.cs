using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace AdventOfCode
{
    class Puzzle2020_4
    {
        private static readonly List<string> _requiredFields = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
        private static readonly List<string> _validEyeColors = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
        private const string INPUT_FILE_PATH = "C:\\Users\\Dan\\Documents\\Code Projects\\AdventOfCode\\AdventOfCode\\2020\\Input\\Puzzle2020_4_input.txt";
        private const string FIELD_TO_IGNORE = "cid";

        public static int CountValidPassports()
        {
            int validPassportCount = 0;

            StreamReader streamReader = new StreamReader(INPUT_FILE_PATH);
            string currentPassport = string.Empty;
            string[] lines = streamReader.ReadToEnd().Split("\r\n");

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrEmpty(line))
                {
                    if (IsValidPassportPart2(currentPassport))
                    {
                        validPassportCount++;
                    }

                    currentPassport = string.Empty;
                }
                else
                {
                    currentPassport += line + " ";
                }
            }

            return validPassportCount;
        }

        private static bool IsValidPassportPart1(string passportString)
        {
            for (int i = 0; i < _requiredFields.Count; i++)
            {
                if (!passportString.Contains(_requiredFields[i] + ":"))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsValidPassportPart2(string passportString)
        {
            bool isValidForPart1 = IsValidPassportPart1(passportString);

            if (!isValidForPart1)
            {
                return false;
            }

            string[] fields = passportString.Trim().Split(" ");

            for (int i = 0; i < fields.Length; i++)
            {
                string[] splitFields = fields[i].Split(":");
                string field = splitFields[0];
                string value = splitFields[1];
                bool validDate = false;
                int year = 0;

                switch (field)
                {
                    case "byr":
                        validDate = int.TryParse(value, out year);
                        if (!validDate || year < 1920 || year > 2002)
                        {
                            return false;
                        }
                        break;
                    case "iyr":
                        validDate = int.TryParse(value, out year);
                        if (!validDate || year < 2010 || year > 2020)
                        {
                            return false;
                        }
                        break;
                    case "eyr":
                        validDate = int.TryParse(value, out year);
                        if (!validDate || year < 2020 || year > 2030)
                        {
                            return false;
                        }
                        break;
                    case "hgt":
                        string unit = value.Substring(value.Length - 2);
                        int height = 0;
                        if (unit == "in")
                        {
                            if (int.TryParse(value.Substring(0, value.Length - 2), out height))
                            {
                                if (height < 59 || height > 76)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (unit == "cm")
                        {
                            if (int.TryParse(value.Substring(0, value.Length - 2), out height))
                            {
                                if (height < 150 || height > 193)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }   
                        else
                        {
                            return false;
                        }
                        break;
                    case "hcl":
                        if (!Regex.IsMatch(value, "#[0-9,a-f][0-9,a-f][0-9,a-f][0-9,a-f][0-9,a-f][0-9,a-f]"))
                        {
                            return false;
                        }
                        break;
                    case "ecl":
                        if (!_validEyeColors.Contains(value))
                        {
                            return false;
                        }
                        break;
                    case "pid":
                        if (value.Length != 9)
                        {
                            return false;
                        }
                        break;
                }
            }

            return true;
        }
    }
}
