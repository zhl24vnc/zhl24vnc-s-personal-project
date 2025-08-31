using System;
using UnityEngine;
using VRHearthLike.Core;

namespace VRHearthLike.Gameplay
{

	public class Hero : MonoBehaviour
	{
		[Header("Hero Stats")]
		public int currentHealth = 30;
		public int maxHealth = 30;
		public int attack = 0;
		
		[Header("References")]
		public VRPlayer owner;
		

		public void TakeDamage(int amount)
		{
			currentHealth = Mathf.Max(0, currentHealth - amount);
			
			if (currentHealth <= 0)
			{
				OnHeroDefeated();
			}
		}
		

		public void Heal(int amount)
		{
			currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
		}
		

		private void OnHeroDefeated()
		{
			Debug.Log($"Hero {name} has been defeated!");
			// Trigger game over logic
		}
		

		public void ReceiveDamage(int amount)
		{
			TakeDamage(amount);
		}
	}
}


