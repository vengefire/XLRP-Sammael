using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;


namespace BattleEngineJsonCreation
{
    class BattleEngineConverter
    {
        static void Main(string[] args)
        {
            var filepath = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(filepath, "*.bed", SearchOption.AllDirectories);
            int[,] cbtISarry = new int[17, 5] { {20,6,5,3,4},{25,8,6,4,6},{30,10,7,5,7},{35,11,8,6,8},
                {40,12,10,6,10},{45,14,11,7,11},{50,16,12,8,12},{55,18,13,9,13},{60,20,14,10,14},
                {65,21,15,10,15},{70,22,15,11,15},{75,23,16,12,16},{80,25,17,13,17},{85,27,18,14,18},
                {90,29,19,15,19},{95,30,20,16,20},{100,31,21,17,21} };
            int[,] cbt2hbsISarry = new int[1, 5] { { 1, 5, 5, 5, 5 } };
            int[,] hbsfromTTISarry = new int[17, 5];
            for (int i = 0; i < hbsfromTTISarry.GetLength(0); i++)
            {
                for (int a = 0; a < cbt2hbsISarry.Length; a++)
                {
                    hbsfromTTISarry[i, a] = cbtISarry[i, a] * cbt2hbsISarry[0, a];
                }
            }
            int tonnageIndex=0;
            //try
            //{
                //Parallel.ForEach (files,(currentFile) =>
                foreach (var currentFile in files)
                {
                    string filename = Path.GetFileName(currentFile);
                    string[] lines = System.IO.File.ReadAllLines(currentFile);
                    var chassisDef = new ChassisDef();
                    var chassisDefDescription = new ChassisDefDescription();
                    var mechDef = new MechDef();
                    var mechDefLocation = new MechDefLocation();
                    if (lines[3].Contains("Biped"))
                    {
                        Console.WriteLine($"Processing {filename} on thread {Thread.CurrentThread.ManagedThreadId}");
                        foreach (string l in lines)
                        {
                            string[] words = l.Split(',');
                            if (words[0].ToLower().Contains("name"))
                            {
                                string[] split = words[1].Split(' ');
                                if (split[1].Contains("("))
                                {
                                    chassisDef.VariantName = split[2];
                                    chassisDefDescription.Name = split[0];
                                }
                                else
                                {
                                    chassisDef.VariantName = split[0];
                                    chassisDefDescription.Name = split[1];
                                }
                            }
                            if (words[0].ToLower().Contains("tons"))
                            {
                                chassisDef.Tonnage = Convert.ToInt32(words[1]);
                                chassisDef.InitialTonnage = chassisDef.Tonnage / 10;
                                for (int i = 0; i < hbsfromTTISarry.GetLength(0); i++)
                                {
                                    if (hbsfromTTISarry[i, 0] == chassisDef.Tonnage)
                                    {
                                        tonnageIndex = i;
                                        Console.WriteLine(hbsfromTTISarry[i, 0]);
                                        break;
                                    }
                                }
                            }

                            if (words[0].ToLower().Contains("armorvals"))
                            {
                                string[] split = words[1].Split(',');

                                mechDef.Locations.Add(new MechDefLocation
                                {
                                    DamageLevel = DamageLevel.Functional,
                                    Location = Location.Head,
                                    CurrentArmor = 45,
                                    CurrentRearArmor = -1,
                                    CurrentInternalStructure = 16,
                                    AssignedArmor = 45,
                                    AssignedRearArmor = -1,
                                });
                                mechDef.Locations.Add(new MechDefLocation
                                {
                                    DamageLevel = DamageLevel.Functional,
                                    Location = Location.Head,
                                    CurrentArmor = Convert.ToInt32(split[1]) * 5,
                                    CurrentRearArmor = -1,
                                    CurrentInternalStructure = hbsfromTTISarry[tonnageIndex, 3],
                                    AssignedArmor = Convert.ToInt32(split[1]) * 5,
                                    AssignedRearArmor = -1,
                                });
                            }/*
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }
                            if (words[0].ToLower().Contains("name"))
                            {

                            }*/
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Skiped {filename} on thread {Thread.CurrentThread.ManagedThreadId}");
                        Console.ReadKey();
                    }
                    //});
                }
            //}
            //catch (System.Exception excpt)
            //{
            //        Console.WriteLine(excpt.Message);
            //}
            Console.ReadKey();
        }
        public void ParseEngine(string[] lines)
        {

        }
    }
}
