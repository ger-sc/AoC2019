using System;
using System.Linq;

namespace day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt");
            var parsedInput = input[0].Split(',').Select(x => int.Parse(x)).ToArray();

            var index = 0;
            
            parsedInput[1] = 12;
            parsedInput[2] = 2;
            while(parsedInput[index] != 99) {
                var command = parsedInput[index];
                var input1 = parsedInput[index+1];
                var input2 = parsedInput[index+2];
                var output = parsedInput[index+3];
                switch(command) {
                    case 1:
                      parsedInput[output] = parsedInput[input1] + parsedInput[input2];
                      index += 4;
                      break;
                    case 2:
                      parsedInput[output] = parsedInput[input1] * parsedInput[input2];
                      index += 4;
                      break;
                }
            }
            Console.Out.WriteLine(parsedInput[0]);
            
            for(var noun = 0; noun < 100; noun++) {
                
                for(var verb = 0; verb < 100; verb++) {
                    
                    var parsedInput2 = input[0].Split(',').Select(x => int.Parse(x)).ToArray();
                    parsedInput2[1] = noun;
                    parsedInput2[2] = verb;
                    index = 0;
                    while(parsedInput2[index] != 99) {
                        var command = parsedInput2[index];
                        var input1 = parsedInput2[index+1];
                        var input2 = parsedInput2[index+2];
                        var output = parsedInput2[index+3];
                        switch(command) {
                            case 1:
                            parsedInput2[output] = parsedInput2[input1] + parsedInput2[input2];
                            index += 4;
                            break;
                            case 2:
                            parsedInput2[output] = parsedInput2[input1] * parsedInput2[input2];
                            index += 4;
                            break;
                        }
                    }
                    if (parsedInput2[0] == 19690720) {
                        Console.Out.WriteLine(100*noun+verb);
                    }
                }
            }

        }
    }
}
