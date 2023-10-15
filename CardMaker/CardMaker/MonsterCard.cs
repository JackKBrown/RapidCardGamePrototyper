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

		// constants
		public static readonly int CardHeight = 750;
		public static readonly int CardWidth = 1050;

		public readonly int CardImageX = (int)(0.01 * CardWidth);
		public readonly int CardImageY = (int)(0.01 * CardHeight);
		public readonly int CardImageWidth = (int)(0.98 * CardWidth);
		public readonly int CardImageHeight = (int)(0.98 * CardHeight);

		public readonly int NameBoxX = (int)(0.071 * CardWidth);
		public readonly int NameBoxY = (int)(0.05 * CardHeight);
		public readonly int NameBoxWidth = (int)(0.3 * CardWidth);
		public readonly int NameBoxHeight = (int)(0.066 * CardHeight);

		public readonly int MBoxX = (int)(0.84 * CardWidth);
		public readonly int MBoxY = (int)(0.33 * CardHeight);
		public readonly int MBoxWidth = (int)(0.1 * CardWidth);
		public readonly int MBoxHeight = (int)(0.1 * CardWidth);//needs to be a square

        public readonly int HPBoxX = (int)(0.74 * CardWidth);
		public readonly int HPBoxY = (int)(0.33 * CardHeight);
		public readonly int HPBoxWidth = (int)(0.1 * CardWidth);
		public readonly int HPBoxHeight = (int)(0.1 * CardWidth);//needs to be a square
		
		public readonly int MBoxBackX = (int)(0.45 * CardWidth);
		public readonly int MBoxBackY = (int)(0.43 * CardHeight);
		public readonly int MBoxBackWidth = (int)(0.1 * CardWidth);
		public readonly int MBoxBackHeight = (int)(0.1 * CardWidth);//needs to be a square


        public readonly int CABoxX = (int)(0.74 * CardWidth);
		public readonly int CABoxY = (int)(0.066 * CardHeight);
		public readonly int CABoxWidth = (int)(0.2 * CardWidth);
		public readonly int CABoxHeight = (int)(0.27 * CardHeight);

        public readonly string MonsterTemplate = @"Img/MonsterTemplateFront.png";
		public readonly string MonsterTemplateBack = @"Img/MonsterTemplateBack.png";
		public readonly string ScrollImage = @"Img/parchment.png";
		public readonly string HPSymbol = @"Img/hp.png";


        public override void DrawCard(string outputDirectory)
		{
            StringFormat Center = new StringFormat();
            Center.Alignment = StringAlignment.Center;
            Center.LineAlignment = StringAlignment.Center;
            List<Layer> layers = new List<Layer>();
			List<Layer> backLayers = new List<Layer>();
			CardDrawer cd = CardDrawer.Instance();
			string CardImage = $"Img/{MonsterName.Replace(" ", "").ToLower()}.png";
			layers.Add(cd.CreateLayerFromFile(CardImageX, CardImageY, CardImageWidth, CardImageHeight, CardImage));//card image
            layers.Add(cd.CreateLayerFromFile(0, 0, CardWidth, CardHeight, MonsterTemplate)); // template
            //layers.Add(cd.CreateLayerFromFile(CAscrollX, CAscrollY, CAscrollWidth, CAscrollHeight, ScrollImage));//card scroll
            layers.Add(cd.CreateTextLayer(NameBoxX, NameBoxY, NameBoxWidth, NameBoxHeight, MonsterName, cd.LargeFont, Center)); //cardname
			layers.Add(cd.CreateTextLayer(HPBoxX, HPBoxY, HPBoxWidth, HPBoxHeight, HitPoints, cd.LargeFont,Center));
			layers.Add(cd.CreateTextLayer(MBoxX, MBoxY, MBoxWidth, MBoxHeight, MaxCards, cd.LargeFont,Center));

            if (!string.IsNullOrEmpty(Effect))
			{
				layers.Add(cd.CreateTextLayer(CABoxX, CABoxY, CABoxWidth, CABoxHeight, Effect, cd.mediumFont, Center)); //cardability
			}

			backLayers.Add(cd.CreateLayerFromFile(0, 0, CardWidth, CardHeight, MonsterTemplateBack)); // template
			backLayers.Add(cd.CreateTextLayer(MBoxBackX, MBoxBackY, MBoxBackWidth, MBoxBackHeight, MaxCards, cd.BoxFont, Center)); //MaxCards


			Bitmap bmap = CardDrawer.Instance().MergeLayers(layers, CardWidth, CardHeight);
			layers =new List<Layer>();
			string filelocation = outputDirectory + MonsterName + "Front.png";
			bmap.Save(filelocation, System.Drawing.Imaging.ImageFormat.Png);

			Bitmap bmapBack = CardDrawer.Instance().MergeLayers(backLayers, CardWidth, CardHeight);
			string filelocationBack = outputDirectory + MonsterName + "Back.png";
			bmapBack.Save(filelocationBack, System.Drawing.Imaging.ImageFormat.Png);
		}

	}
}
