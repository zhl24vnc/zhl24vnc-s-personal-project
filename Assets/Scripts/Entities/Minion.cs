using UnityEngine;
using System;

namespace VRHearthLike.Entities
{
	public class Minion : MonoBehaviour
	{
		[SerializeField] private int baseAttack = 1;
		[SerializeField] private int baseHealth = 1;
		[SerializeField] private int currentHealth;
		[SerializeField] private int currentAttack;
		[SerializeField] private bool isPlayerSide;

		public event Action<Minion> OnStatsChanged;
		public event Action<Minion> OnDied;

		public int CurrentAttack => currentAttack;
		public int CurrentHealth => currentHealth;

		public void Initialize(int attack, int health)
		{
			baseAttack = attack;
			baseHealth = health;
			currentAttack = attack;
			currentHealth = health;
			OnStatsChanged?.Invoke(this);
		}

		public void SetSide(bool playerSide)
		{
			isPlayerSide = playerSide;
		}

		public bool IsPlayerSide => isPlayerSide;

		public void ReceiveDamage(int amount)
		{
			currentHealth -= Mathf.Max(0, amount);
			OnStatsChanged?.Invoke(this);
			if (currentHealth <= 0)
			{
				HandleDeath();
			}
		}

		public void Attack(Minion target)
		{
			if (target == null) return;
			target.ReceiveDamage(currentAttack);
			ReceiveDamage(target.currentAttack);
		}

		private void HandleDeath()
		{
			// TODO: 播放死亡动画，可接 Animator
			OnDied?.Invoke(this);
			Destroy(gameObject);
		}
	}
}


