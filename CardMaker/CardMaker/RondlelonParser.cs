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
        public static List<ActionCard> ParseActionCardCSV(string FilePath)
        {
            List<ActionCard> cards = new List<ActionCard>();
            using (var reader = new StreamReader(FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var card = csv.GetRecord<ActionCard>();
                    cards.Add(card);
                    // Do something with the record.
                }
            }
            return cards;
        }
    }
}
