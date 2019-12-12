using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt");
            var pattern = @"<x=(?<x>[\-\+]?[0-9]+), y=(?<y>[\-\+]?[0-9]+), z=(?<z>[\-\+]?[0-9]+)>";
            var index = 0;
            var moons = 
              input.Select(i => {
                  var match = Regex.Match(i, pattern);
                  var x = int.Parse(match.Groups["x"].Value);
                  var y = int.Parse(match.Groups["y"].Value);
                  var z = int.Parse(match.Groups["z"].Value);
                  return new Moon(index++, x, y, z);
              }).ToList();

            var step = 0;

            while (step < 1000) {
                CalcVelocity(moons);
                CalcNewPosition(moons);
                step++;
            }

            var energy = moons
            .Select(m => CalcEnergy(m.Position) * CalcEnergy(m.Velocity))
            .Sum();

            Console.Out.WriteLine(energy);
        }

        private static int CalcEnergy(Coordinates coords)
        {   
            var value = Math.Abs(coords.X) + Math.Abs(coords.Y) + Math.Abs(coords.Z);
            return value;
        }

        private static void CalcNewPosition(List<Moon> moons)
        {
            foreach(var moon in moons) {
                moon.Position.X = moon.Position.X + moon.Velocity.X;
                moon.Position.Y = moon.Position.Y + moon.Velocity.Y;
                moon.Position.Z = moon.Position.Z + moon.Velocity.Z;

            }
        }

        private static void CalcVelocity(List<Moon> moons)
        {
            foreach(var root in moons) {
                foreach(var other in moons.Where(m => m.Index != root.Index)) {
                    if (root.Position.X < other.Position.X) {
                        root.Velocity.X += 1;
                    } else if (root.Position.X > other.Position.X) {
                        root.Velocity.X -= 1;
                    }
                    
                    if (root.Position.Y < other.Position.Y) {
                        root.Velocity.Y += 1;
                    } else if (root.Position.Y > other.Position.Y) {
                        root.Velocity.Y -= 1;
                    }
                    
                    if (root.Position.Z < other.Position.Z) {
                        root.Velocity.Z += 1;
                    } else if (root.Position.Z > other.Position.Z) {
                        root.Velocity.Z -= 1;
                    }
                }
            }
        }
    }

    class Moon {
        public Coordinates Position { get; set; }
        public Coordinates Velocity { get; set; }
        public readonly int Index;
        public Moon(int index, int x, int y, int z) {
            Velocity = new Coordinates(0,0,0);
            Position = new Coordinates(x, y,z);
            Index = index;
        }
    }

    class Coordinates {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Coordinates(int x, int y, int z) {
            X = x;
            Y = y;
            Z = z;
        }

    }
}
