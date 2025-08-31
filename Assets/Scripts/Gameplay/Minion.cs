using UnityEngine;
using VRHearthLike.Cards;
using VRHearthLike.Core;

namespace VRHearthLike.Gameplay
{

	public class Minion : MonoBehaviour
	{
		[Header("Values")]
		public int currentAttack;
		public int currentHealth;
		
		[Header("Attribution")]
		public VRPlayer owner;
		public Side side;
		public bool canAttackThisTurn;
		
		public void Initialize(CardAsset asset, VRPlayer minionOwner)
		{
			owner = minionOwner;
			// A more robust way to determine side
			side = owner.side; // This requires VRPlayer to have a 'side' field
			
			// CORRECTED: Removed duplicate assignment lines
			currentAttack = Mathf.Max(0, asset != null ? asset.attack : 0);
			currentHealth = Mathf.Max(1, asset != null ? asset.maxHealth : 1);
			canAttackThisTurn = asset != null ? asset.hasCharge : false;
			// If has charge, can attack immediately
		}
		
		public void OnTurnStart()
		{
			canAttackThisTurn = true;
		}
		
		public void DealDamageTo(Minion target)
		{
			if (target == null || !canAttackThisTurn) return;
			
			target.TakeDamage(currentAttack);
			TakeDamage(target.currentAttack);
			canAttackThisTurn = false;
		}
		
		public void TakeDamage(int amount)
		{
			currentHealth -= Mathf.Max(0, amount);
			if (currentHealth <= 0)
			{
				Die();
			}
		}
		
		private void Die()
		{
			// Handle death logic: notify board, play animation, destroy GameObject, etc.
			Destroy(gameObject, 0.5f); // Example: destroy after a short delay for animation
		}
		
		// Methods for backward compatibility
		public void Attack(Minion target)
		{
			DealDamageTo(target);
		}
		
		public void Attack(Hero target)
		{
			if (target == null || !canAttackThisTurn) return;
			target.TakeDamage(currentAttack);
			canAttackThisTurn = false;
		}
		
		public void ReceiveDamage(int amount)
		{
			TakeDamage(amount);
		}
		
		// Properties for backward compatibility
		public int CurrentAttack => currentAttack;
	}
}
