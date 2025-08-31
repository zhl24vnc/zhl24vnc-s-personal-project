using UnityEngine;
using UnityEngine.InputSystem;

namespace VRHearthLike.VR
{
	public class TriggerActionBridge : MonoBehaviour
	{
		public PointerRaycaster pointer;
		public InputActionReference triggerAction;

		private void OnEnable()
		{
			if (triggerAction != null && triggerAction.action != null)
			{
				triggerAction.action.performed += OnPerformed;
				triggerAction.action.Enable();
			}
		}

		private void OnDisable()
		{
			if (triggerAction != null && triggerAction.action != null)
			{
				triggerAction.action.performed -= OnPerformed;
				triggerAction.action.Disable();
			}
		}

		private void OnPerformed(InputAction.CallbackContext ctx)
		{
			if (pointer != null) pointer.FireTrigger();
		}
	}
}


