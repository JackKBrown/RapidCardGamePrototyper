using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMaker
{
    class MonsterCard : Card
	{
		// attributes from csv
		[Name("Monster ID")]
		public string MonsterID { get; set; }
		[Name("Monster Name")]
		public string MonsterName { get; set; }
		[Name("Max Cards")]
		public string MaxCards { get; set; }
		[Name("Hit Points")]
		public string HitPoints { get; set; }
		[Name("Effect")]
		public string Effect { get; set; }

		public string CardImage = @"Img/Innkeeper.png";

		// constants
		public static readonly int CardHeight = 750;
		public static readonly int CardWidth = 1050;

		public readonly int CardImageX = (int)(0.023 * CardWidth);
		public readonly int CardImageY = (int)(0.16 * CardHeight);
		public readonly int CardImageWidth = (int)(0.57 * CardWidth);
		public readonly int CardImageHeight = (int)(0.8 * CardHeight);

		public readonly int NameBoxX = (int)(0.05 * CardWidth);
		public readonly int NameBoxY = (int)(0.05 * CardHeight);
		public readonly int NameBoxWidth = (int)(0.5 * CardWidth);
		public readonly int NameBoxHeight = (int)(0.1 * CardHeight);

		//public readonly int CBoxX = (int)(0.75 * CardWidth);
		//public readonly int CBoxY = (int)(0.21 * CardHeight);
		//public readonly int CBoxWidth = (int)(0.1 * CardWidth);
		//public readonly int CBoxHeight = (int)(0.1 * CardHeight);

		public readonly int HPBoxX = (int)(0.75 * CardWidth);
		public readonly int HPBoxY = (int)(0.21 * CardHeight);
		public readonly int HPBoxWidth = (int)(0.1 * CardWidth);
		public readonly int HPBoxHeight = (int)(0.1 * CardHeight);

		public readonly int CABoxX = (int)(0.62 * CardWidth);
		public readonly int CABoxY = (int)(0.53 * CardHeight);
		public readonly int CABoxWidth = (int)(0.33 * CardWidth);
		public readonly int CABoxHeight = (int)(0.33 * CardHeight);

		public readonly string ActionTemplate = @"Img/MonsterTemplateFront.png";

		public override void DrawCard(string outputDirectory)
		{
			StringFormat Wrap = new StringFormat();
			StringFormat NoWrap = new StringFormat();
			List<Layer> layers = new List<Layer>();
			CardDrawer cd = CardDrawer.Instance();

			layers.Add(cd.CreateLayerFromFile(CardImageX, CardImageY, 0, 0, CardImage));//card image
			layers.Add(cd.CreateLayerFromFile(0, 0, 0, 0, ActionTemplate)); // template
			layers.Add(cd.CreateTextLayer(NameBoxX, NameBoxY, NameBoxWidth, NameBoxHeight, MonsterName, cd.LargeFont, NoWrap)); //cardname
			//layers.Add(cd.CreateTextLayer(CBoxX, CBoxY, CBoxWidth, CBoxHeight, MaxCards, cd.BoxFont, NoWrap)); //MaxCards this isn't on the front of the card
			layers.Add(cd.CreateTextLayer(HPBoxX, HPBoxY, HPBoxWidth, HPBoxHeight, HitPoints, cd.BoxFont, NoWrap)); //HP


			if (!string.IsNullOrEmpty(Effect))
			{
				layers.Add(cd.CreateTextLayer(CABoxX, CABoxY, CABoxWidth, CABoxHeight, Effect, cd.mediumFont, Wrap)); //cardability
			}

			Bitmap bmap = CardDrawer.Instance().MergeLayers(layers, CardWidth, CardHeight);
			string filelocation = outputDirectory + MonsterName + "Front.png";
			bmap.Save(filelocation, System.Drawing.Imaging.ImageFormat.Png);
		}

	}
}
