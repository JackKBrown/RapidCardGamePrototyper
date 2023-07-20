using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMaker
{
    public abstract class Card
    {
        public abstract void DrawCard(string outputDirectory);
    }
}
