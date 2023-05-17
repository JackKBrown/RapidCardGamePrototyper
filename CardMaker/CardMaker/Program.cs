using System;
using System.Collections.Generic;

namespace CardMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("input file");
            string file = Console.ReadLine();
            List<ActionCard> cards = RondlelonParser.ParseActionCardCSV(file);
            Console.WriteLine("Hello World!");
        }
    }
}
