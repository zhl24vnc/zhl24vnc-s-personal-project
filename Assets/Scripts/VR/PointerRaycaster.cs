using UnityEngine;
using VRHearthLike.Gameplay;

namespace VRHearthLike.VR
{

	public class PointerRaycaster : MonoBehaviour
	{
		public Camera rayCamera; // The main camera can be simulated in the editor
		public float rayDistance = 10f;
		public LayerMask hitMask;
		
		public void FireTrigger()
		{
			Ray ray = (rayCamera != null)
				? new Ray(rayCamera.transform.position, rayCamera.transform.forward)
				: new Ray(transform.position, transform.forward);
				
			if (Physics.Raycast(ray, out var hit, rayDistance, hitMask))
			{
				// Process hit objects
				var endTurn = hit.collider.GetComponent<EndTurnButton>();
				if (endTurn != null)
				{
					endTurn.Press();
					return;
				}
			}
		}
	}
}


