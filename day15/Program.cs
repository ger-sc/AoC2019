using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Numerics;
using System.Drawing;
using System.Threading;

namespace day15 {
  public static class DictExtensions {
    public static string GetOrDefault(this IDictionary<Point, string> dict, Point key) {
      return dict.ContainsKey(key) ? dict[key] : " ";
    }
  }

  internal enum Direction {
    North = 1,
    South = 2,
    West = 3,
    East = 4
  }

  internal static class Program {
    private static readonly ConcurrentQueue<BigInteger> Output = new ConcurrentQueue<BigInteger>();
    private static readonly IDictionary<Point, string> Map = new Dictionary<Point, string>();

    private static readonly IDictionary<Point, int> Distances = new Dictionary<Point, int>();

    public static void Main() {
      var input = System.IO.File.ReadAllText("input.txt").Split(",").Select(BigInteger.Parse).ToArray();
      var droid = new Point(0, 0);
      var source = new Point(-1, -1);
      var dir = Direction.North;
      var cts = new CancellationTokenSource();
      var code = new IntCodeInterpreter(input, msg => Output.Enqueue(msg), cts.Token);
      var t = new Thread(code.Runner);
      Map.Add(droid, ".");
      Distances.Add(droid, 0);
      t.Start();
      while (t.IsAlive && droid != source) {
        source = new Point(0, 0);
        DrawMap(droid);
        code._queue.Enqueue((int) dir);
        BigInteger msg;
        while (!Output.TryDequeue(out msg)) {
          Thread.Sleep(TimeSpan.FromMilliseconds(1));
        }

        Point newPosition;
        switch ((int) msg) {
          case 0:
            var wall = GetPosition(droid, dir);
            if (!Map.ContainsKey(wall)) {
              Map.Add(wall, "█");
            }

            dir = TurnRight(dir);
            break;
          case 1:
            newPosition = GetPosition(droid, dir);

            if (Distances.TryGetValue(newPosition, out var newDistance)) {
              Distances[newPosition] = Math.Min(newDistance, Distances[droid] + 1);
            }
            else {
              Distances.Add(newPosition, Distances[droid] + 1);
            }

            if (!Map.ContainsKey(newPosition)) {
              Map.Add(newPosition, ".");
            }

            droid = newPosition;
            var left = GetLeftField(droid, dir);
            if (left != "█") {
              dir = TurnLeft(dir);
            }

            break;
          case 2:
            newPosition = GetPosition(droid, dir);
            if (Distances.TryGetValue(newPosition, out var newDistance2)) {
              Distances[newPosition] = Math.Min(newDistance2, Distances[droid] + 1);
            }
            else {
              Distances.Add(newPosition, Distances[droid] + 1);
            }

            if (!Map.ContainsKey(newPosition)) {
              Map.Add(newPosition, "2");
            }

            droid = newPosition;
            break;
        }
      }

      var oxygenPos = Map.Single(x => x.Value == "2").Key;
      Console.Out.WriteLine(Distances[oxygenPos]);

      Map[oxygenPos] = "O";
      var timer = 0;
      do {
        var oxygen = Map.Where(x => x.Value == "O").ToDictionary(x => x.Key, x => x.Value);
        foreach (var oxy in oxygen.Keys) {
          var space = GetNoWalls(oxy);
          foreach (var s in space) {
            Map[s] = "O";
          }
        }

        DrawMap(new Point(0, 0));
        timer++;
      } while (Map.Any(x => x.Value == "."));

      Console.Out.WriteLine(timer);
      cts.Cancel();
    }

    private static IEnumerable<Point> GetNoWalls(Point p) {
      if (Map.GetOrDefault(new Point(p.X - 1, p.Y)) == ".") {
        yield return new Point(p.X - 1, p.Y);
      }

      if (Map.GetOrDefault(new Point(p.X + 1, p.Y)) == ".") {
        yield return new Point(p.X + 1, p.Y);
      }

      if (Map.GetOrDefault(new Point(p.X, p.Y - 1)) == ".") {
        yield return new Point(p.X, p.Y - 1);
      }

      if (Map.GetOrDefault(new Point(p.X, p.Y + 1)) == ".") {
        yield return new Point(p.X, p.Y + 1);
      }
    }

    private static string GetLeftField(Point droid, Direction dir) {
      switch (dir) {
        case Direction.North:
          return Map.GetOrDefault(new Point(droid.X - 1, droid.Y));
        case Direction.South:
          return Map.GetOrDefault(new Point(droid.X + 1, droid.Y));
        case Direction.West:
          return Map.GetOrDefault(new Point(droid.X, droid.Y + 1));
        case Direction.East:
          return Map.GetOrDefault(new Point(droid.X, droid.Y - 1));
        default:
          throw new ArgumentException("Unknown direction", nameof(dir));
      }
    }

    private static Point GetPosition(Point start, Direction dir) {
      switch (dir) {
        case Direction.North:
          return new Point(start.X, start.Y - 1);
        case Direction.South:
          return new Point(start.X, start.Y + 1);
        case Direction.West:
          return new Point(start.X - 1, start.Y);
        case Direction.East:
          return new Point(start.X + 1, start.Y);
        default:
          throw new ArgumentException("Unknown direction", nameof(dir));
      }
    }

    private static void DrawMap(Point droid) {
      Console.Out.WriteLine();
      var minX = Map.Select(x => x.Key.X).Min();
      var maxX = Map.Select(x => x.Key.X).Max();
      var minY = Map.Select(x => x.Key.Y).Min();
      var maxY = Map.Select(x => x.Key.Y).Max();
      for (var y = minY; y <= maxY; y++) {
        for (var x = minX; x <= maxX; x++) {
          var p = new Point(x, y);
          if (p == droid) {
            Console.Out.Write("D");
          }
          else {
            Console.Out.Write(Map.ContainsKey(p) ? Map[p] : " ");
          }
        }

        Console.Out.WriteLine();
      }

      Console.Out.WriteLine();
    }

    private static Direction TurnRight(Direction dir) {
      switch (dir) {
        case Direction.North:
          return Direction.East;
        case Direction.South:
          return Direction.West;
        case Direction.West:
          return Direction.North;
        case Direction.East:
          return Direction.South;
        default:
          throw new ArgumentException("Unknown direction", nameof(dir));
      }
    }

    private static Direction TurnLeft(Direction dir) {
      switch (dir) {
        case Direction.North:
          return Direction.West;
        case Direction.South:
          return Direction.East;
        case Direction.West:
          return Direction.South;
        case Direction.East:
          return Direction.North;
        default:
          throw new ArgumentException("Unknown direction", nameof(dir));
      }
    }
  }
}