using UnityEngine;
using VRHearthLike.Gameplay;
using VRHearthLike.Cards;

namespace VRHearthLike.VR
{
	public class OVRGameInputController : MonoBehaviour
	{
		[Header("References")]
		public PlayerController player;
		public HandManager handManager;
		public BoardManager boardManager;
		public TurnManager turnManager;
		public UI.MainMenuController menuController;
		public AIController ai;

		[Header("Hand Selection")]
		public int selectedCardIndex = 0;
		public float stickThreshold = 0.5f;
		private float lastStickX;
		private bool cardSelected = false;

		[Header("Board Selection")]
		public int selectedSlotIndex = -1;

		[Header("Attack Selection")]
		public int selectedTargetIndex = -1;
		private Minion selectedAttacker;

		[Header("Input Settings")]
		public float inputCooldown = 0.1f;
		private float lastInputTime;

		private void Update()
		{
			if (Time.time - lastInputTime < inputCooldown) return;

			HandleMenuInput();
			HandleHandSelection();
			HandleCardPlacement();
			HandleAttackSelection();
			HandleTurnEnd();
		}

		#region Menu Input
		private void HandleMenuInput()
		{
			// Y button to open/close menu
			if (OVRInput.GetDown(OVRInput.Button.Four, OVRInput.Controller.RTouch))
			{
				if (menuController != null)
				{
					menuController.Show();
				}
			}
		}
		#endregion

		#region Hand Selection
		private void HandleHandSelection()
		{
			if (handManager == null || handManager.Cards.Count == 0) return;

			// Right stick left/right to select cards
			float stickX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.RTouch).x;
			
			if (stickX > stickThreshold && lastStickX <= stickThreshold)
			{
				MoveCardSelection(1);
				lastInputTime = Time.time;
			}
			else if (stickX < -stickThreshold && lastStickX >= -stickThreshold)
			{
				MoveCardSelection(-1);
				lastInputTime = Time.time;
			}
			lastStickX = stickX;

			// A button to select card
			if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
			{
				SelectCard();
				lastInputTime = Time.time;
			}
		}

		private void MoveCardSelection(int delta)
		{
			if (handManager.Cards.Count == 0) return;
			
			selectedCardIndex = Mathf.Clamp(selectedCardIndex + delta, 0, handManager.Cards.Count - 1);
			HighlightSelectedCard();
		}

		private void SelectCard()
		{
			if (handManager.Cards.Count == 0) return;
			if (selectedCardIndex >= handManager.Cards.Count) return;

			var cardView = GetCardViewByIndex(selectedCardIndex);
			if (cardView != null)
			{
				cardView.OnPointerSelect();
				cardSelected = true;
				Debug.Log($"Selected card: {handManager.Cards[selectedCardIndex].displayName}");
			}
		}

		private void HighlightSelectedCard()
		{
			for (int i = 0; i < handManager.Cards.Count; i++)
			{
				var cardView = GetCardViewByIndex(i);
				if (cardView == null) continue;

				var pos = cardView.transform.localPosition;
				pos.y = (i == selectedCardIndex) ? 0.05f : 0f;
				cardView.transform.localPosition = pos;
			}
		}
		#endregion

		#region Card Placement
		private void HandleCardPlacement()
		{
			if (!cardSelected) return;

			// Right stick up/down to select board slots
			float stickY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.RTouch).y;
			
			if (Mathf.Abs(stickY) > stickThreshold)
			{
				// Select player half board slots
				int newSlotIndex = Mathf.Clamp(Mathf.RoundToInt(stickY * 4), 0, 7);
				if (newSlotIndex != selectedSlotIndex)
				{
					selectedSlotIndex = newSlotIndex;
					HighlightSelectedSlot();
					lastInputTime = Time.time;
				}
			}

			// Trigger to confirm placement
			if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
			{
				PlaceCard();
				lastInputTime = Time.time;
			}

			// B button to cancel selection
			if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
			{
				CancelCardSelection();
				lastInputTime = Time.time;
			}
		}

		private void PlaceCard()
		{
			if (selectedSlotIndex < 0 || selectedSlotIndex >= boardManager.PlayerSlots.Count) return;

			var slot = boardManager.PlayerSlots[selectedSlotIndex];
			if (slot != null && slot.IsEmpty)
			{
				if (player.TryPlacePendingCard(slot))
				{
					cardSelected = false;
					selectedSlotIndex = -1;
					Debug.Log($"Card placed in slot {selectedSlotIndex}");
				}
			}
		}

		private void CancelCardSelection()
		{
			player.CancelPendingCard();
			cardSelected = false;
			selectedSlotIndex = -1;
			Debug.Log("Card selection cancelled");
		}
		#endregion

		#region Attack Selection
		private void HandleAttackSelection()
		{
			// Left stick to select attacker (own minion)
			float leftStickX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
			if (Mathf.Abs(leftStickX) > stickThreshold)
			{
				int attackerIndex = Mathf.Clamp(Mathf.RoundToInt(leftStickX * 4), 0, 7);
				SelectAttacker(attackerIndex);
			}

			// If attacker is selected, right stick to select target
			if (selectedAttacker != null)
			{
				float rightStickX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.RTouch).x;
				float rightStickY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.RTouch).y;

				if (Mathf.Abs(rightStickX) > stickThreshold || Mathf.Abs(rightStickY) > stickThreshold)
				{
					// Select enemy target
					int targetIndex = Mathf.Clamp(Mathf.RoundToInt(rightStickX * 4), 0, 7);
					SelectTarget(targetIndex);
				}

				// Trigger to confirm attack
				if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
				{
					ExecuteAttack();
					lastInputTime = Time.time;
				}

				// B button to cancel attack
				if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
				{
					CancelAttack();
					lastInputTime = Time.time;
				}
			}
		}

		private void SelectAttacker(int index)
		{
			if (index < 0 || index >= boardManager.PlayerSlots.Count) return;

			var slot = boardManager.PlayerSlots[index];
			if (slot != null && slot.occupiedMinion != null)
			{
				selectedAttacker = slot.occupiedMinion;
				player.SelectAttacker(selectedAttacker);
				Debug.Log($"Selected attacker: {selectedAttacker.name}");
			}
		}

		private void SelectTarget(int index)
		{
			if (index < 0 || index >= boardManager.EnemySlots.Count) return;

			var slot = boardManager.EnemySlots[index];
			if (slot != null && slot.occupiedMinion != null)
			{
				player.TryAttackTarget(slot.occupiedMinion);
				selectedAttacker = null;
				Debug.Log($"Attacked target in slot {index}");
			}
		}

		private void ExecuteAttack()
		{
			// Attack enemy hero
			if (ai != null && ai.hero != null)
			{
				player.TryAttackHero(ai.hero);
				selectedAttacker = null;
				Debug.Log("Attacked enemy hero");
			}
		}

		private void CancelAttack()
		{
			selectedAttacker = null;
			Debug.Log("Attack cancelled");
		}
		#endregion

		#region Turn End
		private void HandleTurnEnd()
		{
			// Long press B button to end turn
			if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
			{
				if (Time.time - lastInputTime > 1.0f) // Long press for 1 second
				{
					turnManager.EndTurn();
					lastInputTime = Time.time;
					Debug.Log("Turn ended");
				}
			}
		}
		#endregion

		#region Helper Methods
		private CardInHand GetCardViewByIndex(int index)
		{
			if (handManager == null || handManager.transform.childCount == 0) return null;
			if (index < 0 || index >= handManager.transform.childCount) return null;
			
			return handManager.transform.GetChild(index).GetComponent<CardInHand>();
		}

		private void HighlightSelectedSlot()
		{
			// Highlight selected slot (can add visual feedback)
			for (int i = 0; i < boardManager.PlayerSlots.Count; i++)
			{
				var slot = boardManager.PlayerSlots[i];
				if (slot != null)
				{
					// Can add highlight effect
					var renderer = slot.GetComponent<Renderer>();
					if (renderer != null)
					{
						renderer.material.color = (i == selectedSlotIndex) ? Color.yellow : Color.white;
					}
				}
			}
		}
		#endregion
	}
}
