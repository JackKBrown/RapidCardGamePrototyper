using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CardMaker
{
    class Program
    {
        public static string ActionCardOut = @"ActionCards/";
        public static string RondelCardOut = @"RondelTiles/";
        public static string MonsterCardOut = @"MonsterCards/";
        static void Main(string[] args)
        {
            StringFormat Center = new StringFormat();
            Center.Alignment = StringAlignment.Center;
            Center.LineAlignment = StringAlignment.Center;
            StringFormat Right = new StringFormat();
            Right.LineAlignment = StringAlignment.Center;
            List<Layer> layers = new List<Layer>();
            CardDrawer cd = CardDrawer.Instance();
            Font abilityFont = cd.mediumFont;
            string CardAbility = "Swap the locations of 2 Rondel Tiles @babytyranodon , keeping players on the same Tile.";

            layers.Add(cd.CreateTextLayer(0, 0, 600, 300, CardAbility, abilityFont, Center)); //cardability
            

            Bitmap bmap = CardDrawer.Instance().MergeLayers(layers, 600, 300);
            string filelocation = @"Testcards/" + "test" + ".png";
            bmap.Save(filelocation, System.Drawing.Imaging.ImageFormat.Png);

        }
        static void Main2(string[] args)
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
                List<ActionCard> cards = RondlelonParser.ParseCardCSV<ActionCard>(ActionCardPath);
                bool exists = System.IO.Directory.Exists(ActionCardOut);
                if (!exists)
                    System.IO.Directory.CreateDirectory(ActionCardOut);
                foreach (ActionCard card in cards)
				{
                    card.DrawCard(ActionCardOut);
				}
            }
            if (!String.IsNullOrEmpty(RondelCardPath))
            {
                //Make Rondel Cards
                Console.WriteLine("Making rondel cards found in " + RondelCardPath);
                List<RondelCard> cards = RondlelonParser.ParseCardCSV<RondelCard>(RondelCardPath);
                bool exists = System.IO.Directory.Exists(RondelCardOut);
                if (!exists)
                    System.IO.Directory.CreateDirectory(RondelCardOut);
                foreach (RondelCard card in cards)
                {
                    card.DrawCard(RondelCardOut);
                }
            }
            if (!String.IsNullOrEmpty(MonsterCardPath))
            {
                //Make Monster Cards
                Console.WriteLine("Making monster cards found in " + MonsterCardPath);
                List<MonsterCard> cards = RondlelonParser.ParseCardCSV<MonsterCard>(MonsterCardPath);
                bool exists = System.IO.Directory.Exists(MonsterCardOut);
                if (!exists)
                    System.IO.Directory.CreateDirectory(MonsterCardOut);
                foreach (MonsterCard card in cards)
                {
                    card.DrawCard(MonsterCardOut);
                }
            }
            
            Console.WriteLine("Finished, press enter to exit...");
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
