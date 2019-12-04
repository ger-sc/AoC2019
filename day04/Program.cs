using System;
using System.Linq;

namespace day04
{
    class Program
    {

        private static bool IsValid2(int number) {
            var chars = number.ToString().ToCharArray();
            return chars.GroupBy(x => x).Any(x => x.Count() == 2) && chars.OrderBy(x => x).SequenceEqual(chars);
        }

        private static bool IsValid(int number) {
            var chars = number.ToString().ToCharArray();
            return chars.Distinct().Count() < 6 && chars.OrderBy(x => x).SequenceEqual(chars);
        }

        static void Main(string[] args)
        {
            var min = 387638;
            var max = 919123;
            var sum1 = 0;
            var sum2 = 0;

            for(var i = min; i <= max; i++) {
                if (IsValid(i)) sum1++;
                if (IsValid2(i)) sum2++;
            }

            Console.Out.WriteLine(sum1);
            Console.Out.WriteLine(sum2);

        }
    }
}
