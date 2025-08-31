using System.Collections.Generic;
using UnityEngine;

namespace VRHearthLike.Cards
{

	[CreateAssetMenu(fileName = "New Deck", menuName = "VR Card Game/Deck List", order = 2)]
	public class DeckList : ScriptableObject
	{
		[Header("Deck Information")]
		public string deckName = "New Deck";
		[TextArea(2, 3)]
		public string description = "A deck for the VR card game";
		
		[Header("Cards")]
		public List<CardAsset> cards = new List<CardAsset>();
		

		public bool IsValid()
		{
			return cards != null && cards.Count > 0;
		}
		

		public int GetTotalManaCost()
		{
			int total = 0;
			foreach (var card in cards)
			{
				if (card != null)
				{
					total += card.manaCost;
				}
			}
			return total;
		}
		

		public float GetAverageManaCost()
		{
			if (cards == null || cards.Count == 0) return 0f;
			return (float)GetTotalManaCost() / cards.Count;
		}
	}
}


