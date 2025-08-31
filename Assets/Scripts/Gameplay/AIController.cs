using System.Collections.Generic;
using UnityEngine;
using VRHearthLike.Core;
using VRHearthLike.Cards;

namespace VRHearthLike.Gameplay
{

	public class AIController : PlayerAgent
	{
		public BoardManager boardManager;
		public Hero enemyHero; // Enemy Hero (Player Side)
		public Hero hero; // AI Hero
		
		public override void OnTurnStart()
		{
			base.OnTurnStart();
			if (runtimePlayer == null) return;
			
			// Try to play a viable minion
			TryPlayAvailableMinions();
			// Attack enemy heroes
			AttackEnemyHero();
		}
		
		private void TryPlayAvailableMinions() // CORRECTED: Added missing closing parenthesis for method name
		{
			if (runtimePlayer == null || boardManager == null) return;
			
			List<CardAsset> copy = new List<CardAsset>(runtimePlayer.hand);
			foreach (var cardAsset in copy)
			{
				// Check if the card is playable (has enough mana and is a minion)
				if (cardAsset != null && runtimePlayer.CanPlayCard(cardAsset) && !cardAsset.isSpell)
				{
					if (boardManager.TryGetFirstEmptySlot(Side.AI, out BoardSlot slot))
					{
						// Play the card at the slot's position
						runtimePlayer.PlayCard(cardAsset, slot.transform.position);
					}
				}
			}
		} // CORRECTED: Added the missing closing brace for the method
		
		private void AttackEnemyHero()
		{
			if (runtimePlayer == null || enemyHero == null) return;
			
			foreach (var m in runtimePlayer.minionsOnTable)
			{
				if (m == null) continue;
				if (m.canAttackThisTurn)
				{
					enemyHero.TakeDamage(m.currentAttack);
					m.canAttackThisTurn = false;
				}
			}
		}
	}
}


