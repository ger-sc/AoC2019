using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace day17 {
  public static class Extensions {
    public static int GetOrDefault(this IDictionary<Point, int> map, Point p) {
      return !map.ContainsKey(p) ? 0 : map[p];
    }
  }

  internal static class Program {
    private static readonly IDictionary<Point, int> Map = new Dictionary<Point, int>();

    private static void Main() {
      Part1();
      Part2();
    }

    private static void Part2() {
      var path = FindPath();
      var regex = Regex.Match(path, @"^(.{1,21})\1*(.{1,21})(?:\1|\2)*(.{1,21})(?:\1|\2|\3)*$");

      var a = regex.Groups[1].Value.TrimEnd(',');
      var b = regex.Groups[2].Value.TrimEnd(',');
      var c = regex.Groups[3].Value.TrimEnd(',');
      var main = FindMain(path, a, b, c);

      var input = System.IO.File.ReadAllText("input.txt").Split(",").Select(BigInteger.Parse).ToArray();
      input[0] = 2;
      var cts = new CancellationTokenSource();
      var commands = new ConcurrentQueue<BigInteger>();
      var code = new IntCodeInterpreter(input, m => commands.Enqueue(m), cts.Token);

      var t = new Thread(code.Runner);
      t.Start();

      SendMessage(main, code);
      SendMessage(a, code);
      SendMessage(b, code);
      SendMessage(c, code);
      SendMessage("n", code);

      t.Join();

      Console.Out.WriteLine(commands.Last());
    }

    private static string FindMain(string path, string a, string b, string c) {
      var sb = new StringBuilder();
      while (path.Length > 0) {
        if (path.StartsWith(a)) {
          sb.Append("A,");
          path = path.Substring(a.Length + 1);
        }
        else if (path.StartsWith(b)) {
          sb.Append("B,");
          path = path.Substring(b.Length + 1);
        }
        else if (path.StartsWith(c)) {
          sb.Append("C,");
          path = path.Substring(c.Length + 1);
        }
      }

      return sb.ToString().TrimEnd(',');
    }

    private enum Direction {
      North = 0,
      East = 1,
      South = 2,
      West = 3
    }

    private static string FindPath() {
      var position = Map.Single(x => x.Value == '^').Key;
      var dir = Direction.North;
      var sb = new StringBuilder();
      var end = false;
      while (!end) {
        var newDir = FindNewDir(position, dir);


        if (newDir == "L") {
          dir = (Direction) (((int) dir + 3) % 4);
        }
        else {
          dir = (Direction) (((int) dir + 1) % 4);
        }

        if (!NextStep(ref position, dir)) {
          end = true;
        }
        else {
          sb.Append(newDir);
          sb.Append(",");
          var steps = 1;
          while (NextStep(ref position, dir)) {
            steps++;
          }

          sb.Append(steps);
          sb.Append(",");
        }
      }

      return sb.ToString();
    }

    private static bool NextStep(ref Point position, Direction dir) {
      Point next;
      switch (dir) {
        case Direction.North:
          next = new Point(position.X, position.Y - 1);
          if (Map.GetOrDefault(next) == 35) {
            position = next;
            return true;
          }
          else {
            return false;
          }
        case Direction.East:
          next = new Point(position.X + 1, position.Y);
          if (Map.GetOrDefault(next) == 35) {
            position = next;
            return true;
          }
          else {
            return false;
          }
        case Direction.South:
          next = new Point(position.X, position.Y + 1);
          if (Map.GetOrDefault(next) == 35) {
            position = next;
            return true;
          }
          else {
            return false;
          }
        case Direction.West:
          next = new Point(position.X - 1, position.Y);
          if (Map.GetOrDefault(next) == 35) {
            position = next;
            return true;
          }
          else {
            return false;
          }
        default:
          throw new ArgumentException("Unknown direction", nameof(dir));
      }
    }

    private static string FindNewDir(Point position, Direction dir) {
      switch (dir) {
        case Direction.North:
          return Map.GetOrDefault(new Point(position.X - 1, position.Y)) == 35 ? "L" : "R";
        case Direction.East:
          return Map.GetOrDefault(new Point(position.X, position.Y - 1)) == 35 ? "L" : "R";
        case Direction.South:
          return Map.GetOrDefault(new Point(position.X + 1, position.Y)) == 35 ? "L" : "R";
        case Direction.West:
          return Map.GetOrDefault(new Point(position.X, position.Y + 1)) == 35 ? "L" : "R";
        default:
          throw new ArgumentException("Unknown direction", nameof(dir));
      }
    }

    private static void SendMessage(string main, IntCodeInterpreter code) {
      var parts = StringToMessage(main);
      foreach (var part in parts) {
        code._queue.Enqueue(part);
      }
    }

    private static void Part1() {
      var input = System.IO.File.ReadAllText("input.txt").Split(",").Select(BigInteger.Parse).ToArray();
      var cts = new CancellationTokenSource();
      var commands = new ConcurrentQueue<BigInteger>();
      var code = new IntCodeInterpreter(input, t => commands.Enqueue(t), cts.Token);
      code.Runner();

      var p = new Point(0, 0);
      while (commands.TryDequeue(out var msg)) {
        switch ((int) msg) {
          case 10:
            p = new Point(0, p.Y + 1);
            break;
          default:
            Map[p] = (int) msg;
            p = new Point(p.X + 1, p.Y);
            break;
        }
      }

      DrawMap();
      var crossings = FindCrossings();
      var result = crossings.Select(point => point.X * point.Y).Sum();
      Console.Out.WriteLine(result);

      cts.Cancel();
    }

    private static IEnumerable<int> StringToMessage(string message) {
      return message.ToCharArray().Select(x => (int) x).Concat(new List<int> {10}).ToArray();
    }

    private static IEnumerable<Point> FindCrossings() {
      for (var y = 1; y < Map.Keys.Select(p => p.Y).Max(); y++) {
        for (var x = 1; x < Map.Keys.Select(p => p.X).Max(); x++) {
          var point = new Point(x, y);
          if (Map.ContainsKey(point)) {
            if (IsCrossing(point)) {
              yield return point;
            }
          }
        }
      }
    }

    private static bool IsCrossing(Point point) {
      var up = new Point(point.X, point.Y - 1);
      var right = new Point(point.X + 1, point.Y);
      var down = new Point(point.X, point.Y + 1);
      var left = new Point(point.X - 1, point.Y);
      return Map.GetOrDefault(up) == 35
             && Map.GetOrDefault(right) == 35
             && Map.GetOrDefault(down) == 35
             && Map.GetOrDefault(left) == 35;
    }

    private static void DrawMap() {
      for (var y = 0; y <= Map.Keys.Select(p => p.Y).Max(); y++) {
        for (var x = 0; x <= Map.Keys.Select(p => p.X).Max(); x++) {
          var point = new Point(x, y);
          if (Map.ContainsKey(point)) {
            Console.Out.Write((char) Map[point]);
          }
          else {
            Console.Out.Write(".");
          }
        }

        Console.Out.WriteLine();
      }
    }
  }
}