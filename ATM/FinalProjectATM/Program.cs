using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Dynamic;
using System.IO;

namespace FinalProjectATM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = new filePath().GetPath();
            var brain = new Brain(path: path);

            try
            {
                string jsonContent = File.ReadAllText(path);

                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    Console.WriteLine("The JSON file is empty.");
                }
                else
                {
                    if (brain.CheckData())
                    {
                        if (brain.VerifyPin())
                        {
                            Console.Clear();
                            brain.DisplayMenu();
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                new RegisterNewClient().Register();
                if (brain.CheckData())
                {
                    if (brain.VerifyPin())
                    {
                        Console.Clear();
                        brain.DisplayMenu();
                    }
                }
            }
            

            /*----------  REGISTER ACCOUNT  ----------*/
            //var storedData = new RegisterNewClient().Register();
            //Console.WriteLine(storedData);

            //var path = new Path().GetPath();
            //var brain = new Brain(path: path);
            //if (brain.CheckData())
            //{
            //    if (brain.VerifyPin())
            //    {
            //        Console.Clear();
            //        brain.DisplayMenu();
            //    }
            //}
        }

    }
    
}
