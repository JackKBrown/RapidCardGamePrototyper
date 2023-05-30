using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CardMaker
{
	public class ActionCard
	{
		// attributes from csv
		[Name("Card Name")]
		public string CardName { get; set; }
		[Name("Ability Cost")]
		public string AbilityCost { get; set; }
		[Name("Card Ability")]
		public string CardAbility { get; set; }
		[Name("Rondel Move")]
		public string Movement { get; set; }
		[Name("Attack Value")]
		public string Attack { get; set; }
		[Name("Attack Ability")]
		public string AttackAbility { get; set; }
		[Name("Flavour Text")]
		public string FlavourText { get; set; }
		[Name("Notes")]
		public string Notes { get; set; }
		[Name("Frequency")]
		public string Frequency { get; set; }
		[Name("Classifacation")]
		public string CardClass { get; set; }
		public string CardImage = @"Img/Innkeeper.png";

		// constants
		public static readonly int CardHeight = 1050;
		public static readonly int CardWidth = 750;

		public readonly int CardImageX = (int)(0.05 * CardWidth);
		public readonly int CardImageY = (int)(0.1 * CardHeight);

		public readonly int NameBoxX = (int)(0.1 * CardWidth);
		public readonly int NameBoxY = (int)(0.05 * CardHeight);
		public readonly int NameBoxWidth = (int)(0.8 * CardWidth);
		public readonly int NameBoxHeight = (int)(0.1 * CardHeight);

		public readonly int MBoxX = (int)(0.06 * CardWidth);
		public readonly int MBoxY = (int)(0.18 * CardHeight);
		public readonly int MBoxWidth = (int)(0.1 * CardWidth);
		public readonly int MBoxHeight = (int)(0.1 * CardHeight);

		public readonly int ABoxX = (int)(0.06 * CardWidth);
		public readonly int ABoxY = (int)(0.3 * CardHeight);
		public readonly int ABoxWidth = (int)(0.1 * CardWidth);
		public readonly int ABoxHeight = (int)(0.1 * CardHeight);

		public readonly int CABoxX = (int)(0.05 * CardWidth);
		public readonly int CABoxY = (int)(0.6 * CardHeight);
		public readonly int CABoxWidth = (int)(0.9 * CardWidth);
		public readonly int CABoxHeight = (int)(0.4 * CardHeight);

		public readonly int AABoxX = (int)(0.1 * CardWidth);
		public readonly int AABoxY = (int)(0.7 * CardHeight);
		public readonly int AABoxWidth = (int)(0.8 * CardWidth);
		public readonly int AABoxHeight = (int)(0.2 * CardHeight);


		public readonly string ActionTemplate = @"Img/RondelonTemplate.png";

		public void DrawCard(string outputDirectory)
        {
			StringFormat Wrap = new StringFormat();
			StringFormat NoWrap = new StringFormat();
			List<Layer> layers = new List<Layer>();
			CardDrawer cd = CardDrawer.Instance();

			layers.Add(cd.CreateLayerFromFile(CardImageX, CardImageY, 0, 0, CardImage));//card image
			layers.Add(cd.CreateLayerFromFile(0, 0, 0, 0, ActionTemplate)); // template
			layers.Add(cd.CreateTextLayer(NameBoxX, NameBoxY, NameBoxWidth, NameBoxHeight, CardName, cd.LargeFont, NoWrap)); //cardname
			layers.Add(cd.CreateTextLayer(MBoxX, MBoxY, MBoxWidth, MBoxHeight, Movement, cd.BoxFont, NoWrap)); //cardmove
			layers.Add(cd.CreateTextLayer(ABoxX, ABoxY, ABoxWidth, ABoxHeight, Attack, cd.BoxFont, NoWrap)); //cardattack
			

			if(!string.IsNullOrEmpty(CardAbility))
			{
				layers.Add(cd.CreateTextLayer(CABoxX, CABoxY, CABoxWidth, CABoxHeight, CardAbility, cd.mediumFont, Wrap)); //cardability
			}
			if (!string.IsNullOrEmpty(AttackAbility))
			{
				layers.Add(cd.CreateTextLayer(AABoxX, AABoxY, AABoxWidth, AABoxHeight, AttackAbility,cd.mediumFont, Wrap)); //card
			}
			
			Bitmap bmap = CardDrawer.Instance().MergeLayers(layers, CardWidth, CardHeight);
			string filelocation = outputDirectory + CardName + ".png";
			bmap.Save(filelocation, System.Drawing.Imaging.ImageFormat.Png);
		}

	}
}
