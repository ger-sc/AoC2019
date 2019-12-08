using System;
using System.Linq;
using System.Collections.Generic;

namespace day08
{
  class Program
  {

    const int width = 25;
    const int height = 6;

    private static IEnumerable<char[]> ChunkBySize(char[] array, int chunkSize)
    {
      var index = 0;
      do
      {
        yield return array.Skip(index).Take(chunkSize).ToArray();
        index += chunkSize;
      } while (index < array.Length);
    }

    private static IEnumerable<char> Zip2(char[] image1, char[] image2)
    {
      for (var i = 0; i < image1.Length; i++)
      {
        if (image1[i] == '0')
        {
          yield return '0';
        }
        if (image1[i] == '1')
        {
          yield return '1';
        }
        if (image1[i] == '2')
        {
          yield return image2[i];
        }
      }
    }

    private static void PrintImage(char[] image)
    {
      for (var h = 0; h < height; h++)
      {
        for (var w = 0; w < width; w++)
        {
          switch (image[h * width + w])
          {
            case '0':
            case '2':
              Console.Out.Write(' ');
              break;
            case '1':
              Console.Out.Write('█');
              break;
          };
        }
        Console.Out.WriteLine();
      }
      Console.Out.WriteLine();
    }

    static void Main(string[] args)
    {
      var input = System.IO.File.ReadAllText("input.txt").ToCharArray();

      var pixels = width * height;

      var chunks = ChunkBySize(input, pixels).ToList();

      var minZero = chunks
        .Select(l => new { chunk = l, Zeros = l.Count(c => c == '0') })
        .OrderBy(x => x.Zeros)
        .First().chunk;

      var result = minZero.Where(c => c == '1').Count() * minZero.Where(c => c == '2').Count();

      Console.Out.WriteLine(result);

      var result2 = chunks[0];


      for (var l = 1; l < chunks.Count; l++)
      {
        result2 = Zip2(result2, chunks[l]).ToArray();
      }

      PrintImage(result2);
    }
  }
}
