using UnityEngine;
using VRHearthLike.Core;

namespace VRHearthLike.Gameplay
{
	public class ManaSystem : MonoBehaviour
	{
		[SerializeField] private int maxMana = 0;
		[SerializeField] private int currentMana = 0;

		public int MaxMana => maxMana;
		public int CurrentMana => currentMana;

		public void ResetMana()
		{
			maxMana = 0;
			currentMana = 0;
		}

		public void StartTurnGain()
		{
			maxMana = Mathf.Min(GameConfig.MaxMana, maxMana + 1);
			currentMana = maxMana;
		}

		public bool CanAfford(int cost)
		{
			return cost <= currentMana;
		}

		public bool Spend(int cost)
		{
			if (!CanAfford(cost)) return false;
			currentMana -= cost;
			return true;
		}
	}
}


