using UnityEngine;

namespace VRHearthLike.Cards
{

	public class CardLogic
	{
		public CardAsset cardAsset;
		public bool isSpell;
		public int manaCost;
		public int attack;
		public int health;
		
		public CardLogic(CardAsset asset)
		{
			cardAsset = asset;
			if (asset != null)
			{
				isSpell = asset.isSpell;
				manaCost = asset.manaCost;
				attack = asset.attack;
				health = asset.maxHealth;
			}
		}
		

		public bool IsMinion()
		{
			return !isSpell && cardAsset != null && cardAsset.minionPrefab != null;
		}
		

		public bool IsSpell()
		{
			return isSpell;
		}
		

		public string GetDisplayName()
		{
			return cardAsset != null ? cardAsset.cardName : "Unknown Card";
		}
	}
}
