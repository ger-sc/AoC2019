using System;
using System.Linq;

namespace day09
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("input.txt").Split(",").Select(x => int.Parse(x));
            var amp = new Amplifier(input.ToArray(), t => {Console.WriteLine(t);});
            amp._queue.Enqueue(1);
            amp.Runner();
            var amp2 = new Amplifier(input.ToArray(), t => {Console.WriteLine(t);});
            amp2._queue.Enqueue(2);
            amp2.Runner();
        }
    }
}
