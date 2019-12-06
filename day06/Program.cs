using System;
using System.Linq;
using System.Collections.Generic;

namespace day06
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt");
            var planets = GeneratePlanetList(input);
            var count = CountOrbits(planets);

            Console.Out.WriteLine(count);
        }

        private static int CountOrbits(List<Planet> orbits) {
            var sum = 0;
            foreach(var orbit in orbits) {
                var current = orbit;
                while(current.Parent != "COM") {
                    sum+=1;
                    current = orbits.Single(o => o.Name == current.Parent);
                }
                sum += 1;
            }
            return sum;
        }

        private static List<Planet> GeneratePlanetList(string[] input) {
            var result = new List<Planet>();
            foreach(var i in input) {
                var planets = i.Split(')');
                result.Add(new Planet(planets[1], planets[0]));
            }
            return result;
        }

    }

    class Planet {
        public readonly string Name;
        public readonly string Parent;

        public Planet(string name, string parent) {
            Name = name;
            Parent = parent;
        }
    }

}
