using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DivCardValuer
{
    class Program
    {
        static void Main(string[] args)
        {
            var divJsonPath = @"C:\Users\arandall\poe\divcardvalues.json";
            var divValues = JsonConvert.DeserializeObject<DivCard[]>(File.ReadAllText(divJsonPath));

            var stashPath = @"C:\Users\arandall\poe\PoeTools\StashItemValuer\bin\Debug\netcoreapp2.1\stash.json";
            var stash = JsonConvert.DeserializeObject<StashTab[]>(File.ReadAllText(stashPath));

            var items = stash.SelectMany(t => t.Items).Select(i => i.name).Distinct().ToArray();

            foreach (var name in items)
            {
                Console.WriteLine(name);
            }

        //    var hits = new List<DivCard>();

        //    foreach (var item in items)
        //    {
        //        var matches = divValues.Where(dv => dv.Name == item.name).ToArray();
        //        if (matches.Any())
        //        {
        //            hits.AddRange(matches);
        //        }
        //    }

        //    Console.WriteLine($"Total: {hits.Sum(h => h.ChaosValue)}");

        //    foreach (var hit in hits.OrderByDescending(h => h.ChaosValue))
        //    {
        //        Console.WriteLine($"{hit.Name} - {hit.ChaosValue}");
        //    }
        }
    }
}
