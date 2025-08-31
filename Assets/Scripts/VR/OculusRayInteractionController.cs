using UnityEngine;
using Oculus.Interaction;
using VRHearthLike.Gameplay;
using VRHearthLike.Cards;

namespace VRHearthLike.VR
{
	public class OculusRayInteractionController : MonoBehaviour
	{
		[Header("Oculus Interaction References")]
		public RayInteractor rayInteractor;
		public PokeInteractor pokeInteractor;
		public GrabInteractor grabInteractor;

		[Header("Game References")]
		public PlayerController player;
		public HandManager handManager;
		public BoardManager boardManager;
		public TurnManager turnManager;

		[Header("Interaction Settings")]
		public LayerMask cardLayerMask = 1 << 8; // Card layer
		public LayerMask boardLayerMask = 1 << 9; // Board layer
		public LayerMask heroLayerMask = 1 << 10; // Hero layer

		private IInteractable currentHovered;
		private CardInHand selectedCard;
		private BoardSlot selectedSlot;
		private Minion selectedAttacker;

		private void Start()
		{
			SetupRayInteractor();
		}

		private void SetupRayInteractor()
		{
			if (rayInteractor == null)
			{
				rayInteractor = GetComponent<RayInteractor>();
			}

			if (rayInteractor != null)
			{
							// Note: Different versions of Oculus Interaction may have different event systems
			// Here we use simplified setup to avoid version compatibility issues
				Debug.Log("Ray Interactor setup completed");
			}
		}

		private void Update()
		{
			HandleInput();
		}

		private void HandleInput()
		{
			// Use OVRInput to handle input
			HandleCardSelection();
			HandleCardPlacement();
			HandleAttackSelection();
			HandleTurnEnd();
		}

		#region Card Selection
		private void HandleCardSelection()
		{
			// Right stick left/right to select cards
			float stickX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.RTouch).x;
			
			if (Mathf.Abs(stickX) > 0.5f)
			{
				MoveCardSelection(stickX > 0 ? 1 : -1);
			}

			// A button to select card
			if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
			{
				SelectCurrentCard();
			}
		}

		private void MoveCardSelection(int delta)
		{
			if (handManager == null || handManager.Cards.Count == 0) return;
			
			// Simplified version: directly select card by index
			int currentIndex = 0;
			if (handManager.transform.childCount > 0)
			{
				// Find currently selected card
				for (int i = 0; i < handManager.transform.childCount; i++)
				{
					var child = handManager.transform.GetChild(i);
					var cardView = child.GetComponent<CardInHand>();
					if (cardView != null)
					{
						var pos = child.localPosition;
						if (pos.y > 0.01f) // 如果卡牌被高亮
						{
							currentIndex = i;
							break;
						}
					}
				}
				
				// Move to next card
				int newIndex = Mathf.Clamp(currentIndex + delta, 0, handManager.transform.childCount - 1);
				HighlightCardByIndex(newIndex);
			}
		}

		private void SelectCurrentCard()
		{
			// Simplified version: select currently highlighted card
			if (handManager != null && handManager.transform.childCount > 0)
			{
				for (int i = 0; i < handManager.transform.childCount; i++)
				{
					var child = handManager.transform.GetChild(i);
					var cardView = child.GetComponent<CardInHand>();
					if (cardView != null)
					{
						var pos = child.localPosition;
						if (pos.y > 0.01f) // If card is highlighted
						{
							selectedCard = cardView;
							cardView.OnPointerSelect();
							Debug.Log($"Selected card: {cardView.Data.cardName}");
							break;
						}
					}
				}
			}
		}

		private void HighlightCard(CardInHand cardView)
		{
				// Highlight selected card
			var pos = cardView.transform.localPosition;
			pos.y = 0.05f;
			cardView.transform.localPosition = pos;
		}

		private void HighlightCardByIndex(int index)
		{
			// Reset all card positions
			for (int i = 0; i < handManager.transform.childCount; i++)
			{
				var child = handManager.transform.GetChild(i);
				var pos = child.localPosition;
				pos.y = 0f;
				child.localPosition = pos;
			}

			// Highlight selected card
			if (index >= 0 && index < handManager.transform.childCount)
			{
				var selectedChild = handManager.transform.GetChild(index);
				var pos = selectedChild.localPosition;
				pos.y = 0.05f;
				selectedChild.localPosition = pos;
			}
		}
		#endregion

		#region Card Placement
		private void HandleCardPlacement()
		{
			if (selectedCard == null) return;

			// Simplified version: use joystick to select placement position
			float stickY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.RTouch).y;
			if (Mathf.Abs(stickY) > 0.5f)
			{
				// Select player side slots
				int slotIndex = Mathf.Clamp(Mathf.RoundToInt(stickY * 4), 0, 7);
				if (boardManager != null && slotIndex < boardManager.PlayerSlots.Count)
				{
					selectedSlot = boardManager.PlayerSlots[slotIndex];
					HighlightSlot(selectedSlot);
				}
			}

			// Trigger to confirm placement
			if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
			{
				PlaceCard();
			}

			// B button to cancel selection
			if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
			{
				CancelCardSelection();
			}
		}

		private void PlaceCard()
		{
			if (selectedSlot != null && selectedSlot.IsEmpty)
			{
				if (player.TryPlacePendingCard(selectedSlot))
				{
					selectedCard = null;
					selectedSlot = null;
					Debug.Log("Card placed successfully");
				}
			}
		}

		private void CancelCardSelection()
		{
			player.CancelPendingCard();
			selectedCard = null;
			selectedSlot = null;
			Debug.Log("Card selection cancelled");
		}

		private void HighlightSlot(BoardSlot slot)
		{
			// Highlight selected slot
			var renderer = slot.GetComponent<Renderer>();
			if (renderer != null)
			{
				renderer.material.color = Color.yellow;
			}
		}
		#endregion

		#region Attack Selection
		private void HandleAttackSelection()
		{
			// Simplified version: use joystick to select attacker and target
			float leftStickX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
			if (Mathf.Abs(leftStickX) > 0.5f)
			{
				// Select attacker (friendly minion)
				int attackerIndex = Mathf.Clamp(Mathf.RoundToInt(leftStickX * 4), 0, 7);
				if (boardManager != null && attackerIndex < boardManager.PlayerSlots.Count)
				{
					var slot = boardManager.PlayerSlots[attackerIndex];
					if (slot != null && slot.occupiedMinion != null)
					{
						selectedAttacker = slot.occupiedMinion;
						player.SelectAttacker(selectedAttacker);
						Debug.Log($"Selected attacker: {selectedAttacker.name}");
					}
				}
			}

			// If attacker is selected, select attack target
			if (selectedAttacker != null)
			{
				float rightStickX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.RTouch).x;
				if (Mathf.Abs(rightStickX) > 0.5f)
				{
					// Select attack target (enemy minion)
					int targetIndex = Mathf.Clamp(Mathf.RoundToInt(rightStickX * 4), 0, 7);
					if (boardManager != null && targetIndex < boardManager.EnemySlots.Count)
					{
						var slot = boardManager.EnemySlots[targetIndex];
						if (slot != null && slot.occupiedMinion != null)
						{
							player.TryAttackTarget(slot.occupiedMinion);
							selectedAttacker = null;
							Debug.Log("Attacked enemy minion");
						}
					}
				}
			}
		}
		#endregion

		#region Turn End
		private void HandleTurnEnd()
		{
			// B button to end turn
			if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
			{
				turnManager.EndTurn();
				Debug.Log("Turn ended");
			}
		}
		#endregion

		#region Oculus Interaction Events
		// Note: Due to version compatibility issues, Oculus Interaction events are temporarily not used
		// If needed, can be re-enabled after confirming API version
		#endregion

		#region Setup Helpers
		public void SetupCardInteractables()
		{
			// Add interaction components to hand cards
			if (handManager != null)
			{
				foreach (var card in handManager.Cards)
				{
					var cardView = GetCardViewByData(card);
					if (cardView != null && cardView.GetComponent<InteractableUnityEventWrapper>() == null)
					{
						// Add Oculus Interaction component
						var interactable = cardView.gameObject.AddComponent<InteractableUnityEventWrapper>();
						var collider = cardView.GetComponent<Collider>();
						if (collider == null)
						{
							collider = cardView.gameObject.AddComponent<BoxCollider>();
						}
					}
				}
			}
		}

		public void SetupBoardInteractables()
		{
			// Add interaction components to board slots
			if (boardManager != null)
			{
				foreach (var slot in boardManager.PlayerSlots)
				{
					if (slot.GetComponent<InteractableUnityEventWrapper>() == null)
					{
						var interactable = slot.gameObject.AddComponent<InteractableUnityEventWrapper>();
						var collider = slot.GetComponent<Collider>();
						if (collider == null)
						{
							collider = slot.gameObject.AddComponent<BoxCollider>();
						}
					}
				}

				foreach (var slot in boardManager.EnemySlots)
				{
					if (slot.GetComponent<InteractableUnityEventWrapper>() == null)
					{
						var interactable = slot.gameObject.AddComponent<InteractableUnityEventWrapper>();
						var collider = slot.GetComponent<Collider>();
						if (collider == null)
						{
							collider = slot.gameObject.AddComponent<BoxCollider>();
						}
					}
				}
			}
		}

		private CardInHand GetCardViewByData(CardAsset cardData)
		{
			if (handManager == null) return null;
			
			foreach (Transform child in handManager.transform)
			{
				var cardView = child.GetComponent<CardInHand>();
				if (cardView != null && cardView.Data == cardData)
				{
					return cardView;
				}
			}
			return null;
		}
		#endregion
	}
}
