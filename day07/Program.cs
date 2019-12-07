using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace day07
{
  class Program
  {

    public static void swap(ref int a, ref int b)
    {
      int temp = a;
      a = b;
      b = temp;
    }
    public static void permute(List<int[]> result, int[] list, int level, int m)
    {
      int i;
      if (level == m)
      {
        result.Add(list.ToArray());
      }
      else
        for (i = level; i <= m; i++)
        {
          swap(ref list[level], ref list[i]);
          permute(result, list, level + 1, m);
          swap(ref list[level], ref list[i]);
        }
    }

    static void Main(string[] args)
    {
      var input = System.IO.File.ReadAllText("input.txt");
      var program = input.Split(',').Select(x => int.Parse(x)).ToArray();
      var permutations = new List<int[]>();

      permute(permutations, Enumerable.Range(0, 5).ToArray(), 0, 4);

      var maxSignal = 0;

      foreach (var p in permutations)
      {
        var signal = 0;
        for (var amp = 0; amp < 5; amp++)
        {
          var amplifier = new Amplifier(program.ToArray(), res => { signal = res; });
          amplifier._queue.Enqueue(p[amp]);
          amplifier._queue.Enqueue(signal);
          amplifier.Runner();
          if (signal > maxSignal)
          {
            maxSignal = signal;
          }
        }
      }

      Console.Out.WriteLine(maxSignal);

      var maxSignal2 = 0;

      foreach (var p in permutations)
      {
        Amplifier[] amps = new Amplifier[5];
        amps[0] = new Amplifier(program.ToArray(), s => { amps[1]._queue.Enqueue(s); });
        amps[0]._queue.Enqueue(p[0] + 5);
        amps[1] = new Amplifier(program.ToArray(), s => { amps[2]._queue.Enqueue(s); });
        amps[1]._queue.Enqueue(p[1] + 5);
        amps[2] = new Amplifier(program.ToArray(), s => { amps[3]._queue.Enqueue(s); });
        amps[2]._queue.Enqueue(p[2] + 5);
        amps[3] = new Amplifier(program.ToArray(), s => { amps[4]._queue.Enqueue(s); });
        amps[3]._queue.Enqueue(p[3] + 5);
        amps[4] = new Amplifier(program.ToArray(), s =>
        {
          if (s > maxSignal2) maxSignal2 = s;
          amps[0]._queue.Enqueue(s);
        });
        amps[4]._queue.Enqueue(p[4] + 5);
        Thread[] threads = new Thread[5];
        for (var t = 0; t < 5; t++)
        {
          threads[t] = new Thread(new ThreadStart(amps[t].Runner));
          threads[t].Start();
        }
        amps[0]._queue.Enqueue(0);
        for (var t = 0; t < 5; t++)
        {
            threads[t].Join();
        }
      }
      Console.Out.WriteLine(maxSignal2);
    }
  }
}
