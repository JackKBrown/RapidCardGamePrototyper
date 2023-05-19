using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
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

		// constants
		public readonly int CardHeight = 2100;
		public readonly int CardWidth = 1500;

		public void DrawCard()
        {
			List<Layer> layers = new List<Layer>();

			// populate layer list
			// ask Card Drawer to merge layer
        }

	}
}
