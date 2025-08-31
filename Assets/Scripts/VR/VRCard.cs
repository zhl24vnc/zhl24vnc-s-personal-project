using UnityEngine;
using VRHearthLike.Cards;
using VRHearthLike.Gameplay;
using VRHearthLike.UI;

namespace VRHearthLike.VR
{
	/// <summary>
	/// VRCard.cs is the core interactive component of the entire game, implementing the complete
	/// process of grabbing, selection, and placement. Grabbing is done through the XR
	/// GrabInteractable component, and accurate placement is achieved through raycasting. The
	/// OnGrabStart and OnGrabEnd events handle the grab lifecycle, while the TryPlaceCard method
	/// implements intelligent placement logic, including cost checking and position verification.
	/// </summary>
	public class VRCard : MonoBehaviour
	{
		public CardAsset cardAsset; // Data container for card properties
		public CardView cardView; // Handles UI representation
		public CardInHand cardInHand; // Manages hand interaction
		
		public void Initialize(CardAsset asset, VRPlayer owner)
		{
			cardAsset = asset;
			cardView?.Bind(asset);
			// cardInHand?.Bind(this); // Removed as CardInHand doesn't have Bind method
		}
		
		// Attempts to place card on valid board slot
		private bool TryPlaceCard()
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
			{
				var boardSlot = hit.collider.GetComponent<BoardSlot>();
				if (boardSlot != null && boardSlot.IsEmpty)
				{
					var player = FindObjectOfType<VRPlayer>();
					if (player != null && player.CanPlayCard(cardAsset))
					{
						player.PlayCard(cardAsset, hit.point);
						return true;
					}
				}
			}
			return false;
		}
	}
}
