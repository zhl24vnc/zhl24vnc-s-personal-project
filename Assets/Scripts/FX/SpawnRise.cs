using System.Collections;
using UnityEngine;

namespace VRHearthLike.FX
{
	public class SpawnRise : MonoBehaviour
	{
		public float riseDistance = 0.15f;
		public float duration = 0.35f;

		private void OnEnable()
		{
			StartCoroutine(RiseRoutine());
		}

		private IEnumerator RiseRoutine()
		{
			var start = transform.position - new Vector3(0, riseDistance, 0);
			var end = transform.position;
			transform.position = start;
			float t = 0;
			while (t < duration)
			{
				t += Time.deltaTime;
				float k = Mathf.Clamp01(t / duration);
				transform.position = Vector3.Lerp(start, end, k);
				yield return null;
			}
			transform.position = end;
		}
	}
}


