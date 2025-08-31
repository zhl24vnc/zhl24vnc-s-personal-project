using UnityEngine;
using VRHearthLike.Gameplay;

namespace VRHearthLike.VR
{
	public class OVRSimpleInputController : MonoBehaviour
	{
		[Header("References")]
		public PlayerController player;
		public HandManager handManager;
		public TurnManager turnManager;

		[Header("Input Settings")]
		public float stickThreshold = 0.5f;
		private float lastStickX;
		private int selectedCardIndex = 0;

		private void Update()
		{
			HandleHandSelection();
			HandleTurnEnd();
		}

		private void HandleHandSelection()
		{
			if (handManager == null || handManager.Cards.Count == 0) return;

			// 右摇杆左右选择卡牌
			float stickX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.RTouch).x;
			
			if (stickX > stickThreshold && lastStickX <= stickThreshold)
			{
				MoveCardSelection(1);
			}
			else if (stickX < -stickThreshold && lastStickX >= -stickThreshold)
			{
				MoveCardSelection(-1);
			}
			lastStickX = stickX;

			// A键选择卡牌
			if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
			{
				SelectCard();
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

		private void HandleTurnEnd()
		{
			// B键结束回合
			if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
			{
				turnManager.EndTurn();
				Debug.Log("Turn ended");
			}
		}

		private CardInHand GetCardViewByIndex(int index)
		{
			if (handManager == null || handManager.transform.childCount == 0) return null;
			if (index < 0 || index >= handManager.transform.childCount) return null;
			
			return handManager.transform.GetChild(index).GetComponent<CardInHand>();
		}
	}
}
