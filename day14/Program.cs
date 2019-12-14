using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace day14
{
  class Program
  {
    static void Main(string[] args)
    {
      var input = System.IO.File.ReadAllLines("input.txt");
      var reactions = GetReactions(input);

      Produce(reactions, new Chemical("FUEL", 1));

      Console.Out.WriteLine(OreNeeded);

      var amount = 1000000000000;
      long step = 0;
      OreNeeded = 0;
      surplusDict = new Dictionary<string, long>();
      
      long incr = 10000000;
      
      do{
          var surPlusTemp = surplusDict.ToDictionary(x => x.Key, x => x.Value);
          var oreNeededTemp = OreNeeded;
          Produce(reactions, new Chemical("FUEL", incr));
          if (OreNeeded > amount) {
            OreNeeded = oreNeededTemp;
            surplusDict = surPlusTemp.ToDictionary(x => x.Key, x => x.Value);;
            incr /= 10;
          } else {
            step+= incr;
          }
      } while(OreNeeded < amount && incr >= 1);
      Console.Out.WriteLine(step);      
    }

    private static IDictionary<string, long> surplusDict = new Dictionary<string, long>();

    private static long OreNeeded = 0;

    private static void Produce(IDictionary<Chemical, List<Chemical>> reactions, Chemical produce)
    {
       var produceAmount = produce.Amount;
       if (produce.Name == "ORE") {
           OreNeeded += produceAmount;
           return;
       }

       if (surplusDict.ContainsKey(produce.Name)) {
           var surplusAmount = surplusDict[produce.Name];
           if (surplusAmount >= produceAmount) {
               surplusDict[produce.Name] -= produceAmount;
               return;
           } else {
               produceAmount -= surplusDict[produce.Name];
               surplusDict[produce.Name] = 0;
           }
       }

       var reaction = reactions.Single(r => r.Key.Name == produce.Name);

       var repeat = (int)Math.Ceiling(produceAmount / (double)reaction.Key.Amount);
    
       foreach(var ingridient in reaction.Value) {
         Produce(reactions, new Chemical(ingridient.Name, ingridient.Amount * repeat));
       }

       var surplus = reaction.Key.Amount * repeat - produceAmount;

       if (surplus > 0) {
         if (surplusDict.ContainsKey(produce.Name)) {
           surplusDict[produce.Name] += surplus;
         } else {
           surplusDict[produce.Name] = surplus;
         }
       }

    }

    private static IDictionary<Chemical, List<Chemical>> GetReactions(string[] lines)
    {
      var dict = new Dictionary<Chemical, List<Chemical>>();
      foreach (var line in lines)
      {
        var chemicals = line.Split("=>");
        var result = chemicals[1].Trim();
        var ingridients = chemicals[0].Split(",").Select(x => x.Trim()).ToArray();
        var r = ParseChemical(result);
        var l = ingridients.Select(x => ParseChemical(x)).ToList();
        dict.Add(r, l);
      }
      return dict;
    }

    private static Chemical ParseChemical(string chemical)
    {
      var pattern = @"(?<amount>[0-9]+) (?<name>\w+)";
      var match = Regex.Match(chemical, pattern);
      if (match.Success)
      {
        return new Chemical(match.Groups["name"].Value, int.Parse(match.Groups["amount"].Value));
      }
      throw new ArgumentException("Chemical is not parseable", nameof(chemical));
    }

  }

  internal class Chemical
  {
    public readonly string Name;
    public readonly long Amount;

    public Chemical(string name, long amount)
    {
      Name = name;
      Amount = amount;
    }

    public override string ToString() {
        return Amount+ " " +Name;
    }

    protected bool Equals(Chemical other)
    {
      return Name == other.Name && Amount == other.Amount;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((Chemical) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Amount.GetHashCode();
      }
    }
  }
}
