using System;
using System.Collections.Generic;
using System.IO;

namespace CardMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            string ActionCardPath = null;
            string RondelCardPath = null;
            string MonsterCardPath = null;
            if (args == null)
            {
                Console.WriteLine("No args given.");
                Console.WriteLine("Input Action card path, leave blank if you wish to skip");
                ActionCardPath = Console.ReadLine();
                Console.WriteLine("Input Rondel tile path, leave blank if you wish to skip");
                RondelCardPath = Console.ReadLine();
                Console.WriteLine("Input Monster tile path, leave blank if you wish to skip");
                MonsterCardPath = Console.ReadLine();

            }
            else
            {
                for(int i = 0; i < args.Length; ++i)
                {
                    string arg = args[i];
                    switch(arg)
                    {
                        case "--ActionPath":
                            if(ExtractPath(args, ref i, out string APath))
                                ActionCardPath = APath;
                            break;
                        case "--RondelPath":
                            if (ExtractPath(args, ref i, out string RPath))
                                RondelCardPath = RPath;
                            break;
                        case "--MonsterPath":
                            if (ExtractPath(args, ref i, out string MPath))
                                MonsterCardPath = MPath;
                            break;
                    }
                }
            }
            if (!String.IsNullOrEmpty(ActionCardPath))
            {
                //Make Action Cards
                Console.WriteLine("Making action cards found in " + ActionCardPath);
                List<ActionCard> cards = RondlelonParser.ParseActionCardCSV(ActionCardPath);
                cards[0].DrawCard();
            }
            if (!String.IsNullOrEmpty(RondelCardPath))
            {
                //Make Rondel Cards
                Console.WriteLine("Making rondel cards found in " + RondelCardPath);
            }
            if (!String.IsNullOrEmpty(MonsterCardPath))
            {
                //Make Monster Cards
                Console.WriteLine("Making monster cards found in " + MonsterCardPath);
            }
            
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }

        private static bool ExtractPath(string[] args, ref int i, out string path)
        {
            if (i+1 >= args.Length)
            {
                path = string.Empty;
                Console.WriteLine("Missing arg skipping");
                return false;
            }
            else
            {
                path = args[i + 1];
                if(File.Exists(path))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("could not find path " + path);
                    return false;
                }
            }
        }
    }
}
