using System;
using System.Linq;
using System.Collections.Generic;

namespace day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt");
            var line1 = input[0].Split(',');
            var line2 = input[1].Split(',');

            var coord1 = TraceLine(line1);
            var coord2 = TraceLine(line2);

            var intersect = coord1.Intersect(coord2);

            var dist = intersect.Where(p => p.X != 0 && p.Y != 0).Select(p => Math.Abs(p.X) + Math.Abs(p.Y)).Min();

            Console.Out.WriteLine(dist);

            var result2 = CalcMinDistance(intersect, coord1, coord2);

            Console.Out.WriteLine(result2);
        }

        private static int CalcMinDistance(IEnumerable<Point> intersects, List<Point> coord1, List<Point> coord2) {
            return intersects.Where(p => p.X != 0 && p.Y != 0).Select(i => {
                var dist1 = coord1.First(p => p.X == i.X && p.Y == i.Y);
                var dist2 = coord2.First(p => p.X == i.X && p.Y == i.Y);
                return dist1.Value + dist2.Value;
            }).Min();
        }

        private static List<Point> TraceLine(string[] instructions) {
            var result = new List<Point>();
            var x = 0;
            var y = 0;
            var cnt = 0;
            foreach (var i in instructions) {
                var dir = i.Substring(0, 1);
                var steps = int.Parse(i.Substring(1));
                int end;
                switch(dir){
                    case "R":
                      end = x + steps;
                      for(; x < end; x++) result.Add(new Point(x, y, cnt++));
                      break;
                    case "L":
                      end = x - steps;
                      for(; x > end; x--) result.Add(new Point(x, y, cnt++));
                      break;
                    case "U":
                      end = y + steps;
                      for(; y < end; y++) result.Add(new Point(x, y, cnt++));
                      break;
                    case "D":
                      end = y - steps;
                      for(; y > end; y--) result.Add(new Point(x, y, cnt++));
                      break;  
                }
            }
            return result;
        }
    }

    class Point {
        public readonly int X;
        public readonly int Y;
        public readonly int Value;

        public Point(int x, int y, int value) {
            X = x;
            Y = y;
            Value = value;
        }

        protected bool Equals(Point other) {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (X * 397) ^ Y;
            }
        }
    }
}
