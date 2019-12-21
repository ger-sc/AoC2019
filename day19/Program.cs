using System;
using System.Linq;
using System.Numerics;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace day19 {
  internal static class Program {
    private static void Main() {
      var input = System.IO.File.ReadAllText("input.txt").Split(",").Select(BigInteger.Parse).ToArray();
      Part1(input);
      Part2(input);
    }

    private static void Part2(BigInteger[] input) {
      var y = 1000;
      var x = 0;
      var found = false;
      while (!found) {
        x = y / 2;
        while (!CheckPoint(input, new Point(x, y))) {
          x++;
        }

        var check = CheckPoint(input, new Point(x + 99, y - 99));
        if (check) {
          found = true;
        }
        else {
          y++;
        }
      }
      var result = new Point(x, y - 99);
      Console.Out.WriteLine(result.X*10000+result.Y);

    }

    private static bool CheckPoint(BigInteger[] input, Point p) {
      var messages = new ConcurrentQueue<BigInteger>();
      var cts = new CancellationTokenSource();
      var code = new IntCodeInterpreter(input, o => messages.Enqueue(o), cts.Token);
      code._queue.Enqueue(p.X);
      code._queue.Enqueue(p.Y);
      code.Runner();
      BigInteger msg;
      while (!messages.TryDequeue(out msg)) {
        Thread.Sleep(TimeSpan.FromMilliseconds(1));
      }
      return msg == 1;
    }
    
    private static void Part1(BigInteger[] input) {
      var effected = new List<Point>();
      for (var y = 0; y < 50; y++) {
        for (var x = 0; x < 50; x++) {
          var point = new Point(x, y);
          if (CheckPoint(input, point)) {
            effected.Add(point);
          }
        }
      }

      Console.Out.WriteLine(effected.Count);
    }
  }
}