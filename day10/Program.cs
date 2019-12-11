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
      for (var y = 0; y < input.Length; y++)
      {
        for (var x = 0; x < input[y].Length; x++)
        {
          if (input[y][x] == '#')
          {
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

      var center = result.Where(x => x.Value == result.Values.Max()).Single();
      var solution = center.Value;

      Console.Out.WriteLine(solution);

      var angles = GetAngles(map.ToList(), center.Key);
    
      var sortedAngles = angles.Select(x => x.Key).OrderBy(x => x).ToArray();
      var counter = 0;
      var index = 0;
      var m = map.ToList();
      Asteroid lastRemoved = center.Key;
      while(counter < 200) {
          var angle = sortedAngles[index%sortedAngles.Length];
          var asteroid = angles[angle].SingleOrDefault(a => !IsBlocked(center.Key, a, m));
          if (asteroid != null) {
              m.Remove(asteroid);
              counter++;
              lastRemoved = asteroid;
          }
          index++;
      }
      Console.Out.WriteLine(lastRemoved.X * 100 + lastRemoved.Y);
    }

    private static Dictionary<double, List<Asteroid>> GetAngles(List<Asteroid> map, Asteroid center)
    {
      var mapWithOutCenter = map.Where(x => !(x.X == center.X && x.Y == center.Y)).ToList();
      return mapWithOutCenter
        .Select(a => new { Angle = GetAngle(center, a), Asteroid = a })
        .GroupBy(x => x.Angle)
        .OrderBy(x => x.Key)
        .ToDictionary(x => x.Key, x => x.Select(a => a.Asteroid).ToList());

    }

    private static double GetAngle(Asteroid center, Asteroid asteroid)
    {
      var a = center.Y - asteroid.Y;
      var b = asteroid.X - center.X;
      var r = (double)a / (double)b;
      var quadrant = GetQuadrant(a, b);
      var angle = RadToGrad(Math.Atan(r));
      if (quadrant == 2 || quadrant == 3) {
          angle += 180;
      } else if (quadrant == 4) {
          angle += 360;
      }
      return (90 - angle + 360) % 360;
    }

    private static int GetQuadrant(decimal a, decimal b)
    {
      if (a >= 0)
      {
        if (b >= 0)
        {
          return 1;
        }
        else
        {
          return 2;
        }
      }
      else
      {
        if (b >= 0)
        {
          return 4;
        }
        else
        {
          return 3;
        }
      }
    }

    private static double RadToGrad(double rad)
    {
      return rad * (180 / Math.PI);
    }

    private static bool IsBlocked(Asteroid from, Asteroid to, List<Asteroid> map)
    {

      if (from.X == to.X && from.Y == to.Y) return true;

      var deltaX = to.X - from.X;
      var deltaY = to.Y - from.Y;

      var stepX = 0m;
      var stepY = 0m;

      if (Math.Abs(deltaX) < Math.Abs(deltaY))
      {
        stepX = deltaY == 0 ? 0 : deltaX / Math.Abs(deltaY);
        stepY = deltaY > 0 ? 1 : -1;
      }
      else
      {
        stepX = deltaX > 0 ? 1 : -1;
        stepY = deltaX == 0 ? 0m : deltaY / Math.Abs(deltaX);
      }

      var cleanMap = map
        .Where(a => !(a.X == from.X && a.Y == from.Y))
        .Where(a => !(a.X == to.X && a.Y == to.Y))
        .ToList();

      var a = from;

      while (Math.Abs(a.X - to.X) > 0.00001m || Math.Abs(a.Y - to.Y) > 0.00001m)
      {
        a = new Asteroid(a.X + stepX, a.Y + stepY);
        if (cleanMap.Any(cm => Math.Abs(cm.X - a.X) < 0.01m && Math.Abs(cm.Y - a.Y) < 0.01m))
        {
          return true;
        }
      }
      return false;

    }

  }

  class Asteroid
  {
    public readonly decimal X;
    public readonly decimal Y;

    public Asteroid(decimal x, decimal y)
    {
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
      return Equals((Asteroid)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (X.GetHashCode() * 397) ^ Y.GetHashCode();
      }
    }

    public override string ToString()
    {
      return X + "," + Y;
    }
  }

}
