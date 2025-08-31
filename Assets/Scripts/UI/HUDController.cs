using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VRHearthLike.UI
{
	public class HUDController : MonoBehaviour
	{
		public ManaCrystalRow manaRow;
		public TMP_Text manaText;
		public TMP_Text playerHealthText;
		public TMP_Text enemyHealthText;

		public void BindMana(int current, int max)
		{
			if (manaRow != null) manaRow.UpdateCrystals(current, max);
			if (manaText != null) manaText.text = $"{current}/{max}";
		}

		public void BindHealth(int playerHealth, int enemyHealth)
		{
			if (playerHealthText != null) playerHealthText.text = playerHealth.ToString();
			if (enemyHealthText != null) enemyHealthText.text = enemyHealth.ToString();
		}
	}
}


