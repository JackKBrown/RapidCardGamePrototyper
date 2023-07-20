using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMaker
{
    class RondelCard :Card
	{
		// attributes from csv
		[Name("Rondel ID")]
		public string RondelID { get; set; }
		[Name("Rondel Type")]
		public string RondelType { get; set; }
		[Name("Rondel Name")]
		public string RondelName { get; set; }
		[Name("Effect")]
		public string Effect { get; set; }

		public string CardImage = @"Img/BankRondel.png";

		// constants
		public static readonly int CardHeight = 1000;
		public static readonly int CardWidth = 1000;

		public readonly int CardImageX = 0;
		public readonly int CardImageY = 0;
		//public readonly int CardImageWidth = (int)(0.57 * CardWidth);
		//public readonly int CardImageHeight = (int)(0.8 * CardHeight);

		public readonly int NameBoxX = (int)(0.3 * CardWidth);
		public readonly int NameBoxY = (int)(0.05 * CardHeight);
		public readonly int NameBoxWidth = (int)(0.5 * CardWidth);
		public readonly int NameBoxHeight = (int)(0.1 * CardHeight);

		public readonly int CABoxX = (int)(0.62 * CardWidth);
		public readonly int CABoxY = (int)(0.53 * CardHeight);
		public readonly int CABoxWidth = (int)(0.33 * CardWidth);
		public readonly int CABoxHeight = (int)(0.33 * CardHeight);

		public override void DrawCard(string outputDirectory)
		{
			StringFormat Wrap = new StringFormat();
			StringFormat NoWrap = new StringFormat();
			List<Layer> layers = new List<Layer>();
			CardDrawer cd = CardDrawer.Instance();

			layers.Add(cd.CreateLayerFromFile(CardImageX, CardImageY, 0, 0, CardImage));//card image
			layers.Add(cd.CreateTextLayer(NameBoxX, NameBoxY, NameBoxWidth, NameBoxHeight, RondelName, cd.LargeFont, NoWrap)); //cardname

			if (!string.IsNullOrEmpty(Effect))
			{
				layers.Add(cd.CreateTextLayer(CABoxX, CABoxY, CABoxWidth, CABoxHeight, Effect, cd.mediumFont, Wrap)); //cardability
			}

			Bitmap bmap = CardDrawer.Instance().MergeLayers(layers, CardWidth, CardHeight);
			string filelocation = outputDirectory + RondelName + ".png";
			bmap.Save(filelocation, System.Drawing.Imaging.ImageFormat.Png);
		}

	}
}
