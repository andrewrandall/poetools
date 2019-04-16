using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DivCardValueConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\arandall\poe\divcardsvalues.csv";
            var csv = File.ReadAllLines(path).Skip(1).ToArray();
            var chaosFromEx = 115;
            var cards = new List<DivCard>();
            foreach (var line in csv)
            {
                var parts = line.Split(',');
                var name = parts[0].Replace("wiki", string.Empty);
                double chaosValue;

                double? num1 = null;
                double? num2 = null;
                
                var buffer = new StringBuilder();
                foreach (var c in parts[3])
                {
                    if (char.IsDigit(c) || c == '.')
                    {
                        buffer.Append(c);
                    }
                    else
                    {
                        if (num1.HasValue)
                        {
                            num2 = double.Parse(buffer.ToString());
                        }
                        else
                        {
                            num1 = double.Parse(buffer.ToString());
                        }

                        buffer = new StringBuilder();
                    }
                }

                if (num2.HasValue)
                {
                    chaosValue = (chaosFromEx * num1.Value) + num2.Value;
                }
                else
                {
                    chaosValue = num1.Value;
                }

                cards.Add(new DivCard
                {
                    Name = name,
                    ChaosValue = chaosValue
                });
            }
            var json = JsonConvert.SerializeObject(cards);
            File.WriteAllText(@"c:\users\arandall\poe\divcardvalues.json", json);
        }
    }
}
