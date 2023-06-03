﻿using CsvHelper.Configuration.Attributes;
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

		public readonly int CardImageX = (int)(0.03 * CardWidth);
		public readonly int CardImageY = (int)(0.1 * CardHeight);

		public readonly int NameBoxX = (int)(0.06 * CardWidth);
		public readonly int NameBoxY = (int)(0.035 * CardHeight);
		public readonly int NameBoxWidth = (int)(0.7 * CardWidth);
		public readonly int NameBoxHeight = (int)(0.07 * CardHeight);

		public readonly int MBoxX = (int)(0.02 * CardWidth);
		public readonly int MBoxY = (int)(0.135 * CardHeight);
		public readonly int MBoxWidth = (int)(0.15 * CardWidth);
		public readonly int MBoxHeight = (int)(0.115 * CardHeight);

		public readonly int ABoxX = (int)(0.02 * CardWidth);
		public readonly int ABoxY = (int)(0.265 * CardHeight);
		public readonly int ABoxWidth = (int)(0.15 * CardWidth);
		public readonly int ABoxHeight = (int)(0.115 * CardHeight);

		public readonly int CABoxX = (int)(0.07 * CardWidth);
		public readonly int CABoxY = (int)(0.605 * CardHeight);
		public readonly int CABoxWidth = (int)(0.87 * CardWidth);
		public readonly int CABoxHeight = (int)(0.15 * CardHeight);

		public readonly int AABoxX = (int)(0.07 * CardWidth);
		public readonly int AABoxY = (int)(0.805 * CardHeight);
		public readonly int AABoxWidth = (int)(0.87 * CardWidth);
		public readonly int AABoxHeight = (int)(0.15 * CardHeight);


		public readonly string ActionTemplate = @"Img/RondelonTemplate.png";

		public void DrawCard(string outputDirectory)
        {
			StringFormat Center = new StringFormat();
			Center.Alignment = StringAlignment.Center;
			Center.LineAlignment = StringAlignment.Center;
			StringFormat Right = new StringFormat();
			Right.LineAlignment = StringAlignment.Center;
			List<Layer> layers = new List<Layer>();
			CardDrawer cd = CardDrawer.Instance();

			layers.Add(cd.CreateLayerFromFile(CardImageX, CardImageY, 0, 0, CardImage));//card image
			layers.Add(cd.CreateLayerFromFile(0, 0, 0, 0, ActionTemplate)); // template
			layers.Add(cd.CreateTextLayer(NameBoxX, NameBoxY, NameBoxWidth, NameBoxHeight, CardName, cd.LargeFont, Right)); //cardname
			layers.Add(cd.CreateTextLayer(MBoxX, MBoxY, MBoxWidth, MBoxHeight, Movement, cd.BoxFont, Center)); //cardmove
			layers.Add(cd.CreateTextLayer(ABoxX, ABoxY, ABoxWidth, ABoxHeight, Attack, cd.BoxFont, Center)); //cardattack
			

			if(!string.IsNullOrEmpty(CardAbility))
			{
				Font abilityFont = cd.mediumFont;
				if (CardAbility.Length < 150) abilityFont = cd.LargeFont;
				layers.Add(cd.CreateTextLayer(CABoxX, CABoxY, CABoxWidth, CABoxHeight, CardAbility, abilityFont, Center)); //cardability
			}
			if (!string.IsNullOrEmpty(AttackAbility))
			{
				Font abilityFont = cd.mediumFont;
				if (CardAbility.Length < 150) abilityFont = cd.LargeFont;
				layers.Add(cd.CreateTextLayer(AABoxX, AABoxY, AABoxWidth, AABoxHeight, AttackAbility,abilityFont, Center)); //card
			}
			
			Bitmap bmap = CardDrawer.Instance().MergeLayers(layers, CardWidth, CardHeight);
			string filelocation = outputDirectory + CardName + ".png";
			bmap.Save(filelocation, System.Drawing.Imaging.ImageFormat.Png);
		}

	}
}
