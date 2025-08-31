using UnityEngine;

namespace VRHearthLike.Gameplay
{

	public class ManaManager : MonoBehaviour
	{
		[Header("Mana Settings")]
		public int currentMana = 0;
		public int maxMana = 10;
		public int manaThisTurn = 0;
		

		public bool Spend(int amount)
		{
			if (currentMana >= amount)
			{
				currentMana -= amount;
				return true;
			}
			return false;
		}
		

		public void AddMana(int amount)
		{
			currentMana = Mathf.Min(maxMana, currentMana + amount);
		}
		

		public void SetManaThisTurn(int amount)
		{
			manaThisTurn = Mathf.Min(maxMana, amount);
			currentMana = manaThisTurn;
		}
		

		public void ResetForNewTurn()
		{
			currentMana = Mathf.Min(maxMana, currentMana + 1);
			manaThisTurn = currentMana;
		}
	}
}
