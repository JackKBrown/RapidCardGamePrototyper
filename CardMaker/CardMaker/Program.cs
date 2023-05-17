using Microsoft.VisualBasic.FileIO;
using System;

namespace CardMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("input file");
            string file = Console.ReadLine();
            using (TextFieldParser parser = new TextFieldParser(file))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //Process row
                    string[] fields = parser.ReadFields();
                    foreach (string field in fields)
                    {
                        Console.WriteLine(field);
                    }
                }
            }
            Console.WriteLine("Hello World!");
        }
    }
}
