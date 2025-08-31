using UnityEngine;
using VRHearthLike.Cards;

namespace VRHearthLike.UI
{
	public class CardView : MonoBehaviour
	{
		[SerializeField] private CardAsset data;

		public void Bind(CardAsset cardData)
		{
			data = cardData;
			if (data != null)
			{
				gameObject.name = $"Card_{data.displayName}";
			}
		}
	}
}


