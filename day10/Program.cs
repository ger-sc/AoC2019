using System;
using System.Collections.Generic;
using System.Linq;

namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt");
            var map = new List<Asteroid>();
            for(var y = 0; y < input.Length; y++) {
                for(var x = 0; x < input[y].Length; x++) {
                    if (input[y][x] == '#') {
                        map.Add(new Asteroid(x, y));
                    }
                }
            }

            var result = new Dictionary<Asteroid, int>();

            foreach(var asteroid in map) {
                foreach(var otherAsteroid in map) {
                    if (!IsBlocked(asteroid, otherAsteroid, map)) {
                        if (!result.ContainsKey(asteroid)) {
                            result[asteroid] = 1;
                        } else {
                            result[asteroid] = result[asteroid] + 1;
                        }
                    }
                }
            }

            var solution = result.Where(x => x.Value == result.Values.Max()).Single().Value;

            Console.Out.WriteLine(solution);

        }

        private static bool IsBlocked(Asteroid from, Asteroid to, List<Asteroid> map) {
            
            if (from.X == to.X && from.Y == to.Y) return true;

            var deltaX = to.X - from.X;
            var deltaY = to.Y - from.Y;

            var stepX = 0m;
            var stepY = 0m;

            if (Math.Abs(deltaX) < Math.Abs(deltaY)) {
                stepX = deltaY == 0 ? 0 : deltaX/Math.Abs(deltaY);
                stepY = deltaY > 0 ? 1 : -1;
            } else {
                stepX = deltaX > 0 ? 1 : -1;
                stepY = deltaX == 0 ? 0m : deltaY/Math.Abs(deltaX);
            }

            var cleanMap = map
              .Where(a => !(a.X == from.X && a.Y == from.Y))
              .Where(a => !(a.X == to.X && a.Y == to.Y))
              .ToList();

            var a = from;

            while(Math.Abs(a.X - to.X) > 0.00001m || Math.Abs(a.Y - to.Y) > 0.00001m) {
                a = new Asteroid(a.X + stepX, a.Y + stepY);
                if (cleanMap.Any(cm => Math.Abs(cm.X - a.X) < 0.01m && Math.Abs(cm.Y - a.Y) < 0.01m)) {
                    return true;
                }
            }
            return false;

        }

    }

    class Asteroid {
        public readonly decimal X;
        public readonly decimal Y;

        public Asteroid(decimal x, decimal y) {
            X = x;
            Y = y;
        }

        protected bool Equals(Asteroid other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Asteroid) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public override string ToString() {
            return X+","+Y;
        }
    }

}
