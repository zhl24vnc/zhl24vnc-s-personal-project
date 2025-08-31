using UnityEngine;
using UnityEngine.UI;

namespace VRHearthLike.UI
{

	public class HealthDisplay : MonoBehaviour
	{
		[Header("UI References")]
		public Text healthText;
		public Slider healthSlider;
		public Image healthBarFill;
		
		[Header("Colors")]
		public Color healthyColor = Color.green;
		public Color warningColor = Color.yellow;
		public Color dangerColor = Color.red;
		

		public void UpdateHealth(int currentHealth, int maxHealth)
		{
			if (healthText != null)
			{
				healthText.text = $"{currentHealth}/{maxHealth}";
			}
			
			if (healthSlider != null)
			{
				healthSlider.maxValue = maxHealth;
				healthSlider.value = currentHealth;
			}
			
			if (healthBarFill != null)
			{
				float healthPercentage = (float)currentHealth / maxHealth;
				healthBarFill.color = GetHealthColor(healthPercentage);
			}
		}
		

		private Color GetHealthColor(float percentage)
		{
			if (percentage > 0.6f)
				return healthyColor;
			else if (percentage > 0.3f)
				return warningColor;
			else
				return dangerColor;
		}
	}
}
