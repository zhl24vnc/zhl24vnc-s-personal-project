using UnityEngine;
using VRHearthLike.Cards;
using VRHearthLike.UI;

namespace VRHearthLike.Gameplay
{

	[RequireComponent(typeof(Collider))]
	public class CardInHand : MonoBehaviour
	{
		[SerializeField] private CardAsset data;
		[SerializeField] private CardView view;
		private HandManager ownerHand;
		private PlayerController ownerController;
		
		public bool isHighlighted, isPlayable;
		
		public CardAsset Data => data;

		public void Initialize(CardAsset card, HandManager hand, PlayerController controller)
		{
			data = card;
			ownerHand = hand;
			ownerController = controller;
			if (view == null) view = GetComponent<CardView>();
			if (view == null) view = gameObject.AddComponent<CardView>();
			view.Bind(card);
		}

		public void OnPointerSelect()
		{
			ownerController.NotifyCardSelected(this);
		}

		public void SetVisible(bool visible)
		{
			gameObject.SetActive(visible);
		}
		
		public void SetHighlighted(bool highlighted) 
		{ 
			isHighlighted = highlighted; 
		}
		
		public void SetPlayable(bool playable) 
		{ 
			isPlayable = playable; 
		}
	}
}


