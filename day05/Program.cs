using System;
using System.Linq;

namespace day05
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt");
            var parsedInput = input[0].Split(',').Select(x => int.Parse(x)).ToArray();

            var index = 0;
            
            while(parsedInput[index] != 99) {
                var instr = parsedInput[index];
                var input1 = parsedInput[(index+1)%parsedInput.Length];
                var input2 = parsedInput[(index+2)%parsedInput.Length];
                var input3 = parsedInput[(index+3)%parsedInput.Length];
                var command = instr % 100;
                var modes = ((int)instr/100).ToString().PadLeft(3, '0').ToCharArray().Reverse().ToArray();

                var mode1 = modes.Length > 0 ? int.Parse(modes[0].ToString()) : 0; 
                var mode2 = modes.Length > 1 ? int.Parse(modes[1].ToString()) : 0;
                int value1, value2;

                switch(command) {
                    case 1:
                      value1 = mode1 == 0 ? parsedInput[input1] : input1;
                      value2 = mode2 == 0 ? parsedInput[input2] : input2;
                      parsedInput[input3] = value1 + value2;
                      index += 4;
                      break;
                    case 2:
                      value1 = mode1 == 0 ? parsedInput[input1] : input1;
                      value2 = mode2 == 0 ? parsedInput[input2] : input2;
                      parsedInput[input3] = value1 * value2;
                      index += 4;
                      break;
                    case 3:
                      parsedInput[input1] = 1;
                      index += 2;
                      break;
                    case 4:
                      value1 = mode1 == 0 ? parsedInput[input1] : input1;
                      Console.Out.Write(value1);
                      index+=2;
                      break;
                }
            }
            Console.Out.WriteLine();

            parsedInput = input[0].Split(',').Select(x => int.Parse(x)).ToArray();

            index = 0;
            
            while(parsedInput[index] != 99) {
                var instr = parsedInput[index];
                var input1 = parsedInput[(index+1)%parsedInput.Length];
                var input2 = parsedInput[(index+2)%parsedInput.Length];
                var input3 = parsedInput[(index+3)%parsedInput.Length];
                var command = instr % 100;
                var modes = ((int)instr/100).ToString().PadLeft(3, '0').ToCharArray().Reverse().ToArray();

                var mode1 = modes.Length > 0 ? int.Parse(modes[0].ToString()) : 0; 
                var mode2 = modes.Length > 1 ? int.Parse(modes[1].ToString()) : 0;
                int value1, value2;

                switch(command) {
                    case 1:
                      value1 = mode1 == 0 ? parsedInput[input1] : input1;
                      value2 = mode2 == 0 ? parsedInput[input2] : input2;
                      parsedInput[input3] = value1 + value2;
                      index += 4;
                      break;
                    case 2:
                      value1 = mode1 == 0 ? parsedInput[input1] : input1;
                      value2 = mode2 == 0 ? parsedInput[input2] : input2;
                      parsedInput[input3] = value1 * value2;
                      index += 4;
                      break;
                    case 3:
                      parsedInput[input1] = 5;
                      index += 2;
                      break;
                    case 4:
                      value1 = mode1 == 0 ? parsedInput[input1] : input1;
                      Console.Out.Write(value1);
                      index+=2;
                      break;
                    case 5:
                      value1 = mode1 == 0 ? parsedInput[input1] : input1;
                      value2 = mode2 == 0 ? parsedInput[input2] : input2;
                      if (value1 > 0) {
                          index = value2;
                      } else {
                          index += 3;
                      }
                      break;
                    case 6:
                      value1 = mode1 == 0 ? parsedInput[input1] : input1;
                      value2 = mode2 == 0 ? parsedInput[input2] : input2;
                      if (value1 == 0) {
                          index = value2;
                      } else {
                          index += 3;
                      }
                      break;
                    case 7:
                      value1 = mode1 == 0 ? parsedInput[input1] : input1;
                      value2 = mode2 == 0 ? parsedInput[input2] : input2;
                      if (value1 < value2) {
                        parsedInput[input3] = 1;
                      } else {
                        parsedInput[input3] = 0;
                      }
                      index += 4;
                      break;
                    case 8:
                      value1 = mode1 == 0 ? parsedInput[input1] : input1;
                      value2 = mode2 == 0 ? parsedInput[input2] : input2;
                      if (value1 == value2) {
                        parsedInput[input3] = 1;
                      } else {
                        parsedInput[input3] = 0;
                      }
                      index += 4;
                      break;
                }
            }

            //Console.Out.WriteLine(parsedInput[0]);
        }
    }
}
