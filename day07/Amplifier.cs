using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;

public class Amplifier
{

  private int[] OpCode;
  private Action<int> Output;
  public ConcurrentQueue<int> _queue = new ConcurrentQueue<int>();

  public Amplifier(int[] opCode, Action<int> output)
  {
    OpCode = opCode;
    Output = output;
  }

  public void Runner()
  {
    var index = 0;
    while (OpCode[index] != 99)
    {
      var instr = OpCode[index];
      var input1 = OpCode[(index + 1) % OpCode.Length];
      var input2 = OpCode[(index + 2) % OpCode.Length];
      var input3 = OpCode[(index + 3) % OpCode.Length];
      var command = instr % 100;
      var modes = ((int)instr / 100).ToString().PadLeft(3, '0').ToCharArray().Reverse().ToArray();

      var mode1 = modes.Length > 0 ? int.Parse(modes[0].ToString()) : 0;
      var mode2 = modes.Length > 1 ? int.Parse(modes[1].ToString()) : 0;
      int value1, value2;

      switch (command)
      {
        case 1:
          value1 = mode1 == 0 ? OpCode[input1] : input1;
          value2 = mode2 == 0 ? OpCode[input2] : input2;
          OpCode[input3] = value1 + value2;
          index += 4;
          break;
        case 2:
          value1 = mode1 == 0 ? OpCode[input1] : input1;
          value2 = mode2 == 0 ? OpCode[input2] : input2;
          OpCode[input3] = value1 * value2;
          index += 4;
          break;
        case 3:
          int msg;
          while (!_queue.TryDequeue(out msg))
          {
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
          }
          OpCode[input1] = msg;
          index += 2;
          break;
        case 4:
          value1 = mode1 == 0 ? OpCode[input1] : input1;
          Output(value1);
          index += 2;
          break;
        case 5:
          value1 = mode1 == 0 ? OpCode[input1] : input1;
          value2 = mode2 == 0 ? OpCode[input2] : input2;
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
          value1 = mode1 == 0 ? OpCode[input1] : input1;
          value2 = mode2 == 0 ? OpCode[input2] : input2;
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
          value1 = mode1 == 0 ? OpCode[input1] : input1;
          value2 = mode2 == 0 ? OpCode[input2] : input2;
          if (value1 < value2)
          {
            OpCode[input3] = 1;
          }
          else
          {
            OpCode[input3] = 0;
          }
          index += 4;
          break;
        case 8:
          value1 = mode1 == 0 ? OpCode[input1] : input1;
          value2 = mode2 == 0 ? OpCode[input2] : input2;
          if (value1 == value2)
          {
            OpCode[input3] = 1;
          }
          else
          {
            OpCode[input3] = 0;
          }
          index += 4;
          break;
      }
    }
  }
}


