using System;
using System.IO;
using System.Linq;

namespace day1
{
    class Program
    {

        private static int AddFuel(int mass) {
            var sum = 0;
            while(mass > 0) {
                var fuel = ((int)mass/3)-2;
                if (fuel > 0) {
                  sum += fuel;
                }
                mass = fuel;
            }       
            return sum;  
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var inputParsed = input.Select(x => int.Parse(x)).ToList();

            var sum = inputParsed.Select(x => ((int)x/3) - 2).Sum();
            Console.Out.WriteLine(sum);

            var sum2 = inputParsed.Select(x => AddFuel(x)).Sum();
            Console.Out.WriteLine(sum2);

        }
    }
}
