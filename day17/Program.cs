using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace day17
{
    public static class Extensions {
        public static int GetOrDefault(this IDictionary<Point, int> map, Point p) {
            if (!map.ContainsKey(p)) {
                return 0;
            }
            else {
                return map[p];
            }
        }
    }
    
    class Program
    {
        
        private static IDictionary<Point, int> map = new Dictionary<Point,int>();
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("input.txt").Split(",").Select(BigInteger.Parse).ToArray();
            var cts = new CancellationTokenSource();
            var commands = new ConcurrentQueue<BigInteger>();
            var code = new IntCodeInterpreter(input, t => commands.Enqueue(t), cts.Token);
            code.Runner();

            var p = new Point(0,0);
            while (commands.TryDequeue(out var msg)) {
                switch ((int)msg) {
                    case 10:
                        p = new Point(0, p.Y+1);
                        break;
                    default:
                        map[p] = (int)msg;
                        p = new Point(p.X + 1, p.Y);
                        break;
                        
                }
            }

            DrawMap();
            var crossings = FindCrossings();
            var result = crossings.Select(p => p.X * p.Y).Sum();
            Console.Out.WriteLine(result);
        }

        private static IEnumerable<Point> FindCrossings() {
            for (var y = 1; y < map.Keys.Select(p => p.Y).Max(); y++) {
                for (var x = 1; x < map.Keys.Select(p => p.X).Max(); x++) {
                    var point = new Point(x, y);
                    if (map.ContainsKey(point)) {
                        if (IsCrossing(point)) {
                            yield return point;
                        }
                    }
                }
            }
        }

        private static bool IsCrossing(Point point) {
            var up = new Point(point.X, point.Y -1);
            var right = new Point(point.X+1, point.Y );
            var down = new Point(point.X, point.Y +1);
            var left = new Point(point.X - 1, point.Y);
            return map.GetOrDefault(up) == 35
                   && map.GetOrDefault(right) == 35
                   && map.GetOrDefault(down) == 35
                   && map.GetOrDefault(left) == 35;
        }

        private static void DrawMap() {
            for (var y = 0; y <= map.Keys.Select(p => p.Y).Max(); y++) {
                for (var x = 0; x <= map.Keys.Select(p => p.X).Max(); x++) {
                    var point = new Point(x, y);
                    if (map.ContainsKey(point)) {
                        Console.Out.Write((char)map[point]);
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
