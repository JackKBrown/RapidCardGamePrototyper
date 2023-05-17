using System;
using System.Collections.Generic;
using System.Text;

namespace CardMaker
{
	public class ActionCard
	{
		private string CardName { get; set; }
		private string AbilityCost { get; set; }
		private string CardAbility { get; set; }
		private int Movement { get; set; }
		private int Attack { get; set; }
		private string FlavourText { get; set; }
		private string CardClass { get; set; }

		public ActionCard(string cardName, string abilityCost, string cardAbility,
			int movement, int attack, string flavourText, string cardClass)
		{
			CardName = cardName;
			AbilityCost = abilityCost;
			CardAbility = cardAbility;
			Movement = movement;
			Attack = attack;
			FlavourText = flavourText;
			CardClass = cardClass;
		}

		public void createJson()
		{

		}
	}
}
