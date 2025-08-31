using UnityEngine;
using System.Collections.Generic;
using VRHearthLike.Cards;

namespace VRHearthLike.Gameplay
{

	public abstract class PlayerAgent : MonoBehaviour
	{
			[Header("Player State")]
	public VRPlayer runtimePlayer;
	public List<CardLogic> hand = new List<CardLogic>();
	public List<Minion> minionsOnTable = new List<Minion>();
	
	// Properties for backward compatibility
	public ManaManager mana => runtimePlayer?.manaManager;
	public Hero hero => runtimePlayer?.hero;
		

		public virtual void OnTurnStart()
		{
			// Base implementation - can be overridden by derived classes
			Debug.Log($"Player {name} turn started");
		}
		

		public bool CanPlayCard(CardLogic card)
		{
			if (card == null || runtimePlayer == null) return false;
			return runtimePlayer.CanPlayCard(card.cardAsset);
		}
		

		public void PlayCard(CardLogic card, Vector3 position)
		{
			if (!CanPlayCard(card)) return;
			
			runtimePlayer.PlayCard(card.cardAsset, position);
			hand.Remove(card);
		}
		

		public void AddCardToHand(CardAsset cardAsset)
		{
			if (cardAsset != null)
			{
				hand.Add(new CardLogic(cardAsset));
			}
		}
		

		public void RemoveCardFromHand(CardLogic card)
		{
			hand.Remove(card);
		}
		

		public void AddMinionToTable(Minion minion)
		{
			if (minion != null && !minionsOnTable.Contains(minion))
			{
				minionsOnTable.Add(minion);
			}
		}
		

		public void RemoveMinionFromTable(Minion minion)
		{
			minionsOnTable.Remove(minion);
		}
		

		public void RemoveCard(CardAsset cardAsset)
		{
			hand.RemoveAll(card => card.cardAsset == cardAsset);
		}
	}
}


