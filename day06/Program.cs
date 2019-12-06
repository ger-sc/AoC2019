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

            var minDist = GetMinDistance(planets);

            Console.Out.WriteLine(minDist);

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

        private static int GetMinDistance(List<Planet> orbits) {
            var sanToRoot = GetPathToRoot(orbits.Single(o => o.Name == "SAN"), orbits);
            var youToRoot = GetPathToRoot(orbits.Single(o => o.Name == "YOU"), orbits);
           
            var dist = 0;
            foreach (var planet in sanToRoot) {
                var index = youToRoot.IndexOf(planet);
                if (index >= 0) {
                    dist += index;
                    break;
                }
                dist++;
            }

            return dist;
        }

        private static IList<Planet> GetPathToRoot(Planet p, IList<Planet> orbits)
        {
            var result = new List<Planet>();
            while(p.Parent != "COM") {
                p = orbits.Single(o => o.Name == p.Parent);
                result.Add(p);
            }

            return result;
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

        protected bool Equals(Planet other) {
            return Name == other.Name && Parent == other.Parent;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Planet) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Parent != null ? Parent.GetHashCode() : 0);
            }
        }
    }

}
