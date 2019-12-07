using System;
using System.Linq;
using System.Collections.Generic;

namespace day07
{
  class Program
  {

    private static int Amp(int phase, int signal, int[] program)
    {
      var index = 0;
      var output = "";
      var firstInput = true;
      while (program[index] != 99)
      {
        var instr = program[index];
        var input1 = program[(index + 1) % program.Length];
        var input2 = program[(index + 2) % program.Length];
        var input3 = program[(index + 3) % program.Length];
        var command = instr % 100;
        var modes = ((int)instr / 100).ToString().PadLeft(3, '0').ToCharArray().Reverse().ToArray();

        var mode1 = modes.Length > 0 ? int.Parse(modes[0].ToString()) : 0;
        var mode2 = modes.Length > 1 ? int.Parse(modes[1].ToString()) : 0;
        int value1, value2;

        switch (command)
        {
          case 1:
            value1 = mode1 == 0 ? program[input1] : input1;
            value2 = mode2 == 0 ? program[input2] : input2;
            program[input3] = value1 + value2;
            index += 4;
            break;
          case 2:
            value1 = mode1 == 0 ? program[input1] : input1;
            value2 = mode2 == 0 ? program[input2] : input2;
            program[input3] = value1 * value2;
            index += 4;
            break;
          case 3:
            if (firstInput)
            {
              program[input1] = phase;
              firstInput = false;
            }
            else
            {
              program[input1] = signal;
            }
            index += 2;
            break;
          case 4:
            value1 = mode1 == 0 ? program[input1] : input1;
            output += value1.ToString();
            index += 2;
            break;
          case 5:
            value1 = mode1 == 0 ? program[input1] : input1;
            value2 = mode2 == 0 ? program[input2] : input2;
            if (value1 > 0)
            {
              index = value2;
            }
            else
            {
              index += 3;
            }
            break;
          case 6:
            value1 = mode1 == 0 ? program[input1] : input1;
            value2 = mode2 == 0 ? program[input2] : input2;
            if (value1 == 0)
            {
              index = value2;
            }
            else
            {
              index += 3;
            }
            break;
          case 7:
            value1 = mode1 == 0 ? program[input1] : input1;
            value2 = mode2 == 0 ? program[input2] : input2;
            if (value1 < value2)
            {
              program[input3] = 1;
            }
            else
            {
              program[input3] = 0;
            }
            index += 4;
            break;
          case 8:
            value1 = mode1 == 0 ? program[input1] : input1;
            value2 = mode2 == 0 ? program[input2] : input2;
            if (value1 == value2)
            {
              program[input3] = 1;
            }
            else
            {
              program[input3] = 0;
            }
            index += 4;
            break;
        }
      }
      return int.Parse(output);
    }

    public static void swap(ref int a, ref int b)
    {
      int temp = a;
      a = b;
      b = temp;
    }
    public static void permute(List<int[]> result, int[] list, int level, int m)
    {
      int i;
      if (level == m)
      {
        result.Add(list.ToArray());
      }
      else
        for (i = level; i <= m; i++)
        {
          swap(ref list[level], ref list[i]);
          permute(result, list, level + 1, m);
          swap(ref list[level], ref list[i]);
        }
    }

    static void Main(string[] args)
    {
      var input = System.IO.File.ReadAllText("input.txt");
      var program = input.Split(',').Select(x => int.Parse(x)).ToArray();
      var permutations = new List<int[]>();

      permute(permutations, Enumerable.Range(0, 5).ToArray(), 0, 4);

      var maxSignal = 0;

      foreach (var p in permutations)
      {
        var signal = 0;
        for (var amp = 0; amp < 5; amp++)
        {
          signal = Amp(p[amp], signal, program.ToArray());
        }
        if (signal > maxSignal)
        {
          maxSignal = signal;
        }
      }

      Console.Out.WriteLine(maxSignal);

    }
  }
}
