using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CardMaker
{
	public class ActionCard	: Card
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

		// constants
		public static readonly int CardHeight = 1050;
		public static readonly int CardWidth = 750;

		public readonly int CardImageX = (int)(0.03 * CardWidth);
		public readonly int CardImageY = (int)(0.12 * CardHeight);
		public readonly int CardImageWidth = (int)(0.94 * CardWidth);
		public readonly int CardImageHeight = (int)(0.45 * CardWidth);

		public readonly int NameBoxX = (int)(0.15 * CardWidth);
		public readonly int NameBoxY = (int)(0.03 * CardHeight);
		public readonly int NameBoxWidth = (int)(0.7 * CardWidth);
		public readonly int NameBoxHeight = (int)(0.06 * CardHeight);
		
		public readonly int ClassBoxX = (int)(0.72 * CardWidth);
		public readonly int ClassBoxY = (int)(0.03 * CardHeight);
		public readonly int ClassBoxWidth = (int)(0.16 * CardWidth);
		public readonly int ClassBoxHeight = (int)(0.06 * CardHeight);

		public readonly int MBoxX = (int)(0.013 * CardWidth);
		public readonly int MBoxY = (int)(0.14 * CardHeight);
		public readonly int MBoxWidth = (int)(0.175 * CardWidth);
		public readonly int MBoxHeight = (int)(0.115 * CardHeight);

		public readonly int ABoxX = (int)(0.013 * CardWidth);
		public readonly int ABoxY = (int)(0.265 * CardHeight);
		public readonly int ABoxWidth = (int)(0.175 * CardWidth);
		public readonly int ABoxHeight = (int)(0.115 * CardHeight);

		public readonly int CABoxX = (int)(0.14 * CardWidth);
		public readonly int CABoxY = (int)(0.59 * CardHeight);
		public readonly int CABoxWidth = (int)(0.72 * CardWidth);
		public readonly int CABoxHeight = (int)(0.142 * CardHeight);
		//public readonly int CABoxHeight = (int)(0.40 * CardHeight);

		public readonly int AABoxX = (int)(0.133 * CardWidth);
		public readonly int AABoxY = (int)(0.805 * CardHeight);
		public readonly int AABoxWidth = (int)(0.72 * CardWidth);
		public readonly int AABoxHeight = (int)(0.142 * CardHeight);


		public readonly string ActionTemplate = @"Img/RondelonTemplate.png";

		public Font ChooseFont(string text)
        {
			CardDrawer cd = CardDrawer.Instance();
			int size = 30;
			if (text.Length >= 60) size = 20;
			Font customFont = new Font(cd.FONT_FILE, size);
			return customFont;
        }


		public override void DrawCard(string outputDirectory)
        {
			StringFormat Center = new StringFormat();
			Center.Alignment = StringAlignment.Center;
			Center.LineAlignment = StringAlignment.Center;
			StringFormat Right = new StringFormat();
			Right.LineAlignment = StringAlignment.Center;
			List<Layer> layers = new List<Layer>();
			CardDrawer cd = CardDrawer.Instance();

			string CardImage = $"Img/{CardName.Replace(" ", "").ToLower()}.png";
			//string CardImage = $"Img/innkeeper.png";

			layers.Add(cd.CreateLayerFromFileUniform(CardImageX, CardImageY, CardImageWidth, CardImageHeight, CardImage));//card image
			layers.Add(cd.CreateLayerFromFile(0, 0, CardWidth, CardHeight, ActionTemplate)); // template
			//string Namebox = ((string.IsNullOrEmpty(CardClass) ? CardName : CardName + " - " + CardClass));
			layers.Add(cd.CreateTextLayer(NameBoxX, NameBoxY, NameBoxWidth, NameBoxHeight, CardName, cd.NameFont, Right)); //cardname
			layers.Add(cd.CreateTextLayer(ClassBoxX, ClassBoxY, ClassBoxWidth, ClassBoxHeight, CardClass, cd.BoxFont, Right)); //cardname
			
			layers.Add(cd.CreateTextLayer(MBoxX, MBoxY, MBoxWidth, MBoxHeight, Movement, cd.SuperFont, Center)); //cardmove
			layers.Add(cd.CreateTextLayer(ABoxX, ABoxY, ABoxWidth, ABoxHeight, Attack, cd.SuperFont, Center)); //cardattack
			Font abilityFont = cd.mediumFont;
			if(CardAbility.Length > AttackAbility.Length) abilityFont = ChooseFont(CardAbility);
			else abilityFont = ChooseFont(AttackAbility);
			if (!string.IsNullOrEmpty(CardAbility))
			{
				layers.Add(cd.CreateTextLayer(CABoxX, CABoxY, CABoxWidth, CABoxHeight, CardAbility, abilityFont, Center)); //cardability
			}
			if (!string.IsNullOrEmpty(AttackAbility))
			{
				layers.Add(cd.CreateTextLayer(AABoxX, AABoxY, AABoxWidth, AABoxHeight, AttackAbility,abilityFont, Center)); //card
			}

			if (CardClass.Length>0)
			{
				string[] classes = CardClass.Split(',');
				foreach (string cClass in classes)
				{
					string classImage = $"Img/{cClass.Replace(" ", "").ToLower()}";
				}
			}
			
			Bitmap bmap = CardDrawer.Instance().MergeLayers(layers, CardWidth, CardHeight);
			layers=new List<Layer>();
			string filelocation = outputDirectory + CardName + ".png";
			bmap.Save(filelocation, System.Drawing.Imaging.ImageFormat.Png);
		}

	}
}
