using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMaker
{
    class RondlelonParser
    {
        public static List<Card> ParseCardCSV<Card>(string FilePath)
        {
            List<Card> cards = new List<Card>();
            using (var reader = new StreamReader(FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var card = csv.GetRecord<Card>();
                    cards.Add(card);
                }
            }
            return cards;
        }
    }
}
