using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BattleEngineParser.Models;

namespace BattleEngineParser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var designsDirectory = args[0];
            if (!Directory.Exists(designsDirectory))
            {
                throw new InvalidProgramException($"Specified designs directory [{designsDirectory}] cannot be found.");
            }

            var designFiles = Directory.EnumerateFiles(designsDirectory, "*.bed", SearchOption.AllDirectories);
            var mechDesigns = new List<MechDesign>();

            designFiles
                .ToList()
                .ForEach(filePath =>
                {
                    var mechDesign = MechDesign.MechDesignFromFile(filePath);
                    mechDesigns.Add(mechDesign);
                });

            Console.WriteLine($"Processed [{mechDesigns.Count()}] designs...");
        }
    }
}