using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Drawing;

namespace day13
{
  enum Tile
  {
    Empty = 0,
    Wall = 1,
    Block = 2,
    Paddle = 3,
    Ball = 4
  }

  class Program
  {
    static void Main(string[] args)
    {
      var input = System.IO.File.ReadAllText("input.txt").Split(",").Select(x => BigInteger.Parse(x)).ToArray();
      var outPut = new List<BigInteger>();
      var code = new IntCodeInterpreter(input, t => outPut.Add(t));
      code.Runner();
      var index = 0;
      var step = 3;
      var blockTiles = 0;
      while (index < outPut.Count())
      {
        var instr = outPut.Skip(index).Take(step).ToArray();
        var x = instr[0];
        var y = instr[1];
        var id = instr[2];
        if (id == 2)
        {
          blockTiles++;
        }
        index += step;
      }

      Console.Out.WriteLine(blockTiles);
      input = System.IO.File.ReadAllText("input.txt").Split(",").Select(x => BigInteger.Parse(x)).ToArray();
      input[0] = 2;
      BigInteger points = 0;
      var isntructions = new ConcurrentQueue<BigInteger>();
      var display = new Dictionary<Point, Tile>();
      code = new IntCodeInterpreter(input, t => isntructions.Enqueue(t));
      var t = new Thread(new ThreadStart(code.Runner));
      t.Start();
      while (t.IsAlive)
      {
        var instr = new List<BigInteger>();
        Thread.Sleep(TimeSpan.FromMilliseconds(10));
        while (isntructions.TryDequeue(out var cmd))
        {
          instr.Add(cmd);

        }
        var game = DrawDisplay(instr, display, ref points);
        instr = new List<BigInteger>();
        if (game.Item1 < game.Item2)
        {
          code._queue.Enqueue(-1);
        }
        else if (game.Item1 > game.Item2)
        {
          code._queue.Enqueue(1);
        }
        else
        {
          code._queue.Enqueue(0);
        }
      }
      Console.Out.WriteLine(points);
    }

    private static Tuple<int, int> DrawDisplay(List<BigInteger> instr, IDictionary<Point, Tile> display,  ref BigInteger points)
    {
      
      var index = 0;
      var step = 3;
      var ballX = 0;
      var paddleX = 0;
      while (index < instr.Count())
      {
        var cmd = instr.Skip(index).Take(step).ToArray();
        var x = cmd[0];
        var y = cmd[1];
        var id = cmd[2];
        if (x == -1)
        {
          points = id;
        }
        else
        {
          var point = new Point((int)x, (int)y);
          var tile = (Tile)((int)id);
          display[point] = tile;
          if (tile == Tile.Ball)
          {
            ballX = (int)x;
          }
          if (tile == Tile.Paddle)
          {
            paddleX = (int)x;
          }
        }
        index += step;
      }
      DrawArray(display);
      return new Tuple<int, int>(ballX, paddleX);

    }

    private static void DrawArray(IDictionary<Point, Tile> display)
    {
      var maxX = display.Keys.Select(x => x.X).Max();
      var maxY = display.Keys.Select(x => x.Y).Max();
      for (var y = 0; y <= maxY; y++)
      {
        for (var x = 0; x <= maxX; x++)
        {
          var point = new Point(x, y);
          if (display.ContainsKey(point))
          {
            switch (display[point])
            {
              case Tile.Empty:
                Console.Out.Write(" ");
                break;
              case Tile.Ball:
                Console.Out.Write("0");
                break;
              case Tile.Block:
                Console.Out.Write("#");
                break;
              case Tile.Paddle:
                Console.Out.Write("-");
                break;
              case Tile.Wall:
                Console.Out.Write("|");
                break;
              default:
                Console.Out.Write(" ");
                break;
            }
          }
        }
        Console.Out.WriteLine();
      }
    }
  }
}
