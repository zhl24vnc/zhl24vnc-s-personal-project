using UnityEngine;
using UnityEngine.UI;

namespace VRHearthLike.UI
{

	public class ManaCrystal : MonoBehaviour
	{
		[Header("UI References")]
		public Text manaText;
		public Slider manaSlider;
		public Image manaBarFill;
		
		[Header("Colors")]
		public Color availableManaColor = Color.blue;
		public Color spentManaColor = Color.gray;
		

		public void UpdateMana(int currentMana, int maxMana)
		{
			if (manaText != null)
			{
				manaText.text = $"{currentMana}/{maxMana}";
			}
			
			if (manaSlider != null)
			{
				manaSlider.maxValue = maxMana;
				manaSlider.value = currentMana;
			}
			
			if (manaBarFill != null)
			{
				float manaPercentage = (float)currentMana / maxMana;
				manaBarFill.fillAmount = manaPercentage;
				manaBarFill.color = availableManaColor;
			}
		}
		

		public void ShowSpentMana(int spentAmount)
		{
			if (manaBarFill != null)
			{
				manaBarFill.color = spentManaColor;
			}
		}
	}
}
