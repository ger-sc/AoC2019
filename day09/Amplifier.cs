using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Numerics;


public class Amplifier
{


  private enum Mode : int
  {
    Position = 0,
    Immediate = 1,
    Relative = 2
  }

  private IDictionary<BigInteger, BigInteger> OpCode = new Dictionary<BigInteger, BigInteger>();
  private Action<BigInteger> Output;
  public ConcurrentQueue<BigInteger> _queue = new ConcurrentQueue<BigInteger>();

  private BigInteger RelativeBase = 0;


  public Amplifier(int[] opCode, Action<BigInteger> output)
  {
    for (var i = 0; i < opCode.Length; i++)
    {
      OpCode[i] = opCode[i];
    };
    Output = output;
  }

  private BigInteger GetValueAt(BigInteger index)
  {
    if (!OpCode.ContainsKey(index))
    {
      OpCode[index] = 0;
    }
    return OpCode[index];
  }

  private BigInteger GetValue(Mode mode, BigInteger input)
  {
    switch (mode)
    {
      case Mode.Position:
        return GetValueAt(input);
      case Mode.Immediate:
        return input;
      case Mode.Relative:
        return GetValueAt(input + RelativeBase);
      default:
        throw new ArgumentException("Unknown mode", nameof(mode));
    }
  }

  private BigInteger GetAddress(Mode mode, BigInteger input)
  {
    switch (mode)
    {
      case Mode.Position:
        return input;
      case Mode.Relative:
        return RelativeBase + input;
      default:
        throw new ArgumentException("Unknown mode", nameof(mode));
    }
  }

  public void Runner()
  {
    BigInteger index = 0;
    while (OpCode[index] != 99)
    {
      var instr = GetValueAt(index);
      var input1 = GetValueAt(index + 1);
      var input2 = GetValueAt(index + 2);
      var input3 = GetValueAt(index + 3);
      var command = (int)instr % 100;
      var modes = ((int)instr / 100).ToString().PadLeft(3, '0').ToCharArray().Reverse().ToArray();

      var mode1 = int.Parse(modes[0].ToString());
      var mode2 = int.Parse(modes[1].ToString());
      var mode3 = int.Parse(modes[2].ToString());
      BigInteger value1, value2;

      switch (command)
      {
        case 1:
          value1 = GetValue((Mode)mode1, input1);
          value2 = GetValue((Mode)mode2, input2);
          OpCode[GetAddress((Mode)mode3, input3)] = value1 + value2;
          index += 4;
          break;
        case 2:
          value1 = GetValue((Mode)mode1, input1);
          value2 = GetValue((Mode)mode2, input2);
          OpCode[GetAddress((Mode)mode3, input3)] = value1 * value2;
          index += 4;
          break;
        case 3:
          BigInteger msg;
          while (!_queue.TryDequeue(out msg))
          {
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
          }
          OpCode[GetAddress((Mode)mode1, input1)] = msg;
          index += 2;
          break;
        case 4:
          value1 = GetValue((Mode)mode1, input1);
          Output(value1);
          index += 2;
          break;
        case 5:
          value1 = GetValue((Mode)mode1, input1);
          value2 = GetValue((Mode)mode2, input2);
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
          value1 = GetValue((Mode)mode1, input1);
          value2 = GetValue((Mode)mode2, input2);
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
          value1 = GetValue((Mode)mode1, input1);
          value2 = GetValue((Mode)mode2, input2);
          if (value1 < value2)
          {
            OpCode[GetAddress((Mode)mode3, input3)] = 1;
          }
          else
          {
            OpCode[GetAddress((Mode)mode3, input3)] = 0;
          }
          index += 4;
          break;
        case 8:
          value1 = GetValue((Mode)mode1, input1);
          value2 = GetValue((Mode)mode2, input2);
          if (value1 == value2)
          {
            OpCode[GetAddress((Mode)mode3, input3)] = 1;
          }
          else
          {
            OpCode[GetAddress((Mode)mode3, input3)] = 0;
          }
          index += 4;
          break;
        case 9:
          value1 = GetValue((Mode)mode1, input1);
          RelativeBase += value1;
          index += 2;
          break;
      }
    }
  }
}


