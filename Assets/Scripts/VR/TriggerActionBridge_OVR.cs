using UnityEngine;
using Oculus.Interaction;

namespace VRHearthLike.VR
{
	public class TriggerActionBridge_OVR : MonoBehaviour
	{
		public PointerRaycaster pointer;
		public OVRInput.Button triggerButton = OVRInput.Button.PrimaryIndexTrigger;
		
		private void Update()
		{
			// 检测右手扳机按下
			if (OVRInput.GetDown(triggerButton, OVRInput.Controller.RTouch))
			{
				if (pointer != null)
				{
					pointer.FireTrigger();
				}
			}
		}
	}
}
