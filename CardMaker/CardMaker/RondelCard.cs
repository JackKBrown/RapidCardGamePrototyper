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
		[Name("Tile Side")]
		public string Side { get; set; }

		// constants
		public static readonly int CardHeight = 1000;
		public static readonly int CardWidth = 1000;

		public readonly int CardImageX = 0;
		public readonly int CardImageY = 0;
		//public readonly int CardImageWidth = (int)(0.57 * CardWidth);
		//public readonly int CardImageHeight = (int)(0.8 * CardHeight);

		public readonly int NameBoxX = (int)(0.35 * CardWidth);
		public readonly int NameBoxY = (int)(0.03 * CardHeight);
		public readonly int NameBoxWidth = (int)(0.28 * CardWidth);
		public readonly int NameBoxHeight = (int)(0.06 * CardHeight);

		public readonly int CABoxX = (int)(0.22 * CardWidth);
		public readonly int CABoxY = (int)(0.8 * CardHeight);
		public readonly int CABoxWidth = (int)(0.55 * CardWidth);
		public readonly int CABoxHeight = (int)(0.15 * CardHeight);

		public override void DrawCard(string outputDirectory)
		{
            StringFormat Center = new StringFormat();
            Center.Alignment = StringAlignment.Center;
            Center.LineAlignment = StringAlignment.Center;
            List<Layer> layers = new List<Layer>();
			CardDrawer cd = CardDrawer.Instance();
            string CardImage = $"Img/{RondelType.Replace(" ", "").ToLower()}.png";

            layers.Add(cd.CreateLayerFromFile(CardImageX, CardImageY, CardWidth, CardHeight, CardImage));//card image
			layers.Add(cd.CreateTextLayer(NameBoxX, NameBoxY, NameBoxWidth, NameBoxHeight, RondelName, cd.LargeFont, Center)); //cardname

			if (!string.IsNullOrEmpty(Effect))
			{
				layers.Add(cd.CreateTextLayer(CABoxX, CABoxY, CABoxWidth, CABoxHeight, Effect, cd.LargeFont, Center)); //cardability
			}

			Bitmap bmap = CardDrawer.Instance().MergeLayers(layers, CardWidth, CardHeight);
			layers=new List<Layer>();
			string filelocation = outputDirectory + RondelName +"_"+ Side + ".png";
			bmap.Save(filelocation, System.Drawing.Imaging.ImageFormat.Png);
		}

	}
}
