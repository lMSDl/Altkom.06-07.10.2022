using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class FizzBuzz
    {
        public static List<string> Compute(int n)
        {
            return Enumerable.Range(1, n)
                .Select(a => $"{(a % 3 == 0 ? "Fizz" : string.Empty)}{(a % 5 == 0 ? "Buzz" : string.Empty)}")
                .Select((b, i) => string.IsNullOrEmpty(b) ? (i + 1).ToString() : b)
                .ToList();

        }
    }
}
