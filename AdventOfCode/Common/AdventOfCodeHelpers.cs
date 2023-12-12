using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Common
{
    public class AdventOfCodeHelpers
    {
        public static List<long> FindFactors(long number)
        {
            List<long> factors = new List<long>();
            long max = (long)Math.Sqrt(number);

            for (long factor = 2; factor <= max; ++factor)
            {
                if (number % factor == 0)
                {
                    factors.Add(factor);
                    if (factor != number / factor)
                    {
                        factors.Add(number / factor);
                    }
                }
            }

            return factors;
        }
    }
}
