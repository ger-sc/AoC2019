using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Collections.Concurrent;

namespace day11
{

    enum Direction : int {
        Up = 0,
        Left = 1,
        Down = 2,
        Right = 3
    }
    class Program
    {
        static void Main(string[] args)
        {
            var msgs = new ConcurrentQueue<BigInteger>();
            var input = System.IO.File.ReadAllText("input.txt").Split(",").Select(x => BigInteger.Parse(x)).ToArray();
            var amp = new Amplifier(input, o => msgs.Enqueue(o));
            var t = new Thread(new ThreadStart(amp.Runner));
            
            //Uncomment for part 1
            //amp._queue.Enqueue(0); 
            
            //Comment for part 1
            amp._queue.Enqueue(1);
            
            t.Start();
            var colors = new Dictionary<Position, BigInteger>();

            var direction = Direction.Up;
            var position = new Position(0,0);
            
            while(t.IsAlive) {
                BigInteger color;
                while (!msgs.TryDequeue(out color))
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(1));
                }
                colors[position] = color;
                BigInteger turn;
                while (!msgs.TryDequeue(out turn))
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(1));
                }
                direction = Turn(direction, turn);
                position = Advance(position, direction);
                if (colors.ContainsKey(position)) {
                    amp._queue.Enqueue(colors[position]);
                } else {
                    amp._queue.Enqueue(0);
                }
            }

            var xMin = colors.Keys.Select(x => x.X).Min();
            var xMax = colors.Keys.Select(x => x.X).Max();

            var yMin = colors.Keys.Select(x => x.Y).Min();
            var yMax = colors.Keys.Select(x => x.Y).Max();

            for(var y = yMin; y <= yMax; y++) {
                for(var x = xMin; x <= xMax; x++) {
                    var pos = new Position(x, y);
                    if (colors.ContainsKey(pos)) {
                        if (colors[pos] == 0) {
                            Console.Out.Write(" ");
                        } else {
                            Console.Out.Write("█");
                        }
                    } else {
                        Console.Out.Write(" ");
                    }
                }
                Console.Out.WriteLine();
            }

        }

        private static Position Advance(Position position, Direction direction) {
            switch(direction) {
                case Direction.Up:
                    return new Position(position.X, position.Y - 1);
                case Direction.Left:
                    return new Position(position.X - 1, position.Y);
                case Direction.Down:
                    return new Position(position.X, position.Y + 1);
                case Direction.Right:
                    return new Position(position.X + 1, position.Y);
                default:
                    throw new ArgumentException("Uknown direction", nameof(direction));
            }
        }
        private static Direction Turn(Direction current, BigInteger turn) {
            if (turn == 0) {
                var newDir = ((int)current + 1) % 4;
                return (Direction)newDir;
            } else {
                var newDir = ((int)current + 3) % 4;
                return (Direction)newDir;
            }
        }
    }

    class Position {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y) {
            X = x;
            Y = y;
        }

        protected bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }

}
