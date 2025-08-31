using System.Collections.Generic;
using System;
using UnityEngine;
using VRHearthLike.Cards;

namespace VRHearthLike.Gameplay
{

	public class Deck
	{
		private readonly List<CardAsset> runtimeCards = new List<CardAsset>();
		
		public int Count { get { return runtimeCards.Count; } }
		
		public void InitializeFromDeckList(DeckList deckList)
		{
			runtimeCards.Clear();
			if (deckList != null && deckList.cards != null)
			{
				runtimeCards.AddRange(deckList.cards);
			}
		}
		
		// Fisher-Yates shuffle algorithm implementation
		public void Shuffle(System.Random random = null)
		{
			if (runtimeCards.Count <= 1) return;
			
			random = random ?? new System.Random();
			for (int i = runtimeCards.Count - 1; i > 0; i--)
			{
				int j = random.Next(i + 1);
				CardAsset temp = runtimeCards[i];
				runtimeCards[i] = runtimeCards[j];
				runtimeCards[j] = temp;
			}
		}
		
		// Draw card from top of deck (position 0)
		public bool TryDraw(out CardAsset card)
		{
			if (runtimeCards.Count > 0)
			{
				card = runtimeCards[0];
				runtimeCards.RemoveAt(0);
				return true;
			}
			card = null;
			return false;
		}
	}
}


