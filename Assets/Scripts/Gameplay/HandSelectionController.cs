using UnityEngine;
using VRHearthLike.Cards;

namespace VRHearthLike.Gameplay
{
	public class HandSelectionController : MonoBehaviour
	{
		public PlayerController player;
		public HandManager hand;
		public int selectedIndex = 0;
		public float stickThreshold = 0.5f;

		private float lastStickX;

		private void Update()
		{
			if (player == null || hand == null) return;
			// 读取右摇杆X（OVR）
			float x = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
			if (x > stickThreshold && lastStickX <= stickThreshold)
			{
				MoveSelection(1);
			}
			else if (x < -stickThreshold && lastStickX >= -stickThreshold)
			{
				MoveSelection(-1);
			}
			lastStickX = x;

			// A键确定
			if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
			{
				ConfirmSelection();
			}
		}

		private void MoveSelection(int delta)
		{
			if (hand.Cards.Count == 0) return;
			selectedIndex = Mathf.Clamp(selectedIndex + delta, 0, hand.Cards.Count - 1);
			HighlightSelected();
		}

		private void HighlightSelected()
		{
			// 简化：通过让目标卡抬起一点表示高亮
			for (int i = 0; i < hand.Cards.Count; i++)
			{
				var view = FindCardViewByIndex(i);
				if (view == null) continue;
				var pos = view.transform.localPosition;
				pos.y = (i == selectedIndex) ? 0.04f : 0f;
				view.transform.localPosition = pos;
			}
		}

		private CardInHand FindCardViewByIndex(int idx)
		{
			// 视图是 HandManager 在 handAnchor 下实例化，按顺序排列
			if (hand == null) return null;
			if (hand.transform.childCount == 0) return null;
			if (idx < 0 || idx >= hand.transform.childCount) return null;
			return hand.transform.GetChild(idx).GetComponent<CardInHand>();
		}

		private void ConfirmSelection()
		{
			var view = FindCardViewByIndex(selectedIndex);
			if (view != null)
			{
				view.OnPointerSelect();
			}
		}
	}
}


