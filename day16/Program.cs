using System;
using System.Linq;
using System.Collections.Generic;

namespace day16
{
    class Program
    {
        private static int[] Pattern = new int[] {0,1,0,-1};
        private static IEnumerable<int> GeneratePattern(int level) {
            bool first = true;
            while(true) {
                for (var i = 0; i < Pattern.Length; i++) {
                    for(var r = 0; r < level; r++) {
                        if(first) {
                            first = false;
                        } else {
                            yield return Pattern[i];
                        }
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("input.txt").ToCharArray().Select(x => int.Parse(x.ToString())).ToArray();

            //var input = new int[] {1,2,3,4,5,6,7,8};
            var output = new int[input.Length];
            var step = 0;
            while (step < 100) {
                for(var i = 0; i < input.Length; i++) {
                    var p = GeneratePattern(i+1).Take(input.Length).ToArray();
                    var sum = 0;
                    for(var z = 0; z < input.Length; z++) {
                        sum+= input[z]*p[z];
                    }
                    output[i] = Math.Abs(sum % 10);
                   
                } 
                Console.Out.WriteLine(string.Join("", output.Select(x => x.ToString())));
                input = output;
                step++;
            }

            Console.Out.WriteLine(string.Join("", output.Take(8).Select(x => x.ToString())));
            
            var inputString = System.IO.File.ReadAllText("input.txt");
            input = inputString.ToCharArray().Select(x => int.Parse(x.ToString())).ToArray();
            var inputLength = inputString.Length * 10000;
            var realInput = GenerateRealInput(input);
            var offset = int.Parse(inputString.Substring(0, 7));
            output = realInput.Skip(offset).ToArray();
            
            
            step = 0;
            while (step < 100) {
                var sum = 0;
                for(var i = output.Length - 1; i >= 0; i--) {
                    sum += output[i];
                    output[i] = sum % 10;
                }
                step++;
            }
            Console.Out.WriteLine(string.Join("", output.Take(8).Select(x => x.ToString())));
        }

        private static IEnumerable<int> GenerateRealInput(int[] input)
        {
            for(var i = 0; i < 10000; i++) {
                for(var x = 0; x < input.Length; x++) {
                    yield return input[x];
                }
            }
        }
    }
}
